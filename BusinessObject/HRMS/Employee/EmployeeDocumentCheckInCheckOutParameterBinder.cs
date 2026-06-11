using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class EmployeeDocumentCheckInCheckOutParameterBinder
    {
        //<BIZUNIT_PK>1</BIZUNIT_PK>
        // <ACTIVE>1</ACTIVE>
        // <USER_PK>1</USER_PK>
        // <EDT_PK></EDT_PK>
        // <EDT_EMP_DOC_DTL></EDT_EMP_DOC_DTL>
        // <EDT_TYPE></EDT_TYPE>
        // <EDT_SL_NO></EDT_SL_NO>
        // <EDT_CATEGORY></EDT_CATEGORY>

        // <EDT_DATE></EDT_DATE>
        // <EDT_DONE_BY></EDT_DONE_BY>
        // <EDT_DONE_BY_TEXT></EDT_DONE_BY_TEXT>
        // <EDT_ISSUED_FOR></EDT_ISSUED_FOR>
        // <EDT_EXP_RETURN_DT></EDT_EXP_RETURN_DT>
        // <EDT_PURPOSE></EDT_PURPOSE>
        // <EDT_REMARKS></EDT_REMARKS>
        // <Deatil>
        //     <EDD_PK></EDD_PK>
        // <Deatil>
        // <Deatil>
        //     <EDD_PK></EDD_PK>
        // <Deatil>
        // <Deatil>
        //     <EDD_PK></EDD_PK>
        // <Deatil>

        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public short Active { get; set; }

        [XmlElement("USER_PK")]
        public int User { get; set; }

        [XmlElement("EDT_PK")]
        public int? DetailsPk { get; set; }

        [XmlElement("EDT_EMP_DOC_DTL")]
        public int? EmpDocDetail { get; set; }

        [XmlElement("EDT_TYPE")]
        public int CurrentState { get; set; }

        [XmlElement("EDT_SL_NO")]
        public int SlNo { get; set; }

        /// <summary>
        /// 1 for Internal
        /// 2 for External
        /// </summary>
        [XmlElement("EDT_CATEGORY")]
        public short? Category { get; set; }

        [XmlElement("EDT_DATE")]
        public DateTime? CheckedInOnDate { get; set; }

        [XmlElement("EDT_DONE_BY")]
        public int? DoneByID { get; set; }

        [XmlElement("EDT_DONE_BY_TEXT")]
        public string DoneByText { get; set; }
        /// <summary>
        /// Renewal
        /// Exit
        /// Personal
        /// </summary>
        [XmlElement("EDT_ISSUED_FOR")]
        public int? IssuedFor { get; set; }

        [XmlElement("EDT_EXP_RETURN_DT")]
        public DateTime? ExpectedReturnDate { get; set; }

        [XmlElement("EDT_PURPOSE")]
        public string Purpose { get; set; }

        [XmlElement("EDT_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("Detail")]
        public List<EmployeeDocumentCheckInCheckOutDetailsParameterBinder> Details { get; set; }
    }

    [Serializable]
    public sealed class EmployeeDocumentCheckInCheckOutDetailsParameterBinder
    {
        [XmlElement("EDD_PK")]
        public int DetailsPK { get; set; }
    }
}
