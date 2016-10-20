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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void clearAll()
        {
            startNodeIndex = -1;
            stopNodeIndex = -1;
            sourceNodeBox.Clear();
            sourceNodeBox.Refresh();
            targetNodeBox.Clear();
            targetNodeBox.Refresh();
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            arrayCheckBox.Checked = false;
            arrayCheckBox.Refresh();
            return;
        }

        private void clearSome()
        {
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            return;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            int randomSeed = int.Parse(randomSeedBox.Text);
            int size = int.Parse(sizeBox.Text);

            Random rand = new Random(randomSeed);
            seedUsedLabel.Text = "Random Seed Used: " + randomSeed.ToString();

            clearAll();
            this.adjacencyList = generateAdjacencyList(size, rand);
            List<PointF> points = generatePoints(size, rand);
            resetImageToPoints(points);
            this.points = points;
        }

        // Generates the distance matrix.  Values of -1 indicate a missing edge.  Loopbacks are at a cost of 0.
        private const int MIN_WEIGHT = 1;
        private const int MAX_WEIGHT = 100;
        private const double PROBABILITY_OF_DELETION = 0.35;

        private const int NUMBER_OF_ADJACENT_POINTS = 3;

        private List<HashSet<int>> generateAdjacencyList(int size, Random rand)
        {
            List<HashSet<int>> adjacencyList = new List<HashSet<int>>();

            for (int i = 0; i < size; i++)
            {
                HashSet<int> adjacentPoints = new HashSet<int>();
                while (adjacentPoints.Count < 3)
                {
                    int point = rand.Next(size);
                    if (point != i) adjacentPoints.Add(point);
                }
                adjacencyList.Add(adjacentPoints);
            }

            return adjacencyList;
        }

        private List<PointF> generatePoints(int size, Random rand)
        {
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < size; i++)
            {
                points.Add(new PointF((float)(rand.NextDouble() * pictureBox.Width), (float)(rand.NextDouble() * pictureBox.Height)));
            }
            return points;
        }

        private void resetImageToPoints(List<PointF> points)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(pictureBox.Image);
            Pen pen;

            if (points.Count < 100)
                pen = new Pen(Color.Blue);
            else
                pen = new Pen(Color.LightBlue);
            foreach (PointF point in points)
            {
                graphics.DrawEllipse(pen, point.X, point.Y, 2, 2);
            }

            this.graphics = graphics;
            pictureBox.Invalidate();
        }

        // These variables are instantiated after the "Generate" button is clicked
        private List<PointF> points = new List<PointF>();
        private Graphics graphics;
        private List<HashSet<int>> adjacencyList;

        // Use this to generate paths (from start) to every node; then, just return the path of interest from start node to end node
        private void solveButton_Click(object sender, EventArgs e)
        {
            // This was the old entry point, but now it is just some form interface handling
            bool ready = true;

            if (startNodeIndex == -1)
            {
                sourceNodeBox.Focus();
                sourceNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if (stopNodeIndex == -1)
            {
                if (!sourceNodeBox.Focused)
                    targetNodeBox.Focus();
                targetNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if (points.Count > 0)
            {
                resetImageToPoints(points);
                paintStartStopPoints();
            }
            else
            {
                ready = false;
            }
            if (ready)
            {
                clearSome();
                solveButton_Clicked();  // Here is the new entry point
            }
        }



        private void solveButton_Clicked()
        {
            // *** Implement this method, use the variables "startNodeIndex" and "stopNodeIndex" as the indices for your start and stop points, respectively ***

            //Here we define the data objects we'll be using
            String s = sourceNodeBox.Text;
            int startIndex = Int32.Parse(s);
            s = targetNodeBox.Text;
            int endIndex = Int32.Parse(s);
            EuclideanDistanceImplementation edi = new EuclideanDistanceImplementation();
            Font arialFont = new Font("Arial", 11);
            Pen p = new Pen(Color.Black, 1);
            double totalDistance = 0;
            DateTime startTime;
            double arrayTime = 0;
            double heapTime = 0;
            double elapsedMillisecondsTime = 0;

            if (arrayCheckBox.Checked == true)
            {
                //start the timer
                startTime = DateTime.Now;

                //compute dijkstra's algorithm with the data we have.
                ArrayImplementation ai = new ArrayImplementation(startIndex, endIndex, points, adjacencyList);
                List<double> previous = ai.dijkstras();//returns previous with all the indexes till the endIndex. 
 
                int currIndex = endIndex;

                //Draw the shortest path with the costs backwards
                //Time: O(|v|) Because you may have to draw each point at worst.
                //Space: O(1) Beacuse you only change one point.
                while (currIndex != startIndex)
                {
                    //Console.WriteLine("Here");
                    //Console.WriteLine("Prev " + previous.ElementAt(currIndex));
                    //make sure that we can actually reach this thing
                    if (previous.ElementAt(currIndex) == 2147483647)
                    {
                        this.pathCostBox.Text = "unreachable";
                        return;
                    }

                    //draw the line 
                    graphics.DrawLine(p, points[currIndex], points[(int)previous.ElementAt(currIndex)]);

                    //get and display length for every line drawn
                    double dist = edi.calculate(points[currIndex], points[(int)previous.ElementAt(currIndex)]);
                    totalDistance = totalDistance + dist;
                    string str = Convert.ToString((int)dist);

                    //calculation for the slope of the line and put it in the middle
                    RectangleF rectf = new RectangleF(70, 90, 80, 80);//x, y, width, height
                    rectf.X = Math.Abs(points[(int)previous.ElementAt(currIndex)].X + points[currIndex].X) / 2;
                    rectf.Y = Math.Abs(points[(int)previous.ElementAt(currIndex)].Y + points[currIndex].Y) / 2;
                    graphics.DrawString(str, arialFont, Brushes.Black, rectf);
                    currIndex = (int)previous.ElementAt(currIndex);

                    //Stop the timer and write the values to the forms
                    this.pathCostBox.Text = Convert.ToString(totalDistance);
                    elapsedMillisecondsTime = (DateTime.Now - startTime).TotalMilliseconds;
                    arrayTime = elapsedMillisecondsTime / 10000;
                    this.arrayTimeBox.Text = Convert.ToString(arrayTime);
                }
            }


            //Now do the same thing with a Heap
            //O(log |v|))
            //start the timer
            startTime = DateTime.Now;

            HeapImplementation heapQueue = new HeapImplementation(points.Count);
            //ArrayQueue
            List<double> dist1 = new List<double>();
            List<int> prev1 = new List<int>();
            List<double> dist2 = new List<double>();
            List<int> prev2 = new List<int>();
            totalDistance = 0;

            for (int u = 0; u < points.Count; u++)
            {
                dist1.Add(Int16.MaxValue);
                prev1.Add(-1);
                dist2.Add(Int16.MaxValue);
                prev2.Add(-1);
            }
            dist1[startNodeIndex] = 0;
            dist2[startNodeIndex] = 0;

            //Heap implementation
            for (int u = 0; u < dist2.Count; u++)
            {
                Node node = new Node(u);
                heapQueue.Add(node, dist2[u]);
            }

            while (heapQueue.count > 0)
            {
                Node min = heapQueue.ExtractMin();
                foreach (int adj in adjacencyList[min.pointsIndex])
                {
                    double newDistance = dist2[min.pointsIndex] + edi.getMagnitude(min, adj, points);
                    if (dist2[adj] > newDistance)
                    {
                        dist2[adj] = newDistance;
                        prev2[adj] = min.pointsIndex;
                        heapQueue.DecreasePriority(adj, newDistance);
                    }
                }
            }

            //Draw the shortest path with the costs backwards
            //Time: O(|v|) Because you may have to draw each point at worst.
            //Space: O(1) Beacuse you only change one point.
            int currIndexToDraw = stopNodeIndex;
            while (currIndexToDraw != startNodeIndex)
            {
                int prevIndex = prev2[currIndexToDraw];

                //Console.WriteLine("Here");
                //Console.WriteLine("Prev " + points[currIndexToDraw]);
                //make sure that we can actually reach this thing
                if(prevIndex > points.Count-1 || prevIndex < 0)
                {
                    this.pathCostBox.Text = "unreachable";
                    return;
                }

                graphics.DrawLine(p, points[currIndexToDraw], points[prevIndex]);

                double currDistance = edi.calculate(points[prevIndex], points[currIndexToDraw]);
                totalDistance += currDistance;

                String text = Convert.ToString((int)currDistance);
                RectangleF drawingBox = new RectangleF(Math.Abs(points[prevIndex].X + points[currIndexToDraw].X) / 2,
                    Math.Abs(points[prevIndex].Y + points[currIndexToDraw].Y) / 2, 80, 80);
                graphics.DrawString(text, new Font("Arial", 11), Brushes.Black, drawingBox);

                currIndexToDraw = prev2[currIndexToDraw];
            }
            this.pathCostBox.Text = Convert.ToString(totalDistance);

            //Now set the time it takes for all this to happen
            elapsedMillisecondsTime = (DateTime.Now - startTime).TotalMilliseconds;
            heapTime = elapsedMillisecondsTime / 10000;
            this.heapTimeBox.Text = Convert.ToString(heapTime);
            if (arrayCheckBox.Checked == true)
            {
                this.differenceBox.Text = Convert.ToString(arrayTime / heapTime);
            }
        }


        private Boolean startStopToggle = true;
        private int startNodeIndex = -1;
        private int stopNodeIndex = -1;
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (points.Count > 0)
            {
                Point mouseDownLocation = new Point(e.X, e.Y);
                int index = ClosestPoint(points, mouseDownLocation);
                if (startStopToggle)
                {
                    startNodeIndex = index;
                    sourceNodeBox.ResetBackColor();
                    sourceNodeBox.Text = "" + index;
                }
                else
                {
                    stopNodeIndex = index;
                    targetNodeBox.ResetBackColor();
                    targetNodeBox.Text = "" + index;
                }
                resetImageToPoints(points);
                paintStartStopPoints();
            }
        }

        private void sourceNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try { startNodeIndex = int.Parse(sourceNodeBox.Text); }
                catch { startNodeIndex = -1; }
                if (startNodeIndex < 0 | startNodeIndex > points.Count - 1)
                    startNodeIndex = -1;
                if (startNodeIndex != -1)
                {
                    sourceNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }

        private void targetNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try { stopNodeIndex = int.Parse(targetNodeBox.Text); }
                catch { stopNodeIndex = -1; }
                if (stopNodeIndex < 0 | stopNodeIndex > points.Count - 1)
                    stopNodeIndex = -1;
                if (stopNodeIndex != -1)
                {
                    targetNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }

        private void paintStartStopPoints()
        {
            if (startNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Green, 6), points[startNodeIndex].X, points[startNodeIndex].Y, 1, 1);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }

            if (stopNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Red, 2), points[stopNodeIndex].X - 3, points[stopNodeIndex].Y - 3, 8, 8);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }
        }

        private int ClosestPoint(List<PointF> points, Point mouseDownLocation)
        {
            double minDist = double.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < points.Count; i++)
            {
                double dist = Math.Sqrt(Math.Pow(points[i].X - mouseDownLocation.X, 2) + Math.Pow(points[i].Y - mouseDownLocation.Y, 2));
                if (dist < minDist)
                {
                    minIndex = i;
                    minDist = dist;
                }
            }

            return minIndex;
        }
    }
}
