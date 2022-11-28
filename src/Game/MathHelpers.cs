using OpenTK.Mathematics;

namespace Game
{
    internal static class MathHelpers
    {
        public static Matrix4 CreatePerspectiveFieldOfViewLH(float fovy, float aspect, float depthNear, float depthFar)
        {
            if (fovy <= 0f || (double)fovy > Math.PI)
            {
                throw new ArgumentOutOfRangeException("fovy");
            }

            if (aspect <= 0f)
            {
                throw new ArgumentOutOfRangeException("aspect");
            }

            if (depthNear <= 0f)
            {
                throw new ArgumentOutOfRangeException("depthNear");
            }

            if (depthFar <= 0f)
            {
                throw new ArgumentOutOfRangeException("depthFar");
            }

            float halfTan = MathF.Tan(fovy * 0.5f);

            float zoomX = 1.0f / (halfTan * aspect);
            float zoomY = 1.0f / (halfTan);

            //[-1, 1]
            float near = (depthFar + depthNear) / (depthFar - depthNear);
            float far = (-2f * depthFar * depthNear) / (depthFar - depthNear);

            Matrix4 result = new()
            {
                Row0 = new Vector4(zoomX, 0.0f, 0.0f, 0.0f),
                Row1 = new Vector4(0.0f, zoomY, 0.0f, 0.0f),
                Row2 = new Vector4(0.0f, 0.0f, near, 1.0f),
                Row3 = new Vector4(0.0f, 0.0f, far, 0.0f)
            };

            return result;
        }
    }
}
