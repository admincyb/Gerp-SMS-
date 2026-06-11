using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class EmployeeDocumentDetails
    {
        [XmlElement("EDD_PK")]
        public int EmpPK { get; set; }

        [XmlElement("EMP_CODE")]
        public string EmpCode { get; set; }

        [XmlElement("EMP_NAME")]
        public string Employee { get; set; }

        [XmlElement("EMP_NATIONALITY_TEXT")]
        public string Nationality { get; set; }

        [XmlElement("EDD_DOC_TYPE")]
        public int EmpDocType { get; set; }

        [XmlElement("EDD_DOC_TYPE_TEXT")]
        public string EmpDocTypeText { get; set; }

        [XmlElement("EDD_DOC_NO")]
        public string EmpDocNo { get; set; }

        [XmlElement("EDD_ISSUED_BY")]
        public int IssuedBy { get; set; }

        [XmlElement("EDD_ISSUED_ON")]
        public DateTime IssuedOn { get; set; }

        [XmlElement("EDD_PLACE_OF_ISSUE")]
        public string PlaceOfIssue { get; set; }

        [XmlElement("EDD_EXPIRES_ON")]
        public DateTime ExpiresOn { get; set; }

        [XmlElement("EDD_DAYS_LEFT")]
        public int DaysLeft { get; set; }

        [XmlElement("EDD_REF_NO")]
        public string ReferenceNo { get; set; }

        [XmlElement("EDD_REF_DATE")]
        public DateTime ReferenceDate { get; set; }

        [XmlElement("EDD_TITLE")]
        public string Title { get; set; }

        [XmlElement("EDD_ADDL_INFO")]
        public string AdditionalInfo { get; set; }

        [XmlElement("EDD_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("EDD_INTIMATE_BEFORE")]
        public int  IntimateBefore { get; set; }

        [XmlElement("EDD_ORG_SUBMITTED")]
        public short OriginalSubmited { get; set; }

        [XmlElement("EDD_CHECKED_IN_ON")]
        public DateTime CheckedInOn { get; set; }

        [XmlElement("EDD_IS_CHECKED_OUT")]
        public short IsCheckedOut { get; set; }

        [XmlElement("EDD_ACTIVE")]
        public short IsActive { get; set; }

        [XmlElement("EDD_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

    }
}
