using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace word_count
{
    public class word_count
    {
        // функция для проверки краев слова на знаки припенания и прочие
        // ' оставлено, так как есть фран.язык
        private static String remove_start_end(String test)
        {
            if (test.Length == 0)
            {
                return "";
            }
            int start = 0;
            int end = test.Length - 1;
            if (test[start] != '\'' && !Char.IsDigit(test[start]) && !Char.IsLetter(test[start]))
            {
                start++;
            }
            if (test[end] != '\'' && !Char.IsDigit(test[end]) && !Char.IsLetter(test[end]))
            {
                end--;
            }
            if (start > end)
                return "";
            else
                return test.Substring(start, end - start + 1);
        }
        // функция для удаления дефиса из конца слова
        // а так же не считывания --
        private static string parsDef(string test)
        {
            int posishionEnd = test.Length - 1;
            while (posishionEnd >= 0 && test[posishionEnd] == '-')
                --posishionEnd;
            if (posishionEnd != test.Length - 1)
                test = test.Substring(0, posishionEnd + 1);
            return test;
        }

        private Dictionary<string, int> process_count(String[] data)
        {
            //String line;
            // объявляем словарь, в котором ключ - это слово, значение - это кол-во повторений
            Dictionary<string, int> word = new Dictionary<string, int>();

            foreach (String line in data)
            {
                read_string(line, word);
            }


            return word;
        }
        public Dictionary<string, int> process_count_par(String[] data)
        {
            // объявляем словарь, в котором ключ - это слово, значение - это кол-во повторений
            ConcurrentDictionary<string, int> word = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(data, line => {
                read_string(line, word);
            });
            return word.ToDictionary(entry => entry.Key, entry => entry.Value);
        }


        private static void read_string(String line, Dictionary<string, int> word)
        {
            // строка для записи слов
            string test = "";
            // символ для проверки
            char a;
            //начало слова
            int start = 0;
            //идем по всё строке
            for (int i = 0; i < line.Length; i++)
            {
                //берем символ
                a = line[i];

                //учитываем только буквы, цифры и знаки, которые могут встретиться в рус. и фр. грамоте
                if (Char.IsLetter(a) || Char.IsDigit(a) || a == '-' || a == '\'')
                {

                }
                // если столкнулись со знаком
                else
                {
                    //проверка на наличие слова
                    if (i - start > 1)
                    {
                        //запись слова и опускание регистра и пробелов
                        test = line.Substring(start, i - start + 1).ToLower().Replace(" ", "");
                        //проверка на --, так как это диалоговые тире
                        test = parsDef(test);
                        //"уборка" боковых ненужных знаков
                        test = remove_start_end(test);
                        //если не пусто записываем в словарь
                        if (test != "")
                        {
                            if (word.ContainsKey(test))
                                ++word[test];
                            else
                                //если впервые слово в словаре, то создаем и кол-во приравниваем к 1
                                word.Add(test.ToLower(), 1);
                        }

                    }
                    start = i;
                }

            }
            // запись в словарь последнего слова в документе
            if (line.Length - 1 - start > 1)
            {
                test = line.Substring(start, line.Length - 1 - start + 1).ToLower().Replace(" ", "");
                test = parsDef(test);
                test = remove_start_end(test);
                if (test != "")
                {
                    if (word.ContainsKey(test))
                        ++word[test];
                    else
                        word.Add(test.ToLower(), 1);
                }
            }

        }

        
    private void read_string(String line, ConcurrentDictionary<string, int> word)
        {

            // строка для записи слов
            string test = "";
            // символ для проверки
            char a;
            //начало слова
            int start = 0;
            //идем по всё строке
            for (int i = 0; i < line.Length; i++)
            {
                //берем символ
                a = line[i];

                //учитываем только буквы, цифры и знаки, которые могут встретиться в рус. и фр. грамоте
                if (Char.IsLetter(a) || Char.IsDigit(a) || a == '-' || a == '\'')
                {

                }
                // если столкнулись со знаком
                else
                {
                    //проверка на наличие слова
                    if (i - start > 1)
                    {
                        //запись слова и опускание регистра и пробелов
                        test = line.Substring(start, i - start + 1).ToLower().Replace(" ", "");
                        //проверка на --, так как это диалоговые тире
                        test = parsDef(test);
                        //"уборка" боковых ненужных знаков
                        test = remove_start_end(test);
                        //если не пусто записываем в словарь
                        if (test != "")
                        {
                            if (word.ContainsKey(test))
                                ++word[test];
                            else
                                //если впервые слово в словаре, то создаем и кол-во приравниваем к 1
                                word.TryAdd(test.ToLower(), 1);
                        }

                    }
                    start = i;
                }

            }
            // запись в словарь последнего слова в документе
            if (line.Length - 1 - start > 1)
            {
                test = line.Substring(start, line.Length - 1 - start + 1).ToLower().Replace(" ", "");
                test = parsDef(test);
                test = remove_start_end(test);
                if (test != "")
                {
                    if (word.ContainsKey(test))
                        ++word[test];
                    else
                        word.TryAdd(test.ToLower(), 1);
                }
            }
        }
    }
}