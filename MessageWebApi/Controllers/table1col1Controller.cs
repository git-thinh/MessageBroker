using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.WebSockets;
using System.Web.Http;
using CacheEngineShared;
using MessageShared;
using WebApiShared;

namespace MessageWebApi
{
    public class table1col1Controller : ApiController
    {
        static CacheSynchronized<string> store = new CacheSynchronized<string>();

        static readonly mLogService _log;
        static readonly mCacheService _cache;
        static readonly ClientWebSocket _socket;

        static table1col1Controller()
        {
            //for (int i = 0; i < limit; i++) store.Add(i, Guid.NewGuid().ToString());

            try {
                _socket = SocketProvider.init("ws://localhost:56049/message");
                _socket.Send(Guid.NewGuid().ToString());
            }
            catch { }

            try
            {
                _log = LogProvider.init("localhost", 50051);
                _log.Write(Guid.NewGuid().ToString());
            }
            catch { }

            try
            {
                _cache = CacheFindProvider.init("localhost", 50052);
                mCacheReply response = _cache.Find(new oCacheFind() {  });
            }
            catch { }
        }

        void createDynamic()
        {
            var dataType = new Type[] { typeof(string) };
            var genericBase = typeof(List<>);
            var combinedType = genericBase.MakeGenericType(dataType);
            var listStringInstance = Activator.CreateInstance(combinedType);
            var addMethod = listStringInstance.GetType().GetMethod("Add");
            addMethod.Invoke(listStringInstance, new object[] { "Hello World" });

            var a = (new int[] { 1, 2, 3 }).AsQueryable().Where("it > 2").ToArray();  //.("@0.Contains(\"de\")");
            var a2 = (listStringInstance as List<string>).AsQueryable().Where("it.Contains(\"rl9\")").ToArray();

            object x = new Int32[7];
            Type t = x.GetType();
            object y = Array.CreateInstance(t.GetElementType(), 7);

            Type t2 = typeof(int);
            object y2 = Array.CreateInstance(t2, 7);

            Type t3 = typeof(string);
            object y3 = Array.CreateInstance(t3, 7);
            for (int i = 0; i < 7; i++) (y3 as string[])[i] = Guid.NewGuid().ToString();
        }

        // GET api/values
        public IEnumerable<int> Get()
        {            
            return new int[] { 0, 1 };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "id = " + id;
        }

        // POST api/values
        public IEnumerable<int> Post([FromBody]CacheRequestMessage value)
        {
            //int[] a = store.Search(x => x.Contains("abc"));

            //////createDynamic();

            _log.Write(Guid.NewGuid().ToString());

            return new int[] { 3, 4, 5, 6 };
        }

        // PUT api/values/5
        public string Put(int id, [FromBody]string value)
        {
            return "id = " + id + "; value = " + value;
        }

        // DELETE api/values/5
        public string Delete(int id)
        {
            return "id = " + id;
        }
    }
}