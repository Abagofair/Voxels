namespace Game
{
    internal class Image
    {
        private byte[] _bytes;
        private int _width, _height;

        public Image(
            byte[] bytes,
            int width,
            int height)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            _width = width;
            _height = height;
        }

        public byte[] Bytes => _bytes;
        public int Width => _width;
        public int Height => _height;
    }
}
