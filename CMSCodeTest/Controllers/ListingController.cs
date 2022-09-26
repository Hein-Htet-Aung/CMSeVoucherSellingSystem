using CMSCodeTest.DBContext;
using CMSCodeTest.Models;
using CMSCodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Controllers
{
    public class ListingController : Controller
    {
        private readonly EFDBContext _db;

        public ListingController(EFDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var StatusFromDB = await _db.Listing.Where(u => u.Code == "StatusCode").ToListAsync();
            ViewData["StatusFromDB"] = new SelectList(StatusFromDB, "Descr", "Item");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Listing list)
        {
            try
            {
                var LoginToken = HttpContext.Session.GetString("UserToken");
                string ValidToken = GenerateToken.ValidateToken(LoginToken);
                list.Email = ValidToken;
                await _db.Listing.AddAsync(list);
                await _db.SaveChangesAsync();
                return View();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        

        public async Task<IActionResult> Index()
        {
            var LoginToken = HttpContext.Session.GetString("UserToken");
            string ValidToken = GenerateToken.ValidateToken(LoginToken);
            if (ValidToken != null)
            {
                var ListingByUserEmail = await _db.Listing.Where(e => e.Email == ValidToken).ToListAsync();
                return View(ListingByUserEmail);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var ListFromDb = await _db.Listing.FindAsync(Id);
            var StatusFromDB = await _db.Listing.Where(u => u.Code == "StatusCode").ToListAsync();
            ViewData["StatusFromDB"] = new SelectList(StatusFromDB, "Descr", "Item");
            return View(ListFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Listing list)
        {
            var LoginToken = HttpContext.Session.GetString("UserToken");
            string ValidToken = GenerateToken.ValidateToken(LoginToken);
            list.Email = ValidToken;
            _db.Listing.Update(list);
            _db.SaveChanges();
            return RedirectToAction("Index","Listing");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (ModelState.IsValid)
            {
                if (Id != null)
                {
                    var Listing = await _db.Listing.Where(u => u.ID == Id).FirstOrDefaultAsync();
                    if (Listing != null)
                    {
                        _db.Listing.Remove(Listing);
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Listing");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            return RedirectToAction("Index", "eVoucher");
        }
    }
}
