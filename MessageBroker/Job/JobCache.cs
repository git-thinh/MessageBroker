using Grpc.Core;
using MessageShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class JobCache : IJob
    {
        private readonly mRequest _request;

        public JobCache(mRequest request)
        {
            _request = request;
        }

        public void execute()
        {
            if (_request == null) return;
            MESSAGE_TYPE type = (MESSAGE_TYPE)_request.Type;
            switch (type)
            {
                case MESSAGE_TYPE.CACHE_UPDATE_ADD:
                case MESSAGE_TYPE.CACHE_UPDATE_DELETE:
                case MESSAGE_TYPE.CACHE_UPDATE_EDIT:
                    using (var client = new HttpClient())
                    {
                        string url = "api/cusid";
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var jsonRequest = JsonConvert.SerializeObject(_request);
                        var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                        var response = client.PostAsync(url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            //string responseString = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    break;
                case MESSAGE_TYPE.CACHE_SETUP:
                    break;
                case MESSAGE_TYPE.CACHE_WEBAPI_REGISTER: 
                    break; 
                default:
                    break;
            }

        }
    }
}
