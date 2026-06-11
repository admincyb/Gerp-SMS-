using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class HolidayMasterBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class HolidayMasterHeader
    {
        [XmlElement("HDR_PK")]
        public int HDR_PK { get; set; }
        [XmlElement("HDR_CAPTION")]
        public string HDR_CAPTION { get; set; }
        [XmlElement("HDR_DESC")]
        public string HDR_DESC { get; set; }
        [XmlElement("HDR_ACTIVE")]
        public int HDR_ACTIVE { get; set; }
        [XmlElement("HDR_COMPANY")]
        public int HDR_COMPANY { get; set; }
        [XmlElement("HDR_BIZUNIT")]
        public int HDR_BIZUNIT { get; set; }
        [XmlElement("HDR_DEPT")]
        public int HDR_DEPT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("LEAVE_DEL")]
        public int LEAVE_DEL { get; set; }
        [XmlElement("Details")]
        public List<HolidayMasterDetails> HolidayMasterDtl { get; set; }
    }

    [Serializable]
    public class HolidayMasterDetails
    {
        [XmlElement("SLNO")]
        public int SLNO { get; set; }
        [XmlElement("HDL_PK")]
        public int HDL_PK { get; set; }
        [XmlElement("HDL_HDR_PK")]
        public int HDL_HDR_PK { get; set; }
        [XmlElement("HDL_DATE")]
        public DateTime HDL_DATE { get; set; }
        [XmlElement("HDL_NAME")]
        public string HDL_NAME { get; set; }
        [XmlElement("HDL_TYPE")]
        public int HDL_TYPE { get; set; }
        [XmlElement("HDL_TYPE_TEXT")]
        public string HDL_TYPE_TEXT { get; set; }
        [XmlElement("HDL_REMARKS")]
        public string HDL_REMARKS { get; set; }
        [XmlElement("HDL_ACTIVE")]
        public int HDL_ACTIVE { get; set; }
        [XmlElement("IS_DELETED")]
        public int IS_DELETED { get; set; }
    }
}
