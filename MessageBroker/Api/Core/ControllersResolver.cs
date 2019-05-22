using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace MessageBroker
{
    public class ControllersResolver : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private static string _pathRootApiDll = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ApiDll");

        public ControllersResolver(HttpConfiguration configuration) : base(configuration)
        {
            if (Directory.Exists(_pathRootApiDll)) Directory.CreateDirectory(_pathRootApiDll);
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //string controllerName = base.GetControllerName(request);

            string controllerName = request.RequestUri.Segments[2];
            if (controllerName.EndsWith("/")) controllerName = controllerName.Substring(0, controllerName.Length - 1);

            string file = Path.Combine(_pathRootApiDll, "WebApiControllers." + controllerName + ".dll");
            if (File.Exists(file))
            {

                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes(); //GetExportedTypes doesn't work with dynamic assemblies
                var matchedTypes = types.Where(i => typeof(IHttpController).IsAssignableFrom(i)).ToList();

                var matchedController = matchedTypes.FirstOrDefault(i => i.Name.ToLower() == controllerName.ToLower() + "controller");

                HttpControllerDescriptor http = new HttpControllerDescriptor(_configuration, controllerName, matchedController);

                return http;
            }

            return null;
        }
    }























    //public interface AssembliesResolver
    //{
    //    ICollection<Assembly> GetAssemblies();
    //}

    //public class MyAssembliesResolver : DefaultAssembliesResolver
    //{
    //    public override ICollection<Assembly> GetAssemblies()
    //    {
    //        ICollection<Assembly> baseAssemblies = base.GetAssemblies();
    //        List<Assembly> assemblies = new List<Assembly>(baseAssemblies);
    //        var controllersAssembly = Assembly.LoadFrom("c:/myAssymbly.dll");
    //        baseAssemblies.Add(controllersAssembly);
    //        return assemblies;
    //    }
    //}

    ////public class ControllersResolver : DefaultAssembliesResolver
    ////{
    ////    public override ICollection<Assembly> GetAssemblies()
    ////    {
    ////        ICollection<Assembly> baseAssemblies = base.GetAssemblies();
    ////        List<Assembly> assemblies = new List<Assembly>(baseAssemblies);

    ////        string thirdPartySource = "C:\\Research\\ExCoWebApi\\ExternalControllers";

    ////        if (!string.IsNullOrWhiteSpace(thirdPartySource))
    ////        {
    ////            if (Directory.Exists(thirdPartySource))
    ////            {
    ////                foreach (var file in Directory.GetFiles(thirdPartySource, "*.*", SearchOption.AllDirectories))
    ////                {
    ////                    if (Path.GetExtension(file) == ".dll")
    ////                    {
    ////                        var externalAssembly = Assembly.LoadFrom(file);

    ////                        baseAssemblies.Add(externalAssembly);
    ////                    }
    ////                }
    ////            }
    ////        }
    ////        return baseAssemblies;
    ////    }
    ////}

}