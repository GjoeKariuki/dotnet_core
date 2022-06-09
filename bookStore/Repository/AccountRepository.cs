using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using bookStore.Service;

using bookStore.Models;



namespace bookStore.Repository
{
    public class AccountRepository : IAccountRepository
    {

        //private readonly UserManager<IdentityUser> _userManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IUserService _userService;

        private readonly IEmailService _emailService;

        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserService userService, IEmailService emailService,
        IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateUserAsync(UserSignupModel userModel)
        {
            // creating user instance
            // and assigning to db
            var user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.Email,
                DateofBirth = userModel.DateofBirth,
            };

            var result =  await _userManager.CreateAsync(user, userModel.Password);

            if(result.Succeeded){
                await GenerateECT(user);
            }

            return result;

        }

        public async Task GenerateECT(ApplicationUser user){
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if(!string.IsNullOrEmpty(token)){
                    await SendEmailConfirmation(user, token);
                }
        }

        public async Task<ApplicationUser> GetUserByEmail(string email){
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> PasswordSignInAsync(UserSigninModel signinModel)
        {
            var email = signinModel.Email;
            var password = signinModel.Password;
            // cookies
            var rememberme = signinModel.RememberMe;
            // lockout when password trials ends
            var lockout = true;
            return await _signInManager.PasswordSignInAsync(email,password,rememberme,lockout);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            // user id
            var userId = _userService.GetUserId();
            // user details
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }


        private async Task SendEmailConfirmation(ApplicationUser user, string token)
        {

            string appDomain = _configuration.GetSection("").Value;
            string confirmationLink = _configuration.GetSection("").Value;

            UserEmailModel options = new UserEmailModel{
                ToEmails = new List<string>() {user.Email},
                Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token ))
                }
            };
            await _emailService.SendEmailForEmailConfirmation(options);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            var id = await _userManager.FindByIdAsync(uid);
            return await _userManager.ConfirmEmailAsync(id, token);
        }

        public async Task GenerateForgotPassword(ApplicationUser user){
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(!string.IsNullOrEmpty(token))
            {
                await SendForgotPassword(user, token);
            }
            
        }


        private async Task SendForgotPassword(ApplicationUser user, string token)
        {

            string appDomain = _configuration.GetSection("").Value;
            string confirmationLink = _configuration.GetSection("").Value;

            UserEmailModel options = new UserEmailModel{
                ToEmails = new List<string>() {user.Email},
                Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token ))
                }
            };
            await _emailService.ForgotPassword(options);
        }


        public async Task<IdentityResult> ResetPassword(ResetPassword model)
        {
            var userid = await _userManager.FindByIdAsync(model.UserId);
            var token = model.Token;
            var newpassword = model.NewPassword;
            return await _userManager.ResetPasswordAsync(userid, token, newpassword);
        }


    }
}