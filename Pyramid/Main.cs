using System;
using System.Diagnostics;
using Pyramid.Generator;

namespace Pyramid
{
	class MainClass
	{
        const double READ_DURATION = 0.1;
        const double STITCH_DURATION = 0.2;
        const int TILE_DIMENSION = 256;
        const int LEVELS = 3;
		
        public static void Main(string[] args)
		{
			// create a PyramidGenerator
			PyramidGenerator generator = new AsynchronousPyramidGenerator(LEVELS, TILE_DIMENSION, READ_DURATION, STITCH_DURATION);

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
