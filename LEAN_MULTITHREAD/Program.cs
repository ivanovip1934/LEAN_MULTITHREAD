using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LEAN_MULTITHREAD
{
    class MyThread
    {
        public int Count;
        string thrdName;
        public MyThread(string name)
        {
            Count = 0;
            thrdName = name;
        }
        // Точка входа в поток.
        public void Run()
        {
            Console.WriteLine(thrdName + " начат.");
            do
            {
                Thread.Sleep(500);
                Console.WriteLine("В потоке " + thrdName + ", Count = " + Count);
                Count++;
            } while (Count < 10);
            Console.WriteLine(thrdName + " завершен.");
        }
    }
    class MultiThread
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Основной поток начат.");
            // Сначала сконструировать объект типа MyThread.
            MyThread mt1 = new MyThread("Потомок #1");
            MyThread mt2 = new MyThread("Потомок #2");
            // Далее сконструировать поток из этого объекта.
            Thread newThrd1 = new Thread(mt1.Run);
            Thread newThrd2 = new Thread(mt2.Run);
            // И наконец, начать выполнение потока.
            newThrd1.Start();
            newThrd2.Start();
            do
            {
                Console.Write(".");
                Thread.Sleep(5);
            } while (mt1.Count != 10 && mt2.Count != 10);
            Console.WriteLine("Основной поток завершен.");

            Console.ReadKey();  
        }


        
    }
}
