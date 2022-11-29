namespace Game
{
    internal class SceneTree
    {
        public SceneTree(GameEntity gameObject)
        {
            Root = new Node(gameObject);
        }

        public Node Root { get; }

        public void Update(Time time)
        {
            foreach (var child in Root.Children)
            {
                child.Update(Root, time);
            }
        }

        public HashSet<GameEntity> GetFlattenedTree()
        {
            return Root.GetChildrenEntities().ToHashSet();
        }

        public HashSet<DirectionalLight> GetDirectionalLights()
        {
            return GetFlattenedTree()
                .Where(x => x is DirectionalLight)
                .Cast<DirectionalLight>()
                .ToHashSet();
        }

        public class Node
        {
            public Node(GameEntity gameObject)
            {
                GameEntity = gameObject;
            }

            public GameEntity GameEntity { get; }
            public List<Node> Children { get; } = new List<Node>();

            public void Update(Node parent, Time time)
            {
                GameEntity.Update(time);
                GameEntity.Transform.Parent = parent.GameEntity.Transform.Matrix;

                foreach (Node child in Children)
                {
                    child.Update(this, time);
                }
            }

            public IEnumerable<GameEntity> GetChildrenEntities()
            {
                foreach (var child in Children)
                {
                    foreach (var childEntity in child.GetChildrenEntities())
                    {
                        yield return childEntity;
                    }
                }

                yield return GameEntity;
            }
        }
    }
}
