﻿using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebApiShared;

namespace MessageWebApi
{
    public class table1col3Controller : ApiController
    {
        const int limit = 2000000;
        static CacheSynchronized<int> store = new CacheSynchronized<int>();
        static table1col3Controller()
        {
            for (int i = 0; i < limit; i++) store.Add(i, i);
        }

        void createDynamic()
        {
            //var dataType = new Type[] { typeof(string) };
            //var genericBase = typeof(List<>);
            //var combinedType = genericBase.MakeGenericType(dataType);
            //var listStringInstance = Activator.CreateInstance(combinedType,);
            //var addMethod = listStringInstance.GetType().GetMethod("Add");
            //addMethod.Invoke(listStringInstance, new object[] { "Hello World" });

            //var a = (new int[] { 1, 2, 3 }).AsQueryable().Where("it > 2").ToArray();  //.("@0.Contains(\"de\")");
            //var a2 = (listStringInstance as List<string>).AsQueryable().Where("it.Contains(\"rl9\")").ToArray();

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
        public string Get(string id)
        { 
            return Guid.NewGuid().ToString();
        }

        // POST api/values
        public IEnumerable<int> Post([FromBody]CacheRequestMessage value)
        {
            int[] a = store.SearchDynamic(value.Condition);
            return a; 
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