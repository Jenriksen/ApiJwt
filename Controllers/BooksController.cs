using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ApiJwt.Models;

namespace ApiJwt.Controllers
{
    [Route("api/books")]
    public class BooksController : Controller
    {
        [HttpGet]
        [Authorize]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            var resultBookList = new Book[] {
                new Book { Author = "J.K. Rowling", Title = "Harry Potter and the sorceres stone"},
                new Book { Author = "Rasmus Dahlberg", Title = "Solstorm"},
                new Book { Author = "Dr. Bob Rotella", Title = "Steve Jobs Innovationsmetode"}
            };

            return resultBookList;
        }
    }
}