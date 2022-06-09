using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace bookStore.Models 
{
    public class UserSignupModel 
    {
        
        [Required(ErrorMessage = "name missing")]
        public string FirstName {get;set;}
        [Required(ErrorMessage = "name missing")]
        public string LastName {get;set;}

        [Required(ErrorMessage = "date is required")]
        public DateTime DateofBirth {get;set;}

        [Required(ErrorMessage = "email missing")]
        [Display(Name="Email Address")]
        [EmailAddress(ErrorMessage = "enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}

        [Required(ErrorMessage = "password missing")]
        [Compare("ConfirmPassword", ErrorMessage = "password don't match")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Required(ErrorMessage = "password missing")]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
    }
}