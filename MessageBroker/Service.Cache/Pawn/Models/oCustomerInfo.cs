using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttrModelInfo("Thông tin khach hang", _API_CONST.USER_LOGIN)]
    public class oCustomerInfo
    {
        public int PawnID { set; get; } // int] NULL,
        public int Customer_ID { set; get; } // int] NULL,
        public int HouseholdRegionID { set; get; } // int] NULL,
        public string HouseholdAddress { set; get; } // nvarchar] (1000) NULL,
        public int CurrentAddressRegionID { set; get; } // int] NULL,
        public string CurrentAddress { set; get; } // nvarchar] (1000) NULL,
        public string Facebook { set; get; } // nvarchar] (255) NULL,
        public string Zalo { set; get; } // nvarchar] (255) NULL,
        public string Email { set; get; } // nvarchar] (255) NULL,
        public int StayType { set; get; } // int] NULL,
        public int StayShared { set; get; } // int] NULL,
        public int StayTimeType { set; get; } // int] NULL,
        public int StayTime { set; get; } // int] NULL,
        public int Married { set; get; } // int] NULL,
        public int Child { set; get; } // int] NULL,
        public decimal Income { set; get; } // decimal](18, 0) NULL,
        public int WorkType { set; get; } // int] NULL,
        public string CompanyName { set; get; } // nvarchar] (500) NULL,
        public string CompanyPhone { set; get; } // nvarchar] (50) NULL,
        public decimal Salary { set; get; } // decimal](18, 0) NULL,
        public int SalaryType { set; get; } // int] NULL,
        public string MarriedPhone { set; get; } // nvarchar] (50) NULL,
        public string MarriedWork { set; get; } // nvarchar] (255) NULL,
        public string RelativesName1 { set; get; } // nvarchar] (100) NULL,
        public string RelativesPhone1 { set; get; } // nvarchar] (50) NULL,
        public string RelativesName2 { set; get; } // nvarchar] (100) NULL,
        public string RelativesPhone2 { set; get; } // nvarchar] (50) NULL,
        public string NeighborName1 { set; get; } // nvarchar] (100) NULL,
        public string NeighborName2 { set; get; } // nvarchar] (100) NULL,
        public string FriendName { set; get; } // nvarchar] (100) NULL,
        public string FriendPhone { set; get; } // nvarchar] (50) NULL,
        public string CompanyAddress { set; get; } // nvarchar] (500) NULL,
        public int WorkTimeAtCompany { set; get; } // int] NULL,
        public string Position { set; get; } // nvarchar] (200) NULL,
        public string RelativesIdentityID { set; get; } // nvarchar] (50) NULL,
        public DateTime RelativesIdentityDate { set; get; } // date] NULL,
        public string RelativesIdentityPlace { set; get; } // nvarchar] (200) NULL,
        public DateTime RelativesDoB { set; get; } // date] NULL,
        public string RelativesHouseHoldAddress { set; get; } // nvarchar] (500) NULL,
        public string RelativesCurrentAddress { set; get; } // nvarchar] (500) NULL,
        public string RelativesName { set; get; } // nvarchar] (100) NULL,
        public string RelativesCompanyName { set; get; } // nvarchar] (500) NULL,
        public string RelativesCompanyPhone { set; get; } // nvarchar] (50) NULL,
        public string RelativesCompanyAddress { set; get; } // nvarchar] (500) NULL,
        public string RelativesPosition { set; get; } // nvarchar] (200) NULL,
        public int RelativesWorkTimeAtCompany { set; get; } // int] NULL,
        public decimal RelativesSalary { set; get; } // decimal](18, 0) NULL,
        public int RelativesSalaryType { set; get; } // int] NULL,
        public string PaperType { set; get; } // nvarchar] (100) NULL,
        public string PaperTypeNote { set; get; } // nvarchar] (200) NULL,
        public int BankID { set; get; } // int] NULL,
        public string BankName { set; get; } // nvarchar] (200) NULL,
        public string Account { set; get; } // nvarchar] (50) NULL
    }
}
