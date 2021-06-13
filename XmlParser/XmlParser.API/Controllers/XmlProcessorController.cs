using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Services.Models;
using XmlParser.Services.Notifications;
using XmlParser.Services.Services;

namespace XmlParser.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlProcessorController : Controller
    {
        private readonly IXmlService _xmlService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public XmlProcessorController(IXmlService xmlService, IHubContext<NotificationHub> hubContext)
        {
            _xmlService = xmlService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("process")]
        public async Task<ActionResult> Process(string fileName, string clientId, string filterElements)
        {
            var stream = new MemoryStream();
            await Request.Body.CopyToAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);

            var elements = await _xmlService.Process(stream, fileName, clientId, filterElements.Split(';').ToList());
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
                    responseData.Append(String.Format($"\t\"{wordDuplicate.Text}\" - {wordDuplicate.Frequency} found \n"));
                }
            }
            return responseData.ToString();
        }
    }
}
