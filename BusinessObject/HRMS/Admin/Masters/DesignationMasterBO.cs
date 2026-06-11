using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class DesignationMasterBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class DesignationType
        {
            [XmlElement("dsgPK")]
            public int dsgPK { get; set; }
            [XmlElement("dsgCode")]
            public string dsgCode { get; set; }
            [XmlElement("dsgName")]
            public string dsgName { get; set; }
            [XmlElement("dsgDesc")]
            public string dsgDesc { get; set; }
            [XmlElement("dsgActive")]
            public int dsgActive { get; set; }
            [XmlElement("dsgBizUnit")]
            public int dsgBizUnit { get; set; }
            [XmlElement("dsgJobLevel")]
            public string dsgJobLevel { get; set; }
            [XmlElement("dsgJobCategory")]
            public string dsgJobCategory { get; set; }
            //[XmlElement("dsgCrtdBy")]
            //public int dsgCrtdBy { get; set; }
            [XmlElement("DSG_DEPT")]
            public int DSG_DEPT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }

            [XmlElement("Details")]
            public List<TreeDocDetails> DocTreeDetailsList { get; set; }
        }

        [Serializable]
        [XmlRoot("Details")]
        public class TreeDocDetails
        {
            [XmlElement("DDM_PK")]
            public int DDM_PK { get; set; }
            [XmlElement("DDM_DESIGNATION")]
            public int DDM_DESIGNATION { get; set; }
            [XmlElement("DDM_DOC_TYPE")]
            public int DDM_DOC_TYPE { get; set; }
            [XmlElement("DDM_DOC_TYPE_TEXT")]
            public string DDM_DOC_TYPE_TEXT { get; set; }
            [XmlElement("IS_MAP_FL")]
            public int IS_MAP_FL { get; set; }
        }
    }
}
