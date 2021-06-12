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

        public string Name { get; set; }
        public List<string> Values { get; set; }
        public string ValuesJoined { get { return String.Join(' ', Values); } }
        public int Frequency { get; set; }

        public override string ToString()
        {
            return String.Format($"Name: {Name}, Frequency: {Frequency} \n Values: {String.Join(" \n\n", Values)}");
        }
    }
}
