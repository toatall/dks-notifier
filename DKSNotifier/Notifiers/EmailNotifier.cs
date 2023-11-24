using System;
using System.Net.Mail;
using DKSNotifier.Logs;

namespace DKSNotifier.Notifiers
{
    /// <summary>
    /// Уведомление по почте
    /// </summary>
    internal class EmailNotifier: INotifier
    {
        #region Поля

        /// <summary>
        /// Адрес сервера
        /// </summary>
        private readonly string server;

        /// <summary>
        /// Порт сервера
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Адрес отпавителя
        /// </summary>
        private readonly string from;

        /// <summary>
        /// Адрес(а) получателя(ей)
        /// </summary>
        private readonly string to;

        /// <summary>
        /// Тема сообщения
        /// </summary>
        private readonly string subject;

        /// <summary>
        /// Логирование
        /// </summary>
        private readonly Log log;

        #endregion

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="server">адрес сервера</param>
        /// <param name="port">порт сервера</param>
        /// <param name="subject">тема</param>
        /// <param name="from">адрес отправителя</param>
        /// <param name="to">адрес(а) получателя(ей)</param>
        /// <param name="log">лог</param>
        public EmailNotifier(string server, int port, string subject, string from, string to, Log log)
        {
            this.server = server;
            this.port = port;
            this.subject = subject;
            this.from = from;
            this.to = to;
            this.log = log;
        }

        /// <summary>
        /// Направление письма
        /// </summary>
        /// <inheritdoc/>
        /// <param name="text">текст письма</param>
        public void Exec(string text)
        {
            try
            {
                log.Info("Направление почты по адресу: " + to);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.Subject = subject;
                mailMessage.Body = text;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(server, port);
                smtpClient.Send(mailMessage);
                log.Info("Почта отправлена!");
            }
            catch (Exception e)
            {
                log.Error("Ошибка отправки почты! " + e.Message);
            }
        }       

    }
}
