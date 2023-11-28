using DKSNotifier.Logs;
using System;
using System.IO;

namespace DKSNotifier.Notifiers
{
    /// <summary>
    /// Уведомление посредством создания html-файла
    /// </summary>
    internal class HtmlNotifier : INotifier
    {
        #region Поля

        /// <summary>
        /// Путь к html-файлу
        /// </summary>
        private readonly string filename;

        /// <summary>
        /// Лог
        /// </summary>
        private readonly Log log;

        #endregion

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="filename">имя файла</param>
        /// <param name="log">лог</param>
        public HtmlNotifier(string filename, Log log)
        {
            this.filename = filename;
            this.log = log;
        }

        /// <summary>
        /// Генерирование и сохранение html-файла
        /// </summary>
        /// <inheritdoc/>
        /// <param name="text"></param>
        public void Exec(string text)
        {
            string style = @"
                        <style>
                            body { font-family: system-ui, -apple-system, ""Segoe UI"", Roboto, ""Helvetica Neue"", ""Noto Sans"", ""Liberation Sans"", Arial } 
                            table { caption-side: bottom; border-collapse: collapse; margin-bottom: 100px; width: 100%; }
                            table td, table th { border: 1px solid #aaa; padding: 5px; }
                            h1 { text-align: center; }
                        </style>";
            string title = "От " + DateTime.Now.ToShortDateString();
            string html = string.Format("<html><head>{0}</head><h1>{1}</h1><hr /><body>{2}</body></html>", style, title, text);

            try
            {
                log.Info("Запись информации в файл " + this.filename);
                File.WriteAllText(this.filename, html);
                log.Info("Запись в файл " + this.filename + " завершена");
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Ошибка записи в файл {0}. {1}", this.filename, ex.Message));
            }
        }
    }
}
