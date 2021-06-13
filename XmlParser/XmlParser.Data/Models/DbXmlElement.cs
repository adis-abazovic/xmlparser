using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbXmlElement
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }

        public ICollection<DbWordDuplicate> WordDuplicates { get; set; }

        public DbXmlDocument Document { get; set; }
    }
}
