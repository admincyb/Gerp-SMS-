using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class COAFinYearOpening : WorkflowBO
    {
        [XmlElement("COH_PK")]
        public int COAFinYearOpeningPK { get; set; }

        [XmlElement("COH_FIN_YEAR")]
        public int FinYearPK { get; set; }

        [XmlElement("COH_BIZUNIT")]
        public int SbuPK { get; set; }

        [XmlElement("COH_NO")]
        public string TransNo { get; set; }

        [XmlElement("COH_DATE")]
        public DateTime TransDate { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("Detail")]
        public List<COAFinYearOpeningDetail> COAFinYearOpeningDetails { get; set; }
    }

    [Serializable]
    public class COAFinYearOpeningDetail
    {
        public int COAFinYearOpeningDetailPK { get; set; }

        [XmlElement("COB_COA")]
        public int ChartOfAccountPK { get; set; }

        [XmlElement("COB_COA_CODE")]
        public string ChartOfAccountCode { get; set; }

        [XmlElement("COB_COA_NAME")]
        public string ChartOfAccountName { get; set; }

        [XmlElement("COB_COA_PARENT")]
        public int ParentPK { get; set; }

        [XmlElement("COB_COA_PARENT_TEXT")]
        public string Parent { get; set; }

        [XmlElement("COB_COA_TYPE")]
        public int TypePK { get; set; }

        [XmlElement("COB_COA_TYPE_TEXT")]
        public string COAType { get; set; }

        [XmlElement("COB_COA_GROUP")]
        public bool IsGroup { get; set; }

        [XmlElement("COB_COA_CATEGORY")]
        public int CategoryPK { get; set; }

        [XmlElement("COB_COA_CATEGORY_TEXT")]
        public string Category { get; set; }

        [XmlElement("COB_OPENING_BAL")]
        public decimal Balance { get; set; }
    }
}
