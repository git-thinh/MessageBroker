using CacheEngineShared;
using FluentValidation;
using FluentValidation.Attributes;
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
            m_initDataFromDbStore = _DB_STORE_USER_LOGIN._cacheInitData;
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
        public oCacheResult post_AddNew([FromBody]dtoUserLogin_addNew item)
        {
            if (item == null) return new oCacheResult().ToFailConvertJson("Please check format string json of input.");

            oCacheResult rs = this.sqlExecute<dtoUserLogin_addNewResult, dtoUserLogin_addNew>(_DB_STORE_USER_LOGIN._addNew, item);
            rs.Request = null;
            if (rs.Ok && rs.Result.Length > 0)
            {
                object obj = rs.Result[0];
                dtoUserLogin_addNewResult it = (dtoUserLogin_addNewResult)obj;
                if (it.Ok)
                {
                    if (!string.IsNullOrWhiteSpace(it.ServiceCache))
                    {
                        this.reloadCacheByServiceNameArray(it.ServiceCache.Split(',').Select(x => x.Trim().ToLower()).ToArray());
                    }
                }
                else {
                    rs.Ok = false;
                    rs.Message = it.Message;
                    rs.Result = new dynamic[] { };
                }
            }
            return rs;
            //return new oCacheResult() { Result = new dynamic[] { item } };
        }
    }

    public class _DB_STORE_USER_LOGIN
    {
        public const string _cacheInitData = "user_login_cacheInitData";
        public const string _addNew = "user_login_contact_createNew"; 
    }

    public class dtoUserLogin_addNewResult
    {
        public bool Ok { set; get; }
        public string Message { set; get; }
        public string ServiceCache { set; get; }
        public long Contact_ID { set; get; }
        public long User_ID { set; get; }
    }

    [Validator(typeof(dtoUserLogin_addNewValidator))]
    public class dtoUserLogin_addNew
    {
        public string Name { set; get; }
        public string AddressCompany { set; get; }
        public string AddressPlace { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int GroupType_ID { set; get; }
    }

    public class dtoUserLogin_addNewValidator : AbstractValidator<dtoUserLogin_addNew>
    {
        //Simple validator that checks for values in First and Lastname
        public dtoUserLogin_addNewValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("Vui lòng nhập họ và tên"); 
            RuleFor(r => r.AddressCompany).NotEmpty().WithMessage("Vui lòng nhập địa chỉ nơi công tác");
            RuleFor(r => r.AddressPlace).NotEmpty().WithMessage("Vui lòng nhập địa chỉ nơi sống");
            RuleFor(r => r.Phone).NotEmpty().WithMessage("Vui lòng nhập số điện thoại");
            RuleFor(x => x.Phone).Must(_VALID.beAValidPhone).WithMessage("Vui lòng nhập đúng số điện thoại");
            RuleFor(r => r.Email).NotEmpty().WithMessage("Vui lòng nhập địa chỉ hộp thư");
            RuleFor(x => x.Email).Must(_VALID.beAValidEmail).WithMessage("Vui lòng nhập đúng hộp thư");
            //RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
            //RuleFor(x => x.Address).Length(20, 250);
        }
    }
}
