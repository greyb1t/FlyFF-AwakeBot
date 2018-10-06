namespace FlyFF_AwakeBot
{
    public class Awake
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public short? TypeIndex { get; set; }

        public int? Value { get; set; }

        public Awake() { }

        public Awake(string awakeText)
        {
            Text = awakeText;
        }

        public Awake(string awakeName, string awakeText, short? typeIndex)
        {
            Name = awakeName;
            Text = awakeText;
            TypeIndex = typeIndex;
        }

        public static Awake Empty {
            get {
                Awake awake = new Awake("", "", -1)
                {
                    Value = -1
                };
                return awake;
            }
        }

        public override string ToString()
        {
            return "Name: " + Name + ", Value: " + Value.ToString();
        }
    }
}
