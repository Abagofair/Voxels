using System.Numerics;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace MagicaVoxelImporter
{
    //https://github.com/ephtracy/voxel-model/blob/master/MagicaVoxel-file-format-vox.txt
    public class Importer
    {
        private const string FileHeaderName = "VOX ";
        private const string MainChunkId = "MAIN";
        private const string SizeChunkId = "SIZE";
        private const string ChildChunkId = "XYZI";
        private const string RgbaChunkid = "RGBA";

        private const int ChunkIdSize = 4;
        private const int ValidVersionNumber = 150;

        public Chunk[] Import(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new FileNotFoundException(nameof(path));

            using FileStream fileStream = File.OpenRead(path);

            int currentOffset = 0;
            
            //Read "VOX "
            ReadChunkId(fileStream, FileHeaderName, ref currentOffset);
            
            //Read version number 150
            int versionNumber = ReadInt32(fileStream, ref currentOffset);
            if (versionNumber != ValidVersionNumber)
                throw new Exception($"Unexpected version number {versionNumber} - expected {ValidVersionNumber}");

            //Read "MAIN"
            ReadChunkId(fileStream, MainChunkId, ref currentOffset);
            int bytesOfChunkContent = ReadInt32(fileStream, ref currentOffset);
            int bytesOfChildrenChunks = ReadInt32(fileStream, ref currentOffset);

            //Read "SIZE"
            ReadChunkId(fileStream, SizeChunkId, ref currentOffset);
            _ = ReadInt32(fileStream, ref currentOffset);
            _ = ReadInt32(fileStream, ref currentOffset);
            int sizeX = ReadInt32(fileStream, ref currentOffset);
            int sizeY = ReadInt32(fileStream, ref currentOffset);
            int sizeZ = ReadInt32(fileStream, ref currentOffset);

            //Read "XYZI"
            ReadChunkId(fileStream, ChildChunkId, ref currentOffset);
            _ = ReadInt32(fileStream, ref currentOffset);
            _ = ReadInt32(fileStream, ref currentOffset);
            int numVoxels = ReadInt32(fileStream, ref currentOffset);

            Vector4[] voxelCoordinates = new Vector4[numVoxels];
            for (int i = 0; i < numVoxels; ++i)
            {
                voxelCoordinates[i] = ReadVector4(fileStream, ref currentOffset);
            }

            Vector4[] palette = new Vector4[256];
            byte[] content = new byte[fileStream.Length - currentOffset];

            var str = new byte[4];
            fileStream.Read(content, 0, content.Length);
            for (int i = 0; i < content.Length; ++i)
            {
                str[0] = content[i];
                str[1] = content[i+1];
                str[2] = content[i+2];
                str[3] = content[i+3];
                var r = Encoding.ASCII.GetString(str);
                
                if (r == RgbaChunkid)
                {
                    i += 4;
                    i += 4;
                    i += 4;

                    for (int j = 0; j <= 254; ++j)
                    {
                        palette[j+1] = new Vector4(
                            content[i + j] / 255.0f,
                            content[i + j + 1] / 255.0f,
                            content[i + j + 2] / 255.0f,
                            content[i + j + 3] / 255.0f);

                        i += 3;
                    }
                    break;
                }
            }

            var blocks = new Block[voxelCoordinates.Length];
            var chunks = new Chunk[1];
            for (int i = 0; i < voxelCoordinates.Length; i++)
            {
                Vector4 voxel = voxelCoordinates[i];
                blocks[i] = new Block()
                {
                    Position = new Vector3(voxel.X, voxel.Z, voxel.Y),
                    Color = palette[(int)voxel.W]
                };
            }

            chunks[0] = new Chunk()
            {
                Dimensions = new Vector3(sizeX, sizeY, sizeZ),
                Blocks = blocks
            };

            return chunks;
        }

        private void ReadChunkId(FileStream fileStream,
            string expectedId, ref int currentOffset)
        {
            var content = new byte[ChunkIdSize];
            fileStream.Read(content, 0, ChunkIdSize);
            currentOffset += 4;

            var fileHeader = Encoding.ASCII.GetString(content);
            if (fileHeader != expectedId)
                throw new Exception($"Unexpected file header read {fileHeader} - expected \"{expectedId}\"");
        }

        private int ReadInt32(FileStream fileStream, ref int currentOffset)
        {
            var content = new byte[4];
            fileStream.Read(content, 0, 4);
            currentOffset += 4;
            return BitConverter.ToInt32(content);
        }

        // xyz is coords, w is RGBA index
        private Vector4 ReadVector4(FileStream fileStream, ref int currentOffset)
        {
            var content = new byte[4];
            fileStream.Read(content, 0, 4);

            return new Vector4(content[0], content[1], content[2], content[3]);
        }
    }
}