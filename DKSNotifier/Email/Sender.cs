using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DKSNotifier.Logs;

namespace DKSNotifier.Email
{
    /// <summary>
    /// Почтовый клиент
    /// </summary>
    internal class Sender
    {
        private readonly string server;
        private readonly int port;
        private readonly string from;
        private readonly string to;
        private readonly Log log;
        private readonly string subject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="subject"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="log"></param>
        public Sender(string server, int port, string subject, string from, string to, Log log)
        {
            this.server = server;
            this.port = port;
            this.subject = subject;
            this.from = from;
            this.to = to;
            this.log = log;
        }

        /// <summary>
        /// Направить письмо
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            try
            {                
                log.Info("Направление почты по адресу: " + to);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(server, port);
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                log.Error("Ошибка отправки почты! " + e.Message);
            }            
        }

    }
}
