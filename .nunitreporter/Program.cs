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
            Console.Write(Encoding.UTF8.GetString(JsonSerializer.Serialize(new JsonEncoder().Encode(doc, out var success))));
            return success ? 0 : 1;
        }
    }
}
