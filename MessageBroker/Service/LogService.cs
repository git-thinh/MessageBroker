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
    public static class LogExt{
        public static oLOG convertLog(this mLOG l){
            return new oLOG()
            {
                Action = (oLOG_ACTION)l.Action,
                ClassName = l.ClassName,
                FunctionName = l.FunctionName,
                Id = l.Id,
                MessageText = l.MessageText,
                Method = l.Method,
                ParaJson = l.ParaJson,
                ProjectName = l.ProjectName,
                Scope = (oLOG_SCOPE)l.Scope,
                State = (oLOG_STATE)l.State,
                TimeEnd = l.TimeEnd,
                TimeStart = l.TimeStart,
                Url = l.Url
            };
        }
    }

    public class mLogServiceImpl : svcLogService.svcLogServiceBase
    {
        private readonly IDataflowSubscribers _dataflow;
        public mLogServiceImpl(IDataflowSubscribers dataflow) { this._dataflow = dataflow; }

        public override Task<mLogResult> writeLog(mLOG request, ServerCallContext context)
        {
            _dataflow.enqueue(new JobLogPrintOut(request.convertLog())).Wait();
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
            string HOST_LOG_INPUT = ConfigurationManager.AppSettings["HOST_LOG_INPUT"];
            int PORT_LOG_INPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_INPUT"]);
            server = new Server()
            {
                Services = { svcLogService.BindService(new mLogServiceImpl(dataflow)) },
                Ports = { new ServerPort(HOST_LOG_INPUT, PORT_LOG_INPUT, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public static void Stop()
        {
            server.ShutdownAsync().Wait();
        }
    }
}
