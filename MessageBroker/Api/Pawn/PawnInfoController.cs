﻿using CacheEngineShared;
using Dapper;
using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Nustache.Compilation;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnInfoService : BaseServiceCache<oPawnInfo> { public PawnInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnInfoBehavior : BaseServiceCacheBehavior { public PawnInfoBehavior(object instance) : base(instance) { } }

    public class PawnInfoController : BaseController
    {
        static PawnInfoController()
        {
            initCacheService(_API_CONST.PAWN_INFO);
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
            if (item == null) return new oCacheResult().ToFailConvertJson("Please check format string json of input.");

            #region [ If API_POS is not exist, Get automation PawnCode will be genation as follow ]

            String sixdigit = new Random().Next(0, 999999).ToString("D6");
            string formatByCompay = DateTime.Now.ToString("yyMM");
            string GenCodeNo = string.Format("HĐCC/DR/{0}/{1}", formatByCompay, sixdigit);

            #endregion

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

        #region [ IN: PHIEU CHI - HOP DONG ]

        private void fn_format_Gender(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            int gender = (int)args[0];
            ctx.Write(gender == 1 ? "Nam" : "Nữ");
        }

        private void fn_formatDateTime_ddMMyyyy(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            string dateTime = args[0].ToString(), s = args[1].ToString();
            if (!string.IsNullOrWhiteSpace(dateTime) && dateTime.Length > 7)
            {
                DateTime dt;
                if (dateTime == "DATE_NOW") dt = DateTime.Now;
                else dt = DateTime.ParseExact(dateTime, dateTime.Length == 8 ? "yyyyMMdd" : "yyyyMMddHHmmss", null);
                s = dt.ToString("dd/MM/yyyy");
                ctx.Write(s);
            }
        }

        private void fn_formatDateTimeNow(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            string dateTime = args[0].ToString().Replace('_', ' ');
            if (!string.IsNullOrWhiteSpace(dateTime) && dateTime.Length > 7)
            {
                string s = DateTime.Now.ToString(dateTime);
                ctx.Write(s);
            }
        }

        private void fn_formatDateTime(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            string dateTime = args[0].ToString(), s = args[1].ToString();
            if (!string.IsNullOrWhiteSpace(dateTime) && dateTime.Length > 7)
            {
                DateTime dt;
                if (dateTime == "DATE_NOW") dt = DateTime.Now;
                else dt = DateTime.ParseExact(dateTime, dateTime.Length == 8 ? "yyyyMMdd" : "yyyyMMddHHmmss", null);
                s = s.Replace("_dd_", " " + dt.Day.ToString() + " ")
                        .Replace("_MM_", " " + dt.Month.ToString() + " ")
                        .Replace("_yyyy", " " + dt.Year.ToString() + " ");
                ctx.Write(s);
            }
        }

        private void fn_formatMoney(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            long val = (long)args[0];
            string s = String.Format("{0:0,0}", val);
            ctx.Write(s);
        }
        private void fn_numberToText(RenderContext ctx, IList<object> args, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            long val = (long)args[0];
            string s = fn_numberToTextVN(val);
            ctx.Write(s);
        }
        private string fn_numberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }
        }

        // api/pawn_info/get_in_phieu_chi?User_ID=9&Pawn_ID=1167678&so_phieu_chi=PC/HNTC/04/2383&filetemp=in-phieu-chi.html
        public HttpResponseMessage get_in_phieu_chi([FromUri]int User_ID, [FromUri]int Pawn_ID, [FromUri]string so_phieu_chi, [FromUri]string filetemp)
        {
            string html = "";
            oPawnInfo pawn = null;
            oCacheResult rs = this.post_Search(new oCacheRequest("Pawn_ID != null && Pawn_ID = " + Pawn_ID));
            if (rs.Ok && rs.Result.Length > 0) pawn = (oPawnInfo)rs.Result[0];
            //pawn = JsonConvert.DeserializeObject<oPawnInfo>(File.ReadAllText(Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/pawn.json")));
            if (pawn != null)
            {
                string file = Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/" + filetemp);
                string temp = File.ReadAllText(file);
                temp = temp.Replace("[PHIEU_CHI_SO]", so_phieu_chi);

                Helpers.Clear();
                Helpers.Register("FormatDateTime1", fn_formatDateTime);
                Helpers.Register("FormatDateTime2", fn_formatDateTime);
                Helpers.Register("FormatMoney1", fn_formatMoney);
                Helpers.Register("NumberToText1", fn_numberToText);

                html = Render.StringToString(temp, pawn);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(html, Encoding.UTF8, "text/html");
            return response;
        }

        // api/pawn_info/get_in_hop_dong?User_ID=9&Pawn_ID=1167678&filetemp=in-hop-dong.html
        public HttpResponseMessage get_in_hop_dong([FromUri]int User_ID, [FromUri]int Pawn_ID, [FromUri]string filetemp)
        {
            string html = "";
            oPawnInfo pawn = null;
            oCacheResult rs = this.post_Search(new oCacheRequest("Pawn_ID != null && Pawn_ID = " + Pawn_ID));
            if (rs.Ok && rs.Result.Length > 0) pawn = (oPawnInfo)rs.Result[0];
            //pawn = JsonConvert.DeserializeObject<oPawnInfo>(File.ReadAllText(Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/pawn.json")));
            if (pawn != null)
            {
                string file = Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/" + filetemp);
                string temp = File.ReadAllText(file);
                temp = temp.Replace("[SO_HOP_DONG]", pawn.PawnCode);

                Helpers.Clear();
                Helpers.Register("FormatGender", fn_format_Gender);
                Helpers.Register("FormatDateTime", fn_formatDateTime_ddMMyyyy);
                Helpers.Register("FormatDateTimeNow", fn_formatDateTimeNow);
                Helpers.Register("FormatDateTime1", fn_formatDateTime);
                Helpers.Register("FormatDateTime2", fn_formatDateTime);
                Helpers.Register("FormatMoney1", fn_formatMoney);
                Helpers.Register("NumberToText1", fn_numberToText);

                html = Render.StringToString(temp, pawn);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(html, Encoding.UTF8, "text/html");
            return response;
        }

        // api/pawn_info/get_in_giay_yeu_cau_bao_hiem?User_ID=9&Pawn_ID=1167678&lien_hop_dong=1&filetemp=in-giay-yeu-cau-bao-hiem-lien-1.html
        public HttpResponseMessage get_in_giay_yeu_cau_bao_hiem([FromUri]int User_ID, [FromUri]int Pawn_ID, [FromUri]string lien_hop_dong, [FromUri]string filetemp)
        {
            string html = "";
            oPawnInfo pawn = null;
            oCacheResult rs = this.post_Search(new oCacheRequest("Pawn_ID != null && Pawn_ID = " + Pawn_ID));
            if (rs.Ok && rs.Result.Length > 0) pawn = (oPawnInfo)rs.Result[0];
            //pawn = JsonConvert.DeserializeObject<oPawnInfo>(File.ReadAllText(Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/pawn.json")));
            if (pawn != null)
            {
                string file = Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/" + filetemp);
                string temp = File.ReadAllText(file);
                temp = temp.Replace("[SO_HOP_DONG]", pawn.PawnCode);
                if (lien_hop_dong == "1")
                    temp = temp.Replace("[LIEN_HOP_DONG]", "(Liên 1: Liên dành cho doanh nghiệp)");
                else
                    temp = temp.Replace("[LIEN_HOP_DONG]", "(Liên 2: Liên dành cho khách hàng)");

                Helpers.Clear();
                Helpers.Register("FormatGender", fn_format_Gender);
                Helpers.Register("FormatDateTime", fn_formatDateTime_ddMMyyyy);
                Helpers.Register("FormatDateTimeNow", fn_formatDateTimeNow);
                Helpers.Register("FormatDateTime1", fn_formatDateTime);
                Helpers.Register("FormatDateTime2", fn_formatDateTime);
                Helpers.Register("FormatMoney1", fn_formatMoney);
                Helpers.Register("NumberToText1", fn_numberToText);

                html = Render.StringToString(temp, pawn);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(html, Encoding.UTF8, "text/html");
            return response;
        }

        // api/pawn_info/get_in_ban_cam_ket_xe_khong_chinh_chu?User_ID=9&Pawn_ID=1167678&filetemp=in-ban-cam-ket-xe-khong-chinh-chu.html
        public HttpResponseMessage get_in_ban_cam_ket_xe_khong_chinh_chu([FromUri]int User_ID, [FromUri]int Pawn_ID, [FromUri]string filetemp)
        {
            string html = "";
            oPawnInfo pawn = null;
            oCacheResult rs = this.post_Search(new oCacheRequest("Pawn_ID != null && Pawn_ID = " + Pawn_ID));
            if (rs.Ok && rs.Result.Length > 0) pawn = (oPawnInfo)rs.Result[0];
            //pawn = JsonConvert.DeserializeObject<oPawnInfo>(File.ReadAllText(Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/pawn.json")));
            if (pawn != null)
            {
                string file = Path.Combine(Path.GetFullPath("../"), "MessageUI/pdf/" + filetemp);
                string temp = File.ReadAllText(file);
                temp = temp.Replace("[SO_HOP_DONG]", pawn.PawnCode);

                Helpers.Clear();
                Helpers.Register("FormatGender", fn_format_Gender);
                Helpers.Register("FormatDateTime", fn_formatDateTime_ddMMyyyy);
                Helpers.Register("FormatDateTimeNow", fn_formatDateTimeNow);
                Helpers.Register("FormatDateTime1", fn_formatDateTime);
                Helpers.Register("FormatDateTime2", fn_formatDateTime);
                Helpers.Register("FormatMoney1", fn_formatMoney);
                Helpers.Register("NumberToText1", fn_numberToText);

                html = Render.StringToString(temp, pawn);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(html, Encoding.UTF8, "text/html");
            return response;
        }

        #endregion

        public HttpResponseMessage get_export_pdf([FromUri]string User_ID, [FromUri]string Pawn_ID, [FromUri]string Code_Temp, [FromUri]string para)
        {
            string ok = "OK";
            if (!string.IsNullOrWhiteSpace(User_ID) && !string.IsNullOrWhiteSpace(Pawn_ID) && !string.IsNullOrWhiteSpace(Code_Temp))
            {
                string pdf = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"pdf\pdf.exe");
                string url = "http://localhost:9096/api/pawn_info/get_in_" + Code_Temp + "?User_ID=" + User_ID + "&Pawn_ID=" + Pawn_ID + "&filetemp=in-" + Code_Temp.Replace("_", "-") + ".html";

                switch (Code_Temp)
                {
                    case "ban_cam_ket_xe_khong_chinh_chu":
                        url = "http://localhost:9096/api/pawn_info/get_in_ban_cam_ket_xe_khong_chinh_chu?User_ID=" + User_ID + "&Pawn_ID=" + Pawn_ID + "&filetemp=in-ban-cam-ket-xe-khong-chinh-chu.html";
                        break;
                    case "hop_dong":
                        url = "http://localhost:9096/api/pawn_info/get_in_hop_dong?User_ID=" + User_ID + "&Pawn_ID=" + Pawn_ID + "&filetemp=in-hop-dong.html";
                        break;
                    case "phieu_chi":
                        url = "http://localhost:9096/api/pawn_info/get_in_phieu_chi?User_ID=" + User_ID + "&Pawn_ID=" + Pawn_ID + "&so_phieu_chi=" + para + "&filetemp=in-phieu-chi.html";
                        break;
                    case "giay_yeu_cau_bao_hiem":
                        url = "http://localhost:9096/api/pawn_info/get_in_giay_yeu_cau_bao_hiem?User_ID=" + User_ID + "&Pawn_ID=" + Pawn_ID + "&lien_hop_dong=" + para + "&filetemp=in-giay-yeu-cau-bao-hiem-lien-1.html";
                        break;
                }

                string filetemp = string.Format("{0}.{1}.{2}.pdf", User_ID, Code_Temp, Pawn_ID);
                string file = Path.Combine(Path.GetFullPath("../"), @"MessageUI\Exports\" + filetemp);
                var process = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        //UseShellExecute = true,
                        FileName = pdf,
                        Arguments = " --url=\"" + url + "\" " + "\"" + file + "\""
                    }
                };
                process.Start();
                process.WaitForExit();

                var _dataflow = (IDataflowSubscribers)MemoryCache.Default.Get("JOB");
                _dataflow.enqueue(new JobClientNotification(string.Format("#EXPORT_PDF:OK:{0}", filetemp))).Wait();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(ok, Encoding.UTF8, "text/plain");
            return response;
        }
    }

    public class _DB_STORE_PAWN_INFO
    {
        public const string _cacheInitData = "pawn_info_cacheInitData";
        public const string _addNew = "pawn_info_createNew";
        public const string _pawnCode_createNew = "pos.GetMaxPawnIndexByShop";
    }

    public class dtoPawnInfo_addNewResult
    {
        public bool Ok { set; get; }
        public string Message { set; get; }
        public string ServiceCache { set; get; }
        public long Contact_ID { set; get; }
        public long User_ID { set; get; }
    }

    [AttrModelInfo("Thông tin tao moi hợp đồng", _API_CONST.PAWN_INFO)]
    [Validator(typeof(dtoPawnInfo_addNewValidator))]
    public class dtoPawnInfo_addNew
    {
        //--+ PayAccount
        [AttrFieldInfo(1, "Ngan hang", AttrDataType.STRING)]
        public int Bank_ID { set; get; }

        [AttrFieldInfo(1, "Tai khoan", AttrDataType.STRING)]
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

        // It is User_ID logined
        [AttrFieldInfo(11, "Ma CONTACT nguoi gioi thieu", AttrDataType.LONG, true, true)]
        public long InviteContact_ID { get; set; }

        [AttrFieldInfo(11, "Ma TAI KHOAN nguoi gioi thieu", AttrDataType.LONG, true, true)]
        public long InviteUser_ID { get; set; }

        public string CMND_CCCD { set; get; }
        public int CMND_CreateDate { set; get; }
        public string CMND_CreatePlace { set; get; }

        //--+ PawnInfo: @PayAccount_ID
        public string PawnCode { set; get; } // ma hop dong
        public long LoanAmount { set; get; } // so tien vay
        public int SumLoanDate { set; get; } // thoi han vay theo ngay
        public long DatetimeFinish { set; get; }// ngay tat toan hop dong
        //--+ PawnImages, PawnImageLocate
        public string Image_RegistrationBook_1 { set; get; }
        public string Image_RegistrationBook_2 { set; get; }
        public string Image_VehicleRegistration_1 { set; get; }
        public string Image_VehicleRegistration_2 { set; get; }
        public string Image_Asset_1 { set; get; }
        public string Image_Asset_2 { set; get; }
        public string Image_InvoiceElectric_1 { set; get; }

        public int ContractSettlementShop_ID { set; get; }// chon sua hang tat toan
    }

    public class dtoPawnInfo_addNewValidator : AbstractValidator<dtoPawnInfo_addNew>
    {
        //Simple validator that checks for values in First and Lastname
        public dtoPawnInfo_addNewValidator()
        {
            //--+ PayAccount
            RuleFor(r => r.Bank_ID).GreaterThan(0).WithMessage("Vui lòng nhập thông tin ngân hàng");
            RuleFor(r => r.PayAccount_No).NotEmpty().WithMessage("Vui lòng nhập thông tin tài khoản ngân hàng");

            //--+ ContactInfo for type is a ContactRegistrationBook_ID nguoi than tren so ho khau
            RuleFor(r => r.RegistrationBook_Name).NotEmpty().WithMessage("Vui lòng nhập thông tin tên người thân trên sổ hộ khẩu");
            RuleFor(r => r.RegistrationBook_AddressPlace).NotEmpty().WithMessage("Vui lòng nhập thông tin địa chỉ người thân trên sổ hộ khẩu");
            RuleFor(r => r.RegistrationBook_Phone).NotEmpty().WithMessage("Vui lòng nhập thông tin số điện thoại người thân trên sổ hộ khẩu");

            //--+ ContactInfo for type is a ContactColleague_ID dong nghiep
            RuleFor(r => r.Colleague_Name).NotEmpty().WithMessage("Vui lòng nhập thông tin tên đồng nghiệp");
            RuleFor(r => r.Colleague_AddressPlace).NotEmpty().WithMessage("Vui lòng nhập thông tin chỗ ở đồng nghiệp");
            RuleFor(r => r.Colleague_Phone).NotEmpty().WithMessage("Vui lòng nhập thông tin số điện thoại đồng nghiệp");

            //--+ ContactInfo for type is a Customer
            RuleFor(r => r.Custorer_Name).NotEmpty().WithMessage("Vui lòng nhập thông tin tên khách hàng");
            RuleFor(r => r.Custorer_AddressPlace).NotEmpty().WithMessage("Vui lòng nhập thông tin chỗ ở khách hàng");
            RuleFor(r => r.Custorer_Phone).NotEmpty().WithMessage("Vui lòng nhập thông tin số điện thoại khách hàng");

            //--+ CustomerInfo: @Contact_ID 
            RuleFor(r => r.InviteUser_ID).GreaterThan(0).WithMessage("[InviteUser_ID] Điền thông tin ID của User đang đăng nhập"); // It is User_ID logined
            RuleFor(r => r.InviteContact_ID).GreaterThan(0).WithMessage("[InviteContact_ID] Điền thông tin ID của Contact đang đăng nhập"); // It is User_ID logined

            RuleFor(r => r.CMND_CCCD).NotEmpty().WithMessage("Vui lòng nhập thông tin số CMND_CCCD");
            RuleFor(r => r.CMND_CreateDate).GreaterThan(0).WithMessage("Vui lòng nhập thông tin ngày tạo CMND_CCCD");
            RuleFor(r => r.CMND_CreateDate).LessThan(int.Parse(DateTime.Now.ToString("yyyyMMdd"))).WithMessage("Ngày cấp CMND_CCCD không hợp lệ");
            RuleFor(r => r.CMND_CreatePlace).NotEmpty().WithMessage("Vui lòng nhập thông tin nơi cấp CMND_CCCD");

            //--+ PawnInfo: @PayAccount_ID
            RuleFor(r => r.PawnCode).NotEmpty().WithMessage("Thông tin [PawnCode] chưa được điền"); // ma hop dong
            RuleFor(x => x.LoanAmount).Must(beAValid_LoanAmount).WithMessage("Vui lòng chọn thông tin số tiền vay là 3 triệu hoặc 5 triệu");// so tien vay
            RuleFor(r => r.SumLoanDate).Must(beAValid_SumLoanDate).WithMessage("Vui lòng chọn thông tin thời hạn vay là 3|6|12 tháng"); // thoi han vay theo thang
            //RuleFor(r => r.DatetimeFinish).GreaterThan(0).WithMessage("Vui lòng nhập thông tin ");// ngay tat toan hop dong
            RuleFor(r => r.ContractSettlementShop_ID).GreaterThan(0).WithMessage("Vui lòng nhập thông tin cửa hàng tất toán hợp đồng");// chon sua hang tat toan

            //--+ PawnImages, PawnImageLocate
            RuleFor(r => r.Image_RegistrationBook_1).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2 hình ảnh sổ hộ khẩu");
            RuleFor(r => r.Image_RegistrationBook_2).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2  hình ảnh sổ hộ khẩu");
            RuleFor(r => r.Image_VehicleRegistration_1).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2 hình ảnh giấy tờ xe");
            RuleFor(r => r.Image_VehicleRegistration_2).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2 hình ảnh giấy tờ xe");
            RuleFor(r => r.Image_Asset_1).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2 hình ảnh tài sản");
            RuleFor(r => r.Image_Asset_2).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 2 hình ảnh tài sản");
            RuleFor(r => r.Image_InvoiceElectric_1).NotEmpty().WithMessage("Vui lòng tải lên ít nhất 1 hình ảnh hóa đơn điện");
        }

        private bool beAValid_LoanAmount(long arg)
        {
            return arg == 3000000 || arg == 5000000;
        }

        private bool beAValid_SumLoanDate(int arg)
        {
            return arg == 3 || arg == 6 || arg == 12;
        }
    }

}
