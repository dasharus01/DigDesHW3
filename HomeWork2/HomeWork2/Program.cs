using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp2
{
    internal class Program
    {

        // записываем имя файла к имеющемуся пути
        public static string ConvertPath(string path)
        {
            int posishionEnd = path.Length - 1;

            while (posishionEnd >= 0 && path[posishionEnd] != '/' && path[posishionEnd] != '\\')
                --posishionEnd;
            return path.Substring(0, posishionEnd + 1) + "result.txt";
        }

        static void Main(string[] args)
        {
            String line;
            //запрашиваем путь к файлу
            // D:\Project\C#\Sample.txt
            Console.Write("Введите путь к файлу с текстом: ");
            String file = Console.ReadLine();
            if (!File.Exists(file))
            {
                Console.WriteLine("Не удалось считать данные из файла");
                return;
            }
            //считываем данные из файла
            string[] data = File.ReadAllLines(file, Encoding.Default);
            Dictionary<string, int> word;
            Stopwatch stopWatch = new Stopwatch();
            //один поток
            var obj = typeof(word_count.word_count).GetMethod("process_count", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            stopWatch.Start();
            var result = obj?.Invoke(new word_count.word_count(), new object[] { data });
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine(String.Format("{0} мс",
                        ts.Milliseconds));
            //с использованием нескольких потоков
            stopWatch.Reset();
            obj = typeof(word_count.word_count).GetMethod("process_count_par", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            stopWatch.Start();
            result = obj?.Invoke(new word_count.word_count(), new object[] { data });
            stopWatch.Stop();
            TimeSpan ts1 = stopWatch.Elapsed;
            Console.WriteLine(String.Format("{0} мс",
            ts1.Milliseconds));
            


            if (result != null && result.GetType() == typeof(Dictionary<string, int>))
            {
                try
                {
                    //Открываем файл для записи

                    StreamWriter sw = new StreamWriter(ConvertPath(file));
                    // сортируем словарь по убыванию повторений
                    var sortdic = from pair in result as Dictionary<string, int>
                                  orderby pair.Value descending
                                  select pair;
                    // записываем в файл
                    foreach (KeyValuePair<string, int> valuePair in sortdic)
                    {
                        sw.WriteLine(valuePair.Key + "  " + valuePair.Value);
                    }

                    //закрываем файл
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Problem with type");
            }


        }
        }
    
}

