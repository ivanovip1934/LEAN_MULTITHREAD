// 
#define USING_ATTRIBUTE_MethodImpAttribute
// #define NOT_USING_ATTRIBUTE_MethodImpAttribute

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace USING_WAIT_AND_PULSE
{
    internal class TickTock
    {
        object lockOn = new object();
#if NOT_USING_ATTRIBUTE_MethodImpAttribute
        public void Tick(bool running)
        {
            lock (lockOn)
            {
                DateTime dateTime1 = new DateTime();
                DateTime dateTime2 = new DateTime();
                if (!running)
                { // остановить часы
                    Console.WriteLine("В функцию Тick передали false");
                    Monitor.PulseAll(lockOn); // уведомить любые ожидающие потоки
                    return;
                }
                Console.Write("тик ");
                Thread.Sleep(555);
                Monitor.Pulse(lockOn); // разрешить выполнение метода Tock()

                dateTime1 = DateTime.Now;
                Monitor.Wait(lockOn);
                dateTime2 = DateTime.Now;
                Console.WriteLine($"Время ожидания функции Tick: {dateTime2.Subtract(dateTime1)}");
                // ожидать завершения метода Tock()
            }
        }
        public void Tock(bool running)
        {
            lock (lockOn)
            {
                DateTime dateTime1 = new DateTime();
                DateTime dateTime2 = new DateTime();
                if (!running)
                { // остановить часы
                    Console.WriteLine("В функцию Тock передали false");
                    Monitor.PulseAll(lockOn); // уведомить любые ожидающие потоки
                    return;
                }
                Console.WriteLine("так");
                Thread.Sleep(555);
                Monitor.Pulse(lockOn); // разрешить выполнение метода Tick()
                dateTime1 = DateTime.Now;
                Monitor.Wait(lockOn);
                dateTime2 = DateTime.Now;
                Console.WriteLine($"Время ожидания функции Tock: {dateTime2.Subtract(dateTime1)}" );

            }
        }
#endif

#if USING_ATTRIBUTE_MethodImpAttribute
        /* Следующий атрибут полностью синхронизирует метод Tick(). */
        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public void Tick(bool running)
        {

            DateTime dateTime1 = new DateTime();
            DateTime dateTime2 = new DateTime();
            if (!running)
            { // остановить часы
                Console.WriteLine("В функцию Тick передали false");
                Monitor.PulseAll(this); // уведомить любые ожидающие потоки
                return;
            }
            Console.Write("тик ");
            Thread.Sleep(555);
            Monitor.Pulse(this); // разрешить выполнение метода Tock()

            dateTime1 = DateTime.Now;
            Monitor.Wait(this);
            dateTime2 = DateTime.Now;
            Console.WriteLine($"Время ожидания функции Tick: {dateTime2.Subtract(dateTime1)}");
            // ожидать завершения метода Tock()

        }
        /* Следующий атрибут полностью синхронизирует метод Tock(). */
        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public void Tock(bool running)
        {

            DateTime dateTime1 = new DateTime();
            DateTime dateTime2 = new DateTime();
            if (!running)
            { // остановить часы
                Console.WriteLine("В функцию Тock передали false");
                Monitor.PulseAll(this); // уведомить любые ожидающие потоки
                return;
            }
            Console.WriteLine("так");
            Thread.Sleep(555);
            Monitor.Pulse(this); // разрешить выполнение метода Tick()
            dateTime1 = DateTime.Now;
            Monitor.Wait(this);
            dateTime2 = DateTime.Now;
            Console.WriteLine($"Время ожидания функции Tock: {dateTime2.Subtract(dateTime1)}");


        }
#endif

    }
    class MyThread
    {
        public Thread Thrd;
        TickTock ttOb;
        // Сконструировать новый поток.
        public MyThread(string name, TickTock tt)
        {
            Thrd = new Thread(this.Run);
            ttOb = tt;
            Thrd.Name = name;
            Thrd.Start();
        }
        // Начать выполнение нового потока.
        void Run()
        {
            if (Thrd.Name == "Tick")
            {
                for (int i = 0; i < 5; i++) ttOb.Tick(true);
                ttOb.Tick(false);
            }
            else
            {
                for (int i = 0; i < 5; i++) ttOb.Tock(true);
                ttOb.Tock(false);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            {

                TickTock tt = new TickTock();
                MyThread mt1 = new MyThread("Tick", tt);
                MyThread mt2 = new MyThread("Tock", tt);
                mt1.Thrd.Join();
                mt2.Thrd.Join();
                Console.WriteLine("Часы остановлены");
            }

            Console.ReadKey();

        }
    }
}
