using EfDataAccess.FluentConfig;
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

        public DbSet<Fluent_Book> Fluent_Books { get; set; }

        public DbSet<Fluent_Author> Fluent_Authors { get; set; }

        public DbSet<Fluent_Publisher> Fluent_Publishers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // we configure fluent api
            // add a compozit(combine) fkey for book author many to many relation
            modelBuilder.Entity<BookAuthor>().HasKey(a => new { a.Author_Id, a.Book_Id });


            // change category table and column name
            modelBuilder.Entity<Category>().ToTable("tbl_category");
            modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("CategoryName");


            // BookDetails
            //declare primary key
            modelBuilder.Entity<Fluent_BookDetail>().HasKey(b => b.BookDetail_Id);
            //add required property
            modelBuilder.Entity<Fluent_BookDetail>().Property(x => x.NumberOfChapters).IsRequired();


            // Book
            modelBuilder.Entity<Fluent_Book>().HasKey(x => x.Book_Id);
            modelBuilder.Entity<Fluent_Book>().Property(x => x.ISBN).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<Fluent_Book>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Fluent_Book>().Property(x => x.Price).IsRequired();


            // Author
            modelBuilder.Entity<Fluent_Author>().HasKey(x => x.Author_Id);
            modelBuilder.Entity<Fluent_Author>().Property(x => x.FirstName).IsRequired();
            modelBuilder.Entity<Fluent_Author>().Property(x => x.Lastname).IsRequired();
            //for NotMapped usage
            modelBuilder.Entity<Fluent_Author>().Ignore(x => x.FullName);


            // Publisher
            modelBuilder.Entity<Fluent_Publisher>().HasKey(x => x.Publisher_Id);
            modelBuilder.Entity<Fluent_Publisher>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Fluent_Publisher>().Property(x => x.Location).IsRequired();


            // one to one relation between Book and BookDetail
            //modelBuilder.Entity <it has to include navigation property (=FKey)> ()...
            modelBuilder.Entity<Fluent_Book>().HasOne(a => a.Fluent_BookDetail).WithOne(b => b.Fluent_Book).HasForeignKey<Fluent_Book>("BookDetail_Id");


            // one to many relation between Book and Publisher
            modelBuilder.Entity<Fluent_Book>().HasOne(a => a.Fluent_Publisher).WithMany(x => x.Fluent_Books).HasForeignKey(a => a.Publisher_Id);


            // many to many relation between Book and Author
            modelBuilder.Entity<Fluent_BookAuthor>().HasKey(x => new { x.Author_Id, x.Book_Id });
            modelBuilder.Entity<Fluent_BookAuthor>().HasOne(x => x.Fluent_Book).WithMany(x => x.Fluent_BookAuthors).HasForeignKey(a => a.Book_Id);
            modelBuilder.Entity<Fluent_BookAuthor>().HasOne(x => x.Fluent_Author).WithMany(x => x.Fluent_BookAuthors).HasForeignKey(a => a.Author_Id);




            ////////////////// Alternative way to build OnModelCreating with FluentConfig Classes ///////////////////////

            // if you use fluent api and data anotations in hybrid mode dont add FluentConfig class partially

            //modelBuilder.ApplyConfiguration(new FluentBookConfig());
            //modelBuilder.ApplyConfiguration(new FluentBookDetailsConfig());
            //modelBuilder.ApplyConfiguration(new FluentBookAuthorConfig());
            //modelBuilder.ApplyConfiguration(new FluentPublisherConfig());
            //modelBuilder.ApplyConfiguration(new FluentAuthorConfig());

        }
    }
}
