using System;

namespace Awabot.Core.Structures
{
    public enum AwakeComparisonMethod
    {
        Exact,
        Contains,
    }

    public class Awake : ICloneable
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string SubstringToFind { get; set; }
        public short TypeIndex { get; set; }
        public int? Value { get; set; }
        public AwakeComparisonMethod ComparisonMethod { get; set; } = AwakeComparisonMethod.Exact;

        public object Clone()
        {
            return new Awake()
            {
                Name = Name,
                Text = Text,
                TypeIndex = TypeIndex,
                Value = Value,
            };
        }

        public override string ToString() => "Name: " + Name + ", Value: " + Value.ToString();
    }
}
