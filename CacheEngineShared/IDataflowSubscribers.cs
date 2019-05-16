using System.Threading.Tasks;

namespace CacheEngineShared
{
    public interface IDataflowSubscribers
    {
        Task Enqueue(IJob job);
        void freeResourceAllJob();
    }
}
