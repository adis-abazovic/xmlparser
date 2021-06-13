using System.Threading.Tasks;
using XmlParser.Data.Models;

namespace XmlParser.Data.Repositories
{
    public interface IDbDocumentRepository : IRepository<DbXmlDocument>
    {
        Task<DbXmlDocument> GetDocumentByClientAsync(string clientId);
    }
}
