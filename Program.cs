using System;
using System.Diagnostics;

namespace DanskeC_
{
    class Program
    {
        static int memocalls = 0;
        static int nomemocalls = 0;
        static int nrOfLines = 15;
        static int[,] graph = new int[nrOfLines, nrOfLines];
        static int[,] memoizer = new int[nrOfLines, nrOfLines];
        static readonly string filename = "data/inputdata.txt";
        static void Main(string[] args)
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(filename))
            {
                string ln;
                int row = 0;
                while ((ln = file.ReadLine()) != null)
                {
                    // Since ln is representing the whole line we have to split to get each number individually.
                    string[] inrow = ln.Split(" ");
                    for (int column = 0; column < inrow.Length; column++)
                    {
                        graph[row, column] = System.Convert.ToInt32(inrow[column]);
                    }
                    row++;
                }
                file.Close();
            }
            // Testing different implementation as I have used a Dynamic Programming Approach, 
            // the two different calls are done in order to do a test on the saving of space I was trying to do.
            // Using a memoizer array, I am storing the calculated results for later use. 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine(FindMaxSumNoMemoize(0, 0, graph[0, 0] % 2 == 1));
            sw.Stop();
            Console.WriteLine("No memo: "+ sw.Elapsed+ " calls " + nomemocalls);

            
            sw.Restart();
            Console.WriteLine(FindMaxSum(0, 0, graph[0, 0] % 2 == 1));
            sw.Stop();
            Console.WriteLine("Memoized time: "+ sw.Elapsed + " calls " + memocalls);

            
        }

        //  Given row and column as parameters to start the search.
        //  isOdd is an added parameter to check if the next node is actually 
        //  what the previous call has mentioned.
        private static int FindMaxSum(int row, int column, Boolean isOdd)
        {
            memocalls ++;
            int result1, result2;
            //      check if the node value is actually the specified alternating node.
            if ((isOdd && graph[row, column] % 2 == 1) 
                || (!isOdd && graph[row, column] % 2 == 0))
            {
                // If it has reached a leaf, return its value.
                if (row == nrOfLines - 1) return graph[row, column];

                // Checking if the next calculation is already done before.
                if (memoizer[row + 1, column] == 0)
                {
                    result1 = FindMaxSum(row + 1, column, !isOdd);
                    memoizer[row + 1, column] = result1;
                }
                else
                {
                    result1 = memoizer[row + 1, column];
                }

                if (memoizer[row + 1, column + 1] == 0)
                {
                    result2 = FindMaxSum(row + 1, column + 1, !isOdd);
                    memoizer[row + 1, column + 1] = result2;
                }
                else
                {
                    result2 = memoizer[row + 1, column + 1];
                }
                return graph[row, column] + Math.Max(result1, result2);
            }
            return Int32.MinValue;
        }

        private static int FindMaxSumNoMemoize(int row, int column, Boolean isOdd)
        {
            nomemocalls++;
            int res1, res2;

            //      check if the node value is actually the specified alternating node.
            if ((isOdd && graph[row, column] % 2 == 1) || (!isOdd && graph[row, column] % 2 == 0))
            {
                // If it has reached a leaf, return its value.
                if (row == nrOfLines - 1) return graph[row, column];

                res1 = FindMaxSumNoMemoize(row + 1, column, !isOdd);

                res2 = FindMaxSumNoMemoize(row + 1, column + 1, !isOdd);

                return graph[row, column] + Math.Max(res1, res2);
            }
            return Int32.MinValue;
        }
    }
}
