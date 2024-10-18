using BookRental.BusinessLayer;
using BookRental.BusinessLayer.Interfaces;
using BookRental.EFCore;
using BookRental.EFCore.DTO;
using BookRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace BookRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly BookRentalDBContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;       

        public RentalsController(BookRentalDBContext context, IBookRepository bookRepository, IRentalRepository rentalRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
        }

        [HttpPost("rent/{bookId}")]
        public async Task<ActionResult> RentBook(int bookId, int userId)
        {

            try
            {              

                var bookToRental = await _bookRepository.GetBookbyId(bookId);

                if (bookToRental == null)
                    return NotFound($"Book with Id = {bookId} not found");

                var rent = await _rentalRepository.RentBook(bookId, userId);
                return Ok(rent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error renting book");
            }

        }

        [HttpPost("return/{rentalId}")]
        public async Task<ActionResult> ReturnBook(int rentalId)
        {
            try
            {

                var rental = await _rentalRepository.GetRentalbyId(rentalId);
                if (rental == null)
                   return  NotFound($"BookRental with Id = {rentalId} not found");

               rental = await _rentalRepository.ReturnBook(rentalId);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Returning book");
            }
            return Ok("Book returned successfully");
        }

        // GET: api/RentalHistory
        [HttpGet("rentalHistory")]
        public async Task<IEnumerable<Rental>> Get()
        {
            return await _rentalRepository.GetAllRentalBooks();
        }

       

    }
}
