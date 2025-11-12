using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ИСИП523_Андреева1
{
    public static class TextOperations
    {
        // проверка, на гласную букву
        public static bool IsVowel(char c)
        {
            char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
                            'a', 'e', 'i', 'o', 'u', 'y' };
            char lowerC = char.ToLower(c);

            for (int i = 0; i < vowels.Length; i++)
            {
                if (vowels[i] == lowerC)
                    return true;
            }
            return false;
        }

        // проверка, на согласную букву 
        public static bool IsConsonant(char c)
        {
            char[] consonants = { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м',
                                'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
                                'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm',
                                'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };
            char lowerC = char.ToLower(c);

            for (int i = 0; i < consonants.Length; i++)
            {
                if (consonants[i] == lowerC)
                    return true;
            }
            return false;
        }

        // проверка, является ли символ буквой
        public static bool IsLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') ||
                   (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
                   c == 'ё' || c == 'Ё';
        }
    }

    // хранение статистики по тексту
    public class TextStatistics
    {
        public int TextNumber { get; set; }
        public string Preview { get; set; } // Первые 50 символов текста для идентификации
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }

        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }

        public void DisplayStatistics()
        {
            Console.WriteLine($"\n=== СТАТИСТИКА ТЕКСТА #{TextNumber} ===");
            Console.WriteLine($"Предпросмотр: {Preview}...");
            Console.WriteLine($"Количество слов: {WordCount}");
            Console.WriteLine($"Количество предложений: {SentenceCount}");
            Console.WriteLine($"Самое короткое слово: '{ShortestWord}'");
            Console.WriteLine($"Самое длинное слово: '{LongestWord}'");
            Console.WriteLine($"Гласные буквы: {VowelCount}");
            Console.WriteLine($"Согласные буквы: {ConsonantCount}");

            Console.WriteLine("Частота букв:");
            foreach (var pair in LetterFrequency)
            {
                Console.WriteLine($"  {pair.Key}: {pair.Value}");
            }
        }
    }

    // Основной класс приложения
    class Program
    {
        private static List<TextStatistics> allStatistics = new List<TextStatistics>();
        private static int textCounter = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== АНАЛИЗАТОР ТЕКСТА ===");

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AnalyzeNewText();
                        break;
                    case "2":
                        ShowAllStatistics();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine($"Всего проанализировано текстов: {textCounter}");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine($"Проанализировано текстов: {textCounter}");
            Console.WriteLine("1. Анализ нового текста");
            Console.WriteLine("2. Показать статистику по всем текстам");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }

        static void AnalyzeNewText()
        {
            Console.WriteLine("\n=== АНАЛИЗ НОВОГО ТЕКСТА ===");

            string text;
            while (true)
            {
                Console.WriteLine("Введите текст (минимум 100 символов):");
                text = Console.ReadLine();

                if (text.Length >= 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Ошибка: текст содержит только {text.Length} символов. Нужно минимум 100!");
                }
            }

            TextStatistics stats = AnalyzeText(text);
            allStatistics.Add(stats);

            Console.WriteLine("\nАнализ завершен!");
            stats.DisplayStatistics();
        }

        static TextStatistics AnalyzeText(string text)
        {
            textCounter++;
            TextStatistics stats = new TextStatistics
            {
                TextNumber = textCounter,
                Preview = text.Length > 50 ? text.Substring(0, 50) : text
            };

            // Подсчет слов и поиск самого короткого/длинного слова
            string[] words = SplitIntoWords(text);
            stats.WordCount = words.Length;

            if (words.Length > 0)
            {
                stats.ShortestWord = words[0];
                stats.LongestWord = words[0];

                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length < stats.ShortestWord.Length)
                        stats.ShortestWord = words[i];

                    if (words[i].Length > stats.LongestWord.Length)
                        stats.LongestWord = words[i];
                }
            }

            // Подсчет предложений
            stats.SentenceCount = CountSentences(text);

            // Подсчет гласных, согласных и частоты букв
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (TextOperations.IsVowel(c))
                    stats.VowelCount++;
                else if (TextOperations.IsConsonant(c))
                    stats.ConsonantCount++;

                // Статистика по частоте букв
                if (TextOperations.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);
                    if (stats.LetterFrequency.ContainsKey(lowerC))
                    {
                        stats.LetterFrequency[lowerC]++;
                    }
                    else
                    {
                        stats.LetterFrequency[lowerC] = 1;
                    }
                }
            }

            return stats;
        }

        // Метод для разделения текста на слова 
        static string[] SplitIntoWords(string text)
        {
            List<string> words = new List<string>();
            StringBuilder currentWord = new StringBuilder();
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}',
                                '\t', '\n', '\r', '\"', '\'' };

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                bool isSeparator = false;

                // Проверяем, является ли символ разделителем
                for (int j = 0; j < separators.Length; j++)
                {
                    if (separators[j] == c)
                    {
                        isSeparator = true;
                        break;
                    }
                }

                if (isSeparator)
                {
                    if (currentWord.Length > 0)
                    {
                        words.Add(currentWord.ToString());
                        currentWord.Clear();
                    }
                }
                else
                {
                    currentWord.Append(c);
                }
            }

            // Добавляем последнее слово, если оно есть
            if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
            }

            return words.ToArray();
        }

        // Метод для подсчета предложений
        static int CountSentences(string text)
        {
            int count = 0;
            bool inSentence = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsLetter(c) || char.IsDigit(c))
                {
                    if (!inSentence)
                    {
                        inSentence = true;
                    }
                }
                else if (c == '.' || c == '!' || c == '?' || c == '…')
                {
                    if (inSentence)
                    {
                        count++;
                        inSentence = false;
                    }
                }
            }

            // Если текст заканчивается без знака препинания
            if (inSentence)
            {
                count++;
            }

            return count;
        }

        static void ShowAllStatistics()
        {
            Console.WriteLine("\n=== СТАТИСТИКА ПО ВСЕМ ТЕКСТАМ ===");

            if (allStatistics.Count == 0)
            {
                Console.WriteLine("Статистика отсутствует. Сначала проанализируйте тексты.");
                return;
            }

            // Общая статистика
            Console.WriteLine($"Всего проанализировано текстов: {allStatistics.Count}");

            int totalWords = 0;
            int totalSentences = 0;
            int totalVowels = 0;
            int totalConsonants = 0;

            for (int i = 0; i < allStatistics.Count; i++)
            {
                totalWords += allStatistics[i].WordCount;
                totalSentences += allStatistics[i].SentenceCount;
                totalVowels += allStatistics[i].VowelCount;
                totalConsonants += allStatistics[i].ConsonantCount;
            }

            Console.WriteLine($"\nОБЩАЯ СТАТИСТИКА:");
            Console.WriteLine($"Всего слов: {totalWords}");
            Console.WriteLine($"Всего предложений: {totalSentences}");
            Console.WriteLine($"Всего гласных: {totalVowels}");
            Console.WriteLine($"Всего согласных: {totalConsonants}");

            // Статистика по каждому тексту
            Console.WriteLine("\nДЕТАЛЬНАЯ СТАТИСТИКА:");
            for (int i = 0; i < allStatistics.Count; i++)
            {
                allStatistics[i].DisplayStatistics();
                if (i < allStatistics.Count - 1)
                {
                    Console.WriteLine("---");
                }
            }
        }
    }
}

