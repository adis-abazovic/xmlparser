using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using XmlParser.Data.Models;
using XmlParser.Data.Repositories;
using XmlParser.Services.Models;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using XmlParser.Services.Notifications;

namespace XmlParser.Services.Services
{
    public class XmlService : IXmlService
    {
        private readonly IDbDocumentRepository _dbDocumentRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private string _clientId;

        public XmlService(IDbDocumentRepository dbDocumentRepository, IHubContext<NotificationHub> hubContext)
        {
            _dbDocumentRepository = dbDocumentRepository;
            _hubContext = hubContext;
        }

        public async Task<List<Element>> Process(Stream xmlStream, string fileName, string clientId, List<string> filterElements)
        {
            _clientId = clientId;
            var newDbDocument = new DbXmlDocument()
            {
                ClientID = clientId,
                FileName = fileName,
                DateTimeProcessed = DateTime.Now,
                Elements = new List<DbXmlElement>()
            };

            var parsedXml = await this.ParseXml(xmlStream, filterElements);

            await _hubContext.Clients.All.SendAsync(clientId, $"\t Searching for word duplicates - Started");
            foreach (var elem in parsedXml)
            {
                var newDbElement = new DbXmlElement()
                {
                    Tag = elem.Value.Name,
                    Content = elem.Value.ValuesJoined,
                    WordDuplicates = new List<DbWordDuplicate>()
                };

                foreach (var wordPair in elem.Value.WordDuplicates)
                {
                    if (wordPair.Frequency > 1)
                    {
                        newDbElement.WordDuplicates.Add(new DbWordDuplicate()
                        {
                            Word = wordPair.Text,
                            Frequency = wordPair.Frequency
                        });
                    }
                    
                };

                newDbDocument.Elements.Add(newDbElement);
            }

            await _hubContext.Clients.All.SendAsync(clientId, $"\t Searching for word duplicates - Finished\n");

            await _hubContext.Clients.All.SendAsync(clientId, $"\t Saving into database - Started");
            await _dbDocumentRepository.AddAsync(newDbDocument);
            await _hubContext.Clients.All.SendAsync(clientId, $"\t Saving into database - Finished\n");

            return parsedXml.Values.ToList();
        }

        private async Task<Dictionary<string, Element>> ParseXml(Stream xmlStream, List<string> filterElements)
        {
            await _hubContext.Clients.All.SendAsync(_clientId, $"\t Parsing XML - Started");
            var elemValuePairs = new Dictionary<string, Element>();
            using (var xmlReader = XmlReader.Create(xmlStream, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse, Async = true}))
            {
                var prevElementName = String.Empty;
                var prevElementValue = String.Empty;
                while (await xmlReader.ReadAsync())
                {
                    if (xmlReader.NodeType != XmlNodeType.Whitespace)
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            if (!String.IsNullOrEmpty(xmlReader.Name))
                            {
                                prevElementName = xmlReader.Name;
                            }
                        }
                        else if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            if (!String.IsNullOrEmpty(xmlReader.Name) && filterElements.Contains(xmlReader.Name) && xmlReader.Name == prevElementName)
                            {
                                if (!elemValuePairs.TryGetValue(xmlReader.Name, out Element elementModel))
                                {
                                    elemValuePairs.Add(
                                        xmlReader.Name,
                                        new Element(xmlReader.Name, new List<string>() { prevElementValue },
                                        1
                                    ));
                                }
                                else
                                {
                                    elementModel.Frequency++;
                                    elementModel.Values.Add(prevElementValue);
                                }

                                await _hubContext.Clients.All.SendAsync(_clientId, $"\t\t {xmlReader.Name} : {prevElementValue}");
                            }
                        }
                        else if (xmlReader.NodeType == XmlNodeType.Text && !String.IsNullOrEmpty(await xmlReader.GetValueAsync()))
                        {
                            prevElementValue = xmlReader.Value;
                        }
                    }
                }
            }

            await _hubContext.Clients.All.SendAsync(_clientId, $"\t Parsing XML - Finished\n");
            return elemValuePairs;
        }
    }
}
