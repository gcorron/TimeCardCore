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
        private int _curUserId;
        protected readonly ISession Session;

        protected int CurrentUserId
        {
            get
            {
                var user = LookupRepo.GetLookupByVal("Contractor", CurrentUsername);
                return user.Id;
            }
        }

        private string _curUsername;
        protected string CurrentUsername
        {
            get
            {
                var username = HttpContext.User.Identity.Name;
                _curUsername = username.Substring(username.IndexOf(@"\") + 1);
                return _curUsername;
            }
        }

        public BaseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base()
        {
            ConnString = config.GetConnectionString("TimeCard");
            _webHostEnvironment = webHostEnvironment;
            Session = httpContextAccessor.HttpContext.Session;

            LookupRepo = new LookupRepo(ConnString);
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