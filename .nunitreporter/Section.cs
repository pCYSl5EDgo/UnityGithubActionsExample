using System;
using System.Runtime.Serialization;
using Utf8Json;

namespace NUnitReporter
{
    [Serializable]
    public struct BlockSection
    {
        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "text")]
        public string Text;

        public BlockSection(string text)
        {
            Type = "section";
            Text = text;
        }
    }
}
