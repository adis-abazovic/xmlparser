namespace XmlParser.Data.Models
{
    public class DbWordDuplicate
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Frequency { get; set; }

        public DbElement DbElement { get; set; }
    }
}
