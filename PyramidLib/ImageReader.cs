using System;
using System.IO;
using System.Drawing;

namespace Pyramid.Reader
{
    public class ImageReader
    {
        private string filename;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public ImageReader(string filename)
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

        public Bitmap read(int x, int y, int width, int height)
        {
            //Console.WriteLine("Reading image tile at x = {0}, y = {1}, width = {2}, height = {3}", x, y, width, height);

            // create a bitmap from the image strem
            Bitmap subimage = null;
            using (Bitmap wholeImage = new Bitmap(filename))
            {
                Rectangle cropRegion = new Rectangle(x, y, width, height);
                subimage = wholeImage.Clone(cropRegion, wholeImage.PixelFormat);
            }
            
            return subimage;
        }
    }
}
