using System;
using System.Threading;
using System.Diagnostics;
using Pyramid.Generator;

namespace Pyramid
{
	class MainClass
	{	
        public static void Main(string[] args)
		{
            uint levels = uint.Parse(args[0]);
            string filename = args[1];

			// create a PyramidGenerator
			PyramidGenerator generator = new AsynchronousPyramidGenerator(filename, levels);
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
