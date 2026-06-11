using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class EmployeeDoc
    {
        [XmlElement("EDD_EMPLOYEE")]
        public int EmpPK { get; set; }

        [XmlElement("EDD_PK")]
        public int DocPk { get; set; }

        [XmlElement("EDD_DOC_TYPE")]
        public int DocTypePK { get; set; }

        [XmlElement("EDD_DOC_TYPE_TEXT")]
        public string DocTypeText { get; set; }

        [XmlElement("EDD_DOC_NO")]
        public string DocNo { get; set; }

        [XmlElement("EDD_PLACE_OF_ISSUE")]
        public string PalceOfIssue { get; set; }

        [XmlElement("EDD_REF_NO")]
        public string ReferenceNo { get; set; }

        [XmlElement("EDD_ISSUED_BY")]
        public string IssuedBy { get; set; }

        [XmlElement("EDD_ISSUED_ON")]
        public string IssuedOn { get; set; }

        [XmlElement("EDD_EXPIRES_ON")]
        public string ExpiresOn { get; set; }

        [XmlElement("EDD_DAYS_LEFT")]
        public int DaysLeft { get; set; }

        [XmlElement("EDD_REF_DATE")]
        public string ReferenceDate { get; set; }

        [XmlElement("EDD_TITLE")]
        public string Title { get; set; }

        [XmlElement("EDD_ADDL_INFO")]
        public string AdditionalInfo { get; set; }

        [XmlElement("EDD_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("EDD_INTIMATE_BEFORE")]
        public int InitimateBefore { get; set; }

        [XmlElement("EDD_ORG_SUBMITTED")]
        public short OriginalSubmitted { get; set; }

        [XmlElement("EDD_CHECKED_IN_ON")]
        public string CheckedInOn { get; set; }

        [XmlElement("EDD_CHECKED_IN_BY_TEXT")]
        public string CheckedInByText { get; set; }

        [XmlElement("EDD_CHECKED_IN_BY")]
        public string CheckedInBy { get; set; }

        [XmlElement("EDD_IS_CHECKED_IN")]
        public short? IsCheckedIn { get; set; }

        [XmlElement("EDD_ACTIVE")]
        public short DocActive { get; set; }

        [XmlElement("EDD_COMPANY")]
        public int Company { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public short Active { get; set; }

        [XmlElement("USER_PK")]
        public int User { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModifiedDate { get; set; }

        [XmlElement("DOC_EXIST")]
        public int DOC_EXIST { get; set; }

        [XmlElement("DocDetails")]
        public List<EmpDocUploadBinder> DocDetails { get; set; }

    }    
}
