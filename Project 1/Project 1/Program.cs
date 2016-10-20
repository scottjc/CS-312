using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            // Keep the console window open in debug mode.
            while (true)
            {
                Console.WriteLine("Type stuff. Type exit to exit.");

                string line = Console.ReadLine(); // Get string from user
                if (line == "exit") // Check string
                {
                    break;
                }
                Console.Write("You typed "); // Report output
                Console.Write(line.Length);
                Console.WriteLine(" character(s)\n");
                Console.Write(line.Contains("a"));
                Console.Write("\n");
                Console.Beep();
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();




            }
        }

    }
}
