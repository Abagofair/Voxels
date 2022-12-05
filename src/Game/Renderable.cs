using Game.Voxels;
using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class Renderable
    {
        private readonly int _id;
        private Material _material;
        private IDrawable _drawable;

        public Renderable(
            int id,
            Material material,
            IDrawable drawable)
        {
            _id = id;
            _material = material ?? throw new ArgumentNullException(nameof(material));
            _drawable = drawable ?? throw new ArgumentNullException(nameof(drawable));
        }

        public Transform? Transform { get; set; }
        public int RenderOrder { get; set; }
        public bool ShouldRender { get; set; } = true;

        protected virtual void SetupState()
        { }

        protected virtual void RemoveState()
        { }

        public void Render()
        {
            if (!ShouldRender)
                return;

            SetupState();

            if (Transform != null)
            {
                _material.Shader.SetMatrix4f("Model", ref Transform.Matrix);
            }

            _material.Apply();
            _drawable.Draw();

            RemoveState();
        }

        public override int GetHashCode() => _id;
    }
}
