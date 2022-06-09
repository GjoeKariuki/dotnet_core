using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using bookStore.Models;
using System.Security.Claims;

namespace  bookStore.Helpers
{
    public class UserClaims : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public UserClaims(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options): base(userManager,roleManager,options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identy = await base.GenerateClaimsAsync(user);
            identy.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identy.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            return identy;
        }
    }
}