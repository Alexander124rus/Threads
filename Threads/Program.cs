using System;
using System.Threading;
using System.Diagnostics;

namespace Threads
{
    internal class Program
    {
        static int Counter = 10000;
        static void TreadingsMetod(object id)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j <= Counter; j++)
                {
                    for (int k = 0; k <= Counter; k++)
                    {
                        if (j == Counter && k == Counter)
                        {
                            Console.Write(id);
                        }
                    }
                }
            }

            /*try
            {
                Console.WriteLine(id);
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
            }*/

            /*while (true)
            {
                Console.WriteLine(3);
                Thread.Sleep(500);
            }*/
            /*Thread th = Thread.CurrentThread;
            Console.WriteLine("Текущий поток: ");
            Console.WriteLine(" Язык: {0}", th.CurrentCulture);
            Console.WriteLine(" Идентификатор: {0}", th.ManagedThreadId);
            Console.WriteLine(" Приоритет: {0}", th.Priority);
            Console.WriteLine(" Состояние: {0}", th.ThreadState);*/
        }
        static int Main(string[] args)
        {
            int min_cpu, min_io, max_cpu, max_io;
            ThreadPool.GetMinThreads(out min_cpu, out min_io);
            ThreadPool.GetMaxThreads(out max_cpu, out max_io);
            Console.WriteLine("Потоки ЦП: {0}..{1}", min_cpu, max_cpu);
            Console.WriteLine("Асинхронные потоки: {0}..{1}", min_io,
            max_io);
            for (int i = 0; i < 4; i++)
            {
                ThreadPool.QueueUserWorkItem(TreadingsMetod, i + 1);
            }
            Console.ReadKey(true);
            return 0;

            /*Process proc = Process.GetCurrentProcess();
            proc.PriorityClass = ProcessPriorityClass.AboveNormal;
            proc.PriorityBoostEnabled = true;
            Console.WriteLine(proc.BasePriority);
            return 0;*/


            /*Thread th1 = new Thread(TreadingsMetod);
            Console.WriteLine("Запуск вторичного потока th1");
            th1.Start(20);
            Thread.Sleep(5000);
            Console.WriteLine("Состояние th1: {0}", th1.ThreadState);
            Console.WriteLine("Прерываем вторичный поток th1");
            th1.Interrupt();
            Console.WriteLine("Состояние th1: {0}", th1.ThreadState);
            th1.Join();
            Console.WriteLine("Состояние th1: {0}", th1.ThreadState);
            return 0;*/

            /*Thread th = new Thread(TreadingsMetod);
            th.Start();*/


            /*Thread th = Thread.CurrentThread;
            Console.WriteLine("Текущий поток: ");
            Console.WriteLine(" Язык: {0}", th.CurrentCulture);
            Console.WriteLine(" Идентификатор: {0}", th.ManagedThreadId);
            Console.WriteLine(" Приоритет: {0}", th.Priority);
            Console.WriteLine(" Состояние: {0}", th.ThreadState);*/
        }
    }
}
