using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class DashletUserMappingBO
    {
    }


    [Serializable]
    [XmlRoot("Root")]
    public class DashletUserMappingHeader
    {
        [XmlElement("DLC_PK")]
        public int DLC_PK { get; set; }
        [XmlElement("Details")]
        public List<DashletUserMappingDetails> DashletUserMappingDetails { get; set; }
    }

    [Serializable]
    [XmlRoot("Details")]
    public class DashletUserMappingDetails
    {
        [XmlElement("DLM_PK")]
        public int DLM_PK { get; set; }
        [XmlElement("DLM_DASHLET")]
        public int DLM_DASHLET { get; set; }
        [XmlElement("DLM_USER_GROUP")]
        public int DLM_USER_GROUP { get; set; }
        [XmlElement("DLM_USER_GROUP_TEXT")]
        public string DLM_USER_GROUP_TEXT { get; set; }
        [XmlElement("DLM_VIEW")]
        public int DLM_VIEW { get; set; }
        [XmlElement("DLM_ACTION")]
        public int DLM_ACTION { get; set; }
    }

}
