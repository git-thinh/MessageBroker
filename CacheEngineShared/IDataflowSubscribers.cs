using System.Threading.Tasks;

namespace CacheEngineShared
{
    public interface IDataflowSubscribers
    {
        void writeLog(string message);
        Task enqueue(IJob job);
        void freeResourceAllJob();
    }
}
