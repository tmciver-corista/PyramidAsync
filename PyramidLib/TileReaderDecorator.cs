using System.Drawing;

namespace Pyramid.Reader
{
    /**
     * A class to serve as a base class for different types of TileReader decorators.
     */
    public abstract class TileReaderDecorator : TileReader
    {
        protected TileReader tileReader;

        public TileReaderDecorator(TileReader tileReader)
        {
            this.tileReader = tileReader;
        }

        public abstract uint TileWidth { get; }
        public abstract uint TileHeight { get; }
        public abstract uint NumberTilesX { get; }
        public abstract uint NumberTilesY { get; }
        public abstract Bitmap read(uint tileX, uint tileY);
    }
}
