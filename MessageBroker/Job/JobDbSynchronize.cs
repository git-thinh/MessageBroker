using MessageShared;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MessageBroker
{
    public class JobDbSynchronize : IJob
    {
        private readonly string _textSynchronize;
        public JobDbSynchronize(string textSynchronize)
        {
            this._textSynchronize = textSynchronize;
        }
                 
        public void execute()
        {
            using (var client = new HttpClient())
            {
                string url = "api/cusid";
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var jsonRequest = JsonConvert.SerializeObject(_request);
                //var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
                var content = new StringContent(_textSynchronize, Encoding.UTF8, "text/plain");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    //string responseString = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
