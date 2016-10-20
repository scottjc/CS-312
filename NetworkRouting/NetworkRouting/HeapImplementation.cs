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
    public class Node
        {
            public int pointsIndex;
            public int position;
            public double distance;
            public Node(int ptIndex)
            {
                pointsIndex = ptIndex;
                position = -1;
                distance = 0;
            }
        }

    public class HeapImplementation
    {
        
        public List<Node> nodes;
        readonly Node[] data;
        readonly double[] distances;
        public int count;


        //Creates a new, empty priority queue with the specified capacity.
        public HeapImplementation(int capacity)
        {
            data = new Node[capacity];
            distances = new double[capacity];
            nodes = new List<Node>();
            count = 0;
        }

        //Insert
        //Time: O(log v) Because your using a Heap that must reorder itself with every add.
        //Space: O(1) Because your adding one thing.

        //Adds an item to the queue.  Is position is determined by its priority relative to the other items in the queue.
        public void Add(Node item, double priority)
        {
            if (count == data.Length)
                throw new Exception("Heap capacity exceeded");

            // Add the item to the heap in the end position of the array (i.e. as a leaf of the tree)
            int position = count++;
            data[position] = item;
            item.position = position;
            distances[position] = priority;
            // Move it upward into position, if necessary
            MoveUp(position);
            nodes.Add(item);
        }

        //DeleteMin
        //Time: O(log v) Because we're using a Heap and it must rebalance itself after every deletion.
        //Space: O(1) Because we only change one thing.

        //Extracts the item in the queue with the minimal priority value.
        public Node ExtractMin()
        {
            nodes[data[0].pointsIndex].distance = distances[data[0].position];
            //data[0].distance = distances[data[0].position];
            Node minNode = data[0];
            Swap(0, count - 1);
            count--;
            MoveDown(0);
            return minNode;
        }

        //DecreaseKey
        //Time: O(log v) Because if you change the weight of one of the Nodes, the Hea needs to rebalance itself accordingly.
        //Space: O(log v) Because you are changing one thing.
        //Reduces the priority of a node already in the queue. 
        public void DecreasePriority(int index, double priority)
        {
            int position = nodes[index].position;
            while ((position > 0) && (distances[Parent(position)] > priority))
            {
                int original_parent_pos = Parent(position);
                Swap(original_parent_pos, position);
                position = original_parent_pos;
            }
            distances[position] = priority;
        }

        //Moves the node at the specified position upward, it it violates the Heap Property.
        //This is the while loop from the HeapInsert procedure in the slides.
        void MoveUp(int position)
        {
            while ((position > 0) && (distances[Parent(position)] > distances[position]))
            {
                int original_parent_pos = Parent(position);
                Swap(position, original_parent_pos);
                position = original_parent_pos;
            }
        }


        //Moves the node at the specified position down, if it violates the Heap Property
        void MoveDown(int position)
        {
            int lchild = LeftChild(position);//look at it's left child and get its value
            int rchild = RightChild(position); ;//look at it's left child and get its value
            int largest = 0;
            if ((lchild < count) && (distances[lchild] < distances[position]))
            {
                largest = lchild;
            }
            else
            {
                largest = position;
            }
            if ((rchild < count) && (distances[rchild] < distances[largest]))
            {
                largest = rchild;
            }
            if (largest != position)
            {
                Swap(position, largest);
                MoveDown(largest);
            }
        }


        //Get the number of items waiting in queue
        public int Count
        {
            get
            {
                return count;
            }
        }

        //Swaps the nodes at the respective positions in the heap
        //Updates the nodes' position properties accordingly.
        void Swap(int position1, int position2)
        {
            Node temp = data[position1];
            data[position1] = data[position2];
            data[position2] = temp;
            data[position1].position = position1;
            data[position2].position = position2;

            double temp2 = distances[position1];
            distances[position1] = distances[position2];
            distances[position2] = temp2;
        }


        //Gives the position of a node's parent, the node's position in the queue.
        static int Parent(int position)
        {
            return (position - 1) / 2;
        }


        //Returns the position of a node's left child, given the node's position.
        static int LeftChild(int position)
        {
            return 2 * position + 1;
        }

        //Returns the position of a node's right child, given the node's position.
        static int RightChild(int position)
        {
            return 2 * position + 2;
        }
    }    
}
