namespace MessageBroker
{
    [AttrModelInfo("Thông tin hợp đồng", _API_CONST.PAWN_INFO)]
    public class oPawnInfo
    {
        //-------------------------------------------
        // Thong tin khoan vay
        [AttrFieldInfo(11, "Hợp đồng Id", AttrDataType.INT, true, true)]
        public int PawnID { get; set; } // Hop dong id

        [AttrFieldInfo(12, "Mã hợp đồng", AttrDataType.STRING, false, true)]
        public string PawnCode { get; set; } //Ma hop dong

        [AttrFieldInfo(13, "Số tiền vay", AttrDataType.LONG)]
        public long LoanAmount { get; set; } //So tien vay

        [AttrFieldInfo(14, "Thời hạn vay", AttrDataType.INT)]
        public int LoanTerm { get; set; } //Thoi han vay tien

        //-------------------------------------------
        // Thong tin khach hang
        [AttrFieldInfo(201, "Họ và tên", AttrDataType.STRING, false, true, true)]
        public string FullName { set; get; }

        [AttrFieldInfo(202, "CMND/CCCD", AttrDataType.STRING)]
        public string CMND_CCCD { get; set; }

        [AttrFieldInfo(203, "Ngày cấp", AttrDataType.INT_DATE)]
        public int CMND_CreateDate { get; set; }

        [AttrFieldInfo(204, "Nơi cấp", AttrDataType.STRING)]
        public string CMND_CreateAddress { get; set; }

        [AttrFieldInfo(205, "Mã hợp đồng", AttrDataType.INT, false, true, true)]
        public int Phone { set; get; }

        [AttrFieldInfo(206, "Địa chỉ", AttrDataType.STRING)]
        public string Address { get; set; }

        [AttrFieldInfo(207, "Ngân hàng", AttrDataType.ENTITY, EntityName = "oBankInfo")]
        public oBankInfo BankPayment { get; set; }

        //-------------------------------------------
        [AttrFieldInfo(300, "Láng riềng", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        public oContactPeople[] Neighbor { set; get; }

        [AttrFieldInfo(301, "Đồng nghiệp", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        public oContactPeople[] Colleague { set; get; }

        [AttrFieldInfo(302, "Người thân trong sổ hộ khẩu", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        public oContactPeople[] PeopleInRegistrationBook { set; get; }
        //-------------------------------------------

        [AttrFieldInfo(400, "Cửa hàng tất toán hợp đồng", AttrDataType.ENTITY, EntityName = "oShopInfo")]
        public oShopInfo ContractSettlementShop { get; set; }

        [AttrFieldInfo(500, "Ảnh tài sản", AttrDataType.ENTITY, EntityName = "oAssetImages")]
        public oAssetImages AssetImages { get; set; }

        //public oTaiSanLoai[] TaiSan { get; set; }

        [AttrFieldInfo(900, "Trạng thái hợp đồng", AttrDataType.ENUM, EntityName = "oContractStatus")]
        public oContractStatus Status { get; set; }
    }
}
