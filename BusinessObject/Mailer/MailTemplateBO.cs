using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Mailer
{
   [Serializable]   
    public class MailTemplateBO
   {
       [XmlElement("TML_PK")]
       public int TML_PK { get; set; }
       [XmlElement("TML_APP_TYPE")]
       public int TML_APP_TYPE { get; set; }
       [XmlElement("TML_APP_SUB_TYPE")]
       public int TML_APP_SUB_TYPE { get; set; }
       [XmlElement("TML_TYPE")]
       public short TML_TYPE { get; set; }
       [XmlElement("TML_ACTION")]
       public int TML_ACTION { get; set; }
       [XmlElement("TML_NAME")]
       public string TML_NAME { get; set; }
       [XmlElement("TML_TEMPLATE")]
       public string TML_TEMPLATE { get; set; }
       [XmlElement("ACTIVE")]
       public short ACTIVE { get; set; }
       [XmlElement("CRTD_BY")]
       public int CRTD_BY { get; set; }
       [XmlElement("LAST_MOD_DT")]
       public DateTime LAST_MOD_DT { get; set; }
       [XmlElement("TML_TO_CC")]
       public string TML_TO_CC { get; set; }
       [XmlElement("TML_TO_BCC")]
       public string TML_TO_BCC { get; set; }
       [XmlElement("TML_FROM")]
       public string TML_FROM { get; set; }
       [XmlElement("TML_IS_EDIT")]
       public short TML_IS_EDIT { get; set; }
       [XmlElement("TML_NAME2")]
       public string TML_NAME2 { get; set; }
       [XmlElement("TML_MOD_BY")]
       public int TML_MOD_BY { get; set; }      
       
   }
   [Serializable]
   [XmlRoot("Root")]
   public class TemplateParameterBO
   {      
       [XmlElement("Detail")]
       public List<ParameterDetails> Detail { get; set; }
   }
   [Serializable]  
   public class ParameterDetails
   {
       [XmlElement("TMT_TEMPLATE")]
       public int TMT_TEMPLATE { get; set; }
       [XmlElement("TMT_TAG")]
       public string TMT_TAG { get; set; }
       [XmlElement("TMT_DESC")]
       public string TMT_DESC { get; set; }
       [XmlElement("TMT_STATUS")]
       public short TMT_STATUS { get; set; }
       [XmlElement("TMT_ACTIVE")]
       public short TMT_ACTIVE { get; set; }

   }
  
}
