using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic;
using System.Linq;


namespace Problem01
{

    static class Constants
    {
        public const int N = 8;
    }

    class Program
    {
        static List<Thread> lstThreads = new List<Thread>();
        static int N = Constants.N;
        static int MAX = 1000000000;
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = new long[Constants.N];
        static int G_index = 0;
        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void sum(int taskId)
        {
            int index = taskId * (MAX/N);
            int stop  = (taskId+1) * (MAX/N);
            Console.WriteLine("Spawning thread {0,-5} Start {1,-10} Stop {2,-10}", taskId, index, stop);
            Console.WriteLine("Thread {0} is working!", taskId);
            while(index != stop) {
                if (Data_Global[index] % 2 == 0)
                {
                    Sum_Global[taskId] -= Data_Global[index];
                }
                else if (Data_Global[index] % 3 == 0)
                {
                    Sum_Global[taskId] += (Data_Global[index]*2);
                }
                else if (Data_Global[index] % 5 == 0)
                {
                    Sum_Global[taskId] += (Data_Global[index] / 2);
                }
                else if (Data_Global[index] %7 == 0)
                {
                    Sum_Global[taskId] += (Data_Global[index] / 3);
                }
                Data_Global[index] = 0;
                index += 1;
            }  
        }

        static void CreateThreads()
        {
            int nThread = lstThreads.Count;
            Thread th = new Thread(() => { sum(nThread); });
            th.Start();
            lstThreads.Add(th);
        }
        static void Main(string[] args)
        {
            
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
            // Console.Clear();

            if(args.Count() == 1) {
                N = int.Parse(args[0]);
            }

            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            Console.Write("\n\nWorking...\n\n");
            sw.Start();

            for(i=0;i<N;i++) { // create thread   
                CreateThreads();
            }

            foreach (Thread th in lstThreads)
                th.Join();

            sw.Stop();
            Console.WriteLine("Done.");

            // Task.waitAll(t[0]);

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global.Sum());
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            
            if(args.Count() == 1) {
                using StreamWriter file = new("output.txt");
                file.WriteLineAsync(sw.ElapsedMilliseconds.ToString());
            }

        }
    }
}