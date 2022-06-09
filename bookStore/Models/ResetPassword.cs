using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;




namespace bookStore.Models
{
    public class ResetPassword
    {
        [Required]
        public string UserId {get;set;}
        [Required]
        public string Token {get;set;}
        [Required, DataType(DataType.Password)]
        public string NewPassword {get;set;}
        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword {get;set;}
        public bool IsSuccess {get;set;}
    }
}