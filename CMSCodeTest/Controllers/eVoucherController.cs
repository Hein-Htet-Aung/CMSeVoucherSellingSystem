using CMSCodeTest.DBContext;
using CMSCodeTest.Models;
using CMSCodeTest.Services;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMSCodeTest.Controllers
{
    public class eVoucherController : Controller
    {
        private readonly EFDBContext _db;
        private IWebHostEnvironment _Environment;

        public eVoucherController(EFDBContext db, IWebHostEnvironment IW)
        {
            _db = db;
            _Environment = IW;
        }


        public async Task<IActionResult> Index()
        {
            var LoginToken = HttpContext.Session.GetString("UserToken");
            string ValidToken = GenerateToken.ValidateToken(LoginToken);
            if (ValidToken != null)
            {
                var eVoucherByUserEmail = await _db.eVoucher.Where(e => e.Email == ValidToken).ToListAsync();
                return View(eVoucherByUserEmail);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var PaymentMethodFromDB = await _db.Listing.Where(u => u.Code == "PaymentMethodCode").ToListAsync();
            ViewData["PaymentMethodFromDB"] = new SelectList(PaymentMethodFromDB, "Descr", "Item");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(eVoucherViewModel eVoucher)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(eVoucher);
                var LoginToken = HttpContext.Session.GetString("UserToken");
                string ValidToken = GenerateToken.ValidateToken(LoginToken);
                if (ValidToken != null)
                {
                    Random random = new Random();
                    var plainTextBytes = System.Text.ASCIIEncoding.Unicode.GetBytes(eVoucher.MO_PhNo.ToString());
                    string voucherCode = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) + new Random().Next(000000, 999999).ToString();

                    eVoucher Voucher = new eVoucher();
                    Voucher.Title = eVoucher.Title;
                    Voucher.Description = eVoucher.Title;
                    Voucher.ExpiryDate = eVoucher.ExpiryDate;
                    Voucher.Image = uniqueFileName;
                    Voucher.Amount = eVoucher.Amount;
                    Voucher.PaymentMethod = eVoucher.PaymentMethod;
                    Voucher.PaymentMethodPerDiscount = eVoucher.PaymentMethodPerDiscount;
                    Voucher.Qty = eVoucher.Qty;
                    Voucher.Type = eVoucher.Type;
                    Voucher.MO_Name = eVoucher.MO_Name;
                    Voucher.MO_PhNo = eVoucher.MO_PhNo;
                    Voucher.MO_MaxBuyLimit = eVoucher.MO_MaxBuyLimit;
                    Voucher.GTO_Name = eVoucher.GTO_Name;
                    Voucher.GTO_PhNo = eVoucher.GTO_PhNo;
                    Voucher.GTO_GTOLimit = eVoucher.GTO_GTOLimit;
                    Voucher.GTO_MaxBuyLimit = eVoucher.GTO_MaxBuyLimit;


                    GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(voucherCode, 200);
                    barcode.AddBarcodeValueTextBelowBarcode();

                    barcode.SetMargins(10);
                    barcode.ChangeBarCodeColor(Color.BlueViolet);
                    string path = Path.Combine(_Environment.WebRootPath, "GeneratedQRCode");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string QRCodeName = Guid.NewGuid().ToString() + "qrcode.png";
                    string filePath = Path.Combine(_Environment.WebRootPath, "GeneratedQRCode/" + QRCodeName);
                    barcode.SaveAsPng(filePath);
                    string fileName = Path.GetFileName(filePath);

                    Voucher.PromoCode = voucherCode;
                    Voucher.QRCode = QRCodeName;
                    Voucher.Email = ValidToken;

                    await _db.eVoucher.AddAsync(Voucher);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "eVoucher");
                }
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var PaymentMethodFromDB = await _db.Listing.Where(u => u.Code == "PaymentMethodCode").ToListAsync();
            var eVoucherFromDB = await _db.eVoucher.Where(u => u.Id == id).FirstOrDefaultAsync();
            eVoucherViewModel Voucher = new eVoucherViewModel();
            if (eVoucherFromDB != null)
            {
                Voucher.Title = eVoucherFromDB.Title;
                Voucher.Description = eVoucherFromDB.Title;
                Voucher.ExpiryDate = eVoucherFromDB.ExpiryDate;
                Voucher.Image = eVoucherFromDB.Image;
                Voucher.Amount = eVoucherFromDB.Amount;
                Voucher.PaymentMethod = eVoucherFromDB.PaymentMethod;
                Voucher.PaymentMethodPerDiscount = eVoucherFromDB.PaymentMethodPerDiscount;
                Voucher.Qty = eVoucherFromDB.Qty;
                Voucher.Type = eVoucherFromDB.Type;
                Voucher.MO_Name = eVoucherFromDB.MO_Name;
                Voucher.MO_PhNo = eVoucherFromDB.MO_PhNo;
                Voucher.MO_MaxBuyLimit = eVoucherFromDB.MO_MaxBuyLimit;
                Voucher.GTO_Name = eVoucherFromDB.GTO_Name;
                Voucher.GTO_PhNo = eVoucherFromDB.GTO_PhNo;
                Voucher.GTO_GTOLimit = eVoucherFromDB.GTO_GTOLimit;
                Voucher.GTO_MaxBuyLimit = eVoucherFromDB.GTO_MaxBuyLimit;

                Voucher.PromoCode = eVoucherFromDB.PromoCode;
                Voucher.QRCode = eVoucherFromDB.QRCode;
                Voucher.Email = eVoucherFromDB.Email;
            }

            ViewData["PaymentMethodFromDB"] = new SelectList(PaymentMethodFromDB, "Descr", "Item");
            return View(Voucher);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(eVoucherViewModel eVoucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LoginToken = HttpContext.Session.GetString("UserToken");
                    string ValidToken = GenerateToken.ValidateToken(LoginToken);
                    eVoucher Voucher = new eVoucher();
                    Voucher.Id = eVoucher.Id;
                    Voucher.Title = eVoucher.Title;
                    Voucher.Description = eVoucher.Title;
                    Voucher.ExpiryDate = eVoucher.ExpiryDate;
                    Voucher.Image = eVoucher.Image;
                    Voucher.Amount = eVoucher.Amount;
                    Voucher.PaymentMethod = eVoucher.PaymentMethod;
                    Voucher.PaymentMethodPerDiscount = eVoucher.PaymentMethodPerDiscount;
                    Voucher.Qty = eVoucher.Qty;
                    Voucher.Type = eVoucher.Type;
                    Voucher.MO_Name = eVoucher.MO_Name;
                    Voucher.MO_PhNo = eVoucher.MO_PhNo;
                    Voucher.MO_MaxBuyLimit = eVoucher.MO_MaxBuyLimit;
                    Voucher.GTO_Name = eVoucher.GTO_Name;
                    Voucher.GTO_PhNo = eVoucher.GTO_PhNo;
                    Voucher.GTO_GTOLimit = eVoucher.GTO_GTOLimit;
                    Voucher.GTO_MaxBuyLimit = eVoucher.GTO_MaxBuyLimit;

                    Voucher.PromoCode = eVoucher.PromoCode;
                    Voucher.QRCode = eVoucher.QRCode;
                    Voucher.Email = ValidToken;

                    _db.eVoucher.Update(Voucher);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "eVoucher");
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
        public async Task<IActionResult> Delete(int Id)
        {
            if (ModelState.IsValid)
            {
                if (Id != null)
                {
                    var eVoucher = await _db.eVoucher.Where(u => u.Id == Id).FirstOrDefaultAsync();
                    if (eVoucher != null)
                    {
                        _db.eVoucher.Remove(eVoucher);
                        _db.SaveChanges();
                        return RedirectToAction("Index", "eVoucher");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            return RedirectToAction("Index", "eVoucher");
        }
        private string UploadedFile(eVoucherViewModel model)
        {
            string uniqueFileName = null;

            if (model.UploadImage != null)
            {

                string uploadsFolder = Path.Combine(_Environment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.UploadImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
