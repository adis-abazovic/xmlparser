using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Services.Models;
using XmlParser.Services.Services;

namespace XmlParser.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlProcessorController : Controller
    {
        private readonly IXmlService _xmlService;

        public XmlProcessorController(IXmlService xmlService)
        {
            _xmlService = xmlService;
        }

        [HttpPost]
        [Route("process")]
        public async Task<ActionResult> Process(string fileName, string filterElements)
        {
            var stream = new MemoryStream();
            await Request.Body.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var elements = await _xmlService.Process(stream, fileName, Guid.NewGuid().ToString(), filterElements.Split(';').ToList());

            return new OkObjectResult(FormatResponseMessage(elements));
        }

        private String FormatResponseMessage(List<Element> elements)
        {
            StringBuilder responseData = new StringBuilder();
            foreach (var elem in elements)
            {
                responseData.Append(String.Format($"\nElement {elem.Name}: {elem.Frequency} found \n"));

                responseData.Append("Duplicated words: \n");
                foreach (var wordDuplicate in elem.WordDuplicates)
                {
                    responseData.Append(String.Format($"\t\"{wordDuplicate.Text}\" - {wordDuplicate.Frequency} duplicates found \n"));
                }
            }

            return responseData.ToString();
        }
    }
}
