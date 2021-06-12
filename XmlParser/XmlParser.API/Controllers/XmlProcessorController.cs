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
    public class XmlProcessorController : Controller
    {
        private readonly IXmlService _xmlService;

        public XmlProcessorController(IXmlService xmlService)
        {
            _xmlService = xmlService;
        }

        public async Task<ActionResult<Dictionary<string, Element>>> Process(Stream stream, List<string> filterElements)
        {
            return await _xmlService.Process(stream, filterElements);
        }
    }
}
