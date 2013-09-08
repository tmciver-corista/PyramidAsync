using System;
using System.Drawing;

namespace Pyramid.Reader
{
    /// <summary>
    /// The DefaultTileReader considers the tile located at the top left of the image
    /// to have coordinates (0, 0).  Currently tile reads that fall outside of the
    /// image at all throw an exception.  This implies that the user must supply tile
    /// dimensions that evenly divide the image dimensions if the entire image is to
    /// be read.
    /// </summary>
    class DefaultTileReader : TileReader
    {
        public DefaultTileReader(ImageReader imageReader, uint tileWidth, uint tileHeight) : base(imageReader, tileWidth, tileHeight)
        {
        }

        public DefaultTileReader(ImageReader imageReader, uint tileDimension) : this(imageReader, tileDimension, tileDimension)
        {
        }

        public override Bitmap read(uint tileX, uint tileY)
        {
            return ImageReader.read(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
