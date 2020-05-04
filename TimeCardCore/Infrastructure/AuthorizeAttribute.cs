using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardCore.Infrastructure
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string item, string action) : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item, action };
        }
    }
}
