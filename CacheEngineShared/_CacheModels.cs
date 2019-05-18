using System;

namespace CacheEngineShared
{
    public class oCacheModel
    {
        public string ServiceName { set; get; }
        public oCacheField[] Fields { set; get; }

        public oCacheModel()
        {
            this.ServiceName = string.Empty;
            this.Fields = new oCacheField[] { };
        }

        public oCacheModel(string serviceName, oCacheField[] fields) : base()
        {
            this.ServiceName = serviceName;
            this.Fields = fields;
        }
    }

    public class oCacheField
    {
        public string name { set; get; }
        public string type { set; get; }
        public bool iskey { set; get; }
    }

    public enum oCacheResultCode
    {
        FAIL = 0,
        SUCCESS = 1,
        FAIL_EXCEPTION = 1000,
        FAIL_INPUT_NULL = 1001,
        FAIL_NOT_FOUND = 1002,
        FAIL_CONVERT_JSON = 1002,
    }

    public class oCacheRequest
    {
        public string RequestId { set; get; }
        public string ServiceName { set; get; }
        public string Conditions { set; get; }
        public string OrderbyName { set; get; }
        public int PageNumber { set; get; }
        public int PageSize { set; get; }

        public oCacheRequest(string serviceName, string conditions) {
            this.ServiceName = serviceName;
            this.Conditions = conditions;
            this.OrderbyName = string.Empty;
        }
    }

    public class oCacheResult
    {
        public bool Ok { set; get; }

        public oCacheResultCode Code { set; get; }

        public string Message { set; get; }
        public dynamic[] Result { set; get; }
        public oCacheRequest Request { set; get; }
        public int TotalItems { set; get; }
        public int CountResult { set; get; }

        public oCacheResult(oCacheRequest request)
        {
            this.Request = request;

            this.Ok = false;
            this.Code = oCacheResultCode.FAIL;
            this.Message = string.Empty;
            this.Result = new string[] { };
            this.TotalItems = 0;
            this.CountResult = 0;
        }

        public oCacheResult ToOk(dynamic[] results, int totalItems)
        {
            this.Ok = true;
            this.Code = oCacheResultCode.SUCCESS;
            this.Result = results;
            this.TotalItems = totalItems;
            this.CountResult = results.Length;
            return this;
        }

        public oCacheResult ToOkEmpty()
        {
            this.Ok = true;
            this.Code = oCacheResultCode.SUCCESS;
            return this;
        }

        public oCacheResult ToFailException(string message, int totalItems = 0)
        {
            this.Code = oCacheResultCode.FAIL_EXCEPTION;
            this.TotalItems = totalItems;
            this.Message = message;
            return this;
        }

        public oCacheResult ToFailInputNULL(string message = "The input is NULL")
        {
            this.Code = oCacheResultCode.FAIL_INPUT_NULL;
            this.Message = message;
            return this;
        }
        public oCacheResult ToFailNotFound(string message = "Cannot find object")
        {
            this.Code = oCacheResultCode.FAIL_NOT_FOUND;
            this.Message = message;
            return this;
        }
        public oCacheResult ToFailConvertJson(string message = "Cannot convert JSON", string title = "")
        {
            this.Code = oCacheResultCode.FAIL_CONVERT_JSON;
            this.Message = string.IsNullOrWhiteSpace(title) ? message : (title + Environment.NewLine + message);
            return this;
        }
    }
}
