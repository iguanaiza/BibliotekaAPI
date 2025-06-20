﻿using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models.Singles
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
