namespace MessageBroker
{
    [AttrModelInfo("Thông tin danh bạ", _API_CONST.CONTACT_PEOPLE)]
    public class oContactPeople
    {
        [AttrFieldInfo(1, "Danh bạ Id", AttrDataType.LONG, true, true, true)]
        public long ContactId { set; get; }

        [AttrFieldInfo(1, "Họ tên", AttrDataType.STRING)]
        public string FullName { set; get; }

        [AttrFieldInfo(1, "Điện thoại", AttrDataType.STRING)]
        public int Phone { set; get; }

        [AttrFieldInfo(1, "Địa chỉ", AttrDataType.STRING)]
        public string Address { get; set; }
    }
}
