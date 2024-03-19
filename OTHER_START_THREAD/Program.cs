using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OTHER_START_THREAD
{
    class MyThread
    {
        public int Count;
        public Thread Thrd;
        public MyThread(string name)
        {
            Count = 0;
            Thrd = new Thread(this.Run);
            Thrd.Name = name; // задать имя потока
            Thrd.Start(); // начать поток
        }
        // Точка входа в поток.
        void Run()
        {
            Console.WriteLine(Thrd.Name + " начат.");
            do
            {
                Thread.Sleep(500);
                Console.WriteLine("В потоке " + Thrd.Name + ", Count = " + Count);
                Count++;
            } while (Count < 10);
            Console.WriteLine(Thrd.Name + " завершен.");
        }
    }
    class MultiThreadImproved
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Основной поток начат.");
            // Сначала сконструировать объект типа MyThread.
            MyThread mt = new MyThread("Потомок #1");
            do
            {
                Console.Write(".");
                Thread.Sleep(100);
            } while (mt.Count != 10);
            Console.WriteLine("Основной поток завершен.");
            Console.ReadKey();
        }
    }
}
