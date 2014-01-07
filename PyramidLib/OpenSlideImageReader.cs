using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pyramid.Reader
{
    public class OpenSlideImageReader : ImageReader
    {
        private const string OpenSlideDllPath = @"C:\Users\Tim McIver\Documents\workspace\ExportService\OpenSlideReader\src\Resources\libopenslide-0.dll";

        private string filename;
        private IntPtr os;

        public OpenSlideImageReader(string filename)
        {
            this.filename = filename;

            // initialize OpenSlide
            os = openslide_open(filename);

            // get image dimensions
            long imageWidth = 0;
            long imageHeight = 0;
            int level = 0;
            openslide_get_level_dimensions(os, level, ref imageWidth, ref imageHeight);

            // initialize the Width and Height member variables with this data
            Width = (int)imageWidth;
            Height = (int)imageHeight;
        }

        public override Bitmap read(uint x, uint y, uint width, uint height)
        {
            // validate args
            if ((x + width >= Width) || (y + height >= Height))
            {
                throw new ArgumentException(string.Format("Some or all of image read is outside bounds of image (x: {0}, y: {1}, read width: {2}, read height: {3}, image width: {4}, image height: {5}).", x, y, width, height, Width, Height));
            }

            // create a buffer for the raw data
            uint imageBufferSize = 4 * width * height;
            byte[] data = new byte[imageBufferSize];

            // read the raw data
            int level = 0;
            openslide_read_region(os, data, x, y, level, width, height);

            // create a bitmap
            Bitmap image = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, image.PixelFormat);
            Marshal.Copy(data, 0, imageData.Scan0, data.Length);
            image.UnlockBits(imageData);

            return image;
        }

        public void Dispose()
        {
            // close OpenSlide
            openslide_close(os);
        }

        [DllImport(OpenSlideDllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr openslide_open(string filename);

        [DllImport(OpenSlideDllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void openslide_close(IntPtr os);

        [DllImport(OpenSlideDllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void openslide_get_level_dimensions(IntPtr openSlidePtr, int level, ref long imageWidth, ref long imageHeight);

        [DllImport(OpenSlideDllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void openslide_read_region(IntPtr openSlide, byte[] data, long x, long y, int level, long width, long height);
    }
}
