using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class LeaveTypeMasterBO
    {
        [XmlElement("LTM_PK")]
        public int LTM_PK { get; set; }
        [XmlElement("LTM_CODE")]
        public string LTM_CODE { get; set; }
        [XmlElement("LTM_NAME")]
        public string LTM_NAME { get; set; }
        [XmlElement("LTM_DESC")]
        public string LTM_DESC { get; set; }
        [XmlElement("LTM_ACCURAL")]
        public short LTM_ACCURAL { get; set; }
        [XmlElement("LTM_PAID")]
        public short LTM_PAID { get; set; }
        [XmlElement("LTM_CARRY_FWD_LIMIT")]
        public double LTM_CARRY_FWD_LIMIT { get; set; }
        [XmlElement("LTM_LIMIT")]
        public double LTM_LIMIT { get; set; }
        [XmlElement("LTM_ENCASH")]
        public short LTM_ENCASH { get; set; }
        [XmlElement("LTM_CREDIT")]
        public short LTM_CREDIT { get; set; }
        [XmlElement("LTM_CARRY_FWD")]
        public short LTM_CARRY_FWD { get; set; }
        [XmlElement("LTM_RATE")]
        public double LTM_RATE { get; set; }
        [XmlElement("LTM_FACTOR")]
        public double LTM_FACTOR { get; set; }
        [XmlElement("LTM_ACTIVE")]
        public short LTM_ACTIVE { get; set; }
        [XmlElement("LTM_STATUS")]
        public short LTM_STATUS { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LTM_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("LTM_COMPANY")]
        public int LTM_COMPANY { get; set; }
        [XmlElement("LTM_BIZUNIT")]
        public int LTM_BIZUNIT { get; set; }
        [XmlElement("LTM_DEPT")]
        public int LTM_DEPT { get; set; }
        [XmlElement("LTM_AUTO_CREDIT")]
        public int LTM_AUTO_CREDIT { get; set; }
    }
}
