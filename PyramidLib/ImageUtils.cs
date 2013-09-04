using System;
using System.Drawing;

namespace Pyramid
{
    static class ImageUtils
    {
        public static Bitmap combine(Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight)
        {
            // calc the mosaic's dimensions
            int leftColumnWidth = System.Math.Max(topLeft.Width, bottomLeft.Width);
            int rightColumnWidth = System.Math.Max(topRight.Width, bottomRight.Width);
            int topRowHeight = System.Math.Max(topLeft.Height, topRight.Height);
            int bottomRowHeight = System.Math.Max(bottomLeft.Height, bottomRight.Height);
            int mosaicWidth = leftColumnWidth + rightColumnWidth;
            int mosaicHeight = topRowHeight + bottomRowHeight;

            // create the mosaic bitmap and write the four sub images into it
            Bitmap mosaic = new Bitmap(mosaicWidth, mosaicHeight);
            using (Graphics g = Graphics.FromImage(mosaic))
            {
                g.DrawImage(topLeft, 0, 0);
                g.DrawImage(topRight, leftColumnWidth, 0);
                g.DrawImage(bottomLeft, 0, topRowHeight);
                g.DrawImage(bottomRight, leftColumnWidth, topRowHeight);
            }

            return mosaic;
        }
    }
}
