namespace Game
{
    internal class Renderable
    {
        private readonly int _id;
        private Material _material;
        private Model _model;

        public Renderable(
            int id,
            Material material,
            Model model)
        {
            _id = id;
            _material = material ?? throw new ArgumentNullException(nameof(material));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public Transform? Transform { get; set; }
        public int RenderOrder { get; set; }
        public bool ShouldRender { get; set; } = true;

        public void Render()
        {
            if (!ShouldRender)
                return;

            if (Transform != null)
            {
                _material.Shader.SetMatrix4f("Model", ref Transform.Matrix);
            }

            _material.Apply();
            _model.Render();
        }

        public override int GetHashCode() => _id;
    }
}
