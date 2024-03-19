#define THREADPRIORITY_2_THREAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SET_PROIRITY_THREAD
{
    class MyThread2
    {
        public int Count;
        public Thread Thrd;
        static bool stop = false;
        static string currentName;


        /* Сконструировать новый поток. Обратите внимание на то, что
        данный конструктор еще не начинает выполнение потоков. */

        public MyThread2(string name)
        {
            Count = 0;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            currentName = name;
        }
        // Начать выполнение нового потока.
        void Run()
        {
            Console.WriteLine("Поток " + Thrd.Name + " начат.");
            do
            {
                Count++;
                if (currentName != Thrd.Name)
                {
                    currentName = Thrd.Name;
                    Console.WriteLine("В потоке " + currentName);
                }
            } while (stop == false && Count < 1000000000);
            stop = true;
            Console.WriteLine("Поток " + Thrd.Name + " завершен.");
        }
    }

    class MyThread
    {
        public int Count;
        public Thread Thrd;
        // Обратите внимание на то, что конструктору класса
        // MyThread передается также значение типа int.
        public MyThread(string name, int num, ThreadPriority threadPriority)
        {
            Count = 0;
            // Вызвать конструктор типа ParameterizedThreadStart
            // явным образом только ради наглядности примера.
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            Thrd.Priority = threadPriority;
            // Здесь переменная num передается методу Start()
            // в качестве аргумента.
            Thrd.Start(num);
        }
        // Обратите внимание на то, что в этой форме метода Run()
        // указывается параметр типа object.
        void Run(object num)
        {
            Console.WriteLine(Thrd.Name + " начат со счета " + num);
            Console.WriteLine($"Приоритет потока {Thread.CurrentThread.Name} - Priority = {Thread.CurrentThread.Priority}");
            do
            {
                Thread.Sleep(500);

                Console.WriteLine("В потоке " + Thrd.Name + ", Count = " + Count);
                Count++;
            } while (Count < (int)num);
            Console.WriteLine(Thrd.Name + " завершен.");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

            {

                Console.WriteLine("Основной поток начат.");

                // Обратите внимание на то, что число повторений
                // передается этим двум объектам типа MyThread.
                MyThread mt = new MyThread("Потомок #1", 5, ThreadPriority.Highest);
                MyThread mt2 = new MyThread("Потомок #2", 3, ThreadPriority.Lowest);
                do
                {
                    Thread.Sleep(100);
                } while (mt.Thrd.IsAlive || mt2.Thrd.IsAlive);
                Console.WriteLine("Основной поток завершен.");
            }

#if THREADPRIORITY_2_THREAD
            {
                MyThread2 mt1 = new MyThread2("с высоким приоритетом");
                MyThread2 mt2 = new MyThread2("с низким приоритетом");
                // Установить приоритеты для потоков.
                mt1.Thrd.Priority = ThreadPriority.AboveNormal;
                mt2.Thrd.Priority = ThreadPriority.BelowNormal;
                // Начать потоки.
                mt1.Thrd.Start();
                mt2.Thrd.Start();
                mt1.Thrd.Join();
                mt2.Thrd.Join();
                Console.WriteLine();
                Console.WriteLine("Поток " + mt1.Thrd.Name +
                " досчитал до " + mt1.Count);
                Console.WriteLine("Поток " + mt2.Thrd.Name +
                " досчитал до " + mt2.Count);
            }

#endif




            Console.ReadKey();

        }
    }
}
