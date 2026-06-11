using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class SlabDefinitionBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class SlabDefinition
        {
            [XmlElement("PHS_PK")]
            public int PHS_PK { get; set; }
            [XmlElement("PHS_CODE")]
            public string PHS_CODE { get; set; }
            [XmlElement("PHS_NAME")]
            public string PHS_NAME { get; set; }
            //[XmlElement("PHS_EFFECT_FROM")]
            //public DateTime PHS_EFFECT_FROM { get; set; }
            //[XmlElement("PHS_EFFECT_TO")]
            //public DateTime PHS_EFFECT_TO { get; set; }
            [XmlElement("PHS_PAY_ELEMENT")]
            public int PHS_PAY_ELEMENT { get; set; }
            [XmlElement("PHS_PAY_ELEMENT_TEXT")]
            public string PHS_PAY_ELEMENT_TEXT { get; set; }
            [XmlElement("PHS_BASED_ON")]
            public string PHS_BASED_ON { get; set; }
            [XmlElement("PHS_BASED_ON_TEXT")]
            public string PHS_BASED_ON_TEXT { get; set; }
            [XmlElement("PHS_ADD_AMT")]
            public decimal PHS_ADD_AMT { get; set; }
            [XmlElement("PHS_MIN_AMT")]
            public decimal PHS_MIN_AMT { get; set; }
            [XmlElement("PHS_MAX_AMT")]
            public decimal PHS_MAX_AMT { get; set; }
            [XmlElement("PHS_DESC")]
            public string PHS_DESC { get; set; }
            [XmlElement("PHS_IS_DIRECT")]
            public int PHS_IS_DIRECT { get; set; }
            [XmlElement("PHS_ACTIVE")]
            public int PHS_ACTIVE { get; set; }
            [XmlElement("PHS_COMPANY")]
            public int PHS_COMPANY { get; set; }
            [XmlElement("PHS_BIZUNIT")]
            public int PHS_BIZUNIT { get; set; }
            [XmlElement("PHS_DEPT")]
            public int PHS_DEPT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("Version")]
            public List<VersionDetail> VersionDetails { get; set; }
           
        }
        [Serializable]
        [XmlRoot("Version")]
        public class VersionDetail
        {
            [XmlElement("PEV_PK")]
            public int PEV_PK { get; set; }
            [XmlElement("PEV_SLAB_HDR")]
            public int PEV_SLAB_HDR { get; set; }
            [XmlElement("PEV_VERSION")]
            public int PEV_VERSION { get; set; }
            [XmlElement("PEV_EFFECT_FROM")]
            public DateTime PEV_EFFECT_FROM { get; set; }
            [XmlElement("PEV_EFFECT_TO")]
            public DateTime PEV_EFFECT_TO { get; set; }
            [XmlElement("PEV_DESC")]
            public string PEV_DESC { get; set; }
            [XmlElement("PEV_ACTIVE")]
            public int PEV_ACTIVE { get; set; }            
            [XmlElement("Details")]
            public List<SlabDefinitionDetail> SlabDefinitionDetails { get; set; }
        }   

        [Serializable]
        [XmlRoot("Details")]
        public class SlabDefinitionDetail
        {
            [XmlElement("PDS_PK")]
            public int PDS_PK { get; set; }
            [XmlElement("PDS_SLAB_HDR")]
            public int PDS_SLAB_HDR { get; set; }          
            [XmlElement("PDS_RANGE_FROM")]
            public decimal PDS_RANGE_FROM { get; set; }
            [XmlElement("PDS_RANGE_TO")]
            public decimal PDS_RANGE_TO { get; set; }
            [XmlElement("PDS_VALUE")]
            public string PDS_VALUE { get; set; }
            [XmlElement("PDS_VALUE_TEXT")]
            public string PDS_VALUE_TEXT { get; set; }  
        }   
        
    }
}
