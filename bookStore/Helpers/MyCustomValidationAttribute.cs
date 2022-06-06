using System.ComponentModel.DataAnnotations;



// creating custom validation messages
namespace bookStore.Helpers {

    public class MyCustomValidationAttribute : ValidationAttribute {

        // passing value in constructor to make them required
        public MyCustomValidationAttribute (string text) {
            Text = text;
        }
        public string Text { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null) {
                string bookName = value.ToString();
                if (bookName.Contains(Text)) {
                    return ValidationResult.Success;
                }
            }
            // return base.IsValid(value, validationContext);
            return new ValidationResult(ErrorMessage ?? "BookName doesn't contain desired value");
        }
    }
}