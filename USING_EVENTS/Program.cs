//#define USING_MANUALRESETEVENT
#define USING_AUTORESETEVENT


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace USING_EVENTS
{
#if USING_MANUALRESETEVENT
    class MyThread
    {
        public Thread Thrd;
        ManualResetEvent mre;
        public MyThread(string name, ManualResetEvent evt)
        {
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            mre = evt;
            Thrd.Start();
        }
#endif
#if USING_AUTORESETEVENT
        class MyThread
        {
            public Thread Thrd;
            AutoResetEvent mre;
            public MyThread(string name, AutoResetEvent evt)
            {
                Thrd = new Thread(this.Run);
                Thrd.Name = name;
                mre = evt;
                Thrd.Start();
            }


#endif


            // Точка входа в поток.
            void Run()
        {
            Console.WriteLine("Внутри потока " + Thrd.Name);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(Thrd.Name);
                Thread.Sleep(100);
            }
            Console.WriteLine(Thrd.Name + " завершен!");
            // Уведомить о событии.
            mre.Set();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
#if USING_MANUALRESETEVENT
            {

                ManualResetEvent evtObj = new ManualResetEvent(false);
                MyThread mt1 = new MyThread("Событийный Поток 1", evtObj);
                Console.WriteLine("Основной поток ожидает событие.");
                // Ожидать уведомления о событии.
                evtObj.WaitOne();
                Console.WriteLine("Основной поток получил уведомление о событии от первого потока.");
                // Установить событийный объект в исходное состояние.
                evtObj.Reset();
                mt1 = new MyThread("Событийный Поток 2", evtObj);
                // Ожидать уведомления о событии.
                evtObj.WaitOne();
                Console.WriteLine("Основной поток получил уведомление о событии от второго потока.");
            }
#endif
#if USING_AUTORESETEVENT
            {

                AutoResetEvent evtObj = new AutoResetEvent(false);
                MyThread mt1 = new MyThread("Событийный Поток 1", evtObj);
                Console.WriteLine("Основной поток ожидает событие.");
                // Ожидать уведомления о событии.
                evtObj.WaitOne();
                Console.WriteLine("Основной поток получил уведомление о событии от первого потока.");
                mt1 = new MyThread("Событийный Поток 2", evtObj);
                // Ожидать уведомления о событии.
                evtObj.WaitOne();
                Console.WriteLine("Основной поток получил уведомление о событии от второго потока.");
            }
#endif



            Console.ReadKey();  
        }
    }
}
