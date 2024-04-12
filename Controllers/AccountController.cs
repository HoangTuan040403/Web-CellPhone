using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Controllers
    
{
    
    //public interface IAuthenticationHandler
    //{
    //    bool Authenticate(UserModel model);
    //}
    //public class LoginHandler : IAuthenticationHandler
    //{
    //    public bool Authenticate(UserModel model)

    //    {
    //        QL_CHDTEntities db=new QL_CHDTEntities();
    //        // Xác thực thông tin đăng nhập
    //        // Trả về true nếu đăng nhập thành công, ngược lại trả về false
    //        var user = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK && u.Pass == model.Pass);
    //        if (user != null)
    //        {
    //            //ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
    //            HttpContext.Current.Session["taikhoan"] = user;
    //            // Đăng nhập thành công, lưu thông tin người dùng vào Session
    //            HttpContext.Current.Session["UserID"] = user.MaKH;
    //            HttpContext.Current.Session["UserName"] = user.TenKH;
    //            HttpContext.Current.Session["UserRole"] = user.Roleuser;
    //            // Kiểm tra vai trò của người dùng
    //            if (user.Roleuser == "Admin")
    //            {
    //                // Nếu là nhân viên, thiết lập tên mặc định
    //                HttpContext.Current.Session["DisplayName"] = "Admin";
    //            }
    //            else
    //            {
    //                // Nếu không phải nhân viên, để người dùng điền tên
    //                HttpContext.Current.Session["DisplayName"] = "";
    //            }
    //           return true;
    //        }

    //        // Đăng nhập thất bại, hiển thị thông báo lỗi
    //        return false;
    //    }
    //}

    //public class RegistrationHandler : IAuthenticationHandler
    //{
    //    public bool Authenticate(UserModel model)
    //    {
    //        QL_CHDTEntities db= new QL_CHDTEntities();
    //        // Kiểm tra và lưu thông tin đăng ký vào cơ sở dữ liệu
        
    //            var existingUser = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK);
    //            if (existingUser != null)
    //            {
    //                ModelState.AddModelError("", "Tài khoản đã tồn tại."); // Thông báo lỗi
    //                return false;
    //            }
    //            else
    //            {
    //                // Lưu đối tượng KhachHang vào cơ sở dữ liệu
    //                db.KhachHangs.Add(model);
    //                db.SaveChanges();
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}
    public class AccountController : Controller
    {
        //public interface IAuthenticationHandler
        //{
        //    IAuthenticationHandler NextHandler { get; set; }
        //    KhachHang Authenticate(UserModel model);
        //}
        //private QL_CHDTEntities db = new QL_CHDTEntities();


        //public class LoginHandler : IAuthenticationHandler
        //{
        //    private readonly QL_CHDTEntities db;

        //    public LoginHandler(QL_CHDTEntities dbContext)
        //    {
        //        db = dbContext;
        //    }

        //    public IAuthenticationHandler NextHandler { get; set; }

        //    public KhachHang Authenticate(UserModel model)
        //    {
        //        var user = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK && u.Pass == model.Pass);
        //        return user ?? NextHandler?.Authenticate(model);
        //    }
        //}

        //public class RegisterHandler : IAuthenticationHandler
        //{
        //    private readonly QL_CHDTEntities db;

        //    public RegisterHandler(QL_CHDTEntities dbContext)
        //    {
        //        db = dbContext;
        //    }

        //    public IAuthenticationHandler NextHandler { get; set; }

        //    public KhachHang Authenticate(UserModel model)
        //    {
        //        var existingUser = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK);
        //        if (existingUser == null)
        //        {
        //            // Lưu thông tin đăng ký vào cơ sở dữ liệu
        //            var newUser = new KhachHang { TK = model.TK, Pass = model.Pass, /*... other properties ...*/ };
        //            db.KhachHangs.Add(newUser);
        //            db.SaveChanges();
        //            return newUser;
        //        }
        //        if (NextHandler != null)
        //        {
        //            return NextHandler.Authenticate(model);
        //        }
        //        return null;
        //    }
        //}



        //private readonly IAuthenticationHandler authenticationHandler;

        //public AccountController()
        //{
        //    // Tạo chuỗi xử lý với các đối tượng xử lý cụ thể
        //    var loginHandler = new LoginHandler(db);
        //    var registerHandler = new RegisterHandler(db);

        //    // Thiết lập chuỗi xử lý
        //    //loginHandler.NextHandler = registerHandler;

        //    authenticationHandler = registerHandler;
        //}

        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(UserModel model)
        //{
        //    var user = authenticationHandler.Authenticate(model);

        //    if (user != null)
        //    {
        //        // Xử lý khi đăng nhập thành công
        //        // ...
        //        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
        //        Session["taikhoan"] = user;
        //        // Đăng nhập thành công, lưu thông tin người dùng vào Session
        //        Session["UserID"] = user.MaKH;
        //        Session["UserName"] = user.TenKH;
        //        Session["UserRole"] = user.Roleuser;
        //        // Kiểm tra vai trò của người dùng
        //        if (user.Roleuser == "Admin")
        //        {
        //            // Nếu là nhân viên, thiết lập tên mặc định
        //            Session["DisplayName"] = "Admin";
        //        }
        //        else
        //        {
        //            // Nếu không phải nhân viên, để người dùng điền tên
        //            Session["DisplayName"] = "";
        //        }
        //        if (Session["UserRole"] == null)
        //            return RedirectToAction("Index", "SanPhams");
        //        //if (Session["UserRole"] == "Admin")
        //        else if (user.Roleuser.ToString() == "Admin")
        //            return RedirectToAction("./trangchuadmin", "SanPhams");

        //        else
        //            return RedirectToAction("Login", "Account");
        //        //return RedirectToAction("Index", "SanPhams");

        //    }

        //    // Đăng nhập thất bại, hiển thị thông báo lỗi
        //    ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
        //    return View(model);
        //}


        //[HttpPost]
        //public ActionResult Register(KhachHang model)
        //{
        //    // Chỉ gọi phương thức Authenticate để xử lý đăng ký
        //    var user = authenticationHandler.Authenticate(new UserModel { TK = model.TK, Pass = model.Pass });

        //    if (ModelState.IsValid)
        //    {
        //        // Thực hiện kiểm tra và lưu thông tin đăng ký vào cơ sở dữ liệu
        //        var existingUser = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK);
        //        if (existingUser != null)
        //        {
        //            ModelState.AddModelError("", "Tài khoản đã tồn tại."); // Thông báo lỗi
        //        }
        //        else
        //        {
        //            // Lưu đối tượng KhachHang vào cơ sở dữ liệu
        //            db.KhachHangs.Add(model);
        //            db.SaveChanges();

        //            ViewBag.ThongBao = "Chúc mừng, bạn đã đăng ký thành công!";
        //            return View("Login");
        //        }
        //    }

        //    // Đăng ký thất bại, hiển thị thông báo lỗi
        //    return View(model);
        //}
        QL_CHDTEntities db = new QL_CHDTEntities();
        public ActionResult Logout()
        {

            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("UserRole");
            Session.Remove("taikhoan");
            return RedirectToAction("Index", "SanPhams");
        }

        //public interface IProxy
        //{
        //    void PerformAction();
        //}

        //public interface IProxyManager
        //{
        //    IProxy AuthenticateAndRedirect(string userRole);
        //}

        //public class ProxyManager : IProxyManager
        //{
        //    public IProxy AuthenticateAndRedirect(string userRole)
        //    {
        //        switch (userRole.ToLower())
        //        {
        //            case "admin":
        //                return new AdminProxy();                 
        //            case "customer":
        //                return new CustomerProxy();
                   
        //        }
        //    }
        //}





        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            var user = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK && u.Pass == model.Pass);
            if (user != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["taikhoan"] = user;
                // Đăng nhập thành công, lưu thông tin người dùng vào Session
                Session["UserID"] = user.MaKH;
                Session["UserName"] = user.TenKH;
                Session["UserRole"] = user.Roleuser;
                // Kiểm tra vai trò của người dùng
                if (user.Roleuser == "Admin")
                {
                    // Nếu là nhân viên, thiết lập tên mặc định
                    Session["DisplayName"] = "Admin";
                }
                else
                {
                    // Nếu không phải nhân viên, để người dùng điền tên
                    Session["DisplayName"] = "";
                }
                if (Session["UserRole"] == null)
                    return RedirectToAction("Index", "SanPhams");
                //if (Session["UserRole"] == "Admin")
                else if (user.Roleuser.ToString() == "Admin")
                    return RedirectToAction("./trangchuadmin", "SanPhams");

                else
                    return RedirectToAction("Login", "Account");
            }

            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(model);
        }
        // Đăng ký
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                // Thực hiện kiểm tra và lưu thông tin đăng ký vào cơ sở dữ liệu
                var existingUser = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại."); // Thông báo lỗi
                }
                else
                {
                    // Lưu đối tượng KhachHang vào cơ sở dữ liệu
                    db.KhachHangs.Add(model);
                    db.SaveChanges();

                    ViewBag.ThongBao = "Chúc mừng, bạn đã đăng ký thành công!";
                    return View("Login");
                }
            }

            // Đăng ký thất bại, hiển thị thông báo lỗi
            return View(model);
        }


        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult ChatOnline()
        {
            return View();
        }




        //Tạo action khác để đảm bảo người dùng đã đăng nhập(điều hướng từ action Login sau khi đăng nhập thành công)
        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                // Điều hướng về trang đăng nhập nếu người dùng chưa đăng nhập
                return RedirectToAction("Login");
            }

            // Lấy thông tin người dùng từ Session và hiển thị trong section
            var userModel = new UserModel
            {
                MaKH = (int)Session["UserID"],
                TenKH = Session["UserName"].ToString(),
                Roleuser = Session["UserRole"].ToString()
            };

            return View(userModel);

        }

    }
}