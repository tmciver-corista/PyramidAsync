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
        private ImageReader ImageReader;
        private uint tileWidth, tileHeight;
        
        public DefaultTileReader(ImageReader imageReader, uint tileWidth, uint tileHeight)
        {
            this.ImageReader = imageReader;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

        public uint TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public uint TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public DefaultTileReader(ImageReader imageReader, uint tileDimension) : this(imageReader, tileDimension, tileDimension)
        {
        }

        public Bitmap read(uint tileX, uint tileY)
        {
            return ImageReader.read(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
