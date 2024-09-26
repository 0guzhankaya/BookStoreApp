using BookStore.API.Models;
using BookStore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _repositoryContext;

        public BooksController(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _repositoryContext.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _repositoryContext.Books.Where(b => b.Id == id).SingleOrDefault();

                if (book is null)
                {
                    return NotFound(); // 404
                }

                return Ok(book); // 200
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); // 400

                _repositoryContext.Books.Add(book);
                _repositoryContext.SaveChanges();

                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                // check book?
                var entity = _repositoryContext.Books.Where(b => b.Id == id).SingleOrDefault();

                if (entity is null)
                    return NotFound(); // 404

                // check id
                if (id != book.Id)
                    return BadRequest(); // 400

                entity.Title = book.Title;
                entity.Price = book.Price;

                _repositoryContext.SaveChanges();

                return Ok(book); // 200
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _repositoryContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if(entity is null) return NotFound(new
                {
                    StatusCode = 404,
                    message = $"Book with id:{id} could not found."
                });
                _repositoryContext.Books.Remove(entity);
                _repositoryContext.SaveChanges();
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                // check entity
                var entity = _repositoryContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                if (entity is null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);
                _repositoryContext.SaveChanges();

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
