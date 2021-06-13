using System;
using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbXmlDocument
    {
        public int Id { get; set; }
        public string ClientID { get; set; }
        public string FileName { get; set; }
        public DateTime DateTimeProcessed { get; set; }

        public ICollection<DbXmlElement> Elements { get; set; }
    }
}
