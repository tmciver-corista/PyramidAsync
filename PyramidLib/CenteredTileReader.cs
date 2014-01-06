using System;
using System.Drawing;

namespace Pyramid.Reader
{
    class CenteredTileReader : TileReaderDecorator
    {
        private uint offsetX;
        private uint offsetY;
        private uint numTilesX, numTilesY;
        //private Bitmap blank;
        private Color color;

        public CenteredTileReader(TileReader tileReader) : this(tileReader, Color.White) { }

        public CenteredTileReader(TileReader tileReader, Color color) : base(tileReader)
        {
            this.color = color;

            offsetX = 0;
            offsetY = 0;

            // find the number of tiles for the dimension with the largest number of tiles
            uint numTilesLongestSide = Math.Max(tileReader.NumberTilesX, tileReader.NumberTilesY);

            // find the number of tiles that is a power of two that is greater than
            // or equal to numTilesLongestSide
            uint numTilesPowerOf2 = 0;
            uint powerOf2 = 0;
            while (numTilesPowerOf2 < numTilesLongestSide)
            {
                powerOf2++;
                numTilesPowerOf2 = (uint)Math.Pow(2, powerOf2);
            }

            numTilesX = numTilesPowerOf2;
            numTilesY = numTilesPowerOf2;

            // calculate the offsets
            offsetX = (numTilesPowerOf2 - tileReader.NumberTilesX) / 2;
            offsetY = (numTilesPowerOf2 - tileReader.NumberTilesY) / 2;

            // initialize the blank Bitmap
            
        }

        public override uint TileWidth
        {
            get { return tileReader.TileWidth; }
        }

        public override uint TileHeight
        {
            get { return tileReader.TileHeight; }
        }

        public override uint NumberTilesX
        {
            get { return numTilesX; }
        }

        public override uint NumberTilesY
        {
            get { return numTilesY; }
        }

        public override Bitmap read(uint tileX, uint tileY)
        {
            //Console.WriteLine("CenteredTileReader: {0}, {1}.", tileX, tileY);
            // check if we're in the region outside of that define by the enclosing
            // TileReader. If so, return a blank tile.
            if (tileX < offsetX ||
                tileY < offsetY ||
                tileX >= (offsetX + tileReader.NumberTilesX) ||
                tileY >= (offsetY + tileReader.NumberTilesY))
            {
                //Console.Write("Returning blank tile for indices {0}, {1}.\n", tileX, tileY);
                Bitmap blank = new Bitmap((int)TileWidth, (int)TileHeight);
                using (Graphics gfx = Graphics.FromImage(blank))
                using (SolidBrush brush = new SolidBrush(color))
                {
                    gfx.FillRectangle(brush, 0, 0, (int)TileWidth, (int)TileHeight);
                }
                return blank;
            }

            // We're in the eclosing TileReader's region. Transform the tile
            // coordinates.
            uint tileXTrans = tileX - offsetX;
            uint tileYTrans = tileY - offsetY;

            Console.Write("Returning delegated tile for indices {0}, {1} (transformed indices: {2}, {3}).\n", tileX, tileY, tileXTrans, tileYTrans);

            return tileReader.read(tileXTrans, tileYTrans);
        }
    }
}
