﻿using Doanphanmem.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QL_CHDTEntities db = new QL_CHDTEntities();

        public ActionResult GiohangPartial()
        {
            ViewBag.TongSl = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        // GET: Cart
        public List<MatHangMua> Index()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            if (gioHang == null || gioHang.Count == 0)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
                //return View("CartNoProduct");
            }
            //ViewBag.TongSL = TinhTongSL();
            //ViewBag.TongTien = TinhTongTien();
            return gioHang;
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = Index();
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["totalCart"] = 0;
                //return View("CartNoProduct");
                return RedirectToAction("Index", "SanPhams");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();

            //Session["totalCart"] = (Session["GioHang"] as List<MatHangMua>).Count;

            Session["totalCart"] = gioHang.Count;
            return View(gioHang);

        }


        public ActionResult DeleteProduct(int MaSP)
        {

            List<MatHangMua> gioHang = Index();
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaDT == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if (gioHang.Count == 0)
                return RedirectToAction("Index", "SanPhams");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult AddProduct(int MaSP)
        {
            List<MatHangMua> giohang = Index();
            MatHangMua sanpham = giohang.FirstOrDefault(s => s.MaDT == MaSP);
            Vourcher exit = db.Vourchers.Find(MaSP);
            if (sanpham == null)
            {
                sanpham = new MatHangMua(MaSP);
                if (exit != null)
                {
                    // Lấy thông tin giảm giá từ voucher
                    int giamGia = (int)(sanpham.Dongia * exit.Uudai / 100);
                    // Tính giá sản phẩm với giảm giá
                    int giaSauGiamGia = (int)(sanpham.Dongia - giamGia);
                    // Truyền giá sản phẩm đã giảm giá vào view để hiển thị
                    ViewBag.GiaSauGiamGia = giaSauGiamGia;
                    sanpham.Dongia = giaSauGiamGia;
                }
                giohang.Add(sanpham);

            }
            else
            {
                sanpham.Soluong++;
            }
            return RedirectToAction("Index", "SanPhams", new { id = MaSP });
        }

        public ActionResult UpdateCart(int MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = Index();
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                sanpham.Soluong = SoLuong;

            }
            return RedirectToAction("HienThiGioHang"); ;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
            {
                TongTien = (double)gioHang.Sum(sp => sp.Total());
            }
            return TongTien;
        }

        private double TinhTongTienvnd()
        {
            double TongTien = 0;
            decimal b = 23000;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
            {
                TongTien = (double)gioHang.Sum(sp => sp.Total());
            }
            decimal c = (decimal)TongTien / b;
            // Round the result to the nearest integer by casting to int
            int roundedResult = (int)Math.Round((double)c);
            return roundedResult;
        }
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.Soluong);
            return tongSL;
        }
        //dat hang
        public ActionResult DatHang()
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            List<MatHangMua> giohang = Index();
            if (giohang == null || giohang.Count == 0)
                return RedirectToAction("Index", "SanPhams");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            foreach (var sanpham in giohang)
            {
                // Tìm sản phẩm trong cơ sở dữ liệu dựa trên sanpham.MaDT (ID sản phẩm)
                var productInDB = db.SanPhams.FirstOrDefault(p => p.MaSP == sanpham.MaDT);
                if (productInDB != null)
                {
                    // Giảm số lượng tồn kho đi số lượng sản phẩm đã đặt hàng
                    productInDB.Soluongton = productInDB.Soluongton - sanpham.Soluong;
                }
            }
            db.SaveChanges();
            return View(giohang);
        }
        // xử lý đặt hàng
        public ActionResult DongYDatHang(bool isDirectPayment)
        {
            // thanh toán trực tiếp
            if (isDirectPayment)
            {
                KhachHang kh = Session["taikhoan"] as KhachHang;
                List<MatHangMua> giohang = Index();
                // thêm dữ liệu vào đơn hàng
                DONDATHANG donhang = new DONDATHANG();
                donhang.MaKH = kh.MaKH;
                donhang.NgayDH = DateTime.Now;
                donhang.Trigia = (Decimal)TinhTongTien();
                donhang.Dagiao = false;
                donhang.Tennguoinhan = kh.TenKH;
                donhang.Diachinhan = kh.DiaChi;
                donhang.Dienthoainhan = kh.sdt.ToString();
                donhang.HTThanhtoan = true;
                donhang.HTGiaohang = false;
                donhang.GiaoHang = false;
                donhang.HuyDon = false;
                db.DONDATHANGs.Add(donhang);
                db.SaveChanges();
                // thêm vào chi tiết đơn hàng
                foreach (var sanpham in giohang)
                {
                    CTDATHANG ct = new CTDATHANG();
                    {
                        ct.SODH = donhang.SODH;
                        ct.MaSP = sanpham.MaDT;
                        ct.Soluong = sanpham.Soluong;
                        ct.Dongia = (decimal)TinhTongTien();
                    }
                    db.CTDATHANGs.Add(ct);
                    //Session["GioHang"] = null;
                    //foreach (var p in db.SanPhams.Where(s => s.MaSP == ct.MaSP)) // lấy ID Product có trong giỏ hàng
                    //{
                    //    var update_quan_pro = p.Soluongton - ct.Soluong; //Số lượng tồn mới  = số lượng tồn - số lượng đã mua 
                    //    if (p.Soluongton > 0)
                    //    {
                    //        p.Soluongton = (int)update_quan_pro; //Thực hiện cập nhật lại số lượng tồn cho cột Quantity của bảng Product

                    //    }
                    //}
                }
                db.SaveChanges();
                //xóa giỏ hàng
                Session["GioHang"] = null;
                return RedirectToAction("HoanThanhDonHang");
                

            }
            else
            {
                return RedirectToAction("PaymentWithPaypal", "GioHang");
            }
        }
        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
        // xử lý thanh toán qua paypal
        public ActionResult Paypal()
        {
            // thanh toán thành công
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            KhachHang kh = Session["taikhoan"] as KhachHang;
            List<MatHangMua> giohang = Index();

            // thêm dữ liệu vào đơn hàng
            DONDATHANG donhang = new DONDATHANG();
            donhang.MaKH = kh.MaKH;
            donhang.NgayDH = DateTime.Now;
            donhang.Trigia = (Decimal)TinhTongTien();
            donhang.Dagiao = false;
            donhang.Tennguoinhan = kh.TenKH;
            donhang.Diachinhan = kh.DiaChi;
            donhang.Dienthoainhan = kh.sdt.ToString();
            donhang.HTThanhtoan = false;
            donhang.HTGiaohang = false;
            donhang.GiaoHang = false;
           
            db.DONDATHANGs.Add(donhang);
            db.SaveChanges();
            // thêm vào chi tiết đơn hàng
            foreach (var sanpham in giohang)
            {
                CTDATHANG ct = new CTDATHANG();
                ct.SODH = donhang.SODH;
                ct.MaSP = sanpham.MaDT;
                ct.Soluong = sanpham.Soluong;
                ct.Dongia = (decimal)TinhTongTien();
                db.CTDATHANGs.Add(ct);
            }
            db.SaveChanges();
            //xóa giỏ hàng
            Session["GioHang"] = null;
            return View("HoanThanhDonHang");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList
            {
                items = new List<Item>
                {
                    new Item
                    {
                        name = "Item Name",
                        currency = "USD",
                        price = TinhTongTienvnd().ToString(),
                        quantity = "1",
                        sku = "sku"
                    }
                }
            };

            var payer = new Payer
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var details = new Details
            {
                tax = "0", 
                shipping = "0", 
                subtotal = TinhTongTienvnd().ToString() 
            };

            var amount = new Amount
            {
                currency = "USD",
                total = TinhTongTienvnd().ToString(), // Tổng giá trị thanh toán trong USD
                details = details
            };

            var transactionList = new List<Transaction>
            {
                new Transaction
                {
                    description = "Invoice",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };

            var payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }

    }
}