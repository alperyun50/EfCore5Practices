using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDataAccess.FluentConfig
{
    public class FluentBookConfig : IEntityTypeConfiguration<Fluent_Book>
    {
        public void Configure(EntityTypeBuilder<Fluent_Book> modelBuilder)
        {
            // Book
            modelBuilder.HasKey(x => x.Book_Id);
            modelBuilder.Property(x => x.ISBN).IsRequired().HasMaxLength(15);
            modelBuilder.Property(x => x.Title).IsRequired();
            modelBuilder.Property(x => x.Price).IsRequired();

            // one to one relation between Book and BookDetail
            //modelBuilder.Entity <it has to include navigation property (=FKey)> ()...
            modelBuilder.HasOne(a => a.Fluent_BookDetail).WithOne(b => b.Fluent_Book).HasForeignKey<Fluent_Book>("BookDetail_Id");

            // one to many relation between Book and Publisher
            modelBuilder.HasOne(a => a.Fluent_Publisher).WithMany(x => x.Fluent_Books).HasForeignKey(a => a.Publisher_Id);
        }
    }
}
