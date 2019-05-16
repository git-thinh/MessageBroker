using CacheEngineShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace MessageBroker
{
    public class JobCacheStore : JobBase, IJob
    {
        public void setOptions(Dictionary<string, object> options) => _options = options;
        public void freeResource() { }

        ////////////////////////////////////////////////////////////////////
        ///

        private static bool _inited = false;
        //private readonly mUpdateRequest _request;
        //public JobCacheStore(mUpdateRequest request)
        //{
        //    _request = request;
        //}

        ////////////////////////////////////////////////////////////////////
        ///

        static void cacheStart()
        {
            int port = (int)getOptions("port");

            //string file = @"C:\Projects\MessageBroker\MessageBrokerBuild\CacheDll\CacheEngine.Test.dll";
            string file = @"C:\Projects\MessageBroker\CacheEngine.Test\CacheEngine.Test.dll";
            if (File.Exists(file))
            {
                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes(); 
                var matchedTypes = types.Where(i => typeof(ICacheFind).IsAssignableFrom(i)).ToList();
                var typeService = matchedTypes.FirstOrDefault(i => i.Name.ToLower() == "test1");

                var typeBehavior = types.FirstOrDefault(i => i.Name.ToLower() == "test1behavior");
                 
                IServiceBehavior instanceBehavior = (IServiceBehavior)Activator.CreateInstance(typeBehavior);

                ServiceHost host = new ServiceHost(typeService, new Uri("http://localhost:" + port + "/test1/"));
                host.AddServiceEndpoint(typeof(ICacheFind), new BasicHttpBinding(), "");
                host.Description.Behaviors.Add(instanceBehavior);
                host.Open();

                ChannelFactory<ICacheFind> factory = new ChannelFactory<ICacheFind>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test1/"));
                ICacheFind proxy = factory.CreateChannel();
                string res = proxy.execute();
                Console.WriteLine("asset-> {0}", res);
            }



            ServiceHost host1 = new ServiceHost(typeof(AssetService), new Uri("http://localhost:" + port + "/asset/"));
            host1.AddServiceEndpoint(typeof(ICacheFind), new BasicHttpBinding(), "");
            host1.Description.Behaviors.Add(new AssetBehavior());
            host1.Open();

            ServiceHost host2 = new ServiceHost(typeof(TestService), new Uri("http://localhost:" + port + "/test/"));
            host2.AddServiceEndpoint(typeof(ICacheFind), new BasicHttpBinding(), "");
            host2.Description.Behaviors.Add(new TestBehavior());
            host2.Open();

            ChannelFactory<ICacheFind> factory1 = new ChannelFactory<ICacheFind>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/asset/"));
            ICacheFind proxy1 = factory1.CreateChannel();
            string res1 = proxy1.execute();
            Console.WriteLine("asset-> {0}", res1);

            ChannelFactory<ICacheFind> factory2 = new ChannelFactory<ICacheFind>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test/"));
            ICacheFind proxy2 = factory2.CreateChannel();
            string res2 = proxy2.execute();
            Console.WriteLine("test-> {0}", res2);

            ((IClientChannel)proxy1).Close();
            factory1.Close();
            host1.Close();

            ((IClientChannel)proxy2).Close();
            factory2.Close();
            host2.Close();
        }

        public void execute()
        {
            if (!_inited)
            {
                _inited = true;
                cacheStart();
                return;
            }

            //if (_request == null) return;
            //MESSAGE_TYPE type = (MESSAGE_TYPE)_request.Type;
            //switch (type)
            //{
            //    case MESSAGE_TYPE.CACHE_UPDATE_ADD:
            //    case MESSAGE_TYPE.CACHE_UPDATE_DELETE:
            //    case MESSAGE_TYPE.CACHE_UPDATE_EDIT:
            //        using (var client = new HttpClient())
            //        {
            //            string url = "api/cusid";
            //            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
            //            client.DefaultRequestHeaders.Accept.Clear();
            //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //            var jsonRequest = JsonConvert.SerializeObject(_request);
            //            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            //            var response = client.PostAsync(url, content).Result;
            //            if (response.IsSuccessStatusCode)
            //            {
            //                //string responseString = response.Content.ReadAsStringAsync().Result;
            //            }
            //        }
            //        break;
            //    case MESSAGE_TYPE.CACHE_SETUP:
            //        break;
            //    case MESSAGE_TYPE.CACHE_WEBAPI_REGISTER: 
            //        break; 
            //    default:
            //        break;
            //}

        }
    }
}
