using System;
using System.Drawing;

namespace Pyramid.Reader
{
    public abstract class ImageReader
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public abstract Bitmap read(uint x, uint y, uint width, uint height);
    }
}
