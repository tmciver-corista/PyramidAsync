using System;
using System.Drawing;
using Pyramid.Reader;

namespace Pyramid.Reader
{
    public abstract class TileReader
    {
        protected ImageReader ImageReader;
        public uint TileWidth { get; private set; }
        public uint TileHeight { get; private set; }

        public TileReader(ImageReader imageReader, uint tileWidth, uint tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            ImageReader = imageReader;
        }

        public abstract Bitmap read(uint tileX, uint tileY);
    }
}
