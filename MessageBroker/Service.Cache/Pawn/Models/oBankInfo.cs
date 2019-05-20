namespace MessageBroker
{
    [AttrModelInfo("Thông tin ngân hàng", _API_CONST.BANK_INFO)]
    public class oBankInfo
    {
        public int MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public string[] ChiNhanh { set; get; }
    }
}
