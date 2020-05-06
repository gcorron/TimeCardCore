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
        private readonly int _curContractorId;
        private readonly string _curUserName;

        protected int CurrentUserId { get => _curUserId; }
        protected string CurrentUsername { get => _curUserName; }
        protected int ContractorId { get => _curContractorId; }

        public BaseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base()
        {
            ConnString = config.GetConnectionString("TimeCard");
            LookupRepo = new LookupRepo(ConnString);
            _webHostEnvironment = webHostEnvironment;
            var user = httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                _curUserName = user.Claims.Where(x => x.Type == "FullName").Single().Value;
                _curUserId = int.Parse(user.Claims.Where(x => x.Type == "UserId").Single().Value);
                _curContractorId = int.Parse(user.Claims.Where(x => x.Type == "ContractorId").Single().Value);
            }
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