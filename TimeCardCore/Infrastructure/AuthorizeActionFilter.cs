using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TimeCardCore.Controllers
{
    public enum PermissionItem
    {
        User,
        Product,
        Contact,
        Review,
        Client
    }

    public enum PermissionAction
    {
        Read,
        Create,
    }

    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string item, string action) : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item, action };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _item;
        private readonly string _action;
        public AuthorizeActionFilter(string item, string action)
        {
            _item = item;
            _action = action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = false;
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                isAuthorized = user.Claims.Where(x => x.Value == _item).Any();
                if (!isAuthorized)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
    public class ClaimsTransformer : IClaimsTransformation
    {
        public readonly TimeCard.Repo.Repos.LookupRepo _lookupRepo;
        public ClaimsTransformer(IConfiguration config)
        {
            _lookupRepo = new TimeCard.Repo.Repos.LookupRepo(config.GetConnectionString("TimeCard"));
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //var ci = (ClaimsIdentity)principal.Identity;
            //var roles = _lookupRepo.GetRolesForUser(ci.Name.Substring(ci.Name.IndexOf(@"\") + 1));
            //foreach(var role in roles)
            //{
            //    var c = new Claim(ci.RoleClaimType, role);
            //    ci.AddClaim(c);
            //}
            return Task.FromResult(principal);
        }
    }

}
