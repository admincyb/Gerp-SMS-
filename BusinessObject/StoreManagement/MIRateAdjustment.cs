using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.StoreManagement
{
    [Serializable]
    [XmlRoot("Root")]
    public class MIRateAdjustment : WorkflowBO
    {
        [XmlElement("IRH_PK")]
        public int MIRateAdjustmentPK { get; set; }

        [XmlElement("FROM_DT")]
        public DateTime FromDate { get; set; }

        [XmlElement("TO_DT")]
        public DateTime ToDate { get; set; }

        [XmlElement("DEPT_PK")]
        public int DeptPK { get; set; }

        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }

        [XmlElement("TRAN_DT")]
        public DateTime TransDate { get; set; }

        [XmlElement("TRAN_NO")]
        public string TransNo{ get; set; }

        [XmlElement("USER_PK")]
        public int UserPK { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("Details")]
        public List<MIRateAdjustmentDetails> MIRateAdjustmentDetailList { get; set; }
    }

    [Serializable]
    public class MIRateAdjustmentDetails
    {
        [XmlElement("ITM_CODE")]
        public string ItemCode { get; set; }

        [XmlElement("ITM_NAME")]
        public string ItemName { get; set; }

        [XmlElement("ICD_PK")]
        public int MaterialIssuePK { get; set; }

        [XmlElement("ICD_NO")]
        public string MaterialIssueNo { get; set; }

        [XmlElement("ICD_DATE")]
        public DateTime MaterialIssueDate { get; set; }

        [XmlElement("ICD_RATE")]
        public decimal Rate { get; set; }
        public decimal NewRate { get; set; }

        [XmlElement("SBD_BATCH_NO")]
        public string GRNNo { get; set; }

        [XmlElement("SBD_RATE")]
        public string GRNRate { get; set; }

        [XmlElement("DPT_NAME")]
        public string DepartmentName { get; set; }

        [XmlElement("VOUCHER_NO")]
        public string VoucherNo { get; set; }

        [XmlElement("VOUCHER_AMT")]
        public decimal VoucherAmt { get; set; }

        [XmlElement("ICH_AMOUNT")]
        public decimal TotalInvAmt { get; set; }
    }
}
