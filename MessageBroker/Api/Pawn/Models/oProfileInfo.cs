namespace MessageBroker
{
    [AttrModelInfo("Thông tin profile", _API_CONST.SHOP_INFO)]
    public class oProfileInfo
    {
        [AttrFieldInfo(1, "", AttrDataType.LONG, true, true, true)]
        public long Profile_ID { set; get; }

        // UserLogin
        [AttrFieldInfo(1, "", AttrDataType.INT, true, true, true)]
        public int User_ID { set; get; }

        [AttrFieldInfo(5, "Admin|User", AttrDataType.STRING)]
        public int GroupType_ID { get; set; }

        [AttrFieldInfo(1, "Trang thai", AttrDataType.STRING)]
        public int Status { get; set; }

        ////////////////////////////////////////////////////////////////////

        //ContactInfo
        [AttrFieldInfo(1, "Danh bạ Id", AttrDataType.LONG)]
        public long Contact_ID { set; get; }

        [AttrFieldInfo(1, "Họ tên", AttrDataType.STRING)]
        public string Name { set; get; }

        [AttrFieldInfo(1, "Dia chi", AttrDataType.STRING)]
        public string AddressPlace { set; get; }

        [AttrFieldInfo(1, "Điện thoại", AttrDataType.STRING)]
        public string Phones { set; get; }

        [AttrFieldInfo(1, "Điện thoại", AttrDataType.STRING)]
        public string Emails { set; get; }

        ////////////////////////////////////////////////////////////////////
        // ProfileInfo

        [AttrFieldInfo(1, "Khach hang Id", AttrDataType.LONG)]
        public long Customer_ID { set; get; }

        [AttrFieldInfo(1, "Ma Khach hang", AttrDataType.LONG)]
        public long CustomerCode { set; get; }

        [AttrFieldInfo(1, "Avatar", AttrDataType.STRING)]
        public string Avatar { get; set; }

        [AttrFieldInfo(5, "Hợp đồng đang vay", AttrDataType.STRING)]
        public int ContactActiveCounter { get; set; }

        [AttrFieldInfo(1, "Tổng số tiền", AttrDataType.STRING)]
        public long PawnTotalMoney { get; set; }

        [AttrFieldInfo(1, "Hoa Hồng", AttrDataType.STRING)]
        public long PawnTotalBonus { get; set; }

        [AttrFieldInfo(1, "Giới tính", AttrDataType.STRING)]
        public long Gender { get; set; }

        [AttrFieldInfo(1, "Ngày sinh", AttrDataType.STRING)]
        public long Brithday { get; set; }

        [AttrFieldInfo(1, "Thu nhập", AttrDataType.STRING)]
        public long Salary { get; set; }

        [AttrFieldInfo(1, "Thời gian cập nhật gần nhất", AttrDataType.STRING)]
        public long DatetimeUplateLastest { get; set; }
        ////////////////////////////////////////////////////////////////////

    }
}
