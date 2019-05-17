using CacheEngineShared;
using Grpc.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MessageShared;
using Grpc.Core.Utils;
using System.Configuration;

namespace MessageBroker
{
    public class mLogServiceImpl : svcLogService.svcLogServiceBase
    {
        private readonly IDataflowSubscribers _dataflow;
        public mLogServiceImpl(IDataflowSubscribers dataflow) { this._dataflow = dataflow; }

        public override Task<mLogResult> writeLog(mLOG request, ServerCallContext context)
        {
            _dataflow.enqueue(new JobLogPrintOut(request)).Wait();
            return Task.FromResult(new mLogResult { Ok = true, Code = 1, MessageText = Guid.NewGuid().ToString() });
        }

        public override Task<mLogTextResult> writeLogText(mLogText request, ServerCallContext context)
        {
            _dataflow.enqueue(new JobLogPrintOut(request.Text)).Wait();
            return Task.FromResult(new mLogTextResult());
        }
    }

    public class LogService
    {
        static Server server;
        public static void Start(IDataflowSubscribers dataflow)
        {
            int PORT_LOG_INPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_INPUT"]);
            server = new Server()
            {
                Services = { svcLogService.BindService(new mLogServiceImpl(dataflow)) },
                Ports = { new ServerPort("localhost", PORT_LOG_INPUT, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public static void Stop()
        {
            server.ShutdownAsync().Wait();
        }
    }
}
