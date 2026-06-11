using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class CostCenterMasterBO
    {
        public int CNM_PK { get; set; }
        public string CNM_CODE { get; set; }
        public string CNM_NAME { get; set; }
        public string CNM_DESC { get; set; }
        public int CNM_GROUP { get; set; }
        public int CNM_DEPT { get; set; }
        public int CNM_DEPARMENT { get; set; }
        public int CNM_SUBDEPARMENT { get; set; }
        public int CNM_BIZUNIT { get; set; }
        public int CNM_COMPANY { get; set; }
        public int CNM_ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ActivityMasterBO
    {
        [XmlElement("EAM_PK")]
        public int EAM_PK { get; set; } = 0;

        [XmlElement("EAM_CODE")]
        public string EAM_CODE { get; set; }

        [XmlElement("EAM_NAME")]
        public string EAM_NAME { get; set; }

        [XmlElement("EAM_DESC")]
        public string EAM_DESC { get; set; }

        [XmlElement("EAM_ACTIVE")]
        public int EAM_ACTIVE { get; set; }

        [XmlElement("EAM_MAIN_ACTIVITY")]
        public int EAM_MAIN_ACTIVITY { get; set; }

        [XmlElement("EAM_MAIN_ACTIVITY_PK")]
        public int EAM_MAIN_ACTIVITY_PK { get; set; }

        [XmlElement("EAM_BIZUNIT")]
        public int EAM_BIZUNIT { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("MapDetails")]
        public List<MapDetails> MapDetails { get; set; }
    }

    public class MapDetails
    {
        [XmlElement("SL_NO")]
        public int SL_NO { get; set; }

        [XmlElement("ACM_ACTIVITY")]
        public int EAM_PK { get; set; }

        [XmlElement("ACM_DEPT_PK")]
        public int ACM_DEPT_PK { get; set; }

        [XmlElement("ACM_DEPT_TEXT")]
        public string ACM_DEPT_TEXT { get; set; }

        [XmlElement("ACM_TEAM_PK")]
        public int ACM_TEAM_PK { get; set; }

        [XmlElement("ACM_TEAM_TEXT")]
        public string ACM_TEAM_TEXT { get; set; }

        [XmlElement("ACM_COMPANY_PK")]
        public int ACM_COMPANY_PK { get; set; }

        [XmlElement("ACM_COMPANY_TEXT")]
        public string ACM_COMPANY_TEXT { get; set; }

        //[XmlElement("ACM_GROUP_PK")]
        //public int ACM_GROUP_PK { get; set; }

        //[XmlElement("ACM_GROUP_TEXT")]
        //public string ACM_GROUP_TEXT { get; set; }

    }

    //[XmlElement("ACM_ACTIVE")]
    //public int EAM_ACTIVE { get; set; }

    //[XmlElement("ACM_COST_CENTER")]
    //public string EAM_CODE { get; set; }

    //[XmlElement("ACM_DEPT")]
    //public string EAM_NAME { get; set; }

    //[XmlElement("ACM_COMPANY")]
    //public string EAM_DESC { get; set; }
}
