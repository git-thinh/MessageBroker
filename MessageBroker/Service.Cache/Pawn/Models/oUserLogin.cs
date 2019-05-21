using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttrModelInfo("Thông tin đăng nhập", _API_CONST.USER_LOGIN)]
    public class oUserLogin
    {
        [AttrFieldInfo(1, "Tài khoản Id", AttrDataType.LONG, true, true, true)]
        public long UserId { get; set; }

        [AttrFieldInfo(2, "Tên tài khoản", AttrDataType.STRING, false, true, true)]
        public string Username { get; set; }

        [AttrFieldInfo(3, "Mật khẩu", AttrDataType.STRING)]
        public string Password { get; set; }

        [AttrFieldInfo(4, "Mã thiết bị truy cập", AttrDataType.STRING)]
        public string DeviceCode { get; set; }

        [AttrFieldInfo(5, "Nhóm khách hàng", AttrDataType.STRING)]
        public string CustomerGroup { get; set; }
    }
}
