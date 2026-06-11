using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Configurations
{
    [Serializable]
    [XmlRoot("Root")]
    public class MarginSetupBO
    {
        [XmlElement("MAS_PK")]
        public int MarginSetup_PK { get; set; }
        [XmlElement("MAS_TYPE")]
        public int Type { get; set; }
        [XmlElement("MAS_FROM_DT")]
        public DateTime FromDate { get; set; }
        [XmlElement("MAS_TO_DT")]
        public DateTime ToDate { get; set; }
        [XmlElement("MAS_RATE")]
        public decimal Rate { get; set; }
        [XmlElement("USER_PK")]
        public int UserPk { get; set; }
        [XmlElement("MAS_CRTD_BY")]
        public int CreatedBy { get; set; }
        [XmlElement("MAS_CRTD_DT")]
        public DateTime? CreatedDate { get; set; }
        [XmlElement("MAS_MOD_BY")]
        public int ModifiedBy { get; set; }
        [XmlElement("MAS_MOD_DT")]
        public DateTime? ModifiedDate { get; set; }
        [XmlElement("MAS_BIZUNIT")]
        public int BIZUnit { get; set; }
        [XmlElement("MARGIN_TYPE")]
        public int MarginType { get; set; }
        [XmlElement("MAS_CURRENCY")]
        public int Currency { get; set; }
       
    }
}
