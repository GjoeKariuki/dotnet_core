using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace bookStore.Data 
{
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        // public string Language { get; set; }
        public int LanguageId { get; set; }
        public int TotalPages { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        // reference between books table and the language table
        public Language Language { get; set; }
        public string CoverImageUrl { get; set; }
        // creating table relationship in db one to many
        public ICollection<BookGallery> bookGallery {get;set;}

        // adding pdf column
        public string BookPdfUrl { get;set;}

    }
}