using CacheEngineShared;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Http;

namespace MessageBroker
{
    public class UserLoginService : BaseServiceCache<oUserLogin> { public UserLoginService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class UserLoginBehavior : BaseServiceCacheBehavior { public UserLoginBehavior(object instance) : base(instance) { } }

    public class UserLoginController : BaseController
    {
        static UserLoginController()
        {
            _cache = _API_CONST.USER_LOGIN.initCacheService();
            m_initDataFromDbStore = "user_login_cacheInitData";
        }

        [AttrApiInfo("Đăng nhâp tài khoản", Description = "BodyJson: {\"Username\":\"admin\",\"Password\":\"123\"}", Result = "Thành công nếu mảng Result[].length > 0")]
        public oCacheResult post_Login([FromBody]oUserLogin user)
        {
            oCacheResult result = _cache
                .executeReplyCacheKey("Username=\"" + user.Username + "\" And Password=\"" + user.Password + "\"")
                .getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Thêm mới tài khoản đăng nhập", Description = "BodyJson: {\"UserId\":123456789,\"Username\":\"admin\",\"Password\":\"123\"}")]
        public oCacheResult post_AddNew([FromBody]dtoUserLogin_AddNew item)
        {
            oCacheResult rs = this.sqlExecute<dtoUserLogin_AddNew_Result, dtoUserLogin_AddNew>("user_login_contact_createNew", item);
            if (rs.Ok && rs.Result.Length > 0)
            {
                object obj = rs.Result[0];
                dtoUserLogin_AddNew_Result it = (dtoUserLogin_AddNew_Result)obj;
                if (!string.IsNullOrWhiteSpace(it.ServiceCache)) {
                    this.reloadCacheByServiceNameArray(it.ServiceCache.Split(',').Select(x => x.Trim().ToLower()).ToArray());
                }
            }
            return rs;
        }
    }

    public class dtoUserLogin_AddNew_Result
    {
        public bool Ok { set; get; }
        public string Message { set; get; }
        public string ServiceCache { set; get; }
        public long Contact_ID { set; get; }
        public long User_ID { set; get; }
    }

    public class dtoUserLogin_AddNew
    {
        public string Name { set; get; }
        public string AddressCompany { set; get; }
        public string AddressPlace { set; get; }
        public string Phones { set; get; }
        public string Emails { set; get; }
    }
}
