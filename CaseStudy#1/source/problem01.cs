using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = new long[]{0, 0};
        static int G_index = 0;
        static Thread t1, t2;
        static Boolean[] s = new Boolean[]{false, false};

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

        static void task(int taskId, int start, int stop) {
            int i = start;
            for(; i<stop; i++) {
                // Console.WriteLine(i);
                sum(i, taskId);
            }

            Console.WriteLine(i);

            s[taskId] = true;
        }

        static void sum(int index, int taskId)
        {
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
            // Data_Global[index] = 0;
            // G_index++;   
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
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

            Console.Write("\n\nWorking...");
            sw.Start();

            t1 = new Thread(()=>task(0, 0, 500000000));
            t2 = new Thread(()=>task(1, 500000000, 1000000000));
            
            t1.Start();
            t2.Start();


            // while(s[0] == false || s[1] == false) {

            // }

            t1.Join();
            t2.Join();

            /* Start */
            // for (i = 0; i < 1000000000; i++)
            //     sum();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global[0] + Sum_Global[1]);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}