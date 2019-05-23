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
        public oCacheResult post_AddNew([FromBody]dtoUserLogin_addNew item)
        {
            
            //oCacheResult rs = this.sqlExecute<dtoUserLogin_addNewResult, dtoUserLogin_addNew>("user_login_contact_createNew", item);
            //if (rs.Ok && rs.Result.Length > 0)
            //{
            //    object obj = rs.Result[0];
            //    dtoUserLogin_addNewResult it = (dtoUserLogin_addNewResult)obj;
            //    if (!string.IsNullOrWhiteSpace(it.ServiceCache)) {
            //        this.reloadCacheByServiceNameArray(it.ServiceCache.Split(',').Select(x => x.Trim().ToLower()).ToArray());
            //    }
            //}
            //return rs;
            return new oCacheResult() { Result = new dynamic[] { item } };
        }
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
        public string Phones { set; get; }
        public string Emails { set; get; }
    }

    public class dtoUserLogin_addNewValidator : AbstractValidator<dtoUserLogin_addNew>
    {
        //Simple validator that checks for values in First and Lastname
        public dtoUserLogin_addNewValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("Vui lòng nhập họ và tên");
            RuleFor(r => r.Name).Length(9, 250).WithMessage("Vui lòng nhập họ và tên > 9");
            RuleFor(r => r.AddressCompany).NotEmpty().WithMessage("Vui lòng nhập địa chỉ nơi công tác");
            RuleFor(r => r.AddressPlace).NotEmpty().WithMessage("Vui lòng nhập địa chỉ nơi sống");
            RuleFor(r => r.Phones).NotEmpty().WithMessage("Vui lòng nhập số điện thoại");
            RuleFor(r => r.Emails).NotEmpty().WithMessage("Vui lòng nhập địa chỉ hộp thư");
            //RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
            //RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
            //RuleFor(x => x.Address).Length(20, 250);
        }
        private bool BeAValidPostcode(string postcode)
        {
            return false;
        }
    }
}
