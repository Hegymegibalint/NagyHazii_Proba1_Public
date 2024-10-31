using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nagyhazii_Proba1.Model;

namespace Nagyhazii_Proba1.Application.Services
{
    namespace Nagyhazii_Proba1.Application.Services
    {
        public interface ICustomerService
        {
            Task<List<Customer>> GetAllCustomersAsync();
            Task<Customer> GetCustomerByIdAsync(int id);
            Task AddCustomerAsync(Customer customer);
            Task AddCustomersAsync(IEnumerable<Customer> customers);
            Task UpdateCustomerAsync(Customer customer);
            Task DeleteCustomerAsync(int id);
            List<Customer> LoadCustomersFromXml(string xmlPath);
        }
    }
}
