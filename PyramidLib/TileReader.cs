using System;
using System.Drawing;
using Pyramid.Reader;

namespace Pyramid.Reader
{
    public class TileReader
    {
        private ImageReader reader;
        
        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public TileReader(ImageReader imageReader, uint width, uint height)
        {
            Width = width;
            Height = height;
            reader = imageReader;
        }

        public TileReader(ImageReader imageReader, uint tileDimension) : this(imageReader, tileDimension, tileDimension)
        {
        }

        public Bitmap read(uint tileX, uint tileY)
        {
            return reader.read(tileX * Width, tileY * Height, Width, Height);
        }
    }
}
