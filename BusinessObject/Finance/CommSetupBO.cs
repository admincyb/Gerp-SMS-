using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    public class CommSetupBO
    {

        [Serializable]
        [XmlRoot("Root")]
        public class AgentInfo
        {
            [XmlElement("ACH_CUSTOMER")]
            public int Customer { get; set; }
            [XmlElement("ACH_AGENT")]
            public int Agent { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BizUnit { get; set; }
            [XmlElement("ACTIVE")]
            public int Active { get; set; }
            [XmlElement("USER_PK")]
            public int UserPk { get; set; }
            //[XmlElement("LAST_MOD_DT")]
            //public string LastModDate { get; set; }
            [XmlElement("Detail")]
            public List<Detail> CommisionList { get; set; }
        }

        [Serializable]
        public class Detail
        {
            [XmlElement("ACD_PK")]
            public int acdPk { get; set; }
            [XmlElement("ACD_CUST_ITEM")]
            public int acdCustItem { get; set; }
            [XmlElement("ACD_COMMISION")]
            public decimal acdCommision { get; set; }
            [XmlElement("ACD_COMMISION_TYPE")]
            public int acdCommisionType { get; set; }
            [XmlElement("ACD_CURRENCY")]
            public int acdCurrency { get; set; }
            [XmlElement("ACD_ACTIVE")]
            public int acdActive { get; set; }
            [XmlElement("ACD_BIZUNIT")]
            public int acdBizUnit { get; set; }
            [XmlElement("ACD_FORMULA")]
            public int acdformula { get; set; }
            //[XmlElement("ACD_CRTD_BY")]
            //public int acdCreatedBy { get; set; }
            //[XmlElement("ACD_CRTD_DT")]
            //public string acdCreatedDate { get; set; }
            //[XmlElement("ACD_MOD_BY")]
            //public int acdModifiedBy { get; set; }
            //[XmlElement("ACD_MOD_DT")]
            //public int acdModifiedDate { get; set; }
        }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class COATreeBO
    {
        [XmlElement("Detail")]
        public List<COATreeNode> TreeNodes { get; set; }
    }

    public class COATreeNode
    {
        [XmlElement("COA_PK")]
        public int COA_PK { get; set; }

        [XmlElement("COA_TEXT")]
        public string COA_TEXT { get; set; }

        [XmlElement("COA_DESC")]
        public string COA_DESC { get; set; }

        [XmlElement("COA_IS_GROUP")]
        public int COA_IS_GROUP { get; set; }

        [XmlElement("COA_LEVEL")]
        public int COA_LEVEL { get; set; }
    }
}
