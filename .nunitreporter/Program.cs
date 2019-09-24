using System;
using System.Text;
using System.IO;
using System.Xml;
using Utf8Json;

namespace NUnitReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(args[0], Encoding.UTF8));
            if (args[1] == "--json")
            {
                Console.Write(@"{""text"":");
                Console.Write(Encoding.UTF8.GetString(JsonSerializer.Serialize(new JsonEncoder().Encode(doc, out _))));
                Console.Write('}');
            }
        }
    }
}
