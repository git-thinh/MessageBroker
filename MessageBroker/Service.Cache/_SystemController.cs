using CacheEngineShared;
using System;
using System.Linq;
using System.Web.Http;

namespace MessageBroker
{
    public class SystemController : ApiController
    {
        public dynamic[] get_codeAll()
        {
            var type = typeof(oCacheResultCode);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        } 
    }
}
