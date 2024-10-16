using BookRental.EFCore;
using BookRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly BookRentalDBContext _context;

        public RentalsController(BookRentalDBContext context)
        {
            _context = context;
        }

        [HttpPost("rent/{bookId}")]
        public async Task<ActionResult> RentBook(int bookId, int userId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || !book.IsAvailable)
                return BadRequest("Book is not available");

            var rental = new Rental
            {
                BookId = bookId,
                UserId = userId,
                RentalDate = DateTime.UtcNow
            };

            _context.Rentals.Add(rental);
            book.IsAvailable = false;
            await _context.SaveChangesAsync();

            return Ok("Book rented successfully");
        }

        [HttpPost("return/{rentalId}")]
        public async Task<ActionResult> ReturnBook(int rentalId)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            if (rental == null)
                return NotFound();

            rental.ReturnDate = DateTime.UtcNow;

            var book = await _context.Books.FindAsync(rental.BookId);
            if (book != null)
                book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return Ok("Book returned successfully");
        }
    }
}
