namespace MessageBroker
{
    [AttrModelInfo("Ảnh tài sản", _API_CONST.PAWN_IMAGES)]
    public class oPawnImages
    {
        [AttrFieldInfo(1, "", AttrDataType.INT, true)]
        public int PawnImage_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Pawn_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int PawnImageType_ID { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING)]
        public string Path { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.STRING)]
        public string Host { set; get; }

        [AttrFieldInfo(1, "", AttrDataType.INT)]
        public int Status { set; get; }
    }
}
