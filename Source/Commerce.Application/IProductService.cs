using System.Collections.Generic;
using System.Threading.Tasks;
using Commerce.Domain;

namespace Commerce.Application
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> GetById(int id);

        Task<bool> Exists(int id);
    }
}