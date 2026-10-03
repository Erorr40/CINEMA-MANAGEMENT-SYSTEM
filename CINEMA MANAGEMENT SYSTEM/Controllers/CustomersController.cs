using Microsoft.AspNetCore.Mvc;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.CustomerDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;
using CINEMA_MANAGEMENT_SYSTEM.Repositories;

namespace CINEMA_MANAGEMENT_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _unitOfWork.Customers.GetAll();
            var dtos = customers.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("bookings")]
        public IActionResult GetWithBookings()
        {
            var customers = _unitOfWork.Customers.GetCustomersWithBookings();
            var dtos = customers.Select(c => new CustomerWithBookingsDTO
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Bookings = c.Bookings.Select(b => new BookingDTO
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    NumberOfTickets = b.NumberOfTickets,
                    TotalPrice = b.TotalPrice,
                    CustomerId = b.CustomerId,
                    MovieId = b.MovieId
                }).ToList()
            });

            return Ok(dtos);
        }

        [HttpGet("vip")]
        public IActionResult GetVip()
        {
            var vipCustomers = _unitOfWork.Customers.GetVipCustomers();
            return Ok(vipCustomers);
        }

        [HttpGet("no-bookings")]
        public IActionResult GetNoBookings()
        {
            var customers = _unitOfWork.Customers.GetCustomersWithNoBookings();
            var dtos = customers.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(MapToDto(customer));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_unitOfWork.Customers.EmailExists(dto.Email))
            {
                return BadRequest("Customer email already exists.");
            }

            var customer = new Customer
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim()
            };

            _unitOfWork.Customers.Add(customer);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, MapToDto(customer));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateCustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _unitOfWork.Customers.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            if (_unitOfWork.Customers.EmailExists(dto.Email, id))
            {
                return BadRequest("Customer email already exists.");
            }

            customer.FullName = dto.FullName.Trim();
            customer.Email = dto.Email.Trim().ToLower();
            customer.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();

            _unitOfWork.Customers.Update(customer);
            _unitOfWork.Complete();

            return Ok(MapToDto(customer));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            if (_unitOfWork.Customers.HasBookings(id))
            {
                return BadRequest("Cannot delete customer because bookings exist for this customer.");
            }

            _unitOfWork.Customers.Delete(customer);
            _unitOfWork.Complete();

            return Ok("Customer deleted successfully.");
        }

        private static CustomerDTO MapToDto(Customer c) => new()
        {
            Id = c.Id,
            FullName = c.FullName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber
        };
    }
}

