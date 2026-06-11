using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject
{
    public class MailBO
    {
    }
    public interface IMailBO
    {       
        MailList MailList { get; set; }
    }   

    [Serializable]
    [XmlRoot("Root")]
    public class MailHeader 
    {
        [XmlElement("EMAIL")]
        public MailList MailList { get; set; }       
    }

    public class MailList
    {
        [XmlElement("Details")]
        public List<MailDetailBO> MailDetails { get; set; }
    }

    [Serializable]
    public class MailDetailBO
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Enabled")]
        public int Enabled { get; set; }
        [XmlElement("Stage")]
        public int Stage { get; set; }
        [XmlElement("Subject")]
        public string Subject { get; set; }
        [XmlElement("Template")]
        public string Template { get; set; }
    }
}
