namespace MessageBroker
{
    [AttrModelInfo("Thông tin hợp đồng", _API_CONST.PAWN_INFO)]
    public class oPawnInfo
    {
        [AttrFieldInfo(11, "Hợp đồng Id", AttrDataType.LONG, true, true)]
        public long Pawn_ID { get; set; }

        [AttrFieldInfo(11, "Tiền thưởng", AttrDataType.LONG, true, true)]
        public int Bonus { get; set; }
        //-------------------------------------------
        
        [AttrFieldInfo(12, "Mã hợp đồng", AttrDataType.STRING, false, true)]
        public string PawnCode { get; set; }

        [AttrFieldInfo(13, "Số tiền vay", AttrDataType.LONG)]
        public long LoanAmount { get; set; }
        
        [AttrFieldInfo(14, "Ngày tạo hợp đồng", AttrDataType.LONG_DATETIME, true, true)]
        public long DatetimeCreate { get; set; }  

        [AttrFieldInfo(15, "Thời hạn vay", AttrDataType.LONG_DATETIME, true, true)]
        public long DatetimeFinish { get; set; } 

        [AttrFieldInfo(16, "Số ngày vay tiền", AttrDataType.INT)]
        public int SumLoanDate { get; set; }

        //-------------------------------------------
        [AttrFieldInfo(20, "Thong tin thong bao", AttrDataType.STRING)]
        public string PawnMessage { get; set; }

        [AttrFieldInfo(20, "Tên tài sản", AttrDataType.STRING)]
        public string Asset_Name { get; set; }

        [AttrFieldInfo(21, "Nhom tài sản", AttrDataType.STRING)]
        public string AssetCategory_Name { get; set; }

        [AttrFieldInfo(1, "", AttrDataType.LONG, true)]
        public long Asset_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.LONG, true)]
        public long AssetCategory_ID { set; get; }

        //-------------------------------------------
        // Thong tin khach hang

        [AttrFieldInfo(202, "CMND/CCCD", AttrDataType.STRING)]
        public string CMND_CCCD { get; set; }

        [AttrFieldInfo(203, "Ngày cấp", AttrDataType.INT_DATE)]
        public int CMND_CreateDate { get; set; }

        [AttrFieldInfo(204, "Nơi cấp", AttrDataType.STRING)]
        public string CMND_CreatePlace { get; set; }

        //-------------------------------------------
        
        [AttrFieldInfo(201, "Tên khách hàng", AttrDataType.STRING)]
        public string Custorer_Name { set; get; }

        [AttrFieldInfo(205, "Dien thoai", AttrDataType.INT, false, true, true)]
        public string Custorer_Phone { set; get; }

        [AttrFieldInfo(206, "Địa chỉ", AttrDataType.STRING)]
        public string Custorer_AddressPlace { get; set; }

        [AttrFieldInfo(206, "Giới tính", AttrDataType.STRING)]
        public int Custorer_Gender { get; set; }

        [AttrFieldInfo(1, "Avatar", AttrDataType.STRING)]
        public string Custorer_Avatar { get; set; }

        //-------------------------------------------

        [AttrFieldInfo(207, "Ngân hàng", AttrDataType.STRING)]
        public string BankName { get; set; }

        [AttrFieldInfo(207, "Chi nhanh Ngân hàng", AttrDataType.STRING)]
        public string BankBranchName { get; set; }


        [AttrFieldInfo(207, "Số tài khoản", AttrDataType.STRING)]
        public string BankAccountNo { get; set; }

        //-------------------------------------------

        //--+ ContactInfo for type is a ContactRegistrationBook_ID nguoi than tren so ho khau
        [AttrFieldInfo(207, "Người thân ten", AttrDataType.STRING)]
        public string RegistrationBook_Name { set; get; }

        [AttrFieldInfo(207, "Người thân dia chi", AttrDataType.STRING)]
        public string RegistrationBook_AddressPlace { set; get; }

        [AttrFieldInfo(207, "Người thân dien thoai", AttrDataType.STRING)]
        public string RegistrationBook_Phone { set; get; }

        //--+ ContactInfo for type is a ContactColleague_ID dong nghiep
        [AttrFieldInfo(207, "Đồng nghiệp ten", AttrDataType.STRING)]
        public string Colleague_Name { set; get; }

        [AttrFieldInfo(207, "Đồng nghiệp dia chi", AttrDataType.STRING)]
        public string Colleague_AddressPlace { set; get; }

        [AttrFieldInfo(207, "Đồng nghiệp dien thoai", AttrDataType.STRING)]
        public string Colleague_Phone { set; get; }

        //-------------------------------------------

        [AttrFieldInfo(400, "Cửa hàng tất toán hợp đồng", AttrDataType.STRING)]
        public string ContractSettlementShop_Name { get; set; }

        //-------------------------------------------

        [AttrFieldInfo(900, "Trạng thái hợp đồng", AttrDataType.INT)]
        public int Status { get; set; }
    }
}
