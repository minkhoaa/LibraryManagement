﻿namespace LibraryManagement.Dto.Request
{
    public class AuthorRequest
    {
        public Guid IdTypeBook { get; set; }
        public string NameAuthor { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }
        public IFormFile AvatarImage { get; set; }
    }
}