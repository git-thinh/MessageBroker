using System.Collections;
using System.Collections.Generic;

namespace MessageBroker
{
    public interface IJob
    {
        void setOptions(Dictionary<string, object> options);
        void execute();
        void freeResource();
    }
}
