using System;
using System.Threading;

namespace OS_Problem_02
{
  class Thread_safe_buffer
  {
    static int[] TSBuffer = new int[10];
    static int Front = 0;
    static int Back = 0;
    static int Count = 0;

    static object _Lock = new object();
    static object _LockEn = new object();
    static object _LockDe = new object();

    static void EnQueue(int eq)
    {
      lock (_Lock)
      {
        if (Count == 10)
        {
          //   Console.WriteLine("ten: Lock!");
          do
          {
            Monitor.Wait(_Lock);
          } while (Count == 10);
          //   Console.WriteLine("ten: Unlock!");
        }
        TSBuffer[Back] = eq;
        Back++;
        Back %= 10;
        Count += 1;
        Monitor.PulseAll(_Lock);
      }
    }

    static int DeQueue()
    {
      lock (_Lock)
      {
        // Monitor.Wait(_Lock);
        if (Count == 0)
        {
          //   Console.WriteLine("zero: Lock!");
          do
          {
            Monitor.Wait(_Lock);
          } while (Count == 0);
          //   Console.WriteLine("zero: Unlock!");
        }
        int x = 0;
        x = TSBuffer[Front];
        Front++;
        Front %= 10;
        Count -= 1;
        Monitor.PulseAll(_Lock);
        return x;
      }
    }

    static void th01(object t)
    {
      int i;

      for (i = 1; i < 51; i++)
      {
        // Console.WriteLine("push i={0}, thread:{1}", i, t);
        EnQueue(i);
        Thread.Sleep(5);
      }
    }

    static void th011(object t)
    {
      int i;

      for (i = 100; i < 151; i++)
      {
        // Console.WriteLine("push i={0}, thread:{1}", i, t);
        EnQueue(i);
        Thread.Sleep(5);
      }
    }


    static void th02(object t)
    {
      int i;
      int j;

      for (i = 0; i < 60; i++)
      {
        j = DeQueue();
        Console.WriteLine("pop j={0}, thread:{1}", j, t);
        Thread.Sleep(100);
      }
    }
    static void Main(string[] args)
    {
      Thread t1 = new Thread(th01);
      Thread t11 = new Thread(th011);
      Thread t2 = new Thread(th02);
      Thread t21 = new Thread(th02);
      Thread t22 = new Thread(th02);

      t1.Start(0);
      t11.Start();
      t2.Start(1);
      t21.Start(2);
      t22.Start(3);
    }
  }
}
