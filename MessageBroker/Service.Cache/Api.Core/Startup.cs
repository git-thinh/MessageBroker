using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using System.Linq;
using System.Threading;
using System.Web.Http;

[assembly: OwinStartup(typeof(MessageBroker.Startup))]

namespace MessageBroker
{
    public interface IOwinContextAccessor
    {
        IOwinContext CurrentContext { get; }
    }

    public class CallContextOwinContextAccessor : IOwinContextAccessor
    {
        public static AsyncLocal<IOwinContext> OwinContext = new AsyncLocal<IOwinContext>();
        public IOwinContext CurrentContext => OwinContext.Value;
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API configuration and services
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            //config.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());
            //config.Services.Replace(typeof(IHttpControllerSelector), new ControllersResolver(config));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);

            //var physicalFileSystem = new PhysicalFileSystem(@"./");
            var physicalFileSystem = new PhysicalFileSystem(@"../MessageUI");
            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[]
            {
                "index.html"
            };

            app.UseFileServer(options);

        }
    }
}
