using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MessageBroker
{
    /*
        var q = new DataflowSubscribers();
        q.RegisterHandler<WorkLog>();
        q.RegisterHandler<WorkDatabase>();
        //q.RegisterHandler<WorkLog>(j => SendToPolice(j));
        //q.RegisterHandler<WorkDatabase>(j => SendToFireDpt(j));
        //await q.Enqueue(new WorkLog());
        //await q.Enqueue(new WorkDatabase());
    */

    public interface ICacheFind {
        mCacheReply Find(mCacheRequest request);
    }

    public class DataflowSubscribers: IDataflowSubscribers, ICacheFind
    {
        public mCacheReply Find(mCacheRequest request) {
            return new mCacheReply() { Output = Guid.NewGuid().ToString() };
        }

        private BroadcastBlock<IJob> _jobs;
        private List<IJob> _listFreeResource;

        private static IJobCacheStore _cacheStore;
        public IJobCacheStore CacheStore { get { return _cacheStore; } }

        public object CacheFind { get => (ICacheFind)this; }

        public DataflowSubscribers()
        {
            _jobs = new BroadcastBlock<IJob>(job => job);
            _listFreeResource = new List<IJob>();
        }

        public void RegisterHandler<T>(T obj, Dictionary<string, object> options = null) where T : IJob
        {
            if (obj != null)
            {
                if (obj is IJobCacheStore)
                {
                    _cacheStore = (IJobCacheStore)obj;
                }

                obj.setOptions(options);
                obj.setDataflow(this);
                _listFreeResource.Add(obj);

                // We have to have a wrapper to work with IJob instead of T
                //Action<IJob> actionWrapper = (job) => handleAction((T)job);
                Action<IJob> actionWrapper = (job) =>
                {
                    ((T)job).execute();
                };

                var executionDataflowBlockOptions = new ExecutionDataflowBlockOptions()
                {
                    // execute paranell on 2 core chip (TPL)
                    MaxDegreeOfParallelism = 5,
                };
                // create the action block that executes the handler wrapper
                var actionBlock = new ActionBlock<IJob>((job) => actionWrapper(job), executionDataflowBlockOptions);

                // Link with Predicate - only if a job is of type T
                _jobs.LinkTo(actionBlock, predicate: (job) => job is T);

                obj.execute();
            }
        }

        public void RegisterHandler<T>(Action<T> handleAction) where T : IJob
        {
            // We have to have a wrapper to work with IJob instead of T
            Action<IJob> actionWrapper = (job) => handleAction((T)job);

            var executionDataflowBlockOptions = new ExecutionDataflowBlockOptions()
            {
                // execute paranell on 2 core chip (TPL)
                MaxDegreeOfParallelism = 2,
            };
            // create the action block that executes the handler wrapper
            var actionBlock = new ActionBlock<IJob>((job) => actionWrapper(job), executionDataflowBlockOptions);

            // Link with Predicate - only if a job is of type T
            _jobs.LinkTo(actionBlock, predicate: (job) => job is T);
        }

        public async Task enqueue(IJob job)
        {
            await _jobs.SendAsync(job);
        }

        public async void writeLog(string message)
        {
            await enqueue(new JobLogPrintOut(message));
        }

        public void freeResourceAllJob()
        {
            lock (_listFreeResource)
            {
                foreach (var item in _listFreeResource)
                {
                    item.freeResource();
                }
            }
        }
    }
}
