using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Data.Models;

namespace XmlParser.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DocumentDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
