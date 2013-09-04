using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

using Pyramid.Reader;

namespace Pyramid.Generator
{
	public class AsynchronousPyramidGenerator : PyramidGenerator
	{
        private int levels;
        private TileReader tileReader;
		
        public AsynchronousPyramidGenerator(string filename, int levels)
		{
            this.levels = levels;

            int tilesPerSide = (int)Math.Pow(2, levels - 1);

            ImageReader imageReader = new ImageReader(filename);

            // for now get the smaller of the image width and height; this will mean that
            // some of the longer dimension will be excluded from the final pyramid tiles
            int length = Math.Min(imageReader.Height, imageReader.Width);

            // calculate the tile dimension
            int tileDimension = length / tilesPerSide;

            // finally, create the TileReader
            tileReader = new TileReader(imageReader, tileDimension);

            Console.WriteLine("AsynchronousPyramidGenerator - tiles per side: {0}, tile dimension: {1}", tilesPerSide, tileDimension);
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
		
        private Bitmap createTileAsync(int level, int tileX, int tileY)
        {
            //Console.WriteLine("Creating tile. x: {0}, y: {1}, level: {2}", tileX, tileY, level);

            // check if we're at the base of the pyramid
            if (level == 0)
            {
                Console.WriteLine("Writing tile to file - level: {0}, x: {1}, y: {2}", level, tileX, tileY);
                
                // read a tile at the given coordinates and return the image
                return tileReader.read(tileX, tileY);
            }
			
            // create tasks for the four child tiles in the level below this one
            //Task<Bitmap>[] children = new Task<Bitmap>[4];
            int[] xs = {2 * tileX, 2 * tileX + 1};
            int[] ys = {2 * tileY, 2 * tileY + 1};
            var topLeft = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[0], ys[0]); });
            var topRight = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[1], ys[0]); });
            var bottomLeft = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[0], ys[1]); });
            var bottomRight = Task<Bitmap>.Factory.StartNew(() => { return createTileAsync(level - 1, xs[1], ys[1]); });

            // wait for the tasks to finish
            Task.WaitAll(topLeft, topRight, bottomLeft, bottomRight);
			
            // stitch the four subtiles into one
            Bitmap image = ImageUtils.combine(topLeft.Result, topRight.Result, bottomLeft.Result, bottomRight.Result);

            // dispose of the four sub images
            topLeft.Dispose();
            topRight.Dispose();
            bottomLeft.Dispose();
            bottomRight.Dispose();

            // scale to half the size
            Bitmap scaledImage = new Bitmap(image, image.Width / 2, image.Height / 2);

            Console.WriteLine("Writing tile to file - level: {0}, x: {1}, y: {2}", level, tileX, tileY);

            // dispose of the original
            image.Dispose();

            return scaledImage;
        }
	}
}
