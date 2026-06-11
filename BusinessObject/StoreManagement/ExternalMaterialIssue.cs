using System.Collections.Generic;

namespace BusinessObject.StoreManagement
{
  public class ExternalMaterialIssue
    {
        public int UserPk
        {
            get;
            set;
        }
        public int DeptPk
        { get; set; }
        public int ICH_PK
        { get; set; }
        public int ICH_TRX_TYPE
        { get; set; }
        public string ICH_REF_NO
        { get; set; }
        public string ICH_ISS_RCV_TYPE
        { get; set; }
        public int ICH_DEPT_TEXT
        { get; set; }
        public int ICH_ISS_RCV_PK
        { get; set; }
        public string ICH_ISS_RCV_NAME
        { get; set; }
        public string ICH_REMARKS
        { get; set; }
        public string ICH_DATE
        { get; set; }
        public string ICH_NO
        { get; set; }
        public List<ExternalMaterialIssueDetails> MaterialIssueDetailsList
        { get; set; }
    }
}
