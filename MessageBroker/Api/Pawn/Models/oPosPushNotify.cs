using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttrModelInfo("", _API_CONST.SOURCE_INFO)]
    public class oPosPushNotify
    {
        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public long Pawn_Id { set; get; }
        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public long User_ID { set; get; }


        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public long Asset_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public long AssetCategory_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING, true)]
        public string AssetCode { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING, true)]
        public string AssetCategoryCode { set; get; }


        [AttrFieldInfo(20, "Tên tài sản", AttrDataType.STRING)]
        public string Asset_Name { get; set; }

        [AttrFieldInfo(21, "Nhom tài sản", AttrDataType.STRING)]
        public string AssetCategory_Name { get; set; }
        
        [AttrFieldInfo(2, "", AttrDataType.STRING)]
        public string DateCreate { set; get; }

        [AttrFieldInfo(2, "", AttrDataType.STRING)]
        public string Message { set; get; }

        [AttrFieldInfo(2, "", AttrDataType.INT)]
        public int Bonus { set; get; }

        [AttrFieldInfo(2, "", AttrDataType.LONG)]
        public long DatetimeReceiver { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public int Status { set; get; }


    }
}
