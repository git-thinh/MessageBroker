using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class oUserLogin
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceCode { get; set; }
        public string CustomerGroup { get; set; }
    }
}
