using System;
using System.IO;

namespace DKSNotifier.Logs
{
    /// <summary>
    /// Логирование в файл
    /// Файлы сохраняются в каталог logs с именами YYYY_MM_DD.log
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// Получение пути к файлу
        /// Проверка/создание каталога logs
        /// </summary>
        /// <returns></returns>
        protected string GetFileName()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(basePath + "logs"))
            {
                Directory.CreateDirectory(basePath + "logs");
            }
            return basePath + String.Format("logs\\{0:yyyy_MM_dd}.log", DateTime.Now); 
        }

        /// <summary>
        /// Логирование информационного сообщения
        /// </summary>
        /// <param name="text"></param>
        public void Info(string text)
        {
            Line(text, "информация", true);
        }

        /// <summary>
        /// Логирование сообщения с ошибкой 
        /// </summary>
        /// <param name="text"></param>
        public void Error(string text)
        {
            Line(text, "ошибка", true);
        }

        /// <summary>
        /// Добавление пустых строк (например, в начало или конец файла)
        /// </summary>
        /// <param name="rows"></param>
        public void EmptyLines(int rows = 1)
        {
            for (int i = 0; i < rows; i++)
            {
                Line("");
            }            
        }

        /// <summary>
        /// Сохранение строки в лог-файл
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="type">вид сообщения (информация, ошибка)</param>
        /// <param name="includeDate">включать текущую дату</param>
        protected void Line(string text, string type = "", bool includeDate = false)
        {
            string textDate = includeDate ? DateTime.Now.ToString("[dd.MM.yyyy HH:mm:ss] ") : "";
            string textType = String.IsNullOrEmpty(type) ? "" : "[" + type + "] ";
            string line = textDate + textType + text;
            File.AppendAllText(GetFileName(), line + "\n");
            Console.WriteLine(line);
        }

    }
}
