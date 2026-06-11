using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class BonusTypeMasterBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class BonusType
        {
            [XmlElement("BON_PK")]
            public int BON_PK { get; set; }
            [XmlElement("BON_CODE")]
            public string BON_CODE { get; set; }
            [XmlElement("BON_NAME")]
            public string BON_NAME { get; set; }
            [XmlElement("BON_DESC")]
            public string BON_DESC { get; set; }
            [XmlElement("BON_ACTIVE")]
            public int BON_ACTIVE { get; set; }
            [XmlElement("BON_BIZUNIT")]
            public int BON_BIZUNIT { get; set; }
            [XmlElement("BON_DEPT")]
            public int BON_DEPT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("BON_FORMULA")]
            public string BON_FORMULA { get; set; }
            [XmlElement("BON_FORMULA_TEXT")]
            public string BON_FORMULA_TEXT { get; set; }
        }
    }
}
