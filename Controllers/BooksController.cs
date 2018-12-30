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
            int userAge = 0;
            var resultBookList = new Book[] {
                new Book { Author = "J.K. Rowling", Title = "Harry Potter and the sorceres stone"},
                new Book { Author = "Rasmus Dahlberg", Title = "Solstorm"},
                new Book { Author = "Dr. Bob Rotella", Title = "Steve Jobs Innovationsmetode", AgeRestriction = true}
            };

            // Alderstjek af bruger for at se om aldersbegrænset indhold skal med.
            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            if(userAge < 18)
            {
                resultBookList = resultBookList.Where(b => !b.AgeRestriction).ToArray();
            }

            return resultBookList;
        }
    }
}