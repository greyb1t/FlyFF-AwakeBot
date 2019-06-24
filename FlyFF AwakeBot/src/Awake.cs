using System;

namespace FlyFF_AwakeBot
{
    public class Awake : ICloneable
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public short TypeIndex { get; set; }
        public int? Value { get; set; }

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
