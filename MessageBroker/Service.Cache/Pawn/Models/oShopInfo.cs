namespace MessageBroker
{
    [AttrModelInfo("Thông tin cửa hàng", _API_CONST.SHOP_INFO)]
    public class oShopInfo
    {
        public int MaCuaHang { set; get; }
        public string TenCuaHang { set; get; }
        public string DiaChi { set; get; }
        public string DienThoai { set; get; }
    } 
}
