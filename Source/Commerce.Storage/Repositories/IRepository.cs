using System.Threading.Tasks;
using Commerce.Storage.Entities;

namespace Commerce.Storage.Repositories
{
    public interface IRepository
    {
        Task<BasketEntity> InsertOrUpdate(BasketEntity basket);

        Task<BasketEntity> GetById(string id);

        Task<bool> Exists(string id);
    }
}