using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using Pyramid.Reader;

namespace Pyramid.Generator
{
	public class SequentialRecursivePyramidGenerator : PyramidGenerator
	{
		private uint levels;
        private TileReader tileReader;
        private string outputDir;
		
		public SequentialRecursivePyramidGenerator(string filename, uint levels)
		{
			this.levels = levels;

            // create the output directory
            this.outputDir = Directory.GetParent(filename).ToString() + @"\" + Path.GetFileNameWithoutExtension(filename) + "_sequential";
            Directory.CreateDirectory(outputDir);

            uint tilesPerSide = (uint)Math.Pow(2, levels - 1);

            ImageReader imageReader = new DefaultImageReader(filename);

            // for now get the smaller of the image width and height; this will mean that
            // some of the longer dimension will be excluded from the final pyramid tiles
            uint length = (uint)Math.Min(imageReader.Height, imageReader.Width);

            // calculate the tile dimension
            uint tileDimension = length / tilesPerSide;

            // finally, create the TileReader
            tileReader = new DefaultTileReader(imageReader, tileDimension);

            Console.WriteLine("SequentialRecursivePyramidGenerator - tiles per side: {0}, tile dimension: {1}", tilesPerSide, tileDimension);
		}

		public void generate(StreamReader imageStream)
		{
			createTiles(imageStream);
		}
		
		private void createTiles(StreamReader imageStream)
		{
			// wait for the top pyramid tile to be created
            createTile(levels - 1, 0, 0);
		}
		
		private Bitmap createTile(uint level, uint tileX, uint tileY)
		{
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
			uint[] xs = {2 * tileX, 2 * tileX + 1};
			uint[] ys = {2 * tileY, 2 * tileY + 1};
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

            //Console.WriteLine("Writing tile to file - level: {0}, tileX: {1}, tileY: {2}", level, tileX, tileY);

            // write tile to file
            scaledImage.Save(File.Create(outputDir + @"\" + string.Format("{0}_{1}_{2}.jpg", level, tileX, tileY)), ImageFormat.Jpeg);

            return scaledImage;
		}
	}
}
