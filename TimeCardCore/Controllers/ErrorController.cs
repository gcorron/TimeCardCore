using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TimeCardCore.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NoPermission()
        {
            return View();
        }
    }
}