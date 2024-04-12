using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doanphanmem.Models;

namespace Doanphanmem.Controllers
{
    public class KhachHangController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: KhachHang
 

        // GET: KhachHang/Edit/5
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,sdt,email,DiaChi,NgaySinh,TK,Pass,Roleuser,Hinh")] KhachHang khachHang,
            HttpPostedFileBase Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(Hinh.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Image/Loai"), fileName);
                    //Lưu tên
                    khachHang.Hinh = fileName;
                    //Save vào Images Folder
                    Hinh.SaveAs(path);
                }
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","SanPhams");
            }
            return View(khachHang);
        }
      
    }
}
