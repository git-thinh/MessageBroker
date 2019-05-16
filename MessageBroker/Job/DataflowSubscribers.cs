using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MessageBroker
{
    public interface IDataflowSubscribers {
        Task Enqueue(IJob job);
        void freeResourceAllJob();
    }

    /*
        var q = new DataflowSubscribers();
        q.RegisterHandler<WorkLog>();
        q.RegisterHandler<WorkDatabase>();
        //q.RegisterHandler<WorkLog>(j => SendToPolice(j));
        //q.RegisterHandler<WorkDatabase>(j => SendToFireDpt(j));
        //await q.Enqueue(new WorkLog());
        //await q.Enqueue(new WorkDatabase());
    */

    public class DataflowSubscribers: IDataflowSubscribers
    {
        private BroadcastBlock<IJob> _jobs;
        private List<IJob> _listFreeResource;

        public DataflowSubscribers()
        {
            _jobs = new BroadcastBlock<IJob>(job => job);
            _listFreeResource = new List<IJob>();
        }

        public void RegisterHandler<T>() where T : IJob
        {
            // We have to have a wrapper to work with IJob instead of T
            //Action<IJob> actionWrapper = (job) => handleAction((T)job);
            Action<IJob> actionWrapper = (job) => {

                ((T)job).execute();
            };

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

        public void RegisterHandler<T>(T objFreeResource, bool executeFirstTime = false, Dictionary<string, object> options = null) where T : IJob
        {
            if (objFreeResource != null) _listFreeResource.Add(objFreeResource);
            if (options != null) objFreeResource.setOptions(options);
            RegisterHandler<T>();
            if (executeFirstTime) objFreeResource.execute();
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

        public async Task Enqueue(IJob job)
        {
            await _jobs.SendAsync(job);
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
