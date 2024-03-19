//Средство блокировки встроено в язык С#. Благодаря этому все объекты могут быть
//синхронизированы. Синхронизация организуется с помощью ключевого слова lock.
//Она была предусмотрена в C# с самого начала, и поэтому пользоваться ею намного
//проще, чем кажется на первый взгляд. В действительности синхронизация объектов во
//многих программах на С# происходит практически незаметно.
//Ниже приведена общая форма блокировки:
//lock (lockObj)
//{
//    // синхронизируемые операторы
//}
//где lockObj обозначает ссылку на синхронизируемый объект.
//Если же требуется синхронизировать только один оператор, то фигурные скобки не нужны. 

//#define LOCK_IN_METHOD
#define LOCK_OBJECT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SYNCHRONIZATION_STREAM
{
    class SumArray
    {
        int sum;
        object lockOn = new object(); // закрытый объект, доступный
                                      // для последующей блокировки
        public int SumIt(int[] nums)
        {            
        lock  (lockOn)
            { // заблокировать весь метод
                sum = 0; // установить исходное значение суммы
                for (int i = 0; i < nums.Length; i++)
                {
                    sum += nums[i];
                    Console.WriteLine("Текущая сумма для потока " +
                    Thread.CurrentThread.Name + " равна " + sum);
                    Thread.Sleep(10); // разрешить переключение задач
                }
                Thread.Sleep(1);
                return sum;
            }
        }
        public int SumIt2(int[] nums)   //Использование вместо lock(object) функции Monitor.Enter(object) и Monitor.Exit(object)
        {
            Monitor.Enter(lockOn);
            // заблокировать весь метод
            sum = 0;// установить исходное значение суммы
            int tmpsum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
                Console.WriteLine("! Текущая сумма для потока " +
                Thread.CurrentThread.Name + " равна " + sum);
                Thread.Sleep(5); // разрешить переключение задач
            }
            tmpsum = sum;
            Monitor.Exit(lockOn);
            Thread.Sleep(1);
            return tmpsum;
        }
    }


    class MyThread
    {
        public Thread Thrd;
        int[] a;
        int answer;
        // Создать один объект типа SumArray для всех
        // экземпляров класса MyThread.
        static SumArray sa = new SumArray();
        // Сконструировать новый поток,
        public MyThread(string name, int[] nums)
        {
            a = nums;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            Thrd.Start(); // начать поток
        }
        // Начать выполнение нового потока.
        void Run()
        {
            Console.WriteLine(Thrd.Name + " начат.");
            answer = sa.SumIt(a);
            Thread.Sleep(100); 
            Console.WriteLine("Сумма для потока " + Thrd.Name + " равна " + answer);
            Console.WriteLine(Thrd.Name + " завершен.");
        }
    }

#if LOCK_OBJECT
    class SumArray2
    {
        int sum;
        public int SumIt(int[] nums)
        {
            sum = 0; // установить исходное значение суммы
            for (int i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
                Console.WriteLine("Текущая сумма для потока " +
                Thread.CurrentThread.Name + " равна " + sum);
                Thread.Sleep(10); // разрешить переключение задач
            }
            return sum;
        }
    }
    class MyThread2
    {
        public Thread Thrd;
        int[] a;
        int answer;
        /* Создать один объект типа SumArray для всех
        экземпляров класса MyThread. */
        static SumArray2 sa = new SumArray2();
        // Сконструировать новый поток.
        public MyThread2(string name, int[] nums)
        {
            a = nums;
            Thrd = new Thread(this.Run3);
            Thrd.Name = name;
            Thrd.Start(); // начать поток
        }
        // Начать выполнение нового потока.
        void Run()
        {
            Console.WriteLine(Thrd.Name + " начат.");
            // Заблокировать вызовы метода SumIt().
            lock (sa) answer = sa.SumIt(a);
            //answer = sa.SumIt(a);
            Console.WriteLine("Сумма для потока " + Thrd.Name +
            " равна " + answer);
            Console.WriteLine(Thrd.Name + " завершен.");
        }
        void Run2() //Использование вместо lock(object) функции Monitor.Enter(object) и Monitor.Exit(object)
        {
            Console.WriteLine(Thrd.Name + " начат.");
            // Заблокировать вызовы метода SumIt().
            Monitor.Enter(sa);
            answer = sa.SumIt(a);
            Monitor.Exit(sa);
            //answer = sa.SumIt(a);
            Console.WriteLine("Сумма для потока " + Thrd.Name +
            " равна " + answer);
            Console.WriteLine(Thrd.Name + " завершен.");
        }
        void Run3() //Использование вместо lock(object) функции Monitor.TryEnter(object) и Monitor.Exit(object)
        {
            Console.WriteLine(Thrd.Name + " начат.");
            // Заблокировать вызовы метода SumIt().
            while (!Monitor.TryEnter(sa))
            {
               // Console.WriteLine($"{Thrd.Name} НЕ получил доступ к объекту");
                Thread.Sleep(1);
            }
            
                Console.WriteLine($"{Thrd.Name} получил доступ к объекту");
                answer = sa.SumIt(a);
                Monitor.Exit(sa);
            
            
            //answer = sa.SumIt(a);
            Console.WriteLine("Сумма для потока " + Thrd.Name +
            " равна " + answer);
            Console.WriteLine(Thrd.Name + " завершен.");
        }

    }

#endif





    internal class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
#if LOCK_IN_METHOD

            {
                int[] a = { 1, 2, 3, 4, 5 };
                MyThread mt1 = new MyThread("Потомок #1", a);
                MyThread mt2 = new MyThread("Потомок #2", a);
                mt1.Thrd.Join();
                mt2.Thrd.Join();
            }
#endif

#if LOCK_OBJECT
            {
                int[] a = { 1, 2, 3, 4, 5 };
                MyThread2 mt1 = new MyThread2("Потомок #1", a);
                MyThread2 mt2 = new MyThread2("Потомок #2", a);
                mt1.Thrd.Join();
                mt2.Thrd.Join();
            }

#endif




            Console.ReadKey();  
        }
    }
}
