using System;
using System.Diagnostics;
using System.Threading;

namespace OS_Sync_Ex_00
{
  class Program
  {
    private static string x = "";
    private static int exitflag = 0;

    static void ThReadX()
    {
      lock (x)
      {
        while (exitflag == 0)
        {
          Console.WriteLine("X = {0}", x);
        }
      }
    }
    static void ThWriteX()
    {
      string xx;
      lock (x)
      {
        while (exitflag == 0)
        {
          Console.Write("Input: ");
          xx = Console.ReadLine();
          if (xx == "exit")
          {
            exitflag = 1;
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
