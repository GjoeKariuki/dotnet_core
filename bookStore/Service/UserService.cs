using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;



namespace bookStore.Service
{

    public class UserService : IUserService
    {

        private readonly IHttpContextAccessor _httpContext;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;   
        }
        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        public bool isAuthenticated(){
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}