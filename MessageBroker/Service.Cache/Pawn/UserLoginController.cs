using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class UserLoginController : BaseController
    {
        static UserLoginController() {
            _cache = _API_CONST.USER_LOGIN.initCacheService();
            m_initDataFromDbStore = "[dbo].[mobi_user_login_cacheInitData]";
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
        public oCacheResult post_AddNew([FromBody]oUserLogin item)
        {
            item.UserId =long.Parse(DateTime.Now.ToString("yyMMddHHmmssfff"));

            if (string.IsNullOrWhiteSpace(item.Username))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("Username is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.Password))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("Password is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.CustomerGroup))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("CustomerGroup is NULL or empty");

            oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
            return result;
        }
    }
}
