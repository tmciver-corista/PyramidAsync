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
    public class DefaultTileReader : TileReader
    {
        private ImageReader ImageReader;
        private uint tileWidth, tileHeight;
        private uint numTilesX, numTilesY;
        
        public DefaultTileReader(ImageReader imageReader, uint tileWidth, uint tileHeight)
        {
            this.ImageReader = imageReader;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.numTilesX = (uint)Math.Ceiling(imageReader.Width / (double)tileWidth);
            this.numTilesY = (uint)Math.Ceiling(imageReader.Height / (double)tileHeight);
        }

        public uint TileWidth
        {
            get { return tileWidth; }
        }

        public uint TileHeight
        {
            get { return tileHeight; }
        }

        public uint NumberTilesX
        {
            get { return numTilesX; }
        }

        public uint NumberTilesY
        {
            get { return numTilesY; }
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
