using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
  public   class AccountMapingBO
    {

    }
    
    [Serializable]
    [XmlRoot("Root")]
      public class SOAccountMappingHeader
    {
        [XmlElement("CKH_PK")]
        public int CKH_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }


        [XmlElement("Detail")]
        public List<SOAccountMappingDetails> Detail { get; set; }
        
    }
     [Serializable]

    public class SOAccountMappingDetails
    {
        [XmlElement("CKD_PK")]
        public string  CID_PK { get; set; }
        [XmlElement("CKD_FROM_ACCOUNT")]
        public int CKD_FROM_ACCOUNT { get; set; }
        [XmlElement("CKD_TO_ACCOUNT")]
        public int CKD_TO_ACCOUNT { get; set; }
        [XmlElement("CKD_ACTIVE")]
        public int CKD_ACTIVE { get; set; }
    }
}
