using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbElement
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public List<DbWordDuplicate> WordDuplicates { get; set; }
    }
}
