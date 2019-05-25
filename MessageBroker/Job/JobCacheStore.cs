using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace MessageBroker
{
    public class JobCacheStore : JobBase, IJobCacheStore
    {
        private static bool _inited = false;
        ////////////////////////////////////////////////////////////////////
        //

        static string getServiceName(string key)
        {
            string s = key;
            string[] a = s.Split('_').Select(x => x[0].ToString().ToUpper() + x.Substring(1)).ToArray();
            return string.Join(string.Empty, a);
        }

        static void cacheStart()
        {
            int port = (int)getOptions("port");

            string[] rounters = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                        .Select(x => x.GetRawConstantValue() as string)
                        .ToArray();
            for (int i = 0; i < rounters.Length; i++)
            {
                string serviceName = getServiceName(rounters[i]);

                Type typeService = Type.GetType("MessageBroker." + serviceName + "Service, MessageBroker");
                if (typeService == null) continue;

                Type typeBehavior = Type.GetType("MessageBroker." + serviceName + "Behavior, MessageBroker");
                if (typeBehavior == null) continue;

                object instanceService = Activator.CreateInstance(typeService, Dataflow, new oCacheModel());
                IServiceBehavior instanceBehavior = Activator.CreateInstance(typeBehavior, instanceService) as IServiceBehavior; 

                ServiceHost host = new ServiceHost(typeService, new Uri("http://localhost:" + port + "/"+ rounters[i] + "/"));
                host.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
                host.Description.Behaviors.Add(instanceBehavior);
                host.Open();
            }

            //ServiceHost userService = new ServiceHost(typeof(UserLoginService), new Uri("http://localhost:" + port + "/" + _API_CONST.USER_LOGIN + "/"));
            //userService.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //userService.Description.Behaviors.Add(new UserLoginBehavior(new UserLoginService(Dataflow, new oCacheModel())));
            //userService.Open();

            //ServiceHost taohopdongService = new ServiceHost(typeof(PawnInfoService), new Uri("http://localhost:" + port + "/" + _API_CONST.PAWN_INFO + "/"));
            //taohopdongService.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            //taohopdongService.Description.Behaviors.Add(new PawnInfoBehavior(new PawnInfoService(Dataflow, new oCacheModel())));
            //taohopdongService.Open();
        }

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                cacheStart();
                return;
            }


        }

        ////////////////////////////////////////////////////////////////////
        //

        public void serviceRegister(string name, long dateVersion)
        {
        }

        public void serviceUnRegister(string name, long dateVersion)
        {
        }

        ////////////////////////////////////////////////////////////////////
        //
        #region [ TEST ]

        static T createInstance<T>(params object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }
        static T createInstance<T>(Type type, params object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }

        static void test_1()
        {
            int port = (int)getOptions("port");

            ServiceHost host2 = new ServiceHost(typeof(UserLoginService), new Uri("http://localhost:" + port + "/test/"));
            host2.AddServiceEndpoint(typeof(ICacheService), new BasicHttpBinding(), "");
            host2.Description.Behaviors.Add(new UserLoginBehavior(new UserLoginService(Dataflow, new oCacheModel())));
            host2.Open();

            ChannelFactory<ICacheService> factory2 = new ChannelFactory<ICacheService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:" + port + "/test/"));
            ICacheService proxy2 = factory2.CreateChannel();
            oCacheResult rs2 = proxy2.executeConditonsReplyCacheKey("UserName=\"admin\"").getResultByCacheKey();

            rs2.clearCacheIfExist();

            Console.WriteLine("test-> {0}", rs2.ToJson());

            ((IClientChannel)proxy2).Close();
            factory2.Close();
            host2.Close();
        }

        static void test_dll_1()
        {
            string name = "test";
            long dateVersion = 20190517;

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
            try
            {
                cacheFields = JsonConvert.DeserializeObject<oCacheField[]>(File.ReadAllText(fiJson));
            }
            catch
            {
                return;
            }
            if (cacheFields.Length == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("namespace Models { public class " + modelName + " { ");
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

        static void test_2()
        {
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

        #endregion
    }

}
