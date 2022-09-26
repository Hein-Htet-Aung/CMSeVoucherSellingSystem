using CMSCodeTest.DBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Controllers
{
    public class eVoucherAPIController : Controller
    {
        private readonly EFDBContext _db;
        private IWebHostEnvironment _Environment;


        public eVoucherAPIController(EFDBContext db)
        {
            _db = db;
        }

    }
}
