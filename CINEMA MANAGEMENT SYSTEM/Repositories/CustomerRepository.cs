using Microsoft.EntityFrameworkCore;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.CustomerDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CinemaDbContext context) : base(context)
        {
        }

        public IEnumerable<Customer> GetCustomersWithBookings()
        {
            return _context.Customers
                .Include(c => c.Bookings)
                .ToList();
        }

        public Customer? GetCustomerWithBookingsById(int id)
        {
            return _context.Customers
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<CustomerVipDTO> GetVipCustomers()
        {
            return _context.Customers
                .Select(c => new CustomerVipDTO
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    TotalSpent = c.Bookings.Sum(b => (decimal?)b.TotalPrice) ?? 0m
                })
                .OrderByDescending(c => c.TotalSpent)
                .ToList();
        }

        public IEnumerable<Customer> GetCustomersWithNoBookings()
        {
            return _context.Customers
                .Where(c => !c.Bookings.Any())
                .ToList();
        }

        public bool HasBookings(int customerId)
        {
            return _context.Bookings.Any(b => b.CustomerId == customerId);
        }

        public bool EmailExists(string email, int? excludeId = null)
        {
            var query = _context.Customers.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }
            return query.Any(c => c.Email.ToLower() == email.Trim().ToLower());
        }
    }
}

