using System.IO;
using System.Linq;
using System.Xml.Linq;
using DKSNotifier.Logs;


namespace DKSNotifier.Storage
{
    /// <inheritdoc/>
    /// <summary>
    /// Сохранение информации в xml-файле
    /// </summary>
    internal class XmlStorage: IStorage
    {
        /// <summary>
        /// Объект xml документа
        /// </summary>
        private readonly XDocument doc;

        /// <summary>
        /// Путь к xml-файлу
        /// </summary>
        private readonly string xmlFile;

        /// <summary>
        /// Объект лога
        /// </summary>
        private readonly Log log;

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="xmlFile">путь к файлу</param>
        /// <param name="log">лог</param>
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

        /// <inheritdoc />
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
        
        /// <inheritdoc />
        public void Add(string type, string unid, string description)
        {
            if (doc.Root != null)
            {
                log.Info(string.Format("Сохранение записи в xml-файл: тип={0}, идентификатор={1}, описание={2}", type, unid, description));
                doc.Root.Add(new XElement("Node",
                    new XAttribute("Type", type),
                    new XAttribute("Unid", unid),                  
                    new XAttribute("Description", description)
                ));
                doc.Save(xmlFile);
            }
        }

    }
}
