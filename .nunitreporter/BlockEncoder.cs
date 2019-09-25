using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Utf8Json;

namespace NUnitReporter
{
    class BlockEncoder
    {
        private string repository;
        public BlockEncoder(string repository)
        {
            this.repository = repository;
        }
        public string Encode(XmlDocument document, out bool success)
        {
            var root = document.SelectSingleNode("test-run") as XmlElement ?? throw new NullReferenceException();
            var result = root.GetAttribute("result");
            var builder = new StringBuilder();
            success = !result.StartsWith("Failed");
            var tmpBuilder = new StringBuilder();
            {
                tmpBuilder.Clear()
                    .Append("<https://github.com/")
                    .Append(repository)
                    .Append(success ? "/actions|*ACTION SUCCESS*>" : "/actions|*ACTION FAIL*>")
                    .Append("\n\ntest case count : ")
                    .Append(root.GetAttribute("testcasecount"))
                    .Append("\ntotal : ")
                    .Append(root.GetAttribute("total"))
                    .Append("\nresult : *")
                    .Append(result)
                    .Append("*\npassed : ")
                    .Append(root.GetAttribute("passed"))
                    .Append("\nfailed : ")
                    .Append(root.GetAttribute("failed"))
                    .Append("\ninconclusive : ")
                    .Append(root.GetAttribute("inconclusive"))
                    .Append("\nskipped : ")
                    .Append(root.GetAttribute("skipped"));
                builder.Append(Encoding.UTF8.GetString(JsonSerializer.Serialize(new BlockSection(tmpBuilder.ToString()))));
            }
            success = !result.StartsWith("Failed");
            if (success)
            {
                return builder.ToString();
            }

            var array = root.SelectNodes("//test-case[@result=\"Failed\"]").Cast<XmlElement>().ToArray();
            builder.Append(@",{""type"":""divider""},");
            {
                tmpBuilder.Clear();
                for (var index = 0; index < array.Length; index++)
                {
                    var node = array[index];
                    if (index != 0)
                    {
                        tmpBuilder.Append('\n');
                    }
                    tmpBuilder.Append(@"fullname : " + node.GetAttribute("fullname"));

                    var failure = node.GetElementsByTagName("failure")[0] as XmlElement ?? throw new NullReferenceException();
                    var messages = failure.GetElementsByTagName("message");
                    if (messages.Count != 0)
                    {
                        tmpBuilder.Append("\n  message :");
                        var firstChildValue = messages[0].FirstChild.Value;

                        var lines = firstChildValue.Split('\n');
                        foreach (var line in lines)
                        {
                            tmpBuilder.Append("\n    ").Append(line.TrimEnd());
                        }
                    }

                    var stackTraces = failure.GetElementsByTagName("stack-trace");
                    if (stackTraces.Count != 0)
                    {
                        tmpBuilder.Append("\n  stack-trace :");
                        var firstChildValue = stackTraces[0].FirstChild.Value;
                        var lines = firstChildValue.Split('\n');
                        foreach (var line in lines)
                        {
                            tmpBuilder.Append("\n    ").Append(line.TrimEnd());
                        }
                    }
                }
                builder.Append(Encoding.UTF8.GetString(JsonSerializer.Serialize(new BlockSection(tmpBuilder.ToString()))));
            }
            return builder.ToString();
        }
    }
}
