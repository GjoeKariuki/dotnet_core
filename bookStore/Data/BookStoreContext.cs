using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace bookStore.Data {

    public class BookStoreContext: IdentityDbContext
    {

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) 
        {

        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookGallery> BookGallery { get; set; }
        // can be defined here or on startup

        // adding a new table and referencing it to its class
        public DbSet<Language> Language { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=WebGentle;UserId=potato;Password=22_20-");
            base.OnConfiguring(optionsBuilder);
        }

    }
    
}
