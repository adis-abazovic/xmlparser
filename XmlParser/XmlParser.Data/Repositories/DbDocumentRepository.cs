using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using XmlParser.Data.Models;

namespace XmlParser.Data.Repositories
{
    public class DbDocumentRepository : Repository<DbDocument>, IDbDocumentRepository
    {
        public DbDocumentRepository(DocumentDbContext documentDbContext) : base(documentDbContext)
        {
        }

        public Task<DbDocument> GetDocumentByClientAsync(string clientId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.ClientID == clientId);
        }
    }
}
