using System.Diagnostics.CodeAnalysis;

namespace Game
{
    //i dunno what else to call it :(
    internal static class GameEntityManager
    {
        private static int _nextEntityId = 1;

        private static HashSet<GameEntity> _gameEntities = new HashSet<GameEntity>();
        private static Dictionary<GameEntity, Renderable> _dynamicRenderables = new Dictionary<GameEntity, Renderable>();

        [return: NotNull]
        public static T Create<T>(string name, Transform transform) where T : GameEntity
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            
            transform ??= new Transform();

            int nextId = _nextEntityId++;

            var entity = new GameEntity(nextId, name, transform);
            
            _gameEntities.Add(entity);

            return (T)entity;
        }

        public static void AddAsDynamicRenderable(GameEntity entity, Renderable renderable)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (renderable == null) throw new ArgumentNullException(nameof(renderable));

            renderable.Transform = entity.Transform;

            if (_dynamicRenderables.ContainsKey(entity))
            {
                Logger.Info($"Already contains {entity} - swapping");
                _dynamicRenderables[entity] = renderable;
            }
            else
            {
                _dynamicRenderables.Add(entity, renderable);
            }
        }

        [return: NotNull]
        public static IEnumerable<Renderable> GetRenderables(SceneTree sceneTree)
        {
            if (sceneTree == null) throw new ArgumentNullException(nameof(sceneTree));

            foreach (var item in sceneTree.GetFlattenedTree())
            {
                if (_dynamicRenderables.TryGetValue(item, out Renderable? renderable))
                    yield return renderable;
            }
        }
    }
}
