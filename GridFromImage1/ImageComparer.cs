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
        public byte MaxColorDiff {get{return maxDiff;} set{maxDiff = value;}}
        public Bitmap DiffImg { get{
            Bitmap ret = new Bitmap(baseImg.Width, baseImg.Height);
            for (int y = 0; y < baseImg.Height; y++)
            {
                for (int x = 0; x < baseImg.Width; x++)
                {
                    Color baseColor = baseImg.GetPixel(x, y);
                    Color otherColor = otherImg.GetPixel(x, y);
                    if (!isSimilarWith(baseColor, otherColor))
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
        protected byte maxDiff = 10;//2张图片的颜色可能不是精确相等的(比如放缩,或者压缩算法的不同),我们设置允许的默认的颜色差异最大为10个点。
        //static Color lineColor
        protected bool isSimilarWith(Color baseColor, Color otherColor)
        {
            if (!lessThan(baseColor.A, otherColor.A))
                return false;
            if (!lessThan(baseColor.R, otherColor.R))
                return false;
            if (!lessThan(baseColor.G, otherColor.G))
                return false;
            if (!lessThan(baseColor.B, otherColor.B))
                return false;
            return true;
        }
        protected bool lessThan(byte lhs, byte rhs)
        {
            return Math.Abs(lhs - rhs) <= MaxColorDiff;
        }
    }
}
