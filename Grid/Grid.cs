using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace GridFromImage
{
    public enum DataNodeType
    {
        Normal = ' ',
        Block = '*',
        Left = '4',
        Right = '6',
        Up = '8',
        Down = '2',
        LeftDown = '1',
        LeftUp = '7',
        RightDown = '3',
        RightUp = '9',
    }
    public class Grid //: IEnumerable<Point>
    {
        //不知道为什么，上面的enum写的类型不能被打印出来。我们还是弄个map吧.
        Dictionary<DataNodeType, char> nodeTypeStrings = new Dictionary<DataNodeType, char>(){
            {DataNodeType.Normal, ' '},
            {DataNodeType.Block, '*'},
            {DataNodeType.Left, '4'},
            {DataNodeType.Right, '6'},
            {DataNodeType.Up, '8'},
            {DataNodeType.Down, '2'},
            {DataNodeType.LeftDown, '1'},
            {DataNodeType.LeftUp, '7'},
            {DataNodeType.RightDown, '3'},
            {DataNodeType.RightUp, '9'},
        };
        public int Width { get; set; }
        public int Height { get; set; }
        protected Rectangle[,] grids;
        protected DataNodeType[,] diffs;

        public void setOriginalImageSize(int imgWidth, int imgHeight)
        {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
            grids = getGrids();
            initDiffs(grids.GetLength(0), grids.GetLength(1), DataNodeType.Block);
        }
        public void markAsDifferentXY(int gridx, int gridy)
        {
            diffs[gridy, gridx] = DataNodeType.Block;
        }
        public void markAsDifferentIJ(int gridi, int gridj)
        {
            diffs[gridi, gridj] = DataNodeType.Block;
        }
        protected void initDiffs(int height, int width, DataNodeType defaultType)
        {
            diffs = new DataNodeType[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    diffs[y, x] = DataNodeType.Normal;
                }
            }
        }
        public void saveDiffInfo(string path)
        {
            using (StreamWriter fs = new StreamWriter(path))
            {
                fs.WriteLine("{0} {1}", diffs.GetLength(1), diffs.GetLength(0));
                for (int i = 0; i < diffs.GetLength(0); i++)
                {
                    for (int j = 0; j < diffs.GetLength(1); j++)
                    {
                        char curDataType = nodeTypeStrings[diffs[i, j]];
                        fs.Write(curDataType);
                    }
                    fs.Write(fs.NewLine);
                }
            }

        }
        public void loadDiffs(string path)
        {
            using (StreamReader fs = new StreamReader(path))
            {
                string widthHeight = fs.ReadLine();
                string[] ss = widthHeight.Split(' ');
                int width = Int32.Parse(ss[0]);
                int height = Int32.Parse(ss[1]);
                initDiffs(height, width, DataNodeType.Block);

                for (int y = 0; y < height; y++)
                {
                    string s = fs.ReadLine();
                    for (int x = 0; x < width; x++)
                    {
                        diffs[y, x] = (DataNodeType)s[x];
                    }
                }
            }
        }
        public void expandDiffs()
        {
            if (diffs == null)
            {
                Console.WriteLine("no diff info!");
                return;
            }
            for (int y = 0; y < diffs.GetLength(0); y++)
            {
                for (int x = 0; x < diffs.GetLength(1); x++)
                {
                    if (diffs[y, x] != DataNodeType.Block && diffs[y, x] != DataNodeType.Normal)//如果该点包含不是block的其它block，那么就扩展下，覆盖和它相连的其他block
                    {
                        expand(x, y, diffs[y, x]);
                    }
                }
            }
        }
        protected void expand(int x, int y, DataNodeType newData)
        {
            System.Diagnostics.Debug.Assert(diffs[y, x] == newData);
            expandToNeighbours(x, y, newData);
        }
        protected void expandTo(int x, int y, DataNodeType oldData, DataNodeType newData)
        {
            if (x >= 0 && y >= 0 && y < diffs.GetLength(0) && x < diffs.GetLength(1) && diffs[y, x] == oldData)
            {
                diffs[y, x] = newData;
                expandToNeighbours(x, y, newData);
            }
        }
        protected void expandToNeighbours(int x, int y, DataNodeType newData)
        {
            expandTo(x - 1, y - 1, DataNodeType.Block, newData);//左上
            expandTo(x - 1, y, DataNodeType.Block, newData);//左
            expandTo(x - 1, y + 1, DataNodeType.Block, newData);//左下
            expandTo(x, y - 1, DataNodeType.Block, newData);//上
            expandTo(x, y + 1, DataNodeType.Block, newData);//下
            expandTo(x + 1, y - 1, DataNodeType.Block, newData);//右上
            expandTo(x + 1, y, DataNodeType.Block, newData);//右
            expandTo(x + 1, y + 1, DataNodeType.Block, newData);//右下
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
            Point[,] points = new Point[Width - 1, 2];
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
