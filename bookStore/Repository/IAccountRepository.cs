using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using bookStore.Models;



namespace bookStore.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(UserSignupModel userModel);
        Task<SignInResult> PasswordSignInAsync(UserSigninModel signinModel);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task GenerateECT(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task GenerateForgotPassword(ApplicationUser user);
        // Task SendForgotPassword(ApplicationUser user, string token);
        // 

    }
}