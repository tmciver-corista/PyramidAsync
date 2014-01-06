using System;
using System.IO;
using System.Drawing;

namespace Pyramid.Reader
{
    /**
     * The DefaultImageReader can be used to read any image format
     * supported byt the Bitmap class.
     */
    public class DefaultImageReader : ImageReader
    {
        private string filename;

        public DefaultImageReader(string filename)
        {
            this.filename = filename;

            // init Width and Height
            using (Bitmap bm = new Bitmap(filename))
            {
                Width = bm.Width;
                Height = bm.Height;
                //Console.WriteLine("Width: {0}, Height: {1}", Width, Height);
            }
        }

        public override Bitmap read(uint x, uint y, uint width, uint height)
        {
            // input validation
            if (x + width > Width || y + height > Height)
            {
                throw new ArgumentException(string.Format("Some or all of image read is outside bounds of image (x: {0}, y: {1}, read width: {2}, read height: {3}, image width: {4}, image height: {5}).", x, y, width, height, Width, Height));
            }

            //Console.WriteLine("Reading image tile at x = {0}, y = {1}, width = {2}, height = {3}", x, y, width, height);

            // create a bitmap from the image strem
            Bitmap subimage = null;
            using (Bitmap wholeImage = new Bitmap(filename))
            {
                Rectangle cropRegion = new Rectangle((int)x, (int)y, (int)width, (int)height);
                subimage = wholeImage.Clone(cropRegion, wholeImage.PixelFormat);
            }

            return subimage;
        }
    }
}
