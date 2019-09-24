using System;
using System.Linq;
using System.Text;
using System.Xml;
using Utf8Json;

namespace NUnitReporter
{
    class JsonEncoder
    {
        public string Encode(XmlDocument document, out bool success)
        {
            var root = document.SelectSingleNode("test-run") as XmlElement ?? throw new NullReferenceException();
            var result = root.GetAttribute("result");
            var builder = new StringBuilder();
            builder.Append(@"{""test case count"":")
                .Append(root.GetAttribute("testcasecount"))
                .Append(@",""total"":")
                .Append(root.GetAttribute("total"))
                .Append(@",""result"":")
                .Append(Encoding.UTF8.GetString(JsonSerializer.Serialize(result)))
                .Append(@",""passed"":")
                .Append(root.GetAttribute("passed"))
                .Append(@",""failed"":")
                .Append(root.GetAttribute("failed"))
                .Append(@",""inconclusive"":")
                .Append(root.GetAttribute("inconclusive"))
                .Append(@",""skipped"":")
                .Append(root.GetAttribute("skipped"));
            success = !result.StartsWith("Failed");
            if (success)
            {
                return builder.Append('}').ToString();
            }
            builder.Append(@",""failed-tests"":[");
            var array = root.SelectNodes("//test-case[@result=\"Failed\"]").Cast<XmlElement>().ToArray();
            for (var index = 0; index < array.Length; index++)
            {
                var node = array[index];
                if (index != 0)
                {
                    builder.Append(',');
                }
                builder.Append(@"{""fullname"":" + Encoding.UTF8.GetString(JsonSerializer.Serialize(node.GetAttribute("fullname"))));

                var failure = node.GetElementsByTagName("failure")[0] as XmlElement ?? throw new NullReferenceException();
                var messages = failure.GetElementsByTagName("message");
                if (messages.Count != 0)
                {
                    builder.Append(@",""message"":");
                    builder.Append(Encoding.UTF8.GetString(JsonSerializer.Serialize(messages[0].FirstChild.Value)));
                }

                var stackTraces = failure.GetElementsByTagName("stack-trace");
                if (stackTraces.Count != 0)
                {
                    builder.Append(@",""stack-trace"":");
                    builder.Append(Encoding.UTF8.GetString(JsonSerializer.Serialize(stackTraces[0].FirstChild.Value)));
                }
                builder.Append("}");
            }
            builder.Append(']');
            return builder.Append('}').ToString();
        }
    }
}
