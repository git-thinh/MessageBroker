using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.ServiceModel.Activation;
using System.Web.Http;
using System.Web.Http.Dispatcher;

[assembly: OwinStartup(typeof(MessageBroker.Startup))]

namespace MessageBroker
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            // Note: I prefer my routes to be "api/{controller}/{action}" instead of "api/{controller}/{id}"
            HttpConfiguration config = new HttpConfiguration();

            //// Web API configuration and services
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API configuration and services
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            ////////    //config.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());
            config.Services.Replace(typeof(IHttpControllerSelector), new ControllersResolver(config));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);
        }
    }
}
