using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfModel.Models
{
    // this class added for create many to many relation between book and Author tables
    public class BookAuthor
    {
        [ForeignKey("Book")]
        public int Book_Id { get; set; }

        [ForeignKey("Author")]
        public int Author_Id { get; set; }

        public Book Book{ get; set; }

        public Author Author { get; set; }
    }
}
