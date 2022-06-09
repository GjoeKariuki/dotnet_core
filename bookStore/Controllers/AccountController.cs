using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using bookStore.Models;
using bookStore.Repository;



namespace bookStore.Controllers
{

    public class AccountController : Controller
    {

        // injecting account repo
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [Route("signup")]
        public IActionResult Signup() 
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(UserSignupModel userModel)
        {
            // checking if model is valid
            if (ModelState.IsValid)
            {

                var result = await _accountRepository.CreateUserAsync(userModel);
                // checking if database was successful
                // gets the error messages
                if (!result.Succeeded)
                {
                    foreach(var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new {email=userModel.Email});
                // return View();
            }
            
            return View(userModel);
        }

        [Route("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSigninModel signinModel, string returnUrl)
        {
            if (ModelState.IsValid){

                var result = await _accountRepository.PasswordSignInAsync(signinModel);
                if(result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl)){
                        return LocalRedirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                if(result.IsNotAllowed){
                    ModelState.AddModelError("", "not allowed");
                }

                ModelState.AddModelError("", "Invalid credentials");
            }
            return View(signinModel);
        }

        [Route("logout")]
        public async Task<IActionResult> LogOut(){
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);
                if(result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email){
            EmailConfirmModel model = new EmailConfirmModel{
                Email = email
            };
            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                //
                token = token.Replace(" ", "+");
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if(result.Succeeded){
                    model.EmailVerified = true;
                }
            }
            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {

            var user = await _accountRepository.GetUserByEmail(model.Email);
            if(user != null){
                if(user.EmailConfirmed)
                {
                    //model.IsConfirmed = true;
                    model.EmailVerified = true;
                    return View(model);
                }
                await _accountRepository.GenerateECT(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else 
            {
                ModelState.AddModelError("", "ooopsss");
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmail(model.Email);
                if(user != null)
                {
                    await _accountRepository.GenerateForgotPassword(user);
                }
                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }
    }
}