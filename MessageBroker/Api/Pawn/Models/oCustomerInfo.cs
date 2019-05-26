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
        [AttrFieldInfo(1, "", AttrDataType.LONG)]
        public long Customer_ID { set; get; }

        [AttrFieldInfo(1, "Ma Khach hang", AttrDataType.LONG)]
        public long CustomerCode { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.LONG)]
        public long Contact_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.LONG)]
        public long CompanyContact_ID { set; get; }
        
        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Source_ID { set; get; }
        
        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Invite_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.BOOL)]
        public bool IsInviter { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING)]
        public string CMND_CCCD { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int CMND_CreateDate { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING)]
        public string CMND_CreatePlace { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int CompanyPhone { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Brithday { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Gender { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Salary { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING)]
        public string CompanyName { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.LONG)]
        public long DateCreate { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.LONG)]
        public long DateActive { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Status { set; get; }
    }
}
