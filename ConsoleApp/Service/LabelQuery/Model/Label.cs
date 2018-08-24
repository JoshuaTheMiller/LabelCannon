namespace Service.LabelQuery.Model
{
    public sealed class Label
    {
        public Label(string name, string description, string color)
        {
            Name = name;
            Description = description;
            Color = color;
        }

        public string Name { get; }
        public string Description { get; }
        public string Color { get; }
    }
}