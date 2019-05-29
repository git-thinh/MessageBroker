using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Api.ApiUrl
{
    public class oCheckTransactionRequest
    {
        //Mã yêu cầu đã gửi sang F88 trước đó
        public string TransactionCode { get; set; }
        //Ngày thực hiện giao dịch đó
        public string TransactionDate { get; set; }//yyyyMMdd
        //ID hợp đồng F88                                              
        public string PawnID { get; set; }
        //Loại giao dịch  
        public string TransactionType { get; set; }
        public string ReferenceId { get; set; }


        public string requestId { set; get; }
        public string partnerCode { set; get; }
        public string locationCode { set; get; }

        public string signKey { set; get; }

    }
}
