using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace NUnitReporter
{
    class ConsoleEncoder
    {
        public string Encode(XmlDocument document, out bool success)
        {
            var root = document.SelectSingleNode("test-run") as XmlElement ?? throw new NullReferenceException();
            var result = root.GetAttribute("result");
            var builder = new StringBuilder();
            builder.Append("test case count : ")
                .Append(root.GetAttribute("testcasecount"))
                .Append("\ntotal : ")
                .Append(root.GetAttribute("total"))
                .Append("\nresult : ")
                .Append(result)
                .Append("\npassed : ")
                .Append(root.GetAttribute("passed"))
                .Append("\nfailed : ")
                .Append(root.GetAttribute("failed"))
                .Append("\ninconclusive : ")
                .Append(root.GetAttribute("inconclusive"))
                .Append("\nskipped : ")
                .Append(root.GetAttribute("skipped"));
            success = !result.StartsWith("Failed");
            if (success)
            {
                return builder.ToString();
            }
            var array = root.SelectNodes("//test-case[@result=\"Failed\"]").Cast<XmlElement>().ToArray();
            for (var index = 0; index < array.Length; index++)
            {
                var node = array[index];
                if (index != 0)
                {
                    builder.Append('\n');
                }
                builder.Append(@"fullname : " + node.GetAttribute("fullname"));

                var failure = node.GetElementsByTagName("failure")[0] as XmlElement ?? throw new NullReferenceException();
                var messages = failure.GetElementsByTagName("message");
                if (messages.Count != 0)
                {
                    builder.Append("\n\tmessage : ");
                    builder.Append(messages[0].FirstChild.Value);
                }

                var stackTraces = failure.GetElementsByTagName("stack-trace");
                if (stackTraces.Count != 0)
                {
                    builder.Append("\n\tstack-trace : ");
                    builder.Append(stackTraces[0].FirstChild.Value);
                }
            }
            return builder.ToString();
        }
    }
}
