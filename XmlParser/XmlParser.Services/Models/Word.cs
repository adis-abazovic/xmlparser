namespace XmlParser.Services.Models
{
    public class Word
    {
        public Word(string text, int frequency)
        {
            Text = text;
            Frequency = frequency;
        }

        public string Text { get; set; }
        public int Frequency { get; set; }
    }
}
