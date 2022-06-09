using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Dynamic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


using bookStore.Repository;
using bookStore.Models;

namespace bookStore.Controllers {

    [Route("[controller]/[action]")]
    public class BookController : Controller {

        private readonly IBookRepository _bookRepository = null;
        private readonly ILanguageRepository _languageRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookRepository bookRepository, 
        ILanguageRepository languageRepository,
        IWebHostEnvironment webHostEnvironment) {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // https://localhost:7081/book/GetallBooks
        // public List<BookModel> GetAllBooks()
        [Route("all-books")]
        public async Task<ViewResult> GetAllBooks() {

            var data =  await _bookRepository.GetAllBooks();
            return View(data);
            // return _bookRepository.GetAllBooks();
        }

        // https://localhost:7081/book/GetBook/id
        // public BookModel GetBook(int id) {

        //     return _bookRepository.GetBookById(id);
        // }

        
        // public ViewResult GetBook(int id, string nameOfBook) {

        //     // creating dynamic views
        //     // use Model.book.prop inside the views
        //     // Model.Name
        //     // dynamic data = new ExpandoObject();
        //     // data.book = _bookRepository.GetBookById(id);
        //     // data.Name = "jijoo";

        //     var data =  _bookRepository.GetBookById(id);
        //     return View(data);
        // }

        [Route("book-details/{id:int:min(1)}", Name="bookDetailsroute")]
        public async Task<ViewResult> GetBook(int id)
        {
            var data = await _bookRepository.GetBookById(id);
            return View(data);
        }

        // [Route("book-details/{id:int:min(1)}", Name = "bookDetailsRoute")]
        // public async Task<ViewResult> GetBook(int id)
        // {
        //     var data = await _bookRepository.GetBookById(id);

        //     return View(data);
        // }

        // https://localhost:7081/book/searchbooks?bookName=xcx&authorName=sdfjsN 
        // public List<BookModel> SearchBook(string bookName, string authorName) {

        //     return _bookRepository.SearchBook(bookName, authorName);
        // }


        [Authorize]
        public async Task<ViewResult> AddNewBook(bool isSuccess = false, int bookId =0 ) {

            // dynamic selection
            var model = new BookModel() {
                // Language = "English",
                // LanguageId = 4 ,
            };

            ViewBag.Language= new SelectList(await _languageRepository.GetLanguages(), "Id", "Name");
            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookId;
            return View(model);
            // var list = new SelectList(new List<string>() {"Spanish","English","Kiswahili","Gikuyu","Mandarin"});
            // ViewBag.Language = list;

            // ViewBag.Language = new SelectList(GetLanguage(), "Id", "Text");

            // ViewBag.Language = GetLanguage().Select(x => new SelectListItem() {
            //     Text = x.Text,
            //     Value = x.Id.ToString(),
            // }).ToList();

            
        }
        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel bookModel) {

            if (ModelState.IsValid) {

                if(bookModel.CoverImage != null){
                    string folder = "images/book/";
                    // folder += bookModel.CoverImage.FileName + Guid.NewGuid().ToString();
                    // bookModel.CoverImageUrl = "/" + folder;
                    // string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    // await bookModel.CoverImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                    bookModel.CoverImageUrl = await UploadImage(folder, bookModel.CoverImage);
                }
                
                if(bookModel.GalleryFiles != null) {
                    string folder = "images/book/Gallery/";
                    bookModel.Gallery = new List<GalleryModel>();
                    foreach (var file in bookModel.GalleryFiles){
                        var gallery = new GalleryModel(){
                            Name = file.FileName,
                            URL = await UploadImage(folder, file),
                        };
                        bookModel.Gallery.Add(gallery);
                        
                    }
                }
                if(bookModel.BookPdf != null){
                    string folder = "images/book/pdfs/";
                    bookModel.BookPdfUrl = await UploadImage(folder, bookModel.BookPdf);
                }

                int id = await _bookRepository.AddNewBook(bookModel);
                if(id > 0){
                    return RedirectToAction(nameof(AddNewBook), new{isSuccess = true, bookId = id});
                }

            }

            return View();
            // var list = new SelectList(new List<string>() {"Spanish","English","Kiswahili","Gikuyu","Mandarin"});
            // ViewBag.Language = list;

            // ViewBag.Language = new SelectList(GetLanguage(), "Id", "Text");

            
            // fixing sth 
            // or set ViewBag.IsSuccess == true in the view file
            // ViewBag.IsSuccess = false;
            // ViewBag.BookId = 0;
            // ViewBag.Language= new SelectList(await _languageRepository.GetLanguages(), "Id", "Name");


            // ModelState.AddModelError("","customessage");
            // ModelState.AddModelError("","customessage1");
            
        }

        // private List<LanguageModel> GetLanguage() {

        //     // var group1 = new SelectListGroup() {Name="Group 1"}.ToString();
        //     // var group2 = new SelectListGroup() {Name="Group 2"}.ToString();
        //     // var group3 = new SelectListGroup() {Name="Group 3"}.ToString();

        //     var special = new List<LanguageModel>() {
        //         new LanguageModel() { Id=1, Text="Kiswahili"},
        //         new LanguageModel() { Id=2, Text="English"},
        //         new LanguageModel() { Id=3, Text="Spanish"},
        //         new LanguageModel() { Id=4, Text="Gikuyu"},
        //         new LanguageModel() { Id=5, Text="Mandarin"},
        //         new LanguageModel() { Id=5, Text="Urdu"},
        //         new LanguageModel() { Id=5, Text="Amharic"},
        //     };
        //     return special;
            
        // }

        private async Task<string> UploadImage(string folderPath, IFormFile file) {
            
            folderPath += file.FileName + Guid.NewGuid().ToString();
            
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }
    }
}