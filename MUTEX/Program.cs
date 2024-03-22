// Герберт Шилд - с# стр. 864

#define VARIANT_1
// #define VARIANT_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MUTEX
{
    internal class Program
    {
        // В этом классе содержится общий ресурс(переменная Count),
        // а также мьютекс (Mtx), управляющий доступом к ней.
        class SharedResource {
            public static int Count = 0;
            public static Mutex Mtx = new Mutex();
        }

        // В этом потоке переменная SharedResource.Count инкрементируется.
        class IncThread {
            int num;
            public Thread Thrd;

            public IncThread(string name, int n) {
                Thrd = new Thread(this.Run);
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

#if VARIANT_1

            // точка входа в поток.
            void Run()
            {
                Console.WriteLine(Thrd.Name + " ожидает мьютекс.");
                // Получить мьютекс.
                SharedResource.Mtx.WaitOne();
                Console.WriteLine(Thrd.Name + " получил мьютекс.");
                SharedResource.Mtx.ReleaseMutex();
                Console.WriteLine(Thrd.Name + " освободил мьютекс");
                // Получить мьютекс 2 раз.
                SharedResource.Mtx.WaitOne();
                Console.WriteLine(Thrd.Name + " получил мьютекс 2 раз.");
                do {
                    Thread.Sleep(500);
                    SharedResource.Count++;
                    Console.WriteLine("В потоке " + Thrd.Name + ", SharedResource.Count = " + SharedResource.Count);
                    num--;
                } 
                while (num >0);

                Console.WriteLine(Thrd.Name + " освободил мьютекс");
                // осовободить мьютекс
                SharedResource.Mtx.ReleaseMutex();
            }
#endif

#if VARIANT_2

            void Run()
            {
                Console.WriteLine(Thrd.Name + " ожидает мьютекс.");
                
                
                
                do
                {
                    // Получить мьютекс.
                    SharedResource.Mtx.WaitOne();
                    Console.WriteLine(Thrd.Name + " получил мьютекс.");
                    Thread.Sleep(500);
                    SharedResource.Count++;
                    Console.WriteLine("В потоке " + Thrd.Name + ", SharedResource.Count = " + SharedResource.Count);
                    Console.WriteLine(Thrd.Name + " освободил мьютекс");
                    // осовободить мьютекс
                    SharedResource.Mtx.ReleaseMutex();
                    num--;
                }
                while (num > 0);

                
            }
#endif
        }
        // В этом потоке переменная SharedResource.Count декрементируется.
        class DecThread
        {
            int num;
            public Thread Thrd;

            public DecThread(string name, int n)
            {
                Thrd = new Thread(new ThreadStart( this.Run));
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

#if VARIANT_1
            // точка входа в поток.
            void Run()
            {
                Console.WriteLine(Thrd.Name + " ожидает мьютекс.");
                // Получить мьютекс.
                SharedResource.Mtx.WaitOne();
                Console.WriteLine(Thrd.Name + " получил мьютекс.");
                do
                {
                    Thread.Sleep(500);
                    SharedResource.Count--;
                    Console.WriteLine("В потоке " + Thrd.Name + ", SharedResource.Count = " + SharedResource.Count);
                    num--;
                }
                while (num > 0);

                Console.WriteLine(Thrd.Name + " освободил мьютекс");
                // осовободить мьютекс
                SharedResource.Mtx.ReleaseMutex();
            }
#endif
#if VARIANT_2
            // точка входа в поток.
            void Run()
            {
                Console.WriteLine(Thrd.Name + " ожидает мьютекс.");
                
                do
                {
                    // Получить мьютекс.
                    SharedResource.Mtx.WaitOne();
                    Console.WriteLine(Thrd.Name + " получил мьютекс.");
                    Thread.Sleep(500);
                    SharedResource.Count--;
                    Console.WriteLine("В потоке " + Thrd.Name + ", SharedResource.Count = " + SharedResource.Count);
                    Console.WriteLine(Thrd.Name + " освободил мьютекс");
                    // осовободить мьютекс
                    SharedResource.Mtx.ReleaseMutex();
                    num--;
                }
                while (num > 0);

                
            }
#endif
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Сконструировать два потока.
            {
                IncThread mt1 = new IncThread("Инкрементирующий поток", 5);
                Thread.Sleep(1); // разрешить инкрементирующему потоку начаться
                DecThread mt2 = new DecThread("Декрементирующий поток", 5);
                mt1.Thrd.Join();
                mt2.Thrd.Join();
            }

            Console.ReadKey();
        }
    }
}
