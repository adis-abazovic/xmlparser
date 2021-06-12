using System.Linq;
using System.Threading.Tasks;

namespace XmlParser.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> AddAsync(TEntity entity);
    }
}
