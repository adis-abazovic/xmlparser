using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [HttpGet]
        public async Task<ActionResult> Process()
        {
            var xmlPath = @"C:\Users\Adis\Desktop\XMLPlay\congree.xml";
            using (StreamReader stream = new StreamReader(xmlPath))
            {
                await _xmlService.Process(stream.BaseStream, new List<string>() { "p", "li" } );
            }

            return new OkObjectResult(1);
        }
    }
}
