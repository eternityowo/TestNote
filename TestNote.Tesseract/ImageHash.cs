using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TestNote.Tesseract
{
    public static class ImageHash
    {
        static int size = 9;
        static int sizeI = size - 1;

        public static string ProcessImage(string fileName)
        {
            Bitmap original_bm = new Bitmap(fileName);
            Bitmap shrunk_bm = ScaleTo(original_bm, size, size, InterpolationMode.High);
            Bitmap grayscale_bm = ToMonochrome(shrunk_bm);
            string hash_code = GetHashCode(grayscale_bm);

            return hash_code;
        }

        // Scale an image.
        public static Bitmap ScaleTo(Bitmap bm, int wid, int hgt, InterpolationMode interpolation_mode)
        {
            Bitmap new_bm = new Bitmap(wid, hgt);
            using (Graphics gr = Graphics.FromImage(new_bm))
            {
                RectangleF source_rect = new RectangleF(-0.5f, -0.5f, bm.Width, bm.Height);
                Rectangle dest_rect = new Rectangle(0, 0, wid, hgt);
                gr.InterpolationMode = interpolation_mode;
                gr.DrawImage(bm, dest_rect, source_rect, GraphicsUnit.Pixel);
            }
            return new_bm;
        }

        // Convert an image to monochrome.
        public static Bitmap ToMonochrome(Image image)
        {
            // Make the ColorMatrix.
            ColorMatrix cm = new ColorMatrix(new float[][]
            {
                new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                new float[] { 0, 0, 0, 1, 0},
                new float[] { 0, 0, 0, 0, 1}
            });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            // Draw the image onto the new bitmap while applying the new ColorMatrix.
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }

        // Return the hashcode for this sizeXsize image.
        public static string GetHashCode(Bitmap bm)
        {
            string row_hash = "";
            for (int r = 0; r < sizeI; r++)
                for (int c = 0; c < sizeI; c++)
                    if (bm.GetPixel(c + 1, r).R >= bm.GetPixel(c, r).R)
                    {
                        row_hash += "1";
                    }
                    else
                    {
                        row_hash += "0";
                    }

            string col_hash = "";
            for (int c = 0; c < sizeI; c++)
                for (int r = 0; r < sizeI; r++)
                    if (bm.GetPixel(c, r + 1).R >= bm.GetPixel(c, r).R)
                    {
                        col_hash += "1";
                    }
                    else
                    {
                        col_hash += "0";
                    }

            return row_hash + col_hash;
        }
    }
}
