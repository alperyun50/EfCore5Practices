using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfModel.Models
{
    [Table("tb_Genre")]
    public class Genre
    {
        public int GenreId { get; set; }

        [Column("Genre_Name")]
        public string GenreName { get; set; }

        //public int DisplayOrder { get; set; }
    }
}
