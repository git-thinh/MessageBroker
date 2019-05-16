using CacheEngineShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class JobBase : IJob
    {
        private static IDataflowSubscribers _dataflow;
        private static Dictionary<string, object> _options;

        public static IDataflowSubscribers Dataflow { get { return _dataflow; } }
        public static Dictionary<string, object> Options { get { return _options; } }

        protected static object getOptions(string key)
        {
            if (_options != null && _options.ContainsKey(key)) return _options[key];
            return null;
        }

        public void setDataflow(IDataflowSubscribers dataflow) => _dataflow = dataflow;
        public void setOptions(Dictionary<string, object> options) => _options = options;

        public virtual void freeResource() { }
        public virtual void execute() { }
    }
}
