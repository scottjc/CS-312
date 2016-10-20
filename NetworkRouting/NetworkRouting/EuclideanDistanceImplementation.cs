using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace NetworkRouting
{
    class EuclideanDistanceImplementation
    {
        //gets the euclidean distance between two points
        //Time: O(1)
        //Space: O(1)
        public double calculate(PointF one, PointF two)
        {
            //square root[(y2 - y1)^2 + (x2 - x1)^2]
            double first = Math.Pow(two.Y - one.Y, 2);
            double second = Math.Pow(two.X - one.X, 2);
            return (double)Math.Sqrt(first + second);
        }

        //gets the euclidean distance from a heap Node and the set of points
        //Time: O(1)
        //Space: O(1)
        public double getMagnitude(Node u, int adj, List<PointF> points)
        {
            //square root of [(y2-y1)^2 + (x2-x1)^2]
            double firstTerm = (points[adj].Y - points[u.pointsIndex].Y) * (points[adj].Y - points[u.pointsIndex].Y);
            double secondTerm = (points[adj].X - points[u.pointsIndex].X) * (points[adj].X - points[u.pointsIndex].X);
            return Math.Sqrt(firstTerm + secondTerm);
        }
    }
}
