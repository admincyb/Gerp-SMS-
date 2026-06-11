using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject
{
    public class SmsBO
    {
    }
    public interface ISmsBO
    {       
        SmsList SmsList { get; set; }
    }   

    [Serializable]
    [XmlRoot("Root")]
    public class SmsHeader 
    {
        [XmlElement("SMS")]
        public SmsList SmsList { get; set; }       
    }

    public class SmsList
    {
        [XmlElement("Details")]
        public List<SmsDetailsBO> SmsDetails { get; set; }
    }

    [Serializable]
    public class SmsDetailsBO
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Enabled")]
        public int Enabled { get; set; }
        [XmlElement("Stage")]
        public int Stage { get; set; }
        [XmlElement("Template")]
        public string Template { get; set; }
    }
}
