using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.ApiUrl
{
    public class ApiConstants
    {

        public class Code
        {
            public const int Success = 200;

            public const int Fail = 400;

            public const int NotFound = 400;

            public const int AccountNotExist = 201;

            public const int AccountNotActive = 206;

            public const int AccountExist = 202;

            public const int InvalidEmail = 203;

            public const int LoginFailed = 204;

            public const int RegisterFailed = 205;

            public const int Default = 222;

            public const int Exist = 300;

            public const int Exception = -1;

            public const int Unauthorized = 401;

            public const int InvalidRequest = 995;
            public const int InvalidSignature = 996;
            public const int DuplicateRequest = 997;

        }

    }
}
