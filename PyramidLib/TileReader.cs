using System;
using System.Drawing;
using Pyramid.Reader;

namespace Pyramid.Reader
{
    public class TileReader
    {
        private ImageReader reader;
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        public TileReader(ImageReader imageReader, int width, int height)
        {
            Width = width;
            Height = height;
            reader = imageReader;
        }

        public TileReader(ImageReader imageReader, int tileDimension) : this(imageReader, tileDimension, tileDimension)
        {
        }

        public Bitmap read(int tileX, int tileY)
        {
            return reader.read(tileX * Width, tileY * Height, Width, Height);
        }
    }
}
