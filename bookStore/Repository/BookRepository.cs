
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bookStore.Models;
using bookStore.Data;


namespace bookStore.Repository {

    public class BookRepository : IBookRepository
    {

        // private List<BookModel> DataSource() {
        //     return new List<BookModel>() {
        //         new BookModel() { Id = 1, Title = "MVC", Author = "Jk Rowling", Description="this describes all about the book", Category="programming", Language="English", TotalPages=32423},
        //         new BookModel(){ Id = 2, Title = "Dot Net Core", Author = "JK Rowling", Description="this describes all about the book", Category="programming", Language="Latin", TotalPages=322},
        //         new BookModel(){ Id = 3, Title = "C#", Author = "JK", Description="this describes all about the book", Category="programming", Language="English", TotalPages=32},
        //         new BookModel(){ Id = 4, Title = "Java", Author = "Rowling", Description="this describes all about the book", Category="programming", Language="English", TotalPages=423},
        //         new BookModel(){ Id = 5, Title = "Php", Author = "webgentle", Description="this describes all about the book", Category="programming", Language="Latin", TotalPages=2423},
        //         new BookModel(){ Id = 6, Title = "Python", Author = "Hammond", Description="this describes all about the book", Category="programming", Language="Spanish", TotalPages=23},
        //     };
        // }

        private readonly BookStoreContext _context = null;

        public BookRepository(BookStoreContext context) {
            _context = context;
        }
        public async Task<int> AddNewBook(BookModel model) {

            var newBook = new Book(){
                Author = model.Author,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                Title = model.Title,
                TotalPages = model.TotalPages.HasValue ? model.TotalPages.Value : 0,
                UpdatedOn = DateTime.UtcNow,
                LanguageId = model.LanguageId,
                CoverImageUrl = model.CoverImageUrl,
                BookPdfUrl = model.BookPdfUrl,
            };

            newBook.bookGallery = new List<BookGallery>();
            foreach(var file in model.Gallery){
                newBook.bookGallery.Add(new BookGallery(){
                    Name = file.Name,
                    URL = file.URL
                });
            }

            // mapping to context class
            await _context.Book.AddAsync(newBook);
            // saving
            await _context.SaveChangesAsync();

            return newBook.Id;
        }
        // public List<BookModel> GetAllBooks() {
        //     return DataSource();
        // }
        // public async Task<List<BookModel>> GetAllBooks() {

        //     var books = new List<BookModel>();
        //     var allbooks = await _context.Book.ToListAsync();
        //     if(allbooks?.Any() == true){
        //         foreach(var book in allbooks){

        //             books.Add(new BookModel() 
        //             {
        //                 Author = book.Author,
        //                 Category = book.Category,
        //                 Description = book.Description,
        //                 Id = book.Id,
        //                 LanguageId = book.LanguageId,
        //                 Title = book.Title,
        //                 TotalPages = book.TotalPages,
        //                 Language = book.Language.Name,
        //             });
        //         }
        //     }
        //     return books;
        // }
        public async Task<List<BookModel>> GetAllBooks() {

            return await _context.Book.Select(book => new BookModel(){
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                Id = book.Id,
                LanguageId = book.LanguageId,
                Title = book.Title,
                TotalPages = book.TotalPages,
                Language = book.Language.Name,
                CoverImageUrl = book.CoverImageUrl,
            }).ToListAsync();
        }

        // get top 5 books
        public async Task<List<BookModel>> GetTopBooksAsync(int count) {

            return await _context.Book.Select(book => new BookModel(){
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                Id = book.Id,
                LanguageId = book.LanguageId,
                Title = book.Title,
                TotalPages = book.TotalPages,
                Language = book.Language.Name,
                CoverImageUrl = book.CoverImageUrl,
            }).Take(count).ToListAsync();
        }

        // public async Task<BookModel> GetBookById(int id) {

        //     return DataSource().Where(x => x.Id == id).FirstOrDefault();
        // }

        // _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<BookModel> GetBookById(int id) {

            // var book = await _context.Book.FindAsync(id);
            // // verifying if data is in the object
            // if (book != null){
            //     var bookDetails = new BookModel(){
            //         Author = book.Author,
            //             Category = book.Category,
            //             Description = book.Description,
            //             Id = book.Id,
            //             LanguageId = book.LanguageId,
            //             Title = book.Title,
            //             TotalPages = book.TotalPages,
            //             Language = book.Language.Name,
            //     };
            //     return bookDetails;
            // }
            // return null;

            return await _context.Book.Where(x => x.Id == id).Select(book => new BookModel(){
                    Author = book.Author,
                    Category = book.Category,
                    Description = book.Description,
                    Id = book.Id,
                    LanguageId = book.LanguageId,
                    Title = book.Title,
                    TotalPages = book.TotalPages,
                    Language = book.Language.Name,
                    CoverImageUrl = book.CoverImageUrl,
                    Gallery = book.bookGallery.Select(g => new GalleryModel(){
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList(),
                    BookPdfUrl = book.BookPdfUrl,
            }).FirstOrDefaultAsync();
        }

        public List<BookModel> SearchBook(string title, string authorName){
            //return DataSource().Where(x => x.Title.Contains(title) || x.Author.Contains(authorName)).ToList();
            return null;
        }
    
    }
}