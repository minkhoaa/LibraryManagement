﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{

    public class CreateBook
    {
        [Column("id_headerbook")]
        public Guid IdHeaderBook { get; set; }

        [Column("id_author")]
        public Guid IdAuthor { get; set; }

        //public HeaderBook HeaderBook { get; set; }
        //public Author Author { get; set; }
    }

}
