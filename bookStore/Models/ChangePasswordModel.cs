using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;



namespace bookStore.Models
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password), Display(Name ="Current Password")]
        public string CurrentPassword {get;set;}

        [Required, DataType(DataType.Password), Display(Name ="New Password")]
        public string NewPassword {get;set;}
        
        [Required, DataType(DataType.Password), Display(Name ="Confirm new Password")]
        [Compare("NewPassword", ErrorMessage ="passwords don't match")]
        public string ConfirmNewPassword {get;set;}
    }
}