namespace Game
{
    internal class GameEntity
    {
        public GameEntity(
            int id,
            string name,
            Transform transform) 
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
            Transform = transform ?? throw new ArgumentNullException(nameof(transform));
        }

        public int Id { get; }
        public string Name { get; }
        public Transform Transform { get; }
        
        public virtual void Update(Time time)
        { }

        public override int GetHashCode() => Id;

        public override string ToString() => $"Id: {Id}, Name: {Name}";
    }
}
