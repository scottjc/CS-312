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
    class ArrayImplementation
    {
        int startIndex;
        int endIndex;
        List<PointF> points;
        List<HashSet<int>> adjacencyList;

        List<double> distance = new List<double>();
        List<double> previous = new List<double>();
        List<double> queue = new List<double>();

        public ArrayImplementation(int startIndexIn, int endIndexIn, List<PointF> pointsIn, List<HashSet<int>> adjacencyListIn)
        {
            startIndex = startIndexIn;
            endIndex = endIndexIn;
            points = pointsIn;
            adjacencyList = adjacencyListIn;
        }

        //Time: O(|v|^2)
        //Space: O(V)
        //The main function for doing dijkstra's algorithm and resurning the results.
        public List<double> dijkstras()
        {
            //Console.WriteLine("Inside array dijkstras");
            int max = Int32.MaxValue;

            //We start by adding all values to infinity
            for (int i = 0; i < points.Count; i++)
            {
                //Insert
                //Time: O(1) Beacuse your doing it for one point.
                //Space: O(1) Beacuse your making the queue.
                distance.Add(max);
                previous.Add(max);
                queue.Add(max);
            }
            distance[startIndex] = 0;
            queue[startIndex] = 0;
            int minIndex = startIndex;
            //to compare and change indexes

            int minusOnes = 0;
            EuclideanDistanceImplementation edi = new EuclideanDistanceImplementation();
            while (minusOnes < queue.Count)
            {
                double minValue = max;
                //Console.WriteLine("minusOnes " + minusOnes);

                //the deleteMin function
                //Time: O(|v|) Because you have to iterate over every point to find the minimum.
                //Space: O(1) Nothing changed.
                for (int i = 0; i < queue.Count; i++)
                {
                    //Console.WriteLine("Dist is " + queue[i]);
                    if (queue[i] != -1 && queue[i] < minValue)
                    {
                        minIndex = i;
                        minValue = queue[i];
                        //Console.WriteLine("min index is " + i + " of " + queue[i]);
                    }
                }
                //Console.WriteLine("The current min Index is " + minIndex);
                //previous.Add(points[minIndex]);
                queue[minIndex] = -1;//set the value to -1 to take it out, basically

                //update the distances of each of this node's directed edges. O(3)
                for (int i = 0; i < adjacencyList[minIndex].Count; i++)
                {
                    int currAdj = adjacencyList[minIndex].ElementAt(i);
                    //Console.WriteLine("currAdj is " + currAdj + " of " + adjacencyList[minIndex]);
                    double dist = edi.calculate(points[minIndex], points[currAdj]);
                    //Console.WriteLine("dist path of " + currAdj +"  is " + distance[currAdj] + " versus " + (distance[minIndex] + dist));
                    if (distance[currAdj] > distance[minIndex] + dist)
                    {
                        distance[currAdj] = distance[minIndex] + dist;
                        //Console.WriteLine("Add " + currAdj + " from point " + minIndex);
                        //previous.Add(minIndex);
                        previous[currAdj] = minIndex;

                        //decreaseKey
                        //Time: O(1) Because you can directly access the index.
                        //Space: O(1) Because you change only one thing.
                        queue[currAdj] = distance[currAdj];
                    }
                }
                minusOnes++;
            }

            previous.Add(endIndex);
            return previous;
        }
    }
}
