namespace Game
{
    internal class GameEntity
    {
        private string _name;

        public GameEntity(
            int id,
            string name,
            Transform transform) 
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Id = id;
            _name = name;
            Transform = transform ?? throw new ArgumentNullException(nameof(transform));
        }

        public int Id { get; }
        public ref string Name => ref _name;
        public Transform Transform { get; }
        
        public virtual void Update(Time time)
        { }

        public override int GetHashCode() => Id;

        public override string ToString() => $"Id: {Id}, Name: {Name}";
    }
}
