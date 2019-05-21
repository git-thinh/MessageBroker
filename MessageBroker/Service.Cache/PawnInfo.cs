namespace MessageBroker
{
    public class oCuahang
    {
        public int MaCuaHang { set; get; }
        public string TenCuaHang { set; get; }
        public string DiaChi { set; get; }
        public string DienThoai { set; get; }
    }

    public class oNganHang
    {
        public int MaNganHang { set; get; }
        public int TenNganHang { set; get; }
        public string[] ChiNhanh { set; get; }
    }

    public enum oTaiSanLoai
    {
        CMND = 1,
        SO_HO_KHAU = 2,
        GIAY_TO_XE = 3,
        HOA_DON_DIEN = 4,
        LOAI_KHAC = 99
    }

    public enum oHopDongTrangThai
    {
        HOP_DONG_NHAP_DOI_DUYET = 1,
        HOP_DONG_DA_DUYET_DOI_GIAI_NGAN = 2,
        HOP_DONG_THANH_CONG = 3,
    }

    public class oNguoiLienHe
    {
        public string HoTen { set; get; }
        public int DienThoai { set; get; }
        public string DiaChi { get; set; }
    }



    public class oThongTinNhanThan
    {
        public oNguoiLienHe[] LangRieng { set; get; }
        public oNguoiLienHe[] DongNghiep { set; get; }
        public oNguoiLienHe[] NguoiThanSoHoKhau { set; get; }
    }

    public class oAnhTaiSan
    {
        public string[] SoHoKhau { set; get; }
        public string[] GiayToXe { set; get; }
        public string[] HoaDonDien { set; get; }
        public string[] LoaiKhac { set; get; }
    }

    [AttrModelInfo("Thông tin hợp đồng")]
    public class oPawnInfo
    {
        //-------------------------------------------
        // Thong tin khoan vay
        [AttrFieldInfo("Hợp đồng Id", AttrDataType.INT, true, true)]
        public int PawnID { get; set; } // Hop dong id

        [AttrFieldInfo("Mã hợp đồng", AttrDataType.STRING, false, true)]
        public string PawnCode { get; set; } //Ma hop dong

        [AttrFieldInfo("Số tiền vay", AttrDataType.LONG)]
        public long LoanAmount { get; set; } //So tien vay

        [AttrFieldInfo("Thời hạn vay", AttrDataType.INT)]
        public int LoanTerm { get; set; } //Thoi han vay tien

        //-------------------------------------------
        // Thong tin khach hang
        public string HoTen { set; get; }
        public string CMND_CCCD { get; set; }
        public int CMND_NgayCap_YYYYMMDD { get; set; }
        public string CMND_NoiCap { get; set; }
        public int DienThoai { set; get; }
        public string DiaChi { get; set; }
        public oNganHang Nganhang { get; set; }
        //-------------------------------------------
        public oThongTinNhanThan ThongTinThanNhan { get; set; }
        public oCuahang CuaHangTatToanHopDong { get; set; }
        public oAnhTaiSan AnhTaiSan { get; set; }

        public oTaiSanLoai[] TaiSan { get; set; }
        public oHopDongTrangThai TrangThaiDopDong { get; set; }
    }

}
