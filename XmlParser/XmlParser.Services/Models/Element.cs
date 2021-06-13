using System;
using System.Collections.Generic;

namespace XmlParser.Services.Models
{
    public class Element
    {
        public Element(string name, List<string> values, int frequency)
        {
            Name = name;
            Values = values;
            Frequency = frequency;
        }

        private List<Word> _wordDuplicates;

        public string Name { get; set; }
        public List<string> Values { get; set; }
        public string ValuesJoined { 
            get 
            { 
                if (Values != null && Values.Count > 0)
                {
                    return String.Join(' ', Values);
                }
                return String.Empty;
            } 
        }
        public int Frequency { get; set; }

        public List<Word> WordDuplicates
        {
            get
            {
                if (_wordDuplicates == null)
                {
                    _wordDuplicates = GetWordDuplicates();
                    return _wordDuplicates;
                }
                return _wordDuplicates;
            }
        }

        public override string ToString()
        {
            return String.Format($"Name: {Name}, Frequency: {Frequency} \n Values: {String.Join(" \n\n", Values)}");
        }

        private List<Word> GetWordDuplicates()
        {
            var wordFreqPairs = new Dictionary<string, int>();
            _wordDuplicates = new List<Word>();

            if (ValuesJoined != String.Empty)
            {
                var splitted = ValuesJoined.Split(new char[] { ' ', '\n' }, StringSplitOptions.TrimEntries);
                foreach (var word in splitted)
                {
                    var cleanWord = word.Replace(",", String.Empty).Replace(".", String.Empty).Replace("\n", String.Empty);
                    if (!wordFreqPairs.ContainsKey(cleanWord))
                    {
                        wordFreqPairs.Add(cleanWord, 1);
                    }
                    else
                    {
                        wordFreqPairs[cleanWord]++;
                    }
                }

                foreach (var wordPair in wordFreqPairs)
                {
                    if (wordPair.Value > 1)
                    {
                        _wordDuplicates.Add(new Word(wordPair.Key, wordPair.Value));
                    }
                };
            }
            return _wordDuplicates;
        }
    }
}
