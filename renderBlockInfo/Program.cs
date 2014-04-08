using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace renderBlockInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string imgFile = args[0];
            string gridInfoFile = args[1];

            Bitmap baseImg = new Bitmap(imgFile);

            GridFromImage.Grid grid = new GridFromImage.Grid();
            grid.loadDiffs(gridInfoFile);
            grid.setOriginalImageSize(baseImg.Width, baseImg.Height);

            Graphics g = Graphics.FromImage(baseImg);
            Pen pen = new Pen(Color.Green);
            grid.drawGrids(g, pen);

            grid.drawBlockInfo(g, new Font("Verdana", 10));

            string fullPath = Path.GetFullPath(imgFile);
            string path = Path.GetDirectoryName(fullPath);
            string baseFileName = Path.GetFileNameWithoutExtension(fullPath);
            string ext = Path.GetExtension(imgFile);
            baseImg.Save(path + baseFileName + "_save" + ext);
        }
    }
}
