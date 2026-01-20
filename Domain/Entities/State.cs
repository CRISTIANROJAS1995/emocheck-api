namespace Domain.Entities
{
    public class State
    {
        public int StateID { get; set; }

        public string Name { get; set; }

        public State(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
