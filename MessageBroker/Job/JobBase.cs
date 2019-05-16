using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class JobBase
    {
        protected static Dictionary<string, object> _options;
        public static object getOptions(string key)
        {
            if (_options != null && _options.ContainsKey(key)) return _options[key];
            return null;
        }
    }
}
