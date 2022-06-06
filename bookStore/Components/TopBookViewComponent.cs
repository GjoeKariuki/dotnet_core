using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using bookStore.Repository;



namespace bookStore.Components {
    public class TopBookViewComponent : ViewComponent 
    {

        private readonly IBookRepository _bookRepository;
        // constructor
        public TopBookViewComponent(IBookRepository bookRepository){
            // this.bookRepository = bookRepository;
            _bookRepository = bookRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int count){
            var books = await _bookRepository.GetTopBooksAsync(count);
            return View(books);
        }
    }
}