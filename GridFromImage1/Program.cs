using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace GridFromImage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("usage: GridFromImage baseFile otherFile gridx gridy");
                return;
            }
            string baseFile = args[0];
            string otherFile = args[1];
            int gridWidth = Int32.Parse(args[2]);
            int gridHeight = Int32.Parse(args[3]);

            Bitmap baseImg = new Bitmap(baseFile);
            
            Bitmap otherImg = new Bitmap(otherFile);

            ImageComparer im = new ImageComparer(baseImg, otherImg);
            Bitmap diff = im.DiffImg;

            Graphics g = Graphics.FromImage(diff);
            Pen pen = new Pen(Color.Green);

            Grid grid = new Grid();
            grid.Width = gridWidth;
            grid.Height = gridHeight;
            grid.setOriginalImageSize(baseImg.Width, baseImg.Height);
            //画线
            Point[,] points = grid.HorizontalLines();
            for(int i = 0; i < points.GetLength(0); i++)
            {
                g.DrawLine(pen, points[i,0],points[i,1]);
            }
            points = grid.VerticalLines();
            for (int i = 0; i < points.GetLength(0); i++)
            {
                g.DrawLine(pen, points[i, 0], points[i, 1]);
            }
            //画个半透明矩形从而清楚的看出来哪些是不能通过的
            Rectangle[,] rects = grid.Grids;
            for (int i = 0; i < rects.GetLength(0); i++)
            {
                for (int j = 0; j < rects.GetLength(1); j++)
                {
                    Rectangle rect = rects[i, j];
                    bool marked = markAsDifferent(diff, rect, grid, i, j);
                    if (marked)
                    {
                        Brush trasparentBrush = new SolidBrush(Color.FromArgb(0x80, Color.Red));
                        g.FillRectangle(trasparentBrush, rect);
                    }
                }
            }

            string fullPath = Path.GetFullPath(baseFile);
            string path = Path.GetDirectoryName(fullPath);
            string baseFileName = Path.GetFileNameWithoutExtension(fullPath);
            string ext = Path.GetExtension(baseFile);
            diff.Save(path + baseFileName + "_save" + ext);
            grid.saveDiffInfo(path + baseFileName + "_diff.txt");
        }
        protected static bool markAsDifferent(Bitmap diff, Rectangle rect, Grid grid, int i, int j) {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    Color color = diff.GetPixel(x, y);
                    if (color.ToArgb() == Color.Red.ToArgb())
                    {
                        grid.markAsDifferentIJ(i, j);//默认的i,j和x,y是反着的，
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
