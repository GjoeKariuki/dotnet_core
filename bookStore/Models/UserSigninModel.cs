using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace bookStore.Models
{
    public class UserSigninModel 
    {
        [Required, EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [Display(Name ="Remember Me")]
        public bool RememberMe {get;set;}
    }
}