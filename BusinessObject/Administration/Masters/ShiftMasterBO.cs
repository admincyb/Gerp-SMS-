using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class ShiftMasterBO
    {
        public enum ControlsEnum
        {
            SHIFT
        }

        [Serializable]
        [XmlRoot("Root")]
        public class ShiftType
        {
            [XmlElement("SHF_PK")]
            public int SHF_PK { get; set; }
            [XmlElement("SHF_CODE")]
            public string SHF_CODE { get; set; }
            [XmlElement("SHF_NAME")]
            public string SHF_NAME { get; set; }
            [XmlElement("SHF_DESC")]
            public string SHF_DESC { get; set; }
            [XmlElement("SHF_TIME_FROM")]
            public string SHF_TIME_FROM { get; set; }
            [XmlElement("SHF_TIME_TO")]
             
            public string SHF_TIME_TO { get; set; }
            [XmlElement("SHF_ACTIVE")]
             public int SHF_ACTIVE { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public int LAST_MOD_DT { get; set; }
            [XmlElement("SHF_CODE_NAME_TEXT")]
            public int SHF_CODE_NAME_TEXT { get; set; }
            [XmlElement("SHF_SEQUENCE")]
            public int SHF_SEQUENCE { get; set; }
           
        }
    }

    public class ShiftHeader
    { 
        public int SHF_PK { get; set; }
        public string SHF_CODE { get; set; }
        public string SHF_NAME { get; set; }
        public string SHF_DESC { get; set; }
        public string SHF_TIME_FROM { get; set; }
        public string SHF_TIME_TO { get; set; }
        public int ACTIVE { get; set; }
        public int BIZUNIT_PK { get; set; }
        public string SHF_CRTD_BY { get; set; }
        public string SHF_MOD_BY { get; set; }
        public int SHF_ACTIVE { get; set; }
        public int SHF_BIZUNIT { get; set; }
        public int SHF_IS_PRODUCTION { get; set; }
        public string SHF_SEQUENCE { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
    }

    
}
