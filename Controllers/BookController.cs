using BookRental.BusinessLayer.Interfaces;
using BookRental.BusinessLayer.Repositories;
using BookRental.EFCore;
using BookRental.EFCore.DTO;
using BookRentalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookRepository bookRepository;      

        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        // GET: api/Books
        [HttpGet("GetAllBooks")]
        public async Task<IEnumerable<Book>> Get()
        {
            return await bookRepository.GetAllBooks();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Search(string name, string? gerne)
        {
            try
            {
                var result = await bookRepository.Search(name, gerne);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<BookStatisticsDto>> GetBookStatistics()
        {
            var stats = await bookRepository.GetBookStatisticsAsync();
            return Ok(stats);
        }

    }
}
