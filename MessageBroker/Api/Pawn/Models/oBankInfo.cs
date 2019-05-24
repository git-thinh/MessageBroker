namespace MessageBroker
{
    [AttrModelInfo("Thông tin ngân hàng", _API_CONST.BANK_INFO)]
    public class oBankInfo
    {
        [AttrFieldInfo(1, "Ngân Hàng Id", AttrDataType.INT, true, true, true)]
        public int Bank_ID { set; get; }

        [AttrFieldInfo(2, "Ma ngân hàng", AttrDataType.STRING, false, true, true)]
        public string Code { set; get; }
        
        [AttrFieldInfo(3, "Tên ngân hàng", AttrDataType.STRING, false, true, true)]
        public string Name { set; get; }
        
        [AttrFieldInfo(4, "Chi nhánh", AttrDataType.STRING)]
        public string Branch { set; get; }

        [AttrFieldInfo(5, "Trang thai", AttrDataType.INT)]
        public int Status { set; get; }
    }
}
