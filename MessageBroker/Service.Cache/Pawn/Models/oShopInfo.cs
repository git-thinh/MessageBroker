namespace MessageBroker
{
    [AttrModelInfo("Thông tin cửa hàng", _API_CONST.SHOP_INFO)]
    public class oShopInfo
    {
        [AttrFieldInfo(1, "Cửa hàng Id", AttrDataType.INT, true, true, true)]
        public int ShopID { set; get; }

        [AttrFieldInfo(2, "Mã cửa hàng", AttrDataType.STRING, false, true)]
        public string Code { set; get; }

        [AttrFieldInfo(3, "Tên cửa hàng", AttrDataType.STRING, false, true, true)]
        public string Name { set; get; }

        [AttrFieldInfo(4, "Địa chỉ", AttrDataType.STRING, false, true)]
        public string Address { set; get; }

        [AttrFieldInfo(5, "Điện thoại", AttrDataType.STRING, false, true)]
        public string Phone { set; get; }
    }
}
 