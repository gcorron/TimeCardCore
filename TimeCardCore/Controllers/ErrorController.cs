using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TimeCardCore.Controllers
{
    public class ErrorController : Controller
    {
        
        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult ErrorHandler()
        {
            return View();
        }
    }
}