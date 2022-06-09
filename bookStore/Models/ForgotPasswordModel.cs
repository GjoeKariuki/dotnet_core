using System.ComponentModel.DataAnnotations;



namespace bookStore.Models
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress, Display(Name ="registered email address")]
        public string Email {get;set;}
        public bool EmailSent {get;set;}
    }
}