using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheEngineShared
{
    public interface IJobCacheStore
    { 
        void serviceRegister(string name, long dateVersion);
        void serviceUnRegister(string name, long dateVersion);
    }
}
