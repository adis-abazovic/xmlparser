namespace XmlParser.Data.Models
{
    public class DbWordDuplicate
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int Frequency { get; set; }

        public DbXmlElement DbElement { get; set; }
    }
}
