using System;
using System.Drawing;
using Pyramid.Reader;

namespace Pyramid.Reader
{
    public interface TileReader
    {
        uint TileWidth { get; }
        uint TileHeight { get; }
        uint NumberTilesX { get; }
        uint NumberTilesY { get; }
        Bitmap read(uint tileX, uint tileY);
    }
}
