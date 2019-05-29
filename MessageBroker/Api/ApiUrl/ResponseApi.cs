using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageBroker
{
    public class ResponseApi
    {

        public int code { get; set; }
        public string message { get; set; }
        public string systemMessage { get; set; }

    }
}