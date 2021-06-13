using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using XmlParser.Data.Models;
using XmlParser.Data.Repositories;
using XmlParser.Services.Models;
using System.Linq;

namespace XmlParser.Services.Services
{
    public class XmlService : IXmlService
    {
        private readonly IDbDocumentRepository _dbDocumentRepository;

        public XmlService(IDbDocumentRepository dbDocumentRepository)
        {
            _dbDocumentRepository = dbDocumentRepository;
        }

        public async Task<List<Element>> Process(Stream xmlStream, string fileName, string clientId, List<string> filterElements)
        {
            var newDbDocument = new DbXmlDocument()
            {
                ClientID = clientId,
                FileName = fileName,
                DateTimeProcessed = DateTime.Now,
                Elements = new List<DbXmlElement>()
            };

            var parsedXml = await this.ParseXml(xmlStream, filterElements);
            foreach (var elem in parsedXml)
            {
                var newDbElement = new DbXmlElement()
                {
                    Tag = elem.Value.Name,
                    Content = elem.Value.ValuesJoined,
                    WordDuplicates = new List<DbWordDuplicate>()
                };

                //var wordDuplicates = elem.Value.FindWordDuplicates();

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

            await _dbDocumentRepository.AddAsync(newDbDocument);

            return parsedXml.Values.ToList();
        }

        private async Task<Dictionary<string, Element>> ParseXml(Stream xmlStream, List<string> filterElements)
        {
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
                            }
                        }
                        else if (xmlReader.NodeType == XmlNodeType.Text && !String.IsNullOrEmpty(await xmlReader.GetValueAsync()))
                        {
                            prevElementValue = xmlReader.Value;
                        }
                    }
                }
            }
            return elemValuePairs;
        }

        //private Dictionary<string, int> FindWordDuplicates(string content)
        //{
        //    var wordFreqPairs = new Dictionary<string, int>();

        //    var splitted = content.Split(new char[] { ' ', '\n' }, StringSplitOptions.TrimEntries);
        //    foreach (var word in splitted)
        //    {
        //        var cleanWord = word.Replace(",", String.Empty).Replace(".", String.Empty).Replace("\n", String.Empty);
        //        if (!wordFreqPairs.ContainsKey(cleanWord))
        //        {
        //            wordFreqPairs.Add(cleanWord, 1);
        //        }
        //        else
        //        {
        //            wordFreqPairs[cleanWord]++;
        //        }
        //    }

        //    return wordFreqPairs;
        //}
    }
}
