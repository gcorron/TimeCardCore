using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TimeCardCore.Infrastructure
{

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _item;
        private readonly string _action;
        private readonly HttpContext _context;
        private readonly IJWTokenAuthentication _jwTokenAuthentication;
        public AuthorizeActionFilter(IHttpContextAccessor accessor, IJWTokenAuthentication jWTokenAuthentication, string item, string action)
        {
            _item = item;
            _action = action;
            _context = accessor.HttpContext;
            _jwTokenAuthentication = jWTokenAuthentication;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = _context.Session.GetObjectFromJson<TimeCard.Domain.Identity>("Identity");
            if (identity != null)
            {
                if (!_jwTokenAuthentication.ValidateToken(identity.Token, 1))
                {
                    identity = null;
                }
            }
            if (identity != null)
            {
                var isAuthorized = identity.Roles.Any(x => x == _item);
                if (isAuthorized)
                {
                    return;
                }
            }
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    controller = "Account",
                    action = "Login",
                    UserHost = context.HttpContext.Request.GetEncodedPathAndQuery()
                }
            ));
        }
    }
}
