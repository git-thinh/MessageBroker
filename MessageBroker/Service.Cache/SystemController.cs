using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class SystemController : ApiController
    {
        [AttrApiInfo("Danh sách trạng thái của oCacheResult")]
        public dynamic[] get_code_all()
        {
            var type = typeof(oCacheResultCode);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        string getControllerName(string key)
        {
            string s = key;
            string[] a = s.Split('_').Select(x => x[0].ToString().ToUpper() + x.Substring(1)).ToArray();
            return string.Join(string.Empty, a) + "Controller";
        }

        [AttrApiInfo("Danh sách thông tin các hàm API")]
        public HttpResponseMessage get_api_all()
        {
            List<oApiInfo> ls = new List<oApiInfo>() { };

            var controllers = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                        .Select(x => x.GetRawConstantValue() as string)
                        .Select(x => new { Service = x, Controller = getControllerName(x) })
                        .ToArray();

            foreach (var it in controllers)
            {
                Type type = Type.GetType("MessageBroker." + it.Controller + ", MessageBroker");
                if (type != null)
                {
                    MethodInfo[] props = type.GetMethods();
                    foreach (MethodInfo prop in props)
                    {
                        object[] attrsField = prop.GetCustomAttributes(true);
                        if (attrsField != null && attrsField.Length > 0)
                        {
                            foreach (object attr in attrsField)
                            {
                                AttrApiInfo attrApi = attr as AttrApiInfo;
                                if (attrApi != null)
                                {
                                    string funName = prop.Name;
                                    var fun = new oApiInfo(funName, attrApi.Title)
                                    {
                                        Service = it.Service,
                                        Description = attrApi.Description,
                                        Result = attrApi.Result
                                    };

                                    StringBuilder bi = new StringBuilder();
                                    bi.Append("api/" + it.Service + "/" + funName.ToLower() + "/");

                                    oApiParameterInfo[] parameters = new oApiParameterInfo[] { };
                                    ParameterInfo[] md = prop.GetParameters();
                                    if (md.Length > 0) parameters = md.Select((x, index) => new oApiParameterInfo() { Index = index + 1, Name = x.Name, Type = x.ParameterType.Name }).ToArray();
                                    if (fun.Method == "GET") foreach (var po in parameters) bi.Append("{" + po.Name + "}/");

                                    fun.Parameters = parameters;
                                    fun.Path = bi.ToString();
                                    ls.Add(fun);
                                }
                            }
                        }
                    }
                }
            }

            var gs = ls.GroupBy(x => x.Service).Select(x => new { Service = x.Key, APIs = x.ToArray() });
            string json = JsonConvert.SerializeObject(gs);

            //cache file json
            string file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "get_api_all.json");
            File.WriteAllText(file, JsonConvert.SerializeObject(json));

            return new HttpResponseMessage() { Content = new StringContent(json, Encoding.UTF8, "application/json") };
        }

        string get_modelAttrsJson(string typeName = "")
        {
            oModelInfo model = new oModelInfo();

            Type typeModel = Type.GetType("MessageBroker." + typeName + ", MessageBroker");
            if (typeModel == null) return "{}";

            model.Name = typeModel.Name;

            object[] attrsModel = typeModel.GetCustomAttributes(true);
            foreach (var attr in attrsModel)
            {
                AttrModelInfo attrExt = attr as AttrModelInfo;
                if (attrExt != null)
                {
                    model.Title = attrExt.Title;
                    model.ServiceName = attrExt.ServiceName;
                }
            }

            PropertyInfo[] props = typeModel.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrsField = prop.GetCustomAttributes(true);
                foreach (object attr in attrsField)
                {
                    AttrFieldInfo attrExt = attr as AttrFieldInfo;
                    if (attrExt != null)
                        model.Properties.Add(new oModelFielInfo(prop.Name, attrExt));
                }
            }
            string json = JsonConvert.SerializeObject(model);

            //cache file json
            string file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "schema-" + typeName + ".json");
            File.WriteAllText(file, json);

            return json;
        }

        [AttrApiInfo("Thông tin schema các field của model service", Description = "value = tên model service. Vd: oPawnInfo, oUserLogin...")]
        public HttpResponseMessage get_model_attrs(string value)
        {
            return new HttpResponseMessage() { Content = new StringContent(get_modelAttrsJson(value), Encoding.UTF8, "application/json") };
        }

        [AttrApiInfo("Danh sách các models")]
        public HttpResponseMessage get_models()
        {
            string json = "{}";

            string[] rounters = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                        .Select(x => x.GetRawConstantValue() as string)
                        .ToArray();

            var ts = typeof(SystemController).Assembly.GetTypes()
                .Where(x => x.Name[0] == 'o')
                //.Select(x => x.Name)
                .ToArray();

            List<oModelInfo> ls = new List<oModelInfo>() { };
            foreach (var typeModel in ts) {
                object[] attrsModel = typeModel.GetCustomAttributes(true);
                foreach (var attr in attrsModel)
                {
                    AttrModelInfo attrExt = attr as AttrModelInfo;
                    if (attrExt != null)
                    {
                        oModelInfo model = new oModelInfo();
                        model.Name = typeModel.Name;
                        model.Title = attrExt.Title;
                        model.ServiceName = attrExt.ServiceName;

                        ls.Add(model);
                    }
                }
            }

            json = JsonConvert.SerializeObject(ls);

            return new HttpResponseMessage() { Content = new StringContent(json, Encoding.UTF8, "application/json") };
        }

    }

    public class oApiInfo_GroupService
    {
        public string Service { set; get; }
        public oApiInfo[] APIs { set; get; }

    }

    public class oApiParameterInfo
    {
        public int Index { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
    }

    public class oApiInfo
    {
        public string Service { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Result { set; get; }
        public string Method { set; get; }
        public string Function { set; get; }
        public string Path { set; get; }
        public oApiParameterInfo[] Parameters { set; get; }

        public oApiInfo(string func, string title)
        {
            this.Parameters = new oApiParameterInfo[] { };
            this.Path = string.Empty;
            this.Service = string.Empty;
            this.Description = string.Empty;
            this.Result = string.Empty;
            this.Function = func;
            this.Title = title;

            if (func.StartsWith("get_")) this.Method = "GET";
            else if (func.StartsWith("post_")) this.Method = "POST";
            else if (func.StartsWith("put_")) this.Method = "PUT";
            else if (func.StartsWith("delete_")) this.Method = "DELETE";
            else this.Method = "OPTIONS";
        }
    }

}
