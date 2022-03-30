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
    public class Fluent_BookAuthor
    {
        public int Book_Id { get; set; }

        public int Author_Id { get; set; }

    }
}
