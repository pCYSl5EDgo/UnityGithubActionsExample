using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;

namespace _nunitreporter
{
    class Program
    {
        static int Main(string[] args)
        {
            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(args[0], Encoding.UTF8));
            var root = doc.SelectSingleNode("test-run") as XmlElement ?? throw new NullReferenceException();
            var result = root.GetAttribute("result");
            Console.WriteLine("test case count : " + root.GetAttribute("testcasecount"));
            Console.WriteLine("total : " + root.GetAttribute("total"));
            Console.WriteLine("result : " + result);
            Console.WriteLine("passed : " + root.GetAttribute("passed"));
            Console.WriteLine("failed : " + root.GetAttribute("failed"));
            Console.WriteLine("inconclusive : " + root.GetAttribute("inconclusive"));
            Console.WriteLine("skipped : " + root.GetAttribute("skipped"));
            if (result.StartsWith("Failed"))
            {
                foreach (var node in doc.SelectNodes("//test-case[@result=\"Failed\"]").Cast<XmlElement>())
                {
                    Console.WriteLine("\tfullname : " + node.GetAttribute("fullname"));
                    var failure = node.GetElementsByTagName("failure")[0] as XmlElement ?? throw new NullReferenceException();
                    var messages = failure.GetElementsByTagName("message");
                    if (messages.Count != 0)
                    {
                        Console.WriteLine("\tMessage");
                        var value = messages[0].FirstChild.Value.Split('\n');
                        foreach (var s in value.Select(x => x.TrimEnd()))
                        {
                            Console.WriteLine("\t\t" + s);
                        }
                    }

                    var stackTraces = failure.GetElementsByTagName("stack-trace");
                    if (stackTraces.Count != 0)
                    {
                        Console.WriteLine("\tStack Trace");
                        var value = stackTraces[0].FirstChild.Value.Split('\n');
                        foreach (var s in value.Select(x => x.TrimEnd()))
                        {
                            Console.WriteLine("\t\t" + s);
                        }
                    }
                }
                return 1;
            }
            return 0;
        }
    }
}
