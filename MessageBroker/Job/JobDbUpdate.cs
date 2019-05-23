using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MessageBroker
{
    public class JobDbUpdate : JobBase
    {
        public override void freeResource() { dbStop(); }

        ////////////////////////////////////////////////////////////////////
        /// 

        private static bool _inited = false;
        private readonly mDbUpdateRequest _requestUpdateDb;
        public JobDbUpdate(mDbUpdateRequest requestUpdateDb = null)
        {
            _requestUpdateDb = requestUpdateDb;
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        static SqlConnection _connect;
        static void dbStart()
        {
            try
            { 
                _connect = new SqlConnection(_DB_CONST.get_connectString_Mobility());
                //_connect.Open();
            }
            catch(Exception ex) {
                Dataflow.enqueue(new JobLogPrintOut(ex.Message)).Wait();
            }
        }

        static void dbStop() {
            if (_connect != null
                && _connect.State != System.Data.ConnectionState.Closed
                && _connect.State != System.Data.ConnectionState.Broken)
                _connect.Close();
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                dbStart();
                return;
            }

            if (_requestUpdateDb == null) return;
            MESSAGE_TYPE type = (MESSAGE_TYPE)_requestUpdateDb.Type;
            switch (type)
            {
                case MESSAGE_TYPE.CACHE_UPDATE_ADD:
                case MESSAGE_TYPE.CACHE_UPDATE_DELETE:
                case MESSAGE_TYPE.CACHE_UPDATE_EDIT:
                    //using (var client = new HttpClient())
                    //{
                    //    string url = "api/cusid";
                    //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //    var jsonRequest = JsonConvert.SerializeObject(_request);
                    //    var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                    //    var response = client.PostAsync(url, content).Result;
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        //string responseString = response.Content.ReadAsStringAsync().Result;
                    //    }
                    //}
                    break;
                case MESSAGE_TYPE.CACHE_SETUP:
                    break;
                case MESSAGE_TYPE.CACHE_WEBAPI_REGISTER:
                    break;
                default:
                    break;
            }

            //SqlCommand cmd = new SqlCommand();
            //foreach (var kv in this._request..Pairs)
            //    cmd.Parameters.AddWithValue("@" + kv.Key, kv.Value);

            //cmd.Connection = _connect;
            //cmd.CommandText = _request.Message;
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.ExecuteNonQuery();
             
        }

        void test1()
        {
            //var param = new DynamicParameters();
            //param.Add("@UserName", 1);
            //param.Add("@Password", 2);
            ////param.Add("@Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            ////oUserInfo uinfo;



            ////var count = connection.Execute(@"insert MyTable(colA, colB) values (@a, @b)", new[] { new { a = 1, b = 1 }, new { a = 2, b = 2 }, new { a = 3, b = 3 } });
            ////Assert.Equal(3, count); // 3 rows inserted: "1,1", "2,2" and "3,3"

            //using (IDbConnection conn = new SqlConnection(GetConnString()))
            //{
            //    var multi = SqlMapper.QueryMultiple(conn, "mobi_userLogin", param, commandType: CommandType.StoredProcedure);
            //    //count = param.Get<int>("@Count");

            //    //try
            //    //{
            //    //    uinfo = multi.Read<oUserInfo>().SingleOrDefault();
            //    //    uinfo.ShopList = multi.Read<oUserShop>().ToArray();
            //    //    if (uinfo.ShopList == null) uinfo.ShopList = new oUserShop[] { };

            //    //    var userToken = new oUserToken
            //    //    {
            //    //        Username = uinfo.UserName,
            //    //        Timestamp = ToUnixTimestamp(DateTimeOffset.Now)
            //    //    };
            //    //    uinfo.Token = CreateToken(userToken, SecretKey);
            //    //    rawJson = JsonConvert.SerializeObject(new { Ok = true, Data = uinfo });
            //    //}
            //    //catch (Exception ex1)
            //    //{
            //    //    rawJson = JsonConvert.SerializeObject(new { Ok = false, Code = RESPONSE_STATE.FAIL_EXCEPTION, Data = string.Empty, Message = "ERROR1: " + ex1.Message });
            //    //}

            //    conn.Close();
            //}
        }

    }
}
