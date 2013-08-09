using System;
using System.IO;

namespace Pyramid.Generator
{
	/*
	 * Interface to generate pyramid image data on the file system
	 * from the given file.
	 */
	public interface PyramidGenerator
	{
		void generate(StreamReader imageStream);
	}
}

