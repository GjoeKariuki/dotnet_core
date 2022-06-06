using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using bookStore.Enums;
using bookStore.Helpers;

namespace bookStore.Models {

    public class BookModel {

        public int Id { get; set; }
        // [StringLength(100, MinimumLength =5)]
        // [Required(ErrorMessage ="value needed")]
        [MyCustomValidation("python")]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [StringLength(500,ErrorMessage ="ongezea maneno bwana")]
        [Required]
        public string Description { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        // public int Language { get; set; }
        public int LanguageId { get; set; }

        [Required]
        [Display(Name ="Total pages of kitabus")]
        public int? TotalPages { get; set; }
        [Display(Name = "choose book cover photo")]
        [Required]
        public IFormFile CoverImage {get; set; }
        public string CoverImageUrl { get; set; }
    
        //multiple images
        [Required]
        [Display(Name ="choose multiple images")]
        // List<IFormFile>
        // IEnumerable<IFormFile>
        public IFormFileCollection GalleryFiles {get; set;}
        public List<GalleryModel> Gallery {get;set;}
        [Display(Name = "upload book PDF")]
        [Required]
        public IFormFile BookPdf {get; set;}
        public string BookPdfUrl { get; set;}

        //  public LanguageEnum LanguageEnum{ get; set; }
        // public List<string> MultiLanguage{ get; set; }

        // custom field
        // [DataType(DataType.DateTime)]
        // [Display(Name ="pick date & time")]
        // public string myField { get; set; }
    }
}