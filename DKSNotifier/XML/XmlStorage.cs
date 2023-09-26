using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DKSNotifier.Logs;


namespace DKSNotifier.XML
{
    /// <summary>
    /// Работа с xml-файлом
    /// </summary>
    internal class XmlStorage
    {
        private readonly XDocument doc;
        private readonly string xmlFile;
        private readonly Log log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFile"></param>
        public XmlStorage(string xmlFile, Log log)
        {
            this.xmlFile = xmlFile;
            this.log = log;
            if (File.Exists(xmlFile))
            {
                doc = XDocument.Load(xmlFile);
                doc.Element("root");
            }
            else
            {
                doc = new XDocument();
                if (doc.Element("root") == null)
                {
                    doc.Add(new XElement("root"));
                }
                doc.Element("root");
                doc.Save(xmlFile);
            }                                              
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool CheckEntity(string type, string unid)
        {
            if (doc.Root == null)
            {
                return false;
            }
            var res = doc.Root.Elements("Node")
                .FirstOrDefault(t => t.Attribute("Type").Value == type & t.Attribute("Unid").Value == unid);
            return res != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unid"></param>
        /// <param name="tabNum"></param>
        /// <param name="fio"></param>
        /// <param name="description"></param>
        public void Add(string type, string unid, string tabNum, string login, string fio, string description = "")
        {
            if (doc.Root != null)
            {
                log.Info(string.Format("Сохранение записи в xml-файл: тип={0}, табельный номер={1}, УЗ={2}, ФИО={3}", type, tabNum, login, fio));
                doc.Root.Add(new XElement("Node",
                    new XAttribute("Type", type),
                    new XAttribute("Unid", unid),
                    new XAttribute("TabNum", tabNum),
                    new XAttribute("Login", login),
                    new XAttribute("Fio", fio),
                    new XAttribute("Description", description)
                ));
                doc.Save(xmlFile);
            }
        }

    }
}
