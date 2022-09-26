using CMSCodeTest.DBContext;
using CMSCodeTest.Models;
using CMSCodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Controllers
{
    public class ListingAPIController1 : Controller
    {
        private readonly EFDBContext _db;

        public ListingAPIController1(EFDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("api/PaymentMethod")]
        public string PaymentMethod(string Code)
        {
            try
            {
                var List = _db.Listing.Where(u => u.Code == Code).ToList();
                if (List != null)
                {
                    return (JsonConvert.SerializeObject(List));
                }
                else
                {
                    return "Not Found";
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
}
    }
}


