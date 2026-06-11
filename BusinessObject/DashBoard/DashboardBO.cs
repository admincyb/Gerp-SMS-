using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.DashBoard
{
    public class DashboardBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class DashBoardMaster
    {
        [XmlElement("Dashlet")]
        public List<Dashlet> Dashlet { get; set; }
    }
    [Serializable] 
    public class Dashlet
    {
        [XmlElement("IND_PK")]
        public int IND_PK { get; set; }
        [XmlElement("IND_NAME")]
        public string IND_NAME { get; set; }
        [XmlElement("IND_MODULE")]
        public int IND_MODULE { get; set; }
        [XmlElement("IND_ACTIVE")]
        public int IND_ACTIVE { get; set; }
        [XmlElement("IND_SEQUENCE")]
        public int IND_SEQUENCE { get; set; }
        [XmlElement("IND_USER_GROUP")]
        public int IND_USER_GROUP { get; set; }
        [XmlElement("IND_MODE")]
        public int IND_MODE { get; set; }
        [XmlElement("IND_DESC")]
        public string IND_DESC { get; set; }

        [XmlElement("IND_TYPE")]
        public int IND_TYPE { get; set; }
        [XmlElement("IND_TYPE_TEXT")]
        public string IND_TYPE_TEXT { get; set; }
        [XmlElement("IND_CLASS_TEXT")]
        public string IND_CLASS_TEXT { get; set; }

        [XmlElement("IND_USER")]
        public int IND_USER { get; set; }
        [XmlElement("IND_MOD_DT")]
        public DateTime IND_MOD_DT { get; set; }
        [XmlElement("Group")]
        public List<Group> Group { get; set; }

        
    }
    [Serializable]  
    public class Group
    {
        [XmlElement("DBD_PK")]
        public int DBD_PK { get; set; }
        [XmlElement("DBD_NAME")]
        public string DBD_NAME { get; set; }
        [XmlElement("DBD_DESC")]
        public string DBD_DESC { get; set; }
        [XmlElement("DBD_DASH_CFG")]
        public int DBD_DASH_CFG { get; set; }
        [XmlElement("DBD_SEQUENCE")]
        public int DBD_SEQUENCE { get; set; }
        [XmlElement("DBD_ACTIVE")]
        public int DBD_ACTIVE { get; set; }
        [XmlElement("Item")]
        public List<Item> Item { get; set; }
    }
    [Serializable]   
    public class Item
    {
        [XmlElement("DBL_PK")]
        public int DBL_PK { get; set; }
        [XmlElement("DBL_NAME")]
        public string DBL_NAME { get; set; }
        [XmlElement("DBL_DASH_DTL")]
        public int DBL_DASH_DTL { get; set; }
        [XmlElement("DBL_TYPE")]
        public int DBL_TYPE { get; set; }
        [XmlElement("DBL_MENU_CFG")]
        public int DBL_MENU_CFG { get; set; }
        [XmlElement("DBL_DPT")]
        public int DBL_DPT { get; set; }
        [XmlElement("DBL_LINK")]
        public string DBL_LINK { get; set; }
        [XmlElement("DBL_ACTIVE")]
        public int DBL_ACTIVE { get; set; }
        [XmlElement("DBL_SEQUENCE")]
        public int DBL_SEQUENCE { get; set; }
        [XmlElement("DBL_DEF_PK")]
        public string DBL_DEF_PK { get; set; }
        [XmlElement("MNU_NAME")]
        public string MNU_NAME { get; set; }
        

    }
}
