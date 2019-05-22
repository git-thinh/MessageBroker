using CacheEngineShared;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnInfoService : BaseServiceCache<oPawnInfo> { public PawnInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnInfoBehavior : BaseServiceCacheBehavior { public PawnInfoBehavior(object instance) : base(instance) { } }

    public class _DB_STORE_PAWN_INFO
    {
        public const string _cacheInitData = "pawn_info_cacheInitData";
        public const string _addNew = "pawn_info_createNew";
        public const string _pawnCode_createNew = "pos.GetMaxPawnIndexByShop";
    }

    public class PawnInfoController : BaseController
    {
        static PawnInfoController()
        {
            _cache = _API_CONST.PAWN_INFO.initCacheService();
            m_initDataFromDbStore = _DB_STORE_PAWN_INFO._cacheInitData;
        }

        [AttrApiInfo("Danh sách trạng thái của hợp đồng")]
        public dynamic[] get_state_type()
        {
            var type = typeof(oContractStatus);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        [AttrApiInfo("Tạo mã hợp đồng khi mở hợp đồng mới")]
        public string get_pawnCode_createNew()
        {
            var param = new DynamicParameters();
            //param.Add("@ShopID", 154);// VietPost
            param.Add("@ShopID", 999); // ECPay
            string maxCode;

            using (IDbConnection cnn = new SqlConnection(_DB_CONST.get_connectString_POS()))
            {
                cnn.Open();
                maxCode = cnn.Query<string>(_DB_STORE_PAWN_INFO._pawnCode_createNew, param, null, true, null, CommandType.StoredProcedure).SingleOrDefault();
            }

            int newIndex = 1;
            if (!string.IsNullOrEmpty(maxCode))
            {
                string[] codes = maxCode.Split('/');
                newIndex = Convert.ToInt16(codes.Last()) + 1;
            }

            // Lấy CodeNo dựa vào ShopID = 'DR'
            string codeNo = $"HĐCC/DR/{DateTime.Now.ToString("yyMM")}/{newIndex}";

            return codeNo;
        }

        [AttrApiInfo("Mở hợp đồng mới")]
        public oCacheResult post_AddNew([FromBody]dtoPawnInfo_addNew item)
        {
            oCacheResult rs = this.sqlExecute<dtoPawnInfo_addNewResult, dtoPawnInfo_addNew>(_DB_STORE_PAWN_INFO._addNew, item);
            if (rs.Ok && rs.Result.Length > 0)
            {
                var it = (dtoPawnInfo_addNewResult)rs.Result[0];
                if (!string.IsNullOrWhiteSpace(it.ServiceCache))
                {
                    this.reloadCacheByServiceNameArray(it.ServiceCache.Split(',').Select(x => x.Trim().ToLower()).ToArray());
                }
            }
            return rs;
        }
    }

    public class dtoPawnInfo_addNewResult
    {
        public bool Ok { set; get; }
        public string Message { set; get; }
        public string ServiceCache { set; get; }
        public long Contact_ID { set; get; }
        public long User_ID { set; get; }
    }

    public class dtoPawnInfo_addNew
    {
        //--+ PayAccount
        public int Bank_ID { set; get; } 
        public string PayAccount_No { set; get; }

        //--+ ContactInfo for type is a ContactRegistrationBook_ID nguoi than tren so ho khau
        public string RegistrationBook_Name { set; get; }
        public string RegistrationBook_AddressPlace { set; get; }
        public string RegistrationBook_Phone { set; get; }

        //--+ ContactInfo for type is a ContactColleague_ID dong nghiep
        public string Colleague_Name { set; get; }
        public string Colleague_AddressPlace { set; get; }
        public string Colleague_Phone { set; get; }

        //--+ ContactInfo for type is a Customer
        public string Custorer_Name { set; get; }
        public string Custorer_AddressPlace { set; get; }
        public string Custorer_Phone { set; get; }

        //--+ CustomerInfo: @Contact_ID 
        public long Invite_ID { set; get; } // It is User_ID logined
        public string CMND_CCCD { set; get; }
        public int CMND_CreateDate { set; get; }
        public string CMND_CreatePlace { set; get; }

        //--+ PawnInfo: @PayAccount_ID
        public string PawnCode { set; get; } // ma hop dong
        public long LoanAmount { set; get; } // so tien vay
        public int SumLoanDate { set; get; } // thoi han vay theo ngay
        public long DatetimeFinish { set; get; }// ngay tat toan hop dong
        public int ContractSettlementShop_ID { set; get; }// chon sua hang tat toan

        //--+ PawnImages, PawnImageLocate
        public string Image_RegistrationBook_1 { set; get; }
        public string Image_RegistrationBook_2 { set; get; }
        public string Image_VehicleRegistration_1 { set; get; }
        public string Image_VehicleRegistration_2 { set; get; }
        public string Image_Asset_1 { set; get; }
        public string Image_Asset_2 { set; get; }
        public string Image_InvoiceElectric_1 { set; get; }
    }
}
