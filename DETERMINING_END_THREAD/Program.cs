// #define USING_PROPERTY_ISALIVE
#define USING_FUNC_JOIN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DETERMINING_END_THREAD
{
    class MyThread
    {
        public int Count;
        public Thread Thrd;
        public MyThread(string name)
        {
            Count = 0;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            Thrd.Start();
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
    class MoreThreads
    {
        static void Main()
        {
             Console.OutputEncoding = System.Text.Encoding.UTF8;

#if USING_PROPERTY_ISALIVE
            {
                Console.WriteLine("Основной поток начат.");
                // Сконструировать три потока.
                MyThread mt1 = new MyThread("Поток #1");
                Thread.Sleep(500);
                MyThread mt2 = new MyThread("Поток #2");
                Thread.Sleep(500);
                MyThread mt3 = new MyThread("Поток #3");
                do
                {
                    Console.Write(".");
                    Thread.Sleep(100);
                } while (mt1.Thrd.IsAlive || mt2.Thrd.IsAlive || mt3.Thrd.IsAlive);
                Console.WriteLine("Основной поток завершен.");
            }
#endif

#if USING_FUNC_JOIN
            //Метод Join() ожидает до тех пор, пока поток, для которого он был вызван, не
            //завершится.Его имя отражает принцип ожидания до тех пор, пока вызывающий
            //поток не присоединится к вызванному методу.Если же данный поток не был начат,
            //то генерируется исключение ThreadStateException. В других формах метода Join()
            //можно указать максимальный период времени, в течение которого следует ожидать
            //завершения указанного потока.

            Console.WriteLine("Основной поток начат.");
            // Сконструировать три потока.
            MyThread mt1 = new MyThread("Потомок #1");
            Thread.Sleep(500);
            MyThread mt2 = new MyThread("Потомок #2");
            Thread.Sleep(500);
            MyThread mt3 = new MyThread("Потомок #3");
            mt1.Thrd.Join();
            Console.WriteLine("Потомок #1 присоединен.");
            mt2.Thrd.Join();
            Console.WriteLine("Потомок #2 присоединен.");
            mt3.Thrd.Join();
            Console.WriteLine("Потомок #3 присоединен.");
            Console.WriteLine("Основной поток завершен.");
#endif

            Console.ReadKey();
        }
    }
}

    //Основной поток начат.
    //Поток #1 начат.
    //В потоке Поток #1, Count = 0
    //Поток #2 начат.
    //В потоке Поток #1, Count = 1
    //.В потоке Поток #2, Count = 0
    //Поток #3 начат.
    //....В потоке Поток #2, Count = 1
    //В потоке Поток #1, Count = 2
    //В потоке Поток #3, Count = 0
    //.....В потоке Поток #1, Count = 3
    //В потоке Поток #3, Count = 1
    //В потоке Поток #2, Count = 2
    //.....В потоке Поток #3, Count = 2
    //В потоке Поток #2, Count = 3
    //В потоке Поток #1, Count = 4
    //....В потоке Поток #1, Count = 5
    //В потоке Поток #3, Count = 3
    //В потоке Поток #2, Count = 4
    //.....В потоке Поток #1, Count = 6
    //В потоке Поток #2, Count = 5
    //В потоке Поток #3, Count = 4
    //.....В потоке Поток #2, Count = 6
    //В потоке Поток #1, Count = 7
    //В потоке Поток #3, Count = 5
    //....В потоке Поток #1, Count = 8
    //В потоке Поток #3, Count = 6
    //В потоке Поток #2, Count = 7
    //.....В потоке Поток #3, Count = 7
    //В потоке Поток #1, Count = 9
    //Поток #1 завершен.
    //В потоке Поток #2, Count = 8
    //.....В потоке Поток #2, Count = 9
    //Поток #2 завершен.
    //В потоке Поток #3, Count = 8
    //.....В потоке Поток #3, Count = 9
    //Поток #3 завершен.
    //Основной поток завершен.

