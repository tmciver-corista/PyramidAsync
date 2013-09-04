using System;
using System.Threading;
using System.Diagnostics;
using Pyramid.Generator;

namespace Pyramid
{
	class MainClass
	{
        const double READ_DURATION = 0.01;
        const double STITCH_DURATION = 0.02;
        const int TILE_DIMENSION = 256;
        const int LEVELS = 7;
		
        public static void Main(string[] args)
		{
            string filename = @"C:\Users\Public\Pictures\Sample Pictures\Hydrangeas.jpg";

			// create a PyramidGenerator
			PyramidGenerator generator = new AsynchronousPyramidGenerator(filename, LEVELS);
            //PyramidGenerator generator = new SequentialRecursivePyramidGenerator(filename, LEVELS);

            // start the timer
            Stopwatch sw = Stopwatch.StartNew();
			
            // generate the pyramid
            generator.generate(null);

            // stop the timer
            sw.Stop();

            //Thread.Sleep(1000);

            Console.WriteLine("Time taken: {0} ms", sw.Elapsed.TotalMilliseconds);
		}
	}
}
