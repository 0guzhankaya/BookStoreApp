using BookStore.Entities.Exceptions;
using BookStore.Entities.Models;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BooksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _serviceManager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook([FromRoute(Name = "id")] int id)
        {
            var book = _serviceManager.BookService.GetOneBook(id, false);
            return Ok(book); // 200
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book is null)
                return BadRequest(); // 400

            _serviceManager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        }

        [HttpPut]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            // check book?
            if (book is null)
                return BadRequest(); // 400

            _serviceManager.BookService.UpdateOneBook(id, book, true);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            _serviceManager.BookService.DeleteOneBook(id, false);
            return NoContent(); // 204
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check entity
            var entity = _serviceManager.BookService.GetOneBook(id, true);

            bookPatch.ApplyTo(entity);
            _serviceManager.BookService.UpdateOneBook(id, entity, true);

            return NoContent(); // 204
        }
    }
}
