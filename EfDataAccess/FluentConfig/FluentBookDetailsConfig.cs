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
    public class FluentBookDetailsConfig : IEntityTypeConfiguration<Fluent_BookDetail>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookDetail> modelBuilder)
        {
            // BookDetails
            //declare primary key
            modelBuilder.HasKey(b => b.BookDetail_Id);
            //add required property
            modelBuilder.Property(x => x.NumberOfChapters).IsRequired();
        }
    }
}
