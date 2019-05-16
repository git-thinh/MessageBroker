using CacheEngineShared;
using System.Collections.Generic;

namespace MessageBroker
{
    public class JobSyncDbToCache : JobBase
    {
        private static bool _inited = false;
        private readonly string _message;
        public JobSyncDbToCache(string message = null)
        {
            this._message = message;
        }
                 
        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                return;
            }

            //using (var client = new HttpClient())
            //{
            //    string url = "api/cusid";
            //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    //var jsonRequest = JsonConvert.SerializeObject(_request);
            //    //var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            //    var content = new StringContent(_textSynchronize, Encoding.UTF8, "text/plain");

            //    var response = client.PostAsync(url, content).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        //string responseString = response.Content.ReadAsStringAsync().Result;
            //    }
            //}
        }
    }
}
