using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace MessageBroker
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                //var errors = actionContext.ModelState.Values.Where(v => v.Errors.Count > 0)
                //    .SelectMany(v => v.Errors)
                //    .Select(v => v.ErrorMessage)
                //    .ToList();

                var errors = actionContext.ModelState.Where(v => v.Value.Errors.Count > 0 && v.Value.Errors.Count(x => x.ErrorMessage.Length > 0) > 0)
                    //.SelectMany(v => new { Key = v.Key, Errors = v.Value.Errors })
                    .Select(v => new { Key = v.Key, Messages = v.Value.Errors.Select(x => x.ErrorMessage).ToArray() })
                    //.Select(v => v.ErrorMessage)
                    .ToArray();
                if (errors.Length > 0)
                {
                    var responseObj = new
                    {
                        Ok = false,
                        Message = "Bad Request",
                        Errors = errors
                    };

                    //actionContext.Result = new JsonResult(responseObj)
                    //{
                    //    StatusCode = 400
                    //};
                    HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(responseObj), System.Text.Encoding.UTF8, "application/json");
                    actionContext.Response = response;
                }
            }
        }
    }
}