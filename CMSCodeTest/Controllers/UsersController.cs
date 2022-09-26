using CMSCodeTest.DBContext;
using CMSCodeTest.Models;
using CMSCodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMSCodeTest.Controllers
{
    public class UsersController : Controller
    {
        private readonly EFDBContext _db;
        GenerateToken generateToken = new GenerateToken();
        EncryptPassword Encrypt = new EncryptPassword();
        DecryptPassword Decrypt = new DecryptPassword();
        public UsersController(EFDBContext db)
        {
            _db = db;
        }

        public IActionResult Home()
        {
            return RedirectToPage("Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var RoleFromDB = await _db.Listing.Where(u => u.Code == "rolecode").ToListAsync();
            ViewData["RoleFromDB"] = new SelectList(RoleFromDB, "Descr", "Item");
            return View();
        }
        [HttpPost]
        [Route("Users/Register")]
        public async Task<IActionResult> Register(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userFromDb = await _db.User.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
                    if (userFromDb != null)
                    {
                        TempData["Message"] = "This email is already exist";
                        return View();
                    }
                    user.Password = Encrypt.Encrypt_Password(user.Password);
                    _db.User.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("Users/Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Password = Encrypt.Encrypt_Password(user.Password);
                    var User = await _db.User.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefaultAsync();
                    if (User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var Token = generateToken.Generate_Token(user.Email);
                        HttpContext.Session.SetString("UserToken", Token);
                        HttpContext.Session.SetString("UserName", User.User_Name);
                        HttpContext.Session.SetString("UserRole", User.User_Role);

                        return RedirectToAction("Index", "eVoucher");

                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

        
}
