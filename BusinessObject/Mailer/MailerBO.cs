using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Mailer
{
   [Serializable]
   [XmlRoot("Root")]
   public class MailDetailsBO
   {
       [XmlElement("CMH_PK")]
       public int CMH_PK { get; set; }
       [XmlElement("CMH_DATE")]
       public string CMH_DATE { get; set; }
       [XmlElement("CMH_SUBJECT")]
       public string CMH_SUBJECT { get; set; }
       [XmlElement("CMH_CONTENT")]
       public string CMH_CONTENT { get; set; }
       [XmlElement("CMH_STATUS")]
       public string CMH_STATUS { get; set; }
       [XmlElement("BIZUNIT_PK")]
       public int BIZUNIT_PK { get; set; }
       [XmlElement("ACTIVE")]
       public int ACTIVE { get; set; }
       [XmlElement("USER_PK")]
       public string USER_PK { get; set; }
       [XmlElement("LAST_MOD_DT")]
       public string LAST_MOD_DT { get; set; }
       [XmlElement("Detail")]
       public List<MailDetails> Detail { get; set; }
       [XmlElement("Email")]
       public List<CustomerBO> Email { get; set; }
   }
   [Serializable]
   public class MailDetails
   {
       [XmlElement("CMD_PK")]
       public int CMD_PK { get; set; }
       [XmlElement("CMD_CM")]
       public int CMD_CM { get; set; }
       [XmlElement("CMD_CUSTOMER")]
       public int CMD_CUSTOMER { get; set; }
   }

   [Serializable]
   [XmlRoot("Root")]
   public class CustomerBO
   {
       [XmlElement("CUS_PK")]
       public int CUS_PK { get; set; }
       [XmlElement("CUS_EMAIL")]
       public string CUS_EMAIL { get; set; }

   }

   [Serializable]
   public class MailAttachmentBO
   {
       [XmlElement("ADD_APP_TYPE")]
       public string ADD_APP_TYPE { get; set; }
       [XmlElement("ADD_APP_SUB_TYPE")]
       public int ADD_APP_SUB_TYPE { get; set; }
       [XmlElement("ADD_APP_PK")]
       public long ADD_APP_PK { get; set; }
       [XmlElement("ADD_SL_NO")]
       public int ADD_SL_NO { get; set; }
       [XmlElement("ADD_TITLE")]
       public string ADD_TITLE { get; set; }
       [XmlElement("ADD_NAME")]
       public string ADD_NAME { get; set; }
       [XmlElement("ADD_PATH")]
       public string ADD_PATH { get; set; }
       [XmlElement("ADD_TYPE")]
       public string ADD_TYPE { get; set; }
       [XmlElement("ADD_DESC")]
       public string ADD_DESC { get; set; }
       [XmlElement("USER_PK")]
       public int USER_PK { get; set; }
       [XmlElement("BIZUNIT")]
       public int BIZUNIT { get; set; }
       [XmlElement("ACTIVE")]
       public int ACTIVE { get; set; }
       [XmlElement("ADD_VERSION")]
       public int ADD_VERSION { get; set; }   
   }
}
