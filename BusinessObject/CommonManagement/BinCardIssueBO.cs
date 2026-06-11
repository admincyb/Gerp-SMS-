using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.CommonManagement
{
    [Serializable]
    [XmlRoot("Root")]
    public class BinCardIssueBO
    {
        [XmlElement("BIH_PK")]
        public int BIH_PK { get; set; }

        [XmlElement("BIH_NO")]
        public string BIH_NO { get; set; }

        [XmlElement("BIH_DATE")]
        public string BIH_DATE { get; set; }

        [XmlElement("BIH_DEPT_FROM")]
        public int BIH_DEPT_FROM { get; set; }

        public string BIH_DEPT_FROM_TEXT { get; set; }

        [XmlElement("BIH_DEPT_TO")]
        public int BIH_DEPT_TO { get; set; }

        public string BIH_DEPT_TO_TEXT { get; set; }

        [XmlElement("BIH_STATUS")]
        public int BIH_STATUS { get; set; }

        [XmlElement("BIH_DEPT")]
        public int BIH_DEPT { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("BIH_ISSUED_BY_TEXT")]
        public string BIH_ISSUED_BY_TEXT { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("AST_DOC_MODE")]
        public string AST_DOC_MODE { get; set; }

        [XmlElement("BIH_MOD_DT")]
        public string BIH_MOD_DT { get; set; }

        [XmlElement("BIH_DEPT_TO_LOC")]
        public string BIH_DEPT_TO_LOC { get; set; }

        [XmlElement("BIH_DEPT_TO_LOC_TEXT")]
        public string BIH_DEPT_TO_LOC_TEXT { get; set; }

        [XmlElement("BIH_ACCEPTED_BY_TEXT")]
        public string BIH_ACCEPTED_BY_TEXT { get; set; }

        [XmlElement("BIH_ACCEPTED_DATE")]
        public string BIH_ACCEPTED_DATE { get; set; }

        [XmlElement("Detail")]
        public List<BinIssueDetails> Detail { get; set; }

        [XmlElement("BIH_IS_WORK_ORDER")]
        public int WorkOrderFlag { get; set; }

        [XmlElement("BIH_WO")]
        public int WorkOrderPK { get; set; }

        [XmlElement("BIH_WIH_NO")]
        public string WorkOrderNo { get; set; }
    }

    [Serializable]
    public class BinIssueDetails
    {
        [XmlElement("SlNo")]
        public int SlNo { get; set; }

        [XmlElement("BID_PK")]
        public int BID_PK { get; set; }

        [XmlElement("BID_BIN_CARD")]
        public int BID_BIN_CARD { get; set; }

        [XmlElement("BID_PRODUCT")]
        public int BID_PRODUCT { get; set; }

        [XmlElement("BID_TOTAL_WT")]
        public double BID_TOTAL_WT { get; set; }

        [XmlElement("BID_TOTAL_PCS")]
        public double BID_TOTAL_PCS { get; set; }

        [XmlElement("BID_AVG_GLOVE_WT")]
        public double BID_AVG_GLOVE_WT { get; set; }

        [XmlElement("BID_TRX_MODE")]
        public int BID_TRX_MODE { get; set; }

        [XmlElement("BID_TRX")]
        public int BID_TRX { get; set; }

        [XmlElement("BCH_PK")]
        public int BCH_PK { get; set; }

        [XmlElement("BCH_NO")]
        public string BCH_NO { get; set; }

        [XmlElement("ITM_PK")]
        public int ITM_PK { get; set; }

        [XmlElement("ITM_NAME")]
        public string ITM_NAME { get; set; }

        [XmlElement("LNE_CODE")]
        public string LNE_CODE { get; set; }

        [XmlElement("LNE_NAME")]
        public string LNE_NAME { get; set; }

        [XmlElement("BCH_DATE")]
        public DateTime BCH_DATE { get; set; }

        [XmlElement("SHF_CODE")]
        public string SHF_CODE { get; set; }

        [XmlElement("SHF_NAME")]
        public string SHF_NAME { get; set; }

        [XmlElement("DPT_NAME")]
        public string DPT_NAME { get; set; }

        [XmlElement("DPT_CODE")]
        public string DPT_CODE { get; set; }

        [XmlElement("BWH_TOTAL_WT")]
        public double BWH_TOTAL_WT { get; set; }

        [XmlElement("BTT_TAB")]
        public int BTT_TAB { get; set; }

        [XmlElement("BWH_TOTAL_PCS")]
        public double BWH_TOTAL_PCS { get; set; }

        [XmlElement("BWH_AVG_GLOVE_WT")]
        public double BWH_AVG_GLOVE_WT { get; set; }

        [XmlElement("BWH_PK")]
        public int BWH_PK { get; set; }

        [XmlElement("BID_CHECKED")]
        public int BID_CHECKED { get; set; }

        [XmlElement("BID_DEPT_TO_LOC")]
        public string BID_DEPT_TO_LOC { get; set; }

        [XmlElement("BCH_DEPT_LOC_LAST")]
        public string BCH_DEPT_LOC_LAST { get; set; }

        [XmlElement("BID_DEPT_LOC_LAST_CODE")]
        public string BID_DEPT_LOC_LAST_CODE { get; set; }

        [XmlElement("BID_DEPT_LOC_LAST_TEXT")]
        public string BID_DEPT_LOC_LAST_TEXT { get; set; }

        [XmlElement("BCH_AQL_LAST_TEXT")]
        public string BCH_AQL_LAST_TEXT { get; set; }

        [XmlElement("BID_SL_NO")]
        public int BID_SL_NO { get; set; }

        [XmlElement("BQH_RESULT")]
        public string BQH_RESULT { get; set; }

        [XmlElement("BQH_RESULT_TEXT")]
        public string BQH_RESULT_TEXT { get; set; }

        [XmlElement("ITM_GRADE_TXT")]
        public string ITM_GRADE_TXT { get; set; }

        [XmlElement("ITM_GRADE")]
        public string ITM_GRADE { get; set; }

        [XmlElement("TOT_ROW_COUNT")]
        public string TOT_ROW_COUNT { get; set; }

        [XmlElement("BasketDetail")]
        public List<BasketIssueDtls> BasketDetails { get; set; }
    }

    [Serializable]
    public class BasketIssueDtls
    {
        [XmlElement("BIB_PK")]
        public int BIB_PK { get; set; }

        [XmlElement("BIB_ISSUE_DTL")]
        public int BIB_ISSUE_DTL { get; set; }

        [XmlElement("BIB_BASKET_DTL")]
        public int BIB_BASKET_DTL { get; set; }

        [XmlElement("BIB_WT")]
        public double BIB_WT { get; set; }

        [XmlElement("BIB_PCS")]
        public double BIB_PCS { get; set; }

        [XmlElement("BIB_SL_NO")]
        public string BIB_SL_NO { get; set; }

        [XmlElement("BIB_DEPT_TO_LOC")]
        public string BIB_DEPT_TO_LOC { get; set; }

        [XmlElement("BIB_CHECKED")]
        public int BIB_CHECKED { get; set; }

        [XmlElement("BID_NO")]
        public string BID_NO { get; set; }

        [XmlElement("BID_CARD_NO")]
        public string BID_CARD_NO { get; set; }

        [XmlElement("BIB_DEPT_TO_LOC_TEXT")]
        public string BIB_DEPT_TO_LOC_TEXT { get; set; }

        [XmlElement("BIB_DEPT_LOC_LAST_TEXT")]
        public string BIB_DEPT_LOC_LAST_TEXT { get; set; }

        [XmlElement("BIB_DEPT_FROM_LOC")]
        public string BIB_DEPT_FROM_LOC { get; set; }
    }
}
