using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MessageBroker
{
    public class FileUploadDetails 
    {        
        //public bool Ok { set; get; }
        //public string Message { set; get; }
        //public dynamic[] Result { set; get; }
         
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        public string FileCreatedTime { get; set; }
    }
}