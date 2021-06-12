using System.Collections.Generic;

namespace XmlParser.Data.Models
{
    public class DbElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public ICollection<DbWordDuplicate> WordDuplicates { get; set; }

        public DbDocument Document { get; set; }
    }
}
