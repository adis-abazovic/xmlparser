using System;
using System.Linq;
using System.Threading.Tasks;
using XmlParser.Data.Models;

namespace XmlParser.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly DocumentDbContext XmlDocDbContext;

        public Repository(DocumentDbContext xmlDocDbContext)
        {
            XmlDocDbContext = xmlDocDbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return XmlDocDbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await XmlDocDbContext.AddAsync(entity);
                await XmlDocDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
    }
}
