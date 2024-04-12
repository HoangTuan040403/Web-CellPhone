using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI;
using Doanphanmem.Models;
using PagedList;

namespace Doanphanmem.Controllers
{
    public class SanPhamsController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // danh sách đơn hàng của khách hàng
        public ActionResult DonHangKH(int? IDCus)
        {
            // Truy vấn dữ liệu từ cơ sở dữ liệu dựa trên IDCus
            IDCus = (int)Session["UserID"];
            var orders = db.DONDATHANGs
                .Where(o => o.MaKH == IDCus)
                .Include(o => o.CTDATHANGs)  
                .ToList();
            return View(orders);
        }
        public ActionResult HoanThanhDH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donhang = db.DONDATHANGs.Find(id);
            //donhang.Dagiao = true;

            if (donhang.GiaoHang == false)
            {
                donhang.GiaoHang = true;
            }
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("DonHangKH");
        }

        public ActionResult HuyDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donhang = db.DONDATHANGs.Find(id);
            //donhang.Dagiao = true;

            if (donhang.HuyDon == false)
            {
                donhang.HuyDon = true;
            }
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("DonHangKH");
        }


        public ActionResult getCus(int ID)
        {
            var cus = db.SanPhams
                  .Where(s => s.MaSP == ID)
                  .Select(s => s.TenSP)
                  .FirstOrDefault();
            return View(cus);
        }

        public ActionResult getDanhsachLoaiSP(int ID)
        {
            var orders = db.SanPhams
                .Where(o => o.MaLoai == ID)
                .ToList();
            return View(orders);
        }


        //giảm giá sản phẩm
        // thông tin chi tiết sản phẩm
        public ActionResult SP_giamgia(int? id)
        {
            // Lấy thông tin sản phẩm từ bảng SanPham
            //var sanPham = db.SanPham.FirstOrDefault(s => s.MaSP == maSanPham);
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham != null)
            {
                // Kiểm tra xem sản phẩm có trong bảng Voucher không
                Vourcher voucher = db.Vourchers.Find(id);

                if (voucher != null)
                {
                    // Lấy thông tin giảm giá từ voucher
                    int giamGia = (int)(sanPham.GiaSp * voucher.Uudai /100);
                    // Tính giá sản phẩm với giảm giá
                    int giaSauGiamGia = (int)(sanPham.GiaSp - giamGia);
                    // Truyền giá sản phẩm đã giảm giá vào view để hiển thị
                    ViewBag.GiaSauGiamGia = giaSauGiamGia;
                }

                return View(sanPham);
            }
            // Xử lý khi sản phẩm không tồn tại
            return RedirectToAction("Index");
        }


        // GET: SanPhams
        public ActionResult Index(String SearchString, string sortOrder)
        {
            ViewBag.MaSpSortParm = String.IsNullOrEmpty(sortOrder) ? "masp_desc" : "";
            ViewBag.TenSpSortParm = sortOrder == "tensp" ? "tensp_desc" : "tensp";           
            //var sanPham = db.SanPhams.Include(s => s.Mau).Include(s => s.PhanLoai);         
            //Tạo Products và có tham chiếu đến Loại sản phẩm:
            var sanPham = db.SanPhams.Include(p => p.PhanLoai);
            if (!String.IsNullOrEmpty(SearchString))
            {
                sanPham = sanPham.Where(s => s.TenSP.Contains(SearchString));
            }

            else
            {
                Console.WriteLine("Không tìm thấy sản phẩm nào");
            }
            switch (sortOrder)
            {
                case "masp_desc":
                    sanPham = sanPham.OrderByDescending(s => s.MaSP);
                    break;
                case "tensp":
                    sanPham = sanPham.OrderBy(s => s.TenSP);
                    break;
                case "tensp_desc":
                    sanPham = sanPham.OrderByDescending(s => s.TenSP);
                    break;
                default:
                    sanPham = sanPham.OrderBy(s => s.MaSP);
                    break;
            }

            //ViewBag.ListCategory = db.PhanLoais.ToList();
            return View(sanPham.ToList());
        }




        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaMau = new SelectList(db.Maus, "Mamau", "Tenmau");
            ViewBag.MaLoai = new SelectList(db.PhanLoais, "MaLoai", "Tenloai");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso,Soluongton,MaLoai,MaMau")] SanPham sanPham,
            HttpPostedFileBase Hinh1)
        {

            if (ModelState.IsValid)
            {
                if (Hinh1 != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(Hinh1.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    //Lưu tên
                    sanPham.Hinh1 = fileName;
                    //Save vào Images Folder
                    Hinh1.SaveAs(path);
                }
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.MaMau = new SelectList(db.Maus, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoais, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaMau = new SelectList(db.Maus, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoais, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso, Soluongton,MaLoai,MaMau")] SanPham sanPham,
             HttpPostedFileBase Hinh1)
        {
            if (ModelState.IsValid)
            {
                if (Hinh1 != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(Hinh1.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    //Lưu tên
                    sanPham.Hinh1 = fileName;
                    //Save vào Images Folder
                    Hinh1.SaveAs(path);
                }
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            ViewBag.MaMau = new SelectList(db.Maus, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoais, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult laychitietsp(int id)
        //{
        //   var sp = db.SanPhams.FirstOrDefault(s=>s.MaSP ==  id);
        //    laysanphamtt(sp.MaLoai);
        //    return View(sp);
        //}

        // top 5 sản phẩm bán chạy nhất 
        public ActionResult XacNhanHuyDon()
        {
            return View();
        }
        public ActionResult laysanphamtt()
        {
            var sanpham = db.SanPhams
                .OrderBy(p => p.Soluongton) // Sắp xếp sản phẩm theo số lượng tồn tăng dần
                .Take(5) // Lấy 5 sản phẩm có số lượng tồn thấp nhất
                .ToList();
            return PartialView(sanpham);
        }
        public ActionResult MauSp(int id)
        {
            var s = db.SanPhams.Where(sanpham => sanpham.Mamau == id).ToList();
            return View("Index", s);
        }


        public ActionResult laymausp()
        {
            var mau = db.Maus.ToList();
            return PartialView(mau);
        }

        public ActionResult layloaisp()
        {
            var loaisp = db.PhanLoais.ToList();
            return PartialView(loaisp);
        }

        //
        public ActionResult LoaiSP(int id)
        {
            var s = db.SanPhams.Where(sanpham => sanpham.MaLoai == id).ToList();
            return View("Index", s);
        }

        public ActionResult layloaisp_layout()
        {
            var loaisp = db.PhanLoais.ToList();
            return PartialView(loaisp);
        }
        public ActionResult locgia(int gia, int SortPrice = 0)
        {
            List<SanPham> lsproducts = new List<SanPham>();
            var sp = db.SanPhams.Where(s => s.GiaSp > 0).ToList();

            if(SortPrice == 1)
            {
                 sp = db.SanPhams.Where(s => s.GiaSp > 0 && s.GiaSp <= 500000).ToList();
            }
            ViewBag.SortPrice = SortPrice;
            if (sp != null)
            {
                return View(sp);
            }
            else
            {
                return View("NoProductsFound");
            }

        }


        public ActionResult IndexAdmin(String SearchString, int? page)
        {
            var sanPham = db.SanPhams.Include(s => s.Mau).Include(s => s.PhanLoai);
            if (!String.IsNullOrEmpty(SearchString))
            {
                sanPham = sanPham.Where(s => s.TenSP.Contains(SearchString));
            }
            {
                Console.WriteLine("Không tìm thấy sản phẩm nào");
            }
            var dsSach = sanPham.ToList();
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(dsSach.OrderBy(donhang => donhang.MaSP).ToPagedList(pageNum, pageSize));
            //var sp = db.SanPhams.Include(s => s.PhanLoai);
            //return View(sp.ToList());
        }
       
        public ActionResult TrangChuAdmin()
        {
            return View();
        }


        public ActionResult DonHangChiTietKH(int? id)
        {
            // Thực hiện truy vấn cơ sở dữ liệu để lấy danh sách chi tiết đơn hàng dựa vào SODH

            var cTDATHANGs = db.CTDATHANGs.Include(c => c.DONDATHANG).Include(c => c.SanPham).Where(c => c.SODH == id);
            return View(cTDATHANGs.ToList());
        }
    }

}

