using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }


        public DbSet<Fluent_BookDetail> Fluent_BookDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // we configure fluent api
            // add a compozit(combine) fkey for book author many to many relation
            modelBuilder.Entity<BookAuthor>().HasKey(a => new { a.Author_Id, a.Book_Id });


            // BookDetails
            //declare primary key
            modelBuilder.Entity<Fluent_BookDetail>().HasKey(b => b.BookDetail_Id);
            //add required
            modelBuilder.Entity<Fluent_BookDetail>().Property(x => x.NumberOfChapters).IsRequired();
        }
    }
}
