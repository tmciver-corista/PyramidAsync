using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

namespace Pyramid.Generator
{
	public class AsynchronousPyramidGenerator : PyramidGenerator
	{
		public AsynchronousPyramidGenerator()
		{
		}
		
		public async Task createTilesAsync(StreamReader imageStream)
		{
			// calculate the number of pyramid levels
			int levels = 10;
			
			// wait for the top pyramid tile to be created
			await createTileAsync(levels, 0, 0);
			
			// then write this final tile to file
		}
		
		private async Task<Bitmap> createTileAsync(int level, int tileX, int tileY)
		{
			// check if we're at the base of the pyramid
			if (level == 0)
			{
				// read a tile at the given coordinates and return the image
			}
			
			// create tasks for the four child tiles in the level below this one
			//Task<Bitmap>[] children = new Task<Bitmap>[4];
			int[] xs = {2 * tileX, 2 * tileX + 1};
			int[] ys = {2 * tileY, 2 * tileY + 1};
			var topLeft = createTileAsync(level - 1, xs[0], ys[0]);
			var topRight = createTileAsync(level - 1, xs[1], ys[0]);
			var bottomLeft = createTileAsync(level - 1, xs[0], ys[1]);
			var bottomRight = createTileAsync(level - 1, xs[1], ys[1]);
			var children = new[] {topLeft, topRight, bottomLeft, bottomRight};
			
			// wait for all the children to finish
			await Task.WhenAll(children);
			
			// stitch the four subtiles into one
			
			// then, write it to file
			
			// finally, return it
		}
	}
}
