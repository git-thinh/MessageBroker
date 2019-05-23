namespace MessageBroker
{
    [AttrModelInfo("Thông tin hợp đồng", _API_CONST.PAWN_INFO)]
    public class oPawnInfo
    {
        //[Customer_ID] [bigint] NULL,
        //[CustomerContact_ID] [bigint] NULL,
        //[Bank_ID] [int] NULL,
        //[PayAccount_ID] [bigint] NULL,
        //[Neighbor_IDs] [varchar](255) NULL,
        //[Colleague_IDs] [varchar](255) NULL,
        //[RegistrationBook_IDs] [varchar](255) NULL,
        //[ContractSettlementShop_ID] [int] NULL,
        //[AssetImages_IDs] [varchar](255) NULL,
        //[Asset_IDs] [varchar](255) NULL,
        //[AssetCategory_IDs] [varchar](255) NULL,
        //[Status] [int] NULL,

        [AttrFieldInfo(20, "Tên tài sản", AttrDataType.STRING)]
        public string AssetName { get; set; }

        //------------------------------------------- 
        [AttrFieldInfo(11, "Hợp đồng Id", AttrDataType.LONG, true, true)]
        public long Pawn_ID { get; set; }

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
        // Thong tin khach hang
        [AttrFieldInfo(200, "Mã khách hàng", AttrDataType.LONG)]
        public long Customer_ID { set; get; }

        [AttrFieldInfo(201, "Tên khách hàng", AttrDataType.STRING)]
        public string CustomerName { set; get; }
        
        //[AttrFieldInfo(202, "CMND/CCCD", AttrDataType.STRING)]
        //public string CMND_CCCD { get; set; }

        //[AttrFieldInfo(203, "Ngày cấp", AttrDataType.INT_DATE)]
        //public int CMND_CreateDate { get; set; }

        //[AttrFieldInfo(204, "Nơi cấp", AttrDataType.STRING)]
        //public string CMND_CreateAddress { get; set; }

        //[AttrFieldInfo(205, "Mã hợp đồng", AttrDataType.INT, false, true, true)]
        //public int Phone { set; get; }

        //[AttrFieldInfo(206, "Địa chỉ", AttrDataType.STRING)]
        //public string Address { get; set; }

        //[AttrFieldInfo(207, "Ngân hàng", AttrDataType.ENTITY, EntityName = "oBankInfo")]
        ////public oBankInfo BankPayment { get; set; }
        //public int BankPayment_ID { get; set; }

        //-------------------------------------------
        //////[AttrFieldInfo(300, "Láng riềng", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        ////////public oContactPeople[] Neighbor { set; get; }
        //////public int[] NeighborIDs { set; get; }


        //////[AttrFieldInfo(301, "Đồng nghiệp", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        ////////public oContactPeople[] Colleague { set; get; }
        //////public int[] Colleague_IDs { set; get; }

        //////[AttrFieldInfo(302, "Người thân trong sổ hộ khẩu", AttrDataType.ENTITY_ARRAY, EntityName = "oContactPeople", GroupTitle = "Thông tin thân nhân")]
        ////////public oContactPeople[] PeopleInRegistrationBook { set; get; }
        //////public int[] RegistrationBook_IDs { set; get; }

        //-------------------------------------------

        [AttrFieldInfo(400, "Cửa hàng tất toán hợp đồng", AttrDataType.ENTITY, EntityName = "oShopInfo")]
        //public oShopInfo ContractSettlementShop { get; set; }
        public int ContractSettlementShop_ID { get; set; }

        //////[AttrFieldInfo(500, "Ảnh tài sản", AttrDataType.ENTITY, EntityName = "oAssetImages")]
        ////////public oAssetImages AssetImages { get; set; }
        //////public int[] AssetImages_IDs { get; set; }

        //-------------------------------------------

        [AttrFieldInfo(900, "Trạng thái hợp đồng", AttrDataType.ENUM, EntityName = "oContractStatus")]
        public oContractStatus Status { get; set; }
    }
}
