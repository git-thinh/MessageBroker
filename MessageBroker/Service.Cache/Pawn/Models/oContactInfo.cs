namespace MessageBroker
{
    [AttrModelInfo("Thông tin danh bạ", _API_CONST.CONTACT_INFO)]
    public class oContactInfo
    {
        [AttrFieldInfo(1, "Danh bạ Id", AttrDataType.LONG, true, true, true)]
        public long Contact_ID { set; get; }

        [AttrFieldInfo(1, "Họ tên", AttrDataType.STRING)]
        public string Name { set; get; }

        [AttrFieldInfo(1, "Dia chi cong ty", AttrDataType.STRING)]
        public string AddressCompany { set; get; }

        [AttrFieldInfo(1, "Điện thoại", AttrDataType.STRING)]
        public string Phones { set; get; }

        [AttrFieldInfo(1, "Điện thoại", AttrDataType.STRING)]
        public string Emails { set; get; }

        [AttrFieldInfo(1, "Địa chỉ", AttrDataType.STRING)]
        public string AddressPlace { get; set; }

        [AttrFieldInfo(1, "Trang thai", AttrDataType.STRING)]
        public int Status { get; set; }
    }
}
