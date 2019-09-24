using System;
using System.Text;
using System.IO;
using System.Xml;
using Utf8Json;

namespace NUnitReporter
{
    class Program
    {
        static int Main(string[] args)
        {
            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(args[0], Encoding.UTF8));
            switch (args[1])
            {
                case "--json":
                    {
                        using var writer = new StreamWriter(args[2]);
                        writer.Write(@"{""text"":");
                        writer.Write(Encoding.UTF8.GetString(JsonSerializer.Serialize(new JsonEncoder().Encode(doc, out _))));
                        writer.Write('}');
                        return 0;
                    }
                case "--console":
                    {
                        Console.WriteLine(new ConsoleEncoder().Encode(doc, out _));
                        return 0;
                    }
                default:
                    return 1;
            }
        }
    }
}
