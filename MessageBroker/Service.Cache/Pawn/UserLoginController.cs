using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class UserLoginController : ApiController
    {
        static readonly ICacheService _cache;
        static void initData()
        {
            //ObjectCache cache = MemoryCache.Default;
            //string key = Guid.NewGuid().ToString();
            //cache.Set(key, new oTaiKhoan[] {
            //    new oTaiKhoan(){ TaiKHoanId="1", MatKhau = "123", TenTaiKhoan="admin", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="XE_OM_CONG_NGHE" },
            //    new oTaiKhoan(){ TaiKHoanId="2", MatKhau = "123", TenTaiKhoan="user", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="ECPAY" },
            //}, new CacheItemPolicy());
            //_cache.insertItemsByCacheKey(key);
        }

        static UserLoginController()
        {
            _cache = _API_CONST.USER_LOGIN.initCacheService();
            initData();
        }

        [AttrApiInfo("Danh sách tất cả tài khoản đăng nhập")]
        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Đăng nhâp tài khoản", Description = "BodyJson: {\"Username\":\"admin\",\"Password\":\"123\"}", Result = "Thành công nếu mảng Result[].length > 0")]
        public oCacheResult post_Login([FromBody]oUserLogin user)
        {
            oCacheResult result = _cache
                .executeReplyCacheKey("Username=\"" + user.Username + "\" And Password=\"" + user.Password + "\"")
                .getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Tìm kiếm tài khoản", Description = "{\"Conditions\":\" Linq.Dynamic clause at here ... \"}")]
        public oCacheResult post_Search([FromBody]oCacheRequest request)
        {
            request.RequestId = Guid.NewGuid().ToString();
            oCacheResult result = _cache
                .executeReplyCacheKey(request.Conditions)
                .getResultByCacheKey();
            result.Request = request;
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
