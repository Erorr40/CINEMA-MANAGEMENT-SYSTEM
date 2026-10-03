using CINEMA_MANAGEMENT_SYSTEM.DTOs.CustomerDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Customer> GetCustomersWithBookings();
        Customer? GetCustomerWithBookingsById(int id);
        IEnumerable<CustomerVipDTO> GetVipCustomers();
        IEnumerable<Customer> GetCustomersWithNoBookings();
        bool HasBookings(int customerId);
        bool EmailExists(string email, int? excludeId = null);
    }
}

