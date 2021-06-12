using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XmlParser.Data.Repositories;
using XmlParser.Services.Models;

namespace XmlParser.Services.Services
{
    public class XmlService : IXmlService
    {
        private readonly IDbDocumentRepository _dbDocumentRepository;

        public XmlService(IDbDocumentRepository dbDocumentRepository)
        {
            _dbDocumentRepository = dbDocumentRepository;
        }

        public async Task<Dictionary<string, Element>> Process(Stream xmlStream, List<string> filterElements)
        {
            // TODO: Process XML
            // TODO: Save result to database

            throw new NotImplementedException();
        }
    }
}
