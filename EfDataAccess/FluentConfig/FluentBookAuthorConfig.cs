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
    public class FluentBookAuthorConfig : IEntityTypeConfiguration<Fluent_BookAuthor>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookAuthor> modelBuilder)
        {
            // many to many relation between Book and Author
            modelBuilder.HasKey(x => new { x.Author_Id, x.Book_Id });
            modelBuilder.HasOne(x => x.Fluent_Book).WithMany(x => x.Fluent_BookAuthors).HasForeignKey(a => a.Book_Id);
            modelBuilder.HasOne(x => x.Fluent_Author).WithMany(x => x.Fluent_BookAuthors).HasForeignKey(a => a.Author_Id);
        }
    }
}
