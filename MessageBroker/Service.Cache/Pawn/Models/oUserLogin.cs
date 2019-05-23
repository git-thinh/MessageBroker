namespace MessageBroker
{
    [AttrModelInfo("Thông tin đăng nhập", _API_CONST.USER_LOGIN)]
    public class oUserLogin
    {            
        [AttrFieldInfo(1, "Tài khoản Id", AttrDataType.LONG, true)]
        public long User_ID { get; set; }

        [AttrFieldInfo(2, "Mã liên hệ", AttrDataType.LONG, ServiceLink = _API_CONST.CONTACT_INFO)]
        public long Contact_ID { get; set; }

        [AttrFieldInfo(3, "Mã Nguồn", AttrDataType.INT, ServiceLink = _API_CONST.SOURCE_INFO)]
        public long Source_ID { get; set; }

        [AttrFieldInfo(2, "Tên tài khoản", AttrDataType.STRING)]
        public string Username { get; set; }

        [AttrFieldInfo(3, "Mật khẩu", AttrDataType.STRING)]
        public string Password { get; set; }

        [AttrFieldInfo(4, "Mã thiết bị truy cập", AttrDataType.STRING)]
        public string DeviceCode { get; set; }

        [AttrFieldInfo(5, "Nhóm khách hàng", AttrDataType.STRING)]
        public string GroupType { get; set; }

        [AttrFieldInfo(5, "Trạng thái", AttrDataType.INT)]
        public int Status { get; set; }

        //----------------------------------------------------------

        [AttrFieldInfo(5, "Nguon", AttrDataType.SERVICE_LINK_RESULT, ServiceLinkFieldName = "Source_ID")]
        public dynamic Source { get; set; }

        [AttrFieldInfo(5, "Lien he", AttrDataType.SERVICE_LINK_RESULT, ServiceLinkFieldName = "Contact_ID")]
        public dynamic Contact { get; set; }
    }
}
