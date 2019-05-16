using System.Collections;
using System.Collections.Generic;

namespace CacheEngineShared
{
    public interface IJob
    {
        void setDataflow(IDataflowSubscribers dataflow);
        void setOptions(Dictionary<string, object> options);
        void execute();
        void freeResource();
    }
}
