﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfModel.Models
{
    public class Author
    {
        [Key]
        public int Author_Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Lastname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Location { get; set; }

        [NotMapped]
        public string FullName {
            get
            {
                return $"{FirstName} {Lastname}";
            }
        }
    }
}