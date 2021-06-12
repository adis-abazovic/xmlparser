using System.Threading.Tasks;
using XmlParser.Data.Models;

namespace XmlParser.Data.Repositories
{
    public interface IDbDocumentRepository : IRepository<DbDocument>
    {
        Task<DbDocument> GetDocumentByClientAsync(string clientId);
    }
}
