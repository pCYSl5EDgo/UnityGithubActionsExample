using System;
using System.Runtime.Serialization;

namespace NUnitReporter
{
    [Serializable]
    public struct MarkDownText
    {
        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "text")]
        public string Text;

        public MarkDownText(string text)
        {
            Type = "mrkdwn";
            Text = text;
        }
    }
}
