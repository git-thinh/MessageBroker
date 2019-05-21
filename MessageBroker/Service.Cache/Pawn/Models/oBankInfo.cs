namespace MessageBroker
{
    [AttrModelInfo("Thông tin ngân hàng", _API_CONST.BANK_INFO)]
    public class oBankInfo
    {
        [AttrFieldInfo(1, "Ngân Hàng Id", AttrDataType.INT, true, true, true)]
        public int BankId { set; get; }

        [AttrFieldInfo(2, "Tên ngân hàng", AttrDataType.STRING, false, true, true)]
        public string Name { set; get; }

        [AttrFieldInfo(3, "Chi nhánh", AttrDataType.STRING)]
        public string Branch { set; get; }
    }
}
