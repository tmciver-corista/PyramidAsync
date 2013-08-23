using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace Pyramid.Generator
{
	public class SequentialRecursivePyramidGenerator : PyramidGenerator
	{
		private int levels;
		private int tileDimension;
		private double readDuration;
		private double stitchDuration;
		
		public SequentialRecursivePyramidGenerator(int levels, int tileDimension, double readDuration, double stitchDuration)
		{
			this.levels = levels;
			this.tileDimension = tileDimension;
			this.readDuration = readDuration;
			this.stitchDuration = stitchDuration;
		}

		public void generate(StreamReader imageStream)
		{
			createTiles(imageStream);
		}
		
		public void createTiles(StreamReader imageStream)
		{
			// wait for the top pyramid tile to be created
			createTile(levels, 0, 0);
			
			// then write this final tile to file
		}
		
		private Bitmap createTile(int level, int tileX, int tileY)
		{
			// check if we're at the base of the pyramid
			if (level == 0)
			{
				// read a tile at the given coordinates and return the image
				return readImage(tileX * tileDimension, tileY * tileDimension, tileDimension, tileDimension);
			}
			
			// create tasks for the four child tiles in the level below this one
			//Task<Bitmap>[] children = new Task<Bitmap>[4];
			int[] xs = {2 * tileX, 2 * tileX + 1};
			int[] ys = {2 * tileY, 2 * tileY + 1};
			var topLeft = createTile(level - 1, xs[0], ys[0]);
			var topRight = createTile(level - 1, xs[1], ys[0]);
			var bottomLeft = createTile(level - 1, xs[0], ys[1]);
			var bottomRight = createTile(level - 1, xs[1], ys[1]);
			
			// stitch the four subtiles into one and return it
			return stitchImages(topLeft, topRight, bottomLeft, bottomRight);
		}

		private Bitmap stitchImages(Image topLeft, Image topRight, Image bottomLeft, Image bottomRight)
		{

			Console.WriteLine("Stitching images . . .");

			// wait stitchDuration seconds
			Stopwatch sw = new Stopwatch();
			sw.Start();
			while (sw.ElapsedMilliseconds < stitchDuration * 1000)
			{
				// do nothing
			}

            return null;// new Bitmap(tileDimension, tileDimension); ;
		}

		private Bitmap readImage(int x, int y, int width, int height)
		{
			Console.WriteLine("Reading image . . .");

			// wait readDuration seconds
			Stopwatch sw = new Stopwatch();
			sw.Start();
			//Console.WriteLine("Millis: " + sw.ElapsedMilliseconds);
			while (sw.ElapsedMilliseconds < readDuration * 1000)
			{
				// do nothing
			}

            return null;// new Bitmap(tileDimension, tileDimension);
		}
	}
}
