using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DKSNotifier.Logs
{
    internal class Log
    {        
        protected string GetFileName()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(basePath + "logs"))
            {
                Directory.CreateDirectory(basePath + "logs");
            }
            return basePath + String.Format("logs\\{0:yyyy_MM_dd}.log", DateTime.Now); 
        }

        public void Info(string text)
        {
            Line(text, "информация", true);
        }

        public void Error(string text)
        {
            Line(text, "ошибка", true);
        }

        public void EmptyLines(int rows = 1)
        {
            for (int i = 0; i < rows; i++)
            {
                Line("");
            }            
        }

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
