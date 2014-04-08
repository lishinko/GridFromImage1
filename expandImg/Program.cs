using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace expandImg
{
    class Program
    {
        static void Main(string[] args)
        {
            GridFromImage.Grid g = new GridFromImage.Grid();
            g.loadDiffs(args[0]);
            g.expandDiffs();
            g.saveDiffInfo(args[0]);
        }
    }
}
