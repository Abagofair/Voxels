using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class Skybox : Renderable
    {
        public Skybox(
            int id,
            SkyboxMaterial skyboxMaterial)
            : base(id, skyboxMaterial, Model.CreateUnitCube())
        {
        }

        protected override void SetupState()
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.FrontFace(FrontFaceDirection.Ccw);
        }

        protected override void RemoveState()
        {
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.DepthFunc(DepthFunction.Less);
        }
    }
}
