namespace MessageShared
{
    public enum oLOG_SCOPE
    {
        REQUEST_URL = 0,
        DB_DLL = 1,
        MESSAGE_WEBAPI = 2,
        GRPC_NOTIFICATION = 3,
        IIS_WEB_API = 4
    }

    public enum oLOG_STATE
    {
        EXCEPTION = 0,
        SUCCESS = 1
    }

    public enum oLOG_ACTION
    {
        API_SEARCH = 0,
        API_PUT_ADD = 1,
        API_PUT_EDIT = 2,
        API_PUT_REMOVE = 3
    }

    public class oLOG
    {
        public string Id { get; set; }

        public oLOG_SCOPE Scope { get; set; }
        public oLOG_STATE State { get; set; }
        public oLOG_ACTION Action { get; set; }

        public string ProjectName { get; set; }
        public string ClassName { get; set; }
        public string FunctionName { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }
        public string ParaJson { get; set; }

        public string MessageText { get; set; }

        public long TimeStart { get; set; }
        public long TimeEnd { get; set; }
    }

    public class oLogResult
    {
        public bool Ok { get; set; }
        public int Code { get; set; }
        public string MessageText { get; set; }
    }
}
