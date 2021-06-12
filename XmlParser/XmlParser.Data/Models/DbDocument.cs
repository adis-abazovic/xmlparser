using System;
using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbDocument
    {
        public int Id { get; set; }
        public string ClientID { get; set; }
        public string FileName { get; set; }
        public DateTime DateTimeProcessed { get; set; }

        public ICollection<DbElement> Elements { get; set; }
    }
}
