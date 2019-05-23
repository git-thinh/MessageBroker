using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CacheEngineShared
{
    public class _DB_CONST
    {
        public static string get_connectString_Mobility()
        {
            return ConfigurationManager.ConnectionStrings["DB_UPDATE_DATA"].ConnectionString.ToString();
        }

        public static string get_connectString_POS()
        {
            return ConfigurationManager.ConnectionStrings["DB_POS"].ConnectionString.ToString();
        }
    }
}
