using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace MessageBroker
{
    public class JobCacheStore : JobBase, IJobCacheStore
    { 
        private static bool _inited = false;
        public JobCacheStore()
        {
            
        }

        ////////////////////////////////////////////////////////////////////
        //

        static T createInstance<T>(params object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }
        static T createInstance<T>(Type type, params object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }

        static void cacheStart()
        {
            if (!_inited)
            {
                _inited = true;
                return;
            }

            int port = (int)getOptions("port");

            ////////string file = @"C:\Projects\MessageBroker\MessageBrokerBuild\CacheDll\CacheEngine.Test.dll";
            //////string file = @"C:\Projects\MessageBroker\CacheEngine.Test\CacheEngine.Test.dll";
            ////////string file = @"D:\Projects\Protobuf\MessageBroker\CacheEngine.Test\CacheEngine.Test.dll";
            //////if (File.Exists(file))
            //////{
            //////    var assembly = Assembly.LoadFile(file);
            //////    var types = assembly.GetTypes();
            //////    //var matchedTypes = types.Where(i => typeof(ICacheService).IsAssignableFrom(i)).ToList();

            //////    var typeService = types.FirstOrDefault(i => i.Name.ToLower() == "test1");
            //////    var typeBehavior = types.FirstOrDefault(i => i.Name.ToLower() == "test1behavior");
            //////    IServiceBehavior instanceBehavior = createInstance<IServiceBehavior>(typeBehavior, Dataflow);

            //////    ServiceHost host = new ServiceHost(typeService, new Uri("http://localhost:" + port + "/test1/"));
            //////    host.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //////    host.Description.Behaviors.Add(instanceBehavior);
            //////    host.Open();

            //////    ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test1/"));
            //////    ICacheService proxy = factory.CreateChannel();
            //////    string res = proxy.execute();
            //////    Console.WriteLine("asset-> {0}", res);

            //////    ((IClientChannel)proxy).Close();
            //////    factory.Close();
            //////    host.Close();
            //////}

            ServiceHost host2 = new ServiceHost(typeof(oUserService), new Uri("http://localhost:" + port + "/test/"));
            host2.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            host2.Description.Behaviors.Add(new oUserBehavior(new oUserService(Dataflow, new oCacheField[] { })));
            host2.Open();

            ChannelFactory<ICacheService> factory2 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test/"));
            ICacheService proxy2 = factory2.CreateChannel();
            string key2 = proxy2.execute("UserName=\"admin\"");

            ObjectCache cache = MemoryCache.Default;
            dynamic[] data = (dynamic[])cache.Get(key2);

            Console.WriteLine("test-> {0}", key2);

            ((IClientChannel)proxy2).Close();
            factory2.Close();
            host2.Close();


            //////ServiceHost host1 = new ServiceHost(typeof(AssetService), new Uri("http://localhost:" + port + "/asset/"));
            //////host1.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //////host1.Description.Behaviors.Add(new AssetBehavior());
            //////host1.Open();

            //////ServiceHost host2 = new ServiceHost(typeof(TestService), new Uri("http://localhost:" + port + "/test/"));
            //////host2.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //////host2.Description.Behaviors.Add(new TestBehavior());
            //////host2.Open();

            //////ChannelFactory<ICacheService> factory1 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/asset/"));
            //////ICacheService proxy1 = factory1.CreateChannel();
            //////string res1 = proxy1.execute();
            //////Console.WriteLine("asset-> {0}", res1);

            //////ChannelFactory<ICacheService> factory2 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test/"));
            //////ICacheService proxy2 = factory2.CreateChannel();
            //////string res2 = proxy2.execute();
            //////Console.WriteLine("test-> {0}", res2);

            //////((IClientChannel)proxy1).Close();
            //////factory1.Close();
            //////host1.Close();

            //////((IClientChannel)proxy2).Close();
            //////factory2.Close();
            //////host2.Close();
        }

        public override void execute()
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

        ////////////////////////////////////////////////////////////////////
        //

        public void serviceRegister(string name, long dateVersion)
        {
            name = name.Trim().ToLower();
            string modelName = string.Format("{0}_{1}", name, dateVersion);

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Models");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string fiJson = Path.Combine(path, modelName + ".json");
            if (!File.Exists(fiJson))
            {
                return;
            }

            oCacheField[] cacheFields = new oCacheField[] { };
            try {
                cacheFields = JsonConvert.DeserializeObject<oCacheField[]>(File.ReadAllText(fiJson));
            } catch {
                return;
            }
            if (cacheFields.Length == 0) {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("namespace Models { public class "+ modelName + " { ");
            foreach (var f in cacheFields) sb.Append(string.Format(" public {0} {1} ", f.type, f.name) + " { get; set; } ");
            sb.Append(" } } ");
            string fiCsharp = Path.Combine(path, modelName + ".cs");
            File.WriteAllText(fiCsharp, sb.ToString());
            string fiDllOutput = Path.Combine(path, modelName + ".dll");

            //string cscPath = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe";
            string cscPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            var cscArguments = string.Format(" /t:library /out:{0} {1}", fiDllOutput, fiCsharp);
            //Process.Start(cscPath, cscArguments);
            var process = new Process() { StartInfo = new ProcessStartInfo() { FileName = cscPath, Arguments = cscArguments } };
            process.Start();
            process.WaitForExit();


            var assembly = Assembly.LoadFile(fiDllOutput);
            var types = assembly.GetTypes();
            var typeModel = types.FirstOrDefault(i => i.Name.ToLower() == modelName);            

            int port = (int)getOptions("port");

            ////////ServiceHost host1 = new ServiceHost(typeof(AssetService), new Uri("http://localhost:" + port + "/asset/"));
            ////////host1.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            ////////host1.Description.Behaviors.Add(new AssetBehavior(typeModel));
            ////////host1.Open();

            //////ServiceHost host2 = new ServiceHost(typeof(TestService), new Uri("http://localhost:" + port + "/test/"));
            //////host2.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //////host2.Description.Behaviors.Add(new TestBehavior(JobBase.Dataflow, cacheFields));
            //////host2.Open();

            ////////ChannelFactory<ICacheService> factory1 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/asset/"));
            ////////ICacheService proxy1 = factory1.CreateChannel();
            ////////string res1 = proxy1.execute();
            ////////Console.WriteLine("asset-> {0}", res1);

            //////ChannelFactory<ICacheService> factory2 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test/"));
            //////ICacheService proxy2 = factory2.CreateChannel();
            //////var aa = new object[] {
            //////    new { id = 1, name = Guid.NewGuid().ToString() },
            //////    new { id = 2, name = Guid.NewGuid().ToString() }
            //////};
            //////bool res21 = proxy2.push(JsonConvert.SerializeObject(aa));
            //////string key2 = proxy2.execute("id = 2");

            //////ObjectCache cache = MemoryCache.Default;
            //////dynamic[] data = (dynamic[])cache.Get(key2);

            //////Console.WriteLine("test-> {0}", key2);

            ////////((IClientChannel)proxy1).Close();
            ////////factory1.Close();
            ////////host1.Close();

            //////((IClientChannel)proxy2).Close();
            //////factory2.Close();
            //////host2.Close();


        }

        public void serviceUnRegister(string name, long dateVersion)
        {
        }
    }

}
