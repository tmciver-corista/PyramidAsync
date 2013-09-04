using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using Pyramid.Reader;

namespace Pyramid.Generator
{
	public class SequentialRecursivePyramidGenerator : PyramidGenerator
	{
		private int levels;
        private TileReader tileReader;
        //private int tileDimension;
        //private double readDuration;
        //private double stitchDuration;
		
		public SequentialRecursivePyramidGenerator(string filename, int levels)
		{
			this.levels = levels;
            //this.tileDimension = tileDimension;
            //this.readDuration = readDuration;
            //this.stitchDuration = stitchDuration;

            int tilesPerSide = (int)Math.Pow(2, levels - 1);

            ImageReader imageReader = new ImageReader(filename);

            // for now get the smaller of the image width and height; this will mean that
            // some of the longer dimension will be excluded from the final pyramid tiles
            int length = Math.Min(imageReader.Height, imageReader.Width);

            // calculate the tile dimension
            int tileDimension = length / tilesPerSide;

            // finally, create the TileReader
            tileReader = new TileReader(imageReader, tileDimension);

            Console.WriteLine("SequentialRecursivePyramidGenerator - tiles per side: {0}, tile dimension: {1}", tilesPerSide, tileDimension);
		}

		public void generate(StreamReader imageStream)
		{
			createTiles(imageStream);
		}
		
		public void createTiles(StreamReader imageStream)
		{
			// wait for the top pyramid tile to be created
            try
            {
                createTile(levels, 0, 0);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Caught exception: " + e);
            }
			
			// then write this final tile to file
		}
		
		private Bitmap createTile(int level, int tileX, int tileY)
		{
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
            Bitmap scaledImage = null;
			using (var topLeft = createTile(level - 1, xs[0], ys[0]))
			using (var topRight = createTile(level - 1, xs[1], ys[0]))
			using (var bottomLeft = createTile(level - 1, xs[0], ys[1]))
            using (var bottomRight = createTile(level - 1, xs[1], ys[1]))
            using (var image = ImageUtils.combine(topLeft, topRight, bottomLeft, bottomRight))
            {
                // scale to half the size
                scaledImage = new Bitmap(image, image.Width / 2, image.Height / 2);
            }
			
			

            // dispose of the four sub images
            //topLeft.Dispose();
            //topRight.Dispose();
            //bottomLeft.Dispose();
            //bottomRight.Dispose();

            

            Console.WriteLine("Writing tile to file - level: {0}, x: {1}, y: {2}", level, tileX, tileY);

            // dispose of the original
            //image.Dispose();

            return scaledImage;
		}
	}
}
