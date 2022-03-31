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
    public class FluentAuthorConfig : IEntityTypeConfiguration<Fluent_Author>
    {
        public void Configure(EntityTypeBuilder<Fluent_Author> modelBuilder)
        {
            // Author
            modelBuilder.HasKey(x => x.Author_Id);
            modelBuilder.Property(x => x.FirstName).IsRequired();
            modelBuilder.Property(x => x.Lastname).IsRequired();
            //for NotMapped usage
            modelBuilder.Ignore(x => x.FullName);
        }
    }
}
