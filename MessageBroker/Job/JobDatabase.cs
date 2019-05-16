using MessageShared;
using System.Configuration;
using System.Data.SqlClient;

namespace MessageBroker
{
    public class JobDatabase : IJob
    {
        private readonly mUpdateRequest _request;
        public JobDatabase(mUpdateRequest request)
        {
            _request = request;
        }

        static SqlConnection _connect = null;

        static string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["DB_UPDATE_DATA"].ConnectionString.ToString();
        }

        static JobDatabase()
        {
        }


        public void execute()
        {
            //if (_connect == null)
            //{
            //    _connect = new SqlConnection(GetConnString());
            //    _connect.Open();
            //}

            //SqlCommand cmd = new SqlCommand(); 
            //foreach (var kv in this._request.Arguments.Pairs)
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
