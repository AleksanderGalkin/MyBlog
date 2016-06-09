using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace PluginImagePostType.Tests
{
    public class ImageFactory
    {
        static public  byte[] GetImageData()
        {
            //Create the empty image.
            Bitmap bmImage = new Bitmap(50, 50);
            
            //draw a useless line for some data
            Graphics imageData = Graphics.FromImage(bmImage);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 50, 50);

            //Convert to byte array
            MemoryStream memoryStream = new MemoryStream();
            byte[] bmData;

            using (memoryStream)
            {
                bmImage.Save(memoryStream, ImageFormat.Bmp);
                bmData = memoryStream.ToArray();
            }
            return bmData;
        }

        static public Image GetImage(int Param)
        {

            int ratio = 1 / Param ;
            //Create the empty image.
            Bitmap bmImage = new Bitmap(50, 50);

            //draw a useless line for some data
            Graphics imageData = Graphics.FromImage(bmImage);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 50 * ratio, 50 * ratio);

            
            return bmImage;
        }
    }
}
