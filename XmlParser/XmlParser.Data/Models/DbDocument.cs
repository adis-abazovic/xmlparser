using System;
using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbDocument
    {
        public string ClientID { get; set; }
        public DateTime DateTimeProcessed { get; set; }

        public List<DbElement> Elements { get; set; }

    }
}
