using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace GridFromImage
{
    class ImageComparer
    {
        public ImageComparer(string baseImg, string otherImg)
        {
            this.baseImg = new Bitmap(baseImg);
            this.otherImg = new Bitmap(otherImg);
        }
        public ImageComparer(Bitmap baseImg, Bitmap otherImg)
        {
            this.baseImg = baseImg;
            this.otherImg = otherImg;
        }
        public Bitmap DiffImg { get{
            Bitmap ret = new Bitmap(baseImg.Width, baseImg.Height);
            for (int y = 0; y < baseImg.Height; y++)
            {
                for (int x = 0; x < baseImg.Width; x++)
                {
                    Color baseColor = baseImg.GetPixel(x, y);
                    Color otherColor = otherImg.GetPixel(x, y);
                    if (baseColor != otherColor)
                    {
                        ret.SetPixel(x, y, diffColor);

                    }
                    else
                    {
                        ret.SetPixel(x, y, sameColor);
                    }
                }
            }
            //Graphics g = Graphics.FromImage(ret);
            //Pen pen = new Pen(Color.Green);
            return ret;
        }}
        protected Bitmap baseImg;
        protected Bitmap otherImg;
        static Color sameColor = Color.White;
        static Color diffColor = Color.Red;
        //static Color lineColor
    }
}
