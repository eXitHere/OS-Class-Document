using System;
using System.Diagnostics;
using System.Threading;

namespace OS_Sync_Ex_00
{
  class Program
  {
    private static string x = "";
    private static int exitflag = 0;
    private static object _lock = new object();

    static void ThReadX()
    {
      while (exitflag == 0)
      {
        lock (_lock)
        {
          Console.WriteLine("X = {0}", x);
          Monitor.Wait(_lock);
          // Monitor.Pulse(_lock);
        }
      }
      Console.WriteLine("Thread 1 exit");
    }
    static void ThWriteX()
    {
      string xx;
      while (exitflag == 0)
      {
        lock (_lock)
        {
          Monitor.Pulse(_lock);
          Console.Write("Input: ");
          xx = Console.ReadLine();
          if (xx == "exit")
          {
            exitflag = 1;
            // x = "";
          }
          else
          {
            x = xx;
          }
        }
      }
    }
    static void Main(string[] args)
    {
      Thread A = new Thread(ThReadX);
      Thread B = new Thread(ThWriteX);

      A.Start();
      B.Start();
    }
  }
}
