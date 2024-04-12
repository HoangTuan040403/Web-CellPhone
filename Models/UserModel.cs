using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Models
{
    public class UserModel
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string Roleuser { get; set; }
        // Các trường thông tin khác của người dùng
        public string TK { get; set; }
        public string Pass { get; set; }
    }


    //public interface IUserModel
    //{
    //    int MaKH { get; set; }
    //    string TenKH { get; set; }
    //    string Roleuser { get; set; }
    //    string TK { get; set; }
    //    string Pass { get; set; }
    //}

    //public class UserModel : IUserModel
    //{
    //    public int MaKH { get; set; }
    //    public string TenKH { get; set; }
    //    public string Roleuser { get; set; }
    //    public string TK { get; set; }
    //    public string Pass { get; set; }
    //}

    //public class UserModelProxy : IUserModel
    //{
    //    private readonly IUserModel _userModel;

    //    public UserModelProxy(IUserModel userModel)
    //    {
    //        _userModel = userModel;
    //    }

    //    public int MaKH
    //    {
    //        get { return _userModel.MaKH; }
    //        set { _userModel.MaKH = value; }
    //    }

    //    public string TenKH
    //    {
    //        get { return _userModel.TenKH; }
    //        set { _userModel.TenKH = value; }
    //    }

    //    public string Roleuser
    //    {
    //        get { return _userModel.Roleuser; }
    //        set
    //        {
    //            // Kiểm tra quyền truy cập trước khi cho phép cập nhật Roleuser
    //            if (_userModel.Roleuser != "Admin")
    //            {
    //                _userModel.Roleuser = value;
    //            }
    //            else
    //            {
    //                // Ném một ngoại lệ hoặc thực hiện hành động phù hợp khi cố gắng sửa đổi Roleuser là "Admin"
    //                throw new UnauthorizedAccessException("Không thể sửa đổi quyền truy cập của Admin");
    //            }
    //        }
    //    }

    //    public string TK
    //    {
    //        get { return _userModel.TK; }
    //        set { _userModel.TK = value; }
    //    }

    //    public string Pass
    //    {
    //        get { return _userModel.Pass; }
    //        set { _userModel.Pass = value; }
    //    }
    //}


}