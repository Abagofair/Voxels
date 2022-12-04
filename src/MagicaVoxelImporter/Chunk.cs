using System.Numerics;

namespace MagicaVoxelImporter
{
    public class Chunk
    {
        public Vector3 Dimensions { get; set; } = Vector3.Zero;
        public Block[] Blocks { get; set; } = Array.Empty<Block>();
    }
}
