using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class DepreciationBO : WorkflowBO
    {
        public DepreciationBO()
        {
            this.Details = new List<DepreciationDetails>();
        }

        [XmlElement("FDH_PK")]
        public int FDH_PK { get; set; }

        [XmlElement("FDH_NO")]
        public string FDH_NO { get; set; }

        [XmlElement("FDH_DATE")]
        public DateTime FDH_DATE { get; set; }

        [XmlElement("FDH_MONTH_YEAR")]
        public DateTime FDH_MONTH_YEAR { get; set; }

        [XmlElement("FDH_FROM")]
        public DateTime FDH_FROM { get; set; }

        [XmlElement("FDH_TO")]
        public DateTime FDH_TO { get; set; }

        [XmlElement("FDH_DESC")]
        public string FDH_DESC { get; set; }

        [XmlElement("FDH_DEPT")]
        public int FDH_DEPT { get; set; }

        [XmlElement("FDH_TRX_CURR")]
        public int FDH_TRX_CURR { get; set; }


        [XmlElement("FDH_TYPE")]
        public int FDH_TYPE { get; set; }

        [XmlElement("FDH_TRX_CURR_TEXT")]
        public string FDH_TRX_CURR_TEXT { get; set; }

        [XmlElement("FDH_AMOUNT_TC")]
        public decimal FDH_AMOUNT_TC { get; set; }

        [XmlElement("FDH_AMOUNT_BC")]
        public decimal FDH_AMOUNT_BC { get; set; }

        [XmlElement("FDH_BASE_CURR")]
        public int FDH_BASE_CURR { get; set; }

        [XmlElement("FDH_BASE_CURR_TEXT")]
        public string FDH_BASE_CURR_TEXT { get; set; }

        [XmlElement("FDH_EXCHG_RATE")]
        public double FDH_EXCHG_RATE { get; set; }

        [XmlElement("FDH_STATUS")]
        public string FDH_STATUS { get; set; }

        [XmlElement("FDH_ACTIVE")]
        public string FDH_ACTIVE { get; set; }

        [XmlElement("FDH_DEL_STATUS")]
        public string FDH_DEL_STATUS { get; set; }

        [XmlElement("FDH_BIZUNIT")]
        public string FDH_BIZUNIT { get; set; }

        [XmlElement("FDH_COMPANY")]
        public string FDH_COMPANY { get; set; }

        [XmlElement("FDH_CRTD_BY")]
        public string FDH_CRTD_BY { get; set; }

        [XmlElement("FDH_CRTD_DT")]
        public DateTime FDH_CRTD_DT { get; set; }

        [XmlElement("FDH_MOD_BY")]
        public string FDH_MOD_BY { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("FDH_HAS_JRNL_ENTRY")]
        public short FDH_HAS_JRNL_ENTRY { get; set; }

        //Common Parameters
        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public int Active { get; set; }

        [XmlElement("USER_PK")]
        public int UserPk { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }

        [XmlElement("Detail")]
        public List<DepreciationDetails> Details { get; set; }        
    }

    [Serializable]
    [XmlRoot("Detail")]
    public sealed class DepreciationDetails
    {
        [XmlElement("FDD_PK")]
        public int FDD_PK { get; set; }

        [XmlElement("FDD_HDR")]
        public int FDD_HDR { get; set; }

        [XmlElement("FDD_ASSET")]
        public int FDD_ASSET { get; set; }
        
        [XmlElement("FDD_AMOUNT")]
        public decimal FDD_AMOUNT { get; set; }

        [XmlElement("FDD_ACTIVE")]
        public int FDD_ACTIVE { get; set; }

        [XmlElement("asrCode")]
        public string asrCode { get; set; }

        [XmlElement("asrName")]
        public string asrName { get; set; }

        [XmlElement("asrType")]
        public string asrType { get; set; }

        [XmlElement("asrType_text")]
        public string asrType_text { get; set; }

        [XmlElement("asrCategory")]
        public string asrCategory { get; set; }

        [XmlElement("asrCategory_Text")]
        public string asrCategory_Text { get; set; }

        [XmlElement("amiDatePur")]
        public DateTime amiDatePur { get; set; }
            
        [XmlElement("amiCostPur")]
        public decimal amiCostPur { get; set; }

        [XmlElement("amiCostLand")]
        public decimal amiCostLand { get; set; }
            
        [XmlElement("amiCurrPur")]
        public string amiCurrPur { get; set; }
            
        [XmlElement("amiCurrPur_Text")]
        public string amiCurrPur_Text { get; set; }
            
        [XmlElement("amiDeprPerc")]
        public double amiDeprPerc { get; set; }

        [XmlElement("BalanceDeprAmt")]
        public decimal BalanceDeprAmt { get; set; }
    }

    public class Fields
    {
        public const string SEARCHTEXTFIELD = "FDH_NO";
        public const string SEARCHVALUEFIELD = "FDH_PK";
    }

    #region sample XML
    /*  <Root>
  <FDH_PK>1</FDH_PK>
  <FDH_NO>dpr/15001</FDH_NO>
  <FDH_DATE>2015-01-15T00:00:00</FDH_DATE>
  <FDH_FROM>2015-01-01T00:00:00</FDH_FROM>
  <FDH_TO>2015-01-10T00:00:00</FDH_TO>
  <FDH_TRX_CURR>133</FDH_TRX_CURR>
  <FDH_TRX_CURR_TEXT>THB</FDH_TRX_CURR_TEXT>
  <FDH_AMOUNT_TC>1200.0000</FDH_AMOUNT_TC>
  <FDH_BASE_CURR>133</FDH_BASE_CURR>
  <FDH_BASE_CURR_TEXT>THB</FDH_BASE_CURR_TEXT>
  <FDH_EXCHG_RATE>1.000000000000000e+000</FDH_EXCHG_RATE>
  <FDH_AMOUNT_BC>1200.0000</FDH_AMOUNT_BC>
  <FDH_STATUS>0</FDH_STATUS>
  <FDH_ACTIVE>1</FDH_ACTIVE>
  <FDH_DEL_STATUS>0</FDH_DEL_STATUS>
  <FDH_BIZUNIT>1</FDH_BIZUNIT>
  <FDH_CRTD_BY>1</FDH_CRTD_BY>
  <FDH_CRTD_DT>2015-01-15T15:53:05.300</FDH_CRTD_DT>
  <FDH_MOD_BY>1</FDH_MOD_BY>
  <LAST_MOD_DT>2015-01-15T15:53:05.300</LAST_MOD_DT>
  <Detail>
    <FDD_PK>1</FDD_PK>
    <FDD_HDR>1</FDD_HDR>
    <FDD_ASSET>1</FDD_ASSET>
    <FDD_AMOUNT>2500.0000</FDD_AMOUNT>
    <FDD_ACTIVE>1</FDD_ACTIVE>
    <asrCode>FA-15-00001</asrCode>
    <asrName>Car</asrName>
    <asrCategory>1</asrCategory>
    <asrCategory_Text>Asset</asrCategory_Text>
  </Detail>
</Root>*/
    
    #endregion
}
