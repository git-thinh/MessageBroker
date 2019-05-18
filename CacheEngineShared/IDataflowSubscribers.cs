using System.Threading.Tasks;

namespace CacheEngineShared
{
    public interface IDataflowSubscribers
    {
        object CacheFind { get; }
        void writeLog(string message);
        Task enqueue(IJob job);
        void freeResourceAllJob();
    }
}
