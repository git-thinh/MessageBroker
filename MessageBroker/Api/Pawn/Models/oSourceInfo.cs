using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttrModelInfo("Thông tin nguồn", _API_CONST.SOURCE_INFO)]
    public class oSourceInfo
    {
        [AttrFieldInfo(1, "Mã nguồn", AttrDataType.INT, true)]
        public int Source_Id { set; get; }

        [AttrFieldInfo(2, "Tên nguồn", AttrDataType.STRING)]
        public string Name { set; get; }
    }
}
