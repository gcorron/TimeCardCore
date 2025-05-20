using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using TimeCard.Repo.Repos;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using TimeCard.Domain;
using TimeCardCore.Infrastructure;

namespace TimeCardCore.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string ConnString;
        protected readonly LookupRepo LookupRepo;
        
        private readonly IWebHostEnvironment  _webHostEnvironment;
        protected readonly HttpContext _httpContext;
        private Identity _identity;
        protected IConfiguration _config;

        public BaseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base()
        {
            ConnString = config.GetConnectionString("Timecard");
            LookupRepo = new LookupRepo(ConnString);
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor.HttpContext;
            _config = config;
            var user = httpContextAccessor.HttpContext?.User;
        }
        protected Identity? CurrentIdentity
        {
            get
            {
                if (_identity == null)
                {

                    _identity = _httpContext.Session.GetObjectFromJson<Identity>("Identity");
                }
                return _identity;
            }
            set
            {
                _identity = value;
                _httpContext.Session.SetObjectAsJson("Identity", value);
            }
        }
        public string WebRootPath
        {
            get
            {
                return _webHostEnvironment.WebRootPath;
            }

        }
        public string NewToken(int id, string name, int expireSeconds)
        {
            return new JWTokenAuthentication(_config).GenerateToken(id, name, expireSeconds);
        }
    }

}