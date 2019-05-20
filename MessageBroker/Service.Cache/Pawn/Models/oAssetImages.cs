namespace MessageBroker
{
    [AttrModelInfo("Ảnh tài sản", _API_CONST.ASSET_IMAGES)]
    public class oAssetImages
    {
        [AttrFieldInfo(1, "Sổ hộ khẩu", AttrDataType.STRING_ARRAY_IMAGES)]
        public string[] RegistrationBook { set; get; }

        [AttrFieldInfo(2, "Giấy tờ xe", AttrDataType.STRING_ARRAY_IMAGES)]
        public string[] VehicleRegistration { set; get; }

        [AttrFieldInfo(3, "Hóa đơn điện", AttrDataType.STRING_ARRAY_IMAGES)]
        public string[] ElectricBill { set; get; }

        [AttrFieldInfo(4, "Loại khác", AttrDataType.STRING_ARRAY_IMAGES)]
        public string[] Others { set; get; }
    }
}
