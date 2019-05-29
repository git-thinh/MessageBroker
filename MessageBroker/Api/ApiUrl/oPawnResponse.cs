using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.ApiUrl.oPOS
{
    public class oPawnResponse
    {
        public string CustomerName { get; set; }
        public string PawnID { get; set; }
        public string PawnCode { get; set; }
        public string Created { get; set; }
        public string RefereceCode { get; set; }
        public string PaymementRef { get; set; }
    }


    public class RootObject
    {
        public List<oPawnResponse> PawnResponse { get; set; }
    }

}
