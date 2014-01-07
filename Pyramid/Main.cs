using System;
using System.IO;
using System.Diagnostics;
using Pyramid.Generator;
using Pyramid.Reader;

namespace Pyramid
{
	class MainClass
	{	
        public static void Main(string[] args)
		{
            uint tileDimension = uint.Parse(args[0]);
            string filename = args[1];

            // create the output directory
            string outputDir = Directory.GetParent(filename).ToString() + @"\" + Path.GetFileNameWithoutExtension(filename) + "_parallel";
            Directory.CreateDirectory(outputDir);

            // create an ImageReader
            //ImageReader imageReader = new DefaultImageReader(filename);
            ImageReader imageReader = new OpenSlideImageReader(filename);

            // create a TileReader
            TileReader tileReader = new DefaultTileReader(imageReader, tileDimension);

            // then wrap it in a CenteredTileReader decorator
            tileReader = new CenteredTileReader(tileReader);

            Console.WriteLine("Tile dimension: {2}, X Tiles: {0}, Y Tiles: {1}.", tileReader.NumberTilesX, tileReader.NumberTilesY, tileReader.TileWidth);

			// create a PyramidGenerator
			PyramidGenerator generator = new AsynchronousPyramidGenerator(tileReader, outputDir);
            //PyramidGenerator generator = new SequentialRecursivePyramidGenerator(filename, levels);

            // start the timer
            Stopwatch sw = Stopwatch.StartNew();
			
            // generate the pyramid
            generator.generate(null);

            // stop the timer
            sw.Stop();

            Console.WriteLine("Time taken: {0} ms", sw.Elapsed.TotalMilliseconds);
		}
	}
}
