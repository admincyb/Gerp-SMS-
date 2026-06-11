using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
   public class PayrollTypeMasterBO
    {
       [Serializable]
       [XmlRoot("Root")]
       public class PayrollType
       {
           [XmlElement("PTM_PK")]
           public int PTM_PK { get; set; }
           [XmlElement("PTM_CODE")]
           public string PTM_CODE { get; set; }
           [XmlElement("PTM_NAME")]
           public string PTM_NAME { get; set; }
           [XmlElement("PTM_DESC")]
           public string PTM_DESC { get; set; }
           [XmlElement("PTM_ACTIVE")]
           public int PTM_ACTIVE { get; set; }
           [XmlElement("PTM_BIZUNIT")]
           public int PTM_BIZUNIT { get; set; }
           [XmlElement("USER_PK")]
           public int USER_PK { get; set; }
           [XmlElement("PTM_PRC_MODE")]
           public Byte? PTM_PRC_MODE { get; set; }
           [XmlElement("PTM_PRC_MODE_TEXT")]
           public string PTM_PRC_MODE_TEXT { get; set; }
           [XmlElement("PTM_PAYRL_START")]
           public int PTM_PAYRL_START { get; set; }
           [XmlElement("PTM_OFFSET")]
           public int PTM_OFFSET { get; set; }        
           [XmlElement("LAST_MOD_DT")]
           public DateTime LAST_MOD_DT { get; set; }
           [XmlElement("Details")]
           public List<TreeUserDetails> UsrDetailsList { get; set; }
       }

       //[Serializable]
       //public class PayrollTreeData
       //{
       //    [XmlElement("PUM_USER_PK")]
       //    public int PUM_USER_PK { get; set; }
       //    [XmlElement("PUM_PK")]
       //    public int PUM_PK { get; set; }
       //    [XmlElement("PUM_USER_NAME")]
       //    public string PUM_USER_NAME { get; set; }
       //    [XmlElement("IS_MAP_FL")]
       //    public int IS_MAP_FL { get; set; }
       //    [XmlElement("Details")]
       //    public List<TreeDetails> UsrDetails { get; set; }
       //}

       [Serializable]
       [XmlRoot("Details")]
       public class TreeUserDetails
       {
           [XmlElement("PUM_USER_PK")]
           public int PUM_USER_PK { get; set; }
           [XmlElement("PUM_PK")]
           public int PUM_PK { get; set; }
           [XmlElement("PUM_USER_NAME")]
           public string PUM_USER_NAME { get; set; }
           [XmlElement("IS_MAP_FL")]
           public int IS_MAP_FL { get; set; }
       }
    }
}
