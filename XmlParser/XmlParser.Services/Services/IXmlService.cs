﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Services.Models;

namespace XmlParser.Services.Services
{
    public interface IXmlService
    {
        Task<Dictionary<string, Element>> Process(Stream xmlStream, List<string> filterElements);
    }
}
