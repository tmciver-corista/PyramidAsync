using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

using Pyramid.Reader;

namespace Pyramid.Generator
{
	public class AsynchronousPyramidGenerator : PyramidGenerator
	{
        private uint levels;
        private TileReader tileReader;
        private string outputDir;

        /**
         * Note that the TileReader passed in should support the pyramid algorithm, i.e., that
         * the foloowing two things should be true: 1) the number of tiles in botht the X and Y
         * dimmensions should be equal and 2) that number should be a power of two.
         */
        public AsynchronousPyramidGenerator(TileReader tileReader, string outputDir)
        {
            this.tileReader = tileReader;
            this.outputDir = outputDir;

            // we really should be checking that the given TileReader supports the pyramid algorithm,
            // i.e., that the number of both X and Y tiles are equal and that that number is a power
            // of two.  For now, we'll just let it encounter an exception.

            // calculate the number of levels given the number of tiles along one dimension
            // supported by the given TileReader
            levels = (uint)Math.Log(tileReader.NumberTilesX, 2.0) + 1;

            //Console.WriteLine("Levels: " + levels);
        }

        public void generate(StreamReader imageStream)
        {
            createTilesAsync(imageStream);
        }
		
        public void createTilesAsync(StreamReader imageStream)
        {
            // wait for the top pyramid tile to be created
            createTileAsync(levels - 1, 0, 0);
        }
		
        private Bitmap createTileAsync(uint level, uint tileX, uint tileY)
        {
            //Console.WriteLine("Creating tile. x: {0}, y: {1}, level: {2}", tileX, tileY, level);

            // check if we're at the base of the pyramid
            if (level == 0)
            {
                //Console.WriteLine("Writing tile to file - level: {0}, tileX: {1}, tileY: {2}", level, tileX, tileY);

                Bitmap tile = tileReader.read(tileX, tileY);

                // write tile to file
                tile.Save(File.Create(outputDir + @"\" + string.Format("{0}_{1}_{2}.jpg", level, tileX, tileY)), ImageFormat.Jpeg);
                
                // read a tile at the given coordinates and return the image
                return tile;
            }
			
            // create tasks for the four child tiles in the level below this one
            Bitmap scaledImage = null;
            uint[] xs = {2 * tileX, 2 * tileX + 1};
            uint[] ys = {2 * tileY, 2 * tileY + 1};
            using (var topLeft = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[0], ys[0]); }))
            using (var topRight = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[1], ys[0]); }))
            using (var bottomLeft = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[0], ys[1]); }))
            using (var bottomRight = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[1], ys[1]); }))
            {
                // wait for the tasks to finish
                Task.WaitAll(topLeft, topRight, bottomLeft, bottomRight);

                // stitch the four subtiles into one
                using (Bitmap image = ImageUtils.combine(topLeft.Result, topRight.Result, bottomLeft.Result, bottomRight.Result))
                {
                    // scale to half the size
                    scaledImage = new Bitmap(image, image.Width / 2, image.Height / 2);
                }
            }

            //Console.WriteLine("Writing tile to file - level: {0}, tileX: {1}, tileY: {2}", level, tileX, tileY);

            // write tile to file
            scaledImage.Save(File.Create(outputDir + @"\" + string.Format("{0}_{1}_{2}.jpg", level, tileX, tileY)), ImageFormat.Jpeg);

            return scaledImage;
        }
	}
}
