using CacheEngineShared;
using System;

namespace MessageBroker
{


    public class TaiKhoanService : BaseServiceCache<oTaiKhoan>
    {
        public TaiKhoanService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel)
        {
            this.insertItems(new oTaiKhoan[] {
                new oTaiKhoan(){ TaiKHoanId="1", MatKhau = "123", TenTaiKhoan="admin", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="XE_OM_CONG_NGHE" },
                new oTaiKhoan(){ TaiKHoanId="2", MatKhau = "123", TenTaiKhoan="user", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="ECPAY" },
            });
        }
    }

    public class TaiKhoanBehavior : BaseServiceCacheBehavior { public TaiKhoanBehavior(object instance) : base(instance) { } }
}
