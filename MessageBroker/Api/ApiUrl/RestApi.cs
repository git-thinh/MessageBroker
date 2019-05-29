using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MessageBroker
{
    public class RestApi
    {

        public static object RestApiGet(string url, string ApiLiqRootUrl = "")
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                if (ApiLiqRootUrl == null || ApiLiqRootUrl == "")
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                }
                else
                {
                    client.BaseAddress = new Uri(ApiLiqRootUrl);
                }

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }

        public static object RestApiGet(string url, Dictionary<string, string> dctParams, string ApiLiqRootUrl = "")
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                if (ApiLiqRootUrl == null || ApiLiqRootUrl == "")
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                }
                else
                {
                    client.BaseAddress = new Uri(ApiLiqRootUrl);
                }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }
        public static object RestApiGet(string url, Dictionary<string, string> dctParams, TimeSpan overrideTimeout)
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (overrideTimeout != null)
                {
                    client.Timeout = overrideTimeout;
                }

                BuildUrlParams(ref url, dctParams);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }

        public static object RestApiDelete(string url, Dictionary<string, string> dctParams)
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var response = client.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }


        public static object RestApiLiqDelete(string url, Dictionary<string, string> dctParams)
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiLiqRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var response = client.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }



        public static object RestApiPost(string url, object obj)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var content = new FormUrlEncodedContent(new[]
                //{
                //     new KeyValuePair<string, string>("Name", "F88 Hội sở")
                //});

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }

        public static object RestApiLiqPost(string url, object obj)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiLiqRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var content = new FormUrlEncodedContent(new[]
                //{
                //     new KeyValuePair<string, string>("Name", "F88 Hội sở")
                //});

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }
        public static object RestApiPost(TimeSpan overrideTimeout, string url, object obj)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromHours(1);
                if (overrideTimeout != null)
                {
                    client.Timeout = overrideTimeout;
                }
                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }
        public static object RestApiPost(TimeSpan overrideTimeout, string url, Dictionary<string, string> dctParams, object obj)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (overrideTimeout != null)
                {
                    client.Timeout = overrideTimeout;
                }
                BuildUrlParams(ref url, dctParams);

                StringContent content = null;
                if (obj != null)
                {
                    var jsonRequest = JsonConvert.SerializeObject(obj);
                    content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
                }

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }
        public static object RestApiPost(string url, Dictionary<string, string> dctParams, object obj = null)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                StringContent content = null;
                if (obj != null)
                {
                    var jsonRequest = JsonConvert.SerializeObject(obj);
                    content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
                }

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }




        public static object RestApiPost(string url, string json)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                //byte[] cred = UTF8Encoding.UTF8.GetBytes("username:password");
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
                                             
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }








        public static object RestApiPost(string url, object obj, string paramName)
        {
            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromHours(1);
                var jsonRequest = JsonConvert.SerializeObject(obj);

                var content = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>(paramName, jsonRequest)
                });

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }

        public static object RestApiPut(string url, Dictionary<string, string> dctParams)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var response = client.PutAsync(url, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }


        public static object ResApiLiqPut(string url, Dictionary<string, string> dctParams, object obj)
        {
            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiLiqRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PutAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }
            return objResponse;
        }

        public static object RestApiPut(string url, Dictionary<string, string> dctParams, object obj)
        {

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                BuildUrlParams(ref url, dctParams);

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PutAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }

        public static object RestApiPostFile(string url, Dictionary<string, string> dctParams)
        {
            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MultipartFormDataContent form = new MultipartFormDataContent();
                foreach (var item in dctParams)
                {
                    FileInfo fileInfo = new FileInfo(item.Value);
                    FileStream stream = fileInfo.OpenRead();

                    var streamContent = new StreamContent(stream);
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");

                    String headerValue = "form-data; name=\"" + item.Key + "\"; filename=\"" + fileInfo.Name + "\"";
                    byte[] bytes = Encoding.UTF8.GetBytes(headerValue);
                    headerValue = "";
                    foreach (byte b in bytes)
                    {
                        headerValue += (Char)b;
                    }
                    streamContent.Headers.Add("Content-Disposition", headerValue);

                    //streamContent.Headers.Add("Content-Disposition", "form-data; name=\"" + item.Key + "\"; filename=\"" + fileInfo.Name + "\"");
                    form.Add(streamContent, "file", fileInfo.Name);
                }

                var response = client.PostAsync(url, form).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }

        private static void BuildUrlParams(ref string url, Dictionary<string, string> dctParams)
        {
            int index = 0;
            if (dctParams != null && dctParams.Count > 0)
            {
                foreach (var item in dctParams)
                {
                    if (index == 0)
                    {
                        url += "?" + item.Key + "=" + item.Value;
                    }
                    else
                    {
                        url += "&" + item.Key + "=" + item.Value;
                    }
                    index += 1;
                }
            }
        }

     

    }
}