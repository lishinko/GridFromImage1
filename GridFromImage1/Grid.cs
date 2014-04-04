using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace GridFromImage
{
    class Grid //: IEnumerable<Point>
    {
        public int Width { get; set; }
        public int Height { get; set; }
        protected Rectangle[,] grids;
        protected bool[,] diffs;

        public void setOriginalImageSize(int imgWidth, int imgHeight) {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
            grids = getGrids();
            diffs = new bool[grids.GetLength(0), grids.GetLength(1)];
        }
        public void markAsDifferentXY(int gridx, int gridy) {
            diffs[gridy, gridx] = true;
        }
        public void markAsDifferentIJ(int gridi, int gridj)
        {
            diffs[gridi, gridj] = true;
        }
        public void saveDiffInfo(string path)
        { 
            using(StreamWriter fs = new StreamWriter(path))
            {
                fs.WriteLine("{0} {1}", diffs.GetLength(0), diffs.GetLength(1));
                for (int i = 0; i < diffs.GetLength(0); i++)
                {
                    for (int j = 0; j < diffs.GetLength(1); j++)
                    {
                        if (diffs[i, j])
                        {
                            fs.Write("*");
                        }
                        else
                        {
                            fs.Write(" ");
                        }
                    }
                    fs.Write(fs.NewLine);
                }
            }
            
        }
        public void loadDiffs(string path)
        {
            using (StreamReader fs = new StreamReader(path))
            {
                fs.WriteLine("{0} {1}", diffs.GetLength(0), diffs.GetLength(1));
                string widthHeight = fs.ReadLine();
                string ss = widthHeight.Split(" ");
                int width = Int32.Parse(ss[0]);
                int height = Int32.Parse(ss[1]);
                diffs = new bool[width, height];

                for (int i = 0; i < height; i++)
                {
                    string s = fs.ReadLine();
                    for (int j = 0; j < width; j++)
                    {
                        if (s[i] == '*')
                        {
                            diffs[i, j] = true;
                        }
                        else
                        {
                            diffs[i, j] = false;
                        }
                    }
                }
            }
        }
        //public Point[,] Lines {get{
        //    int[,] ret = new int[Width-1, Height-1];//边缘不需要画线，所以，线的数量要少一圈
        //    double curGridRight = 0.0;
        //    double curGridBottom = 0.0;
        //    for (int i = 0; i < ret.GetLength(0); i++)
        //    {
        //        curGridBottom += rectHeight;
        //        for (int j = 0; j < ret.GetLength(1); j++)
        //        {
        //            curGridRight += rectWidth;
        //            ret[i, j] = new Point(curGridRight, curGridBottom);
        //        }
        //    }
        //    return ret;
        //}}
        public Rectangle[,] Grids
        {
            get
            {
                return grids;
            }

        }
        protected Rectangle[,] getGrids()
        {
            Rectangle[,] rects = new Rectangle[Height, Width];
            double rectWidth = (double)this.imgWidth / Width;
            double rectHeight = (double)this.imgHeight / Height;

            double curGridLeft = 0.0;
            double curGridTop = 0.0;
            for (int i = 0; i < rects.GetLength(0); i++)
            {
                for (int j = 0; j < rects.GetLength(1); j++)
                {
                    rects[i, j] = new Rectangle((int)curGridLeft, (int)curGridTop, (int)(rectWidth), (int)(rectHeight));
                    curGridLeft += rectWidth;
                }
                curGridTop += rectHeight;
                curGridLeft = 0;
            }
            return rects;
        }
        public Point[,] HorizontalLines()
        {
            Point[,] points = new Point[Width-1,2];
            double curGridRight = 0.0;
            double rectWidth = (double)this.imgWidth / Width;
            for (int i = 0; i < points.GetLength(0); i++)
            {
                curGridRight += rectWidth;
                points[i, 0] = new Point((int)curGridRight, 0);
                points[i, 1] = new Point((int)curGridRight, imgHeight);
            }
            return points;

        }
        public Point[,] VerticalLines()
        {
            Point[,] points = new Point[Height - 1, 2];
            double curGridBottom = 0.0;
            double rectHeight = (double)this.imgHeight / Height;
            for (int i = 0; i < points.GetLength(0); i++)
            {
                curGridBottom += rectHeight;
                points[i, 0] = new Point(0, (int)curGridBottom);
                points[i, 1] = new Point(imgWidth, (int)curGridBottom);
            }
            return points;

        }

        //public IEnumerator<Point> GetEnumerator(){}
        protected int imgWidth;
        protected int imgHeight;

        //protected class GridLinesEnumerator : IEnumerator<Point>
        //{
        //    protected int curWidth = 0;
        //    protected int curHeight = 0;
        //    protected int[,] grid;
        //    GridLinesEnumerator(int[,] grid) { this.grid = grid; }
        //    #region 枚举接口实现
        //    public object Current { get {
        //        if (curWidth < grid.GetLength(1) - 1)
        //        {
        //            curWidth++;
        //        }
        //        else
        //        {
        //            curHeight++;
        //            curWidth = 0;
        //        }
        //        return grid[curHeight, curWidth]; 
        //    } }
        //    #endregion
        //}

    }
}
