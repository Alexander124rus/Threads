using System;
using System.Threading;
using System.Diagnostics;

namespace Threads
{
    internal class Program
    {
        //static int Counter = 10000;
        //static Semaphore SemaPool;
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

        const int Counter = 10000;
        class WaitObject {
            public bool TimeOut = false;
            public int ID = 1;
            public RegisteredWaitHandle Handle; // объект для управления
            public static void CallbackMethod(object state, bool timeout)
            {
                WaitObject obj = (WaitObject)state;
                int id = obj.ID++;
                if (!timeout)
                {
                    Console.WriteLine("\nПоток #{0} получил сигнал о запуске", id);
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
                }
                else
                {
                    Console.WriteLine("\nВремя ожидания закончилось");
                    obj.TimeOut = true;//Обозначаем истёкшее время ожидания
                    obj.Handle.Unregister(null); //Отменяет зарегистрированную операцию ожидания, вызванную методом RegisterWaitForSingleObject(WaitHandle, WaitOrTimerCallback, Object, UInt32, Boolean)
                }
            }


        }


        static int Main(string[] args)
        {
            int max_cpu, max_io, cur_cpu, cur_io;//Макс.чмсло рабочих потоко, Макс.число асинхронных потов ввода вывода, Количество доступных рабочих потоков, Количество доступных потоков асинхронного ввода-вывода.
            WaitObject obj = new WaitObject();//Создаём объет класса WaitObject указанного выше
            AutoResetEvent are = new AutoResetEvent(false); //Представляет событие синхронизации потоков,
                                                            //которое при срабатывании автоматически сбрасывается,
                                                            //освобождая один поток в состоянии ожидания. Этот класс не наследуется.

            const int wait = 10; //Время для введения исмвола S/U 
            char key; 
            Console.WriteLine("Для запуска потока в пуле нажмите S");
            Console.WriteLine("Для отмены ожидания новых потоков - U");
            Console.Write("Через {0} сек ожидание будет окончено", wait);


            /*
                Внимание, важно! Обект WaitHandle инкапсулирует связанные с операционной системой объекты,
                ожидающие монопольного доступа к общим ресурсам.
                Это значит синхронизацию доступа потоков к ресурсу.
            */

            obj.Handle = ThreadPool.RegisterWaitForSingleObject(are,
            WaitObject.CallbackMethod, obj, 10000 /*wait * 1000*/, false); /* Регистрирует делегат для ожидания объекта WaitHandle,
                                                                    задавая время ожидания в миллисекундах
                                                                    в виде 32-разрядного целого числа без знака.
                                                                    Присвоение переменной Handle
                                                                 */
            /*
                1) are - ОБЪЕКТ WaitHandle порождённый AutoResetEvent;
                2) WaitObject.CallbackMethod - метод который мы вызываем в потоке/потоках по средствам делегата WaitOrTimerCallback(здесь полное описание делегата опущено),
                3) obj - это объект передаваемый делегату WaitOrTimerCallback;
                4) wait * 1000 - это время ожидания в миллисекундах. 
                    Если параметр millisecondsTimeOutInterval равен 0 (нулю), функция проверяет состояние объекта и немедленно возвращает значение. 
                    Если параметр millisecondsTimeOutInterval равен -1, время ожидания функции никогда не истекает.
                    В целом нам необязвтельно записывать кострукцию вида wait * 1000, мы можем сразу утановить нужное значение к примеру 10000;
                5) false - последний аргумент. Значение true указывает, что после вызова делегата поток не будет ожидать параметр waitObject; 
                    значение false указывает, что таймер сбрасывается всякий раз по завершении операции ожидания до тех пор, 
                    пока регистрация ожидания не будет отменена.
            */

            /*
             * Цикл работает до тех пор пока obj.TimeOut = false.
             * Цикл производит опрос клавиатуры на ввод символов S/U
             */
            do
            {
                /* 
                 * Console.KeyAvailable используется для получения значения, показывающего, доступно ли нажатие клавиши во входном потоке. 
                 * Это свойство не блокирует ввод, пока не будет доступно нажатие клавиши
                 * Значение true, если нажатие клавиши доступно; в противном случае — значение false.
                 */
                if (Console.KeyAvailable) //Выполнить условие если введены входные данные.
                {
                    key = Console.ReadKey(true).KeyChar;//Заносит в переменную key символ юникода считанный с клавиатуры
                    if (key == 'S' || key == 's')
                    {
                        are.Set();//EventWaitHandle.Set Метод: public bool Set (); Данный метод вызывает сигнальное состояние события,
                                  //позволяя одному или нескольким ожидающим потокам продолжить.
                                  //Попросту говоря, запускает поток.

                    }
                    else if (key == 'U' || key == 'u') 
                    {
                        obj.Handle.Unregister(null);//Отменяет зарегистрированную операцию ожидания,
                                                    //вызванную методом RegisterWaitForSingleObject(WaitHandle, WaitOrTimerCallback, Object, UInt32, Boolean)
                                                    //Здесь как и ранее потоки попросту прерываются.
                        Console.WriteLine("\nОжидание отменено");
                        break;
                    }
                }
                else 
                {
                    Thread.Sleep(100);
                    //Если входных данных нет усыпить поток
                }
            } while (!obj.TimeOut);

            

            ThreadPool.GetMaxThreads(out max_cpu, out max_io);  /*
                                                                *Возвращает количество запросов к пулу потоков, которые могут быть активными одновременно. 
                                                                *Все запросы, превышающие это количество, остаются в очереди до тех пор, пока потоки пула не станут доступны.
                                                                *out max_cpu - Максимальное количество рабочих потоков в пуле потоков.
                                                                *out max_io - Максимальное количество потоков асинхронного ввода-вывода в пуле потоков.
                                                                */
            ThreadPool.GetAvailableThreads(out cur_cpu, out cur_io); /*
                                                                    *Возвращает разницу между максимальным числом потоков пула, возвращаемых методом GetMaxThreads(Int32, Int32), 
                                                                    *и числом активных в данный момент потоков.
                                                                    *out cur_cpu - Количество доступных рабочих потоков.
                                                                    *out cur_io - Количество доступных потоков асинхронного ввода-вывода.
                                                                    */

            if (cur_io != max_io)//Если колличество ассинхронных io не равно колличеству максимальных io
            {
                Console.WriteLine("\nПодождите, пока все потоки завершат свою работу");
                do
                {
                    Thread.Sleep(100);
                    ThreadPool.GetAvailableThreads(out cur_cpu, out
                    cur_io);
                } while (cur_io != max_io);
                Console.WriteLine("\nВсе потоки завершили свою работу");
            }
            Console.ReadKey(true);
            return 0;


            /*
            SemaPool = new Semaphore(0, 4);
            for (int i = 0; i < 4; i++)
            {
                ThreadPool.QueueUserWorkItem(TreadingsMetod, i+1);
            }
            for (int i = 0; i < 4; i++)
            {
                SemaPool.WaitOne();
            }
            Console.WriteLine("\nВсе потоки завершили свою работу");
            Console.ReadKey(true);
            return 0;
            */

            /*int min_cpu, min_io, max_cpu, max_io;
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
            return 0;*/

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
