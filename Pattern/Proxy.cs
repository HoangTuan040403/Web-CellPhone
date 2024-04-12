using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Doanphanmem.Pattern
{
    public class Proxy
    {
        public interface IKhachHangProxy
        {
            //KhachHang GetKhachHang(int? id);
            //void EditKhachHang(KhachHang khachHang);
            IEnumerable<KhachHang> GetKhachHangs();
            KhachHang GetKhachHang(int? id);
            void CreateKhachHang(KhachHang khachHang);
            void EditKhachHang(KhachHang khachHang);
            void DeleteKhachHang(int id);
        }
        public class KhachHangProxy : IKhachHangProxy
        {
            private QL_CHDTEntities db = new QL_CHDTEntities();

          

            public KhachHang GetKhachHang(int? id)
            {
                return db.KhachHangs.Find(id);
            }

            public IEnumerable<KhachHang> GetKhachHangs()
            {
                return db.KhachHangs.ToList();
            }

            public void CreateKhachHang(KhachHang khachHang)
            {
                // Kiểm tra và điều chỉnh dữ liệu nếu cần
                // Ví dụ: kiểm tra role user, ghi log, ...

                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }

            public void EditKhachHang(KhachHang khachHang)
            {
                // Kiểm tra và điều chỉnh dữ liệu nếu cần
                if (khachHang.Roleuser.ToLower() == "Admin")
                {
                    // Nếu người dùng cố gắng đặt quyền là "Admin", thực hiện xử lý tương ứng tại đây
                    // Ví dụ: bạn có thể ghi log, hiển thị thông báo, hoặc không lưu dữ liệu và ném một ngoại lệ
                    throw new Exception("Không được phép đặt quyền là Admin.");
                    
                }

                // Tiếp tục lưu dữ liệu nếu không có vấn đề
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
            }

            public void DeleteKhachHang(int id)
            {
                // Kiểm tra và điều chỉnh dữ liệu nếu cần
                // Ví dụ: kiểm tra role user, ghi log, ...

                KhachHang khachHang = db.KhachHangs.Find(id);
                db.KhachHangs.Remove(khachHang);
                db.SaveChanges();
            }
        }

    }
}