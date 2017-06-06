using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosil3_Project3_SuperNgon
{
    class MyPolygon
    {
        private Graphics graphices;
        private Pen pen;
        private int originX, originY;
        private double radius, startDegree, intervalDegree;
        
        MyPolygon(Graphics graphices, Pen pen, int originX, int originY, double radius, double startDegree, double intervalDegree)
        {
            this.graphices = graphices; this.pen = pen; this.originX = originX; this.originY = originY;
            this.radius = radius; this.startDegree = startDegree; this.intervalDegree = intervalDegree;
        }
    }
}
