using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class _VALID
    {
        public static bool beAValidPhone(string phone)
        {
            int n;
            return phone.Length > 9 && phone.Length < 13 && int.TryParse(phone, out n);
        }

        public static bool beAValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
