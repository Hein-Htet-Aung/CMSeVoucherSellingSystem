using CMSCodeTest.DBContext;
using CMSCodeTest.Models;
using CMSCodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Controllers
{
    public class UserAPIController : Controller
    {
        private readonly EFDBContext _db;
        GenerateToken generateToken = new GenerateToken();
        EncryptPassword Encrypt = new EncryptPassword();
        DecryptPassword Decrypt = new DecryptPassword();
        public UserAPIController(EFDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("api/GetVoucher")]

        public string GetVoucher(string Email,string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Password = Encrypt.Encrypt_Password(Password);
                    var User =  _db.User.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
                    if (User == null)
                    {
                        return "Not Found";
                    }
                    else
                    {
                        var Token = generateToken.Generate_Token(Email);
                        HttpContext.Session.SetString("UserToken", Token);
                        HttpContext.Session.SetString("UserName", User.User_Name);
                        HttpContext.Session.SetString("UserRole", User.User_Role);

                        string ValidToken = GenerateToken.ValidateToken(Token);
                        if (ValidToken != null)
                        {
                            var eVoucherByUserEmail = _db.eVoucher.Where(e => e.Email == ValidToken).ToList();
                            return (JsonConvert.SerializeObject(eVoucherByUserEmail));
                        }
                    }
                }
                return "Not Found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
