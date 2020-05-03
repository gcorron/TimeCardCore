using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using TimeCard.Repo.Repos;
using Microsoft.AspNetCore.Http;

namespace TimeCardCore.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string ConnString;
        protected readonly LookupRepo LookupRepo;
        
        private readonly IWebHostEnvironment  _webHostEnvironment;
        private readonly int _curUserId;
        private readonly string _curUserName;

        protected readonly ISession Session;

        protected int CurrentUserId { get => _curUserId; }
        protected string CurrentUsername { get => _curUserName; }

        public BaseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base()
        {
            ConnString = config.GetConnectionString("TimeCard");
            _webHostEnvironment = webHostEnvironment;
            Session = httpContextAccessor.HttpContext.Session;
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            _curUserName = username.Substring(username.IndexOf(@"\") + 1);
            LookupRepo = new LookupRepo(ConnString);
            _curUserId = LookupRepo.GetLookupByVal("Contractor", _curUserName).Id;
        }

        public string WebRootPath
        {
            get
            {
                return _webHostEnvironment.WebRootPath;
            }

        }
    }

}