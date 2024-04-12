using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doanphanmem.Models;
using static Doanphanmem.Pattern.Proxy;

namespace Doanphanmem.Controllers
{
    public class TaiKhoanController : Controller
    {


        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }

        // GET: TaiKhoan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: TaiKhoan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,sdt,email,DiaChi,NgaySinh,TK,Pass,Roleuser,Hinh")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        //GET: TaiKhoan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: TaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,sdt,email,DiaChi,NgaySinh,TK,Pass,Roleuser,Hinh")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }







        // GET: TaiKhoan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: TaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    //public class TaiKhoanController : Controller
    //{
    //    private IKhachHangProxy khachHangProxy; // Trường lưu trữ Proxy

    //    public TaiKhoanController()
    //    {
    //        this.khachHangProxy = new KhachHangProxy(); // Khởi tạo Proxy trong hàm tạo
    //    }

    //    // GET: TaiKhoan
    //    public ActionResult Index()
    //    {
    //        return View(khachHangProxy.GetKhachHangs()); // Sử dụng Proxy để lấy danh sách KhachHang
    //    }

    //    // GET: TaiKhoan/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        KhachHang khachHang = khachHangProxy.GetKhachHang(id); // Sử dụng Proxy để lấy KhachHang cụ thể
    //        if (khachHang == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(khachHang);
    //    }

    //    // GET: TaiKhoan/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: TaiKhoan/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "MaKH,TenKH,sdt,email,DiaChi,NgaySinh,TK,Pass,Roleuser,Hinh")] KhachHang khachHang)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            khachHangProxy.CreateKhachHang(khachHang); // Sử dụng Proxy để tạo mới KhachHang
    //            return RedirectToAction("Index");
    //        }

    //        return View(khachHang);
    //    }

    //    // GET: TaiKhoan/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        KhachHang khachHang = khachHangProxy.GetKhachHang(id); // Sử dụng Proxy để lấy KhachHang cần chỉnh sửa
    //        if (khachHang == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(khachHang);
    //    }

    //    // POST: TaiKhoan/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "MaKH,TenKH,sdt,email,DiaChi,NgaySinh,TK,Pass,Roleuser,Hinh")] KhachHang khachHang)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            khachHangProxy.EditKhachHang(khachHang); // Sử dụng Proxy để chỉnh sửa KhachHang
    //            return RedirectToAction("Index");
    //        }
    //        return View(khachHang);
    //    }

    //    // GET: TaiKhoan/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        KhachHang khachHang = khachHangProxy.GetKhachHang(id); // Sử dụng Proxy để lấy KhachHang cần xóa
    //        if (khachHang == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(khachHang);
    //    }

    //    // POST: TaiKhoan/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        khachHangProxy.DeleteKhachHang(id); // Sử dụng Proxy để xóa KhachHang
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            // Dispose Proxy nếu cần
    //        }
    //        base.Dispose(disposing);
    //    }
    //}

}
