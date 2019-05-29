namespace MessageBroker
{
    [AttrModelInfo("Thông tin phiên bản ứng dụng", _API_CONST.APP_VERSION)]
    public class oAppVersion
    {
        [AttrFieldInfo(1, "", AttrDataType.INT, true, true, true)]
        public long AppVersion_ID { set; get; }                
        public string AppVersion { set; get; }
        public string AppName { set; get; }
        public string Message { set; get; }
        public string LinkAndroid { set; get; }
        public string LinkIOS { set; get; }
        public string LinkWebDocs { set; get; }
        public long DateRelease { set; get; }
        public int Status { set; get; }
    }
}
