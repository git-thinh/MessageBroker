using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

[assembly: OwinStartup(typeof(MessageBroker.Startup))]

namespace MessageBroker
{
    public class MiddlewareUrlRewriter : OwinMiddleware
    {
        private static readonly PathString ContentVersioningUrlSegments = PathString.FromUriComponent("/content/v");

        public MiddlewareUrlRewriter(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var path = context.Request.Uri.AbsolutePath; 
            if (path == "/api")
            {
                string[] rounters = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                            .Select(x => "/api/" + x.GetRawConstantValue() as string)
                            .ToArray();

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200;
                context.Response.Write(JsonConvert.SerializeObject(rounters));
            }

            await Next.Invoke(context);
        }
    }
    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //--------------------------------------------------------------
            // Web API configuration and services
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            //--------------------------------------------------------------
            //config.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());
            //config.Services.Replace(typeof(IHttpControllerSelector), new ControllersResolver(config));

            app.Use(typeof(MiddlewareUrlRewriter));

            //--------------------------------------------------------------
            // Routing staic
            //////config.Routes.MapHttpRoute(
            //////    name: "DefaultApi",
            //////    routeTemplate: "api/{controller}/{action}",
            //////    defaults: new { action = RouteParameter.Optional }
            //////);
            //////app.UseWebApi(config);  

            //--------------------------------------------------------------
            // Routing dynamic
            //config.MapHttpAttributeRoutes();
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            string[] rounters = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                        .Select(x => x.GetRawConstantValue() as string)
                        .ToArray();
            foreach (string route in rounters)
                config.Routes.MapHttpRoute(
                    name: route,
                    routeTemplate: "api/" + route + "/{action}/{value}",
                    defaults: new { controller = route.Replace("_", string.Empty),
                        action = RouteParameter.Optional,
                        value = RouteParameter.Optional }
                );
            app.UseWebApi(config);

            //--------------------------------------------------------------
            // Webserver static at folder
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
