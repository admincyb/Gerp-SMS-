using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeBasicInfoBO
    {

    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeBasicInfomtn
    {
        [XmlElement("empPK")]
        public int empPK { get; set; }
        [XmlElement("empCode")]
        public string empCode { get; set; }
        [XmlElement("empSalutation")]
        public int empSalutation { get; set; }
        [XmlElement("empName_txt")]
        public string empName_txt { get; set; }
        [XmlElement("empName")]
        public string empName { get; set; }
        [XmlElement("empName2")]
        public string empName2 { get; set; }
        [XmlElement("empName3")]
        public string empName3 { get; set; }
        [XmlElement("empName4")]
        public string empName4 { get; set; }
        [XmlElement("empName_LL")]
        public string empName_LL { get; set; }
        [XmlElement("empDOB")]
        public string empDOB { get; set; }
        [XmlElement("empType")]
        public int empType { get; set; }
        [XmlElement("empCategory")]
        public int empCategory { get; set; }
        [XmlElement("empBasicPay")]
        public double empBasicPay { get; set; }
        [XmlElement("empPayRollType")]
        public int empPayRollType { get; set; }
        [XmlElement("PUM_PK")]
        public string PUM_PK { get; set; }
        [XmlElement("EMPCODE_AUTO")]
        public int EMPCODE_AUTO { get; set; }



        [XmlElement("APT_CODE")]
        public string AST_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        //[XmlElement("empAddress")]
        //public string empAddress { get; set; }

        //[XmlElement("empCity")]
        //public string empCity { get; set; }
        //[XmlElement("empCountry1")]
        //public int empCountry { get; set; }
        [XmlElement("empGender")]
        public int empGender { get; set; }
        [XmlElement("empMaritalStatus")]
        public int empMaritalStatus { get; set; }
        [XmlElement("empDepartment")]
        public int empDepartment { get; set; }
        [XmlElement("EmpCostcenterId")]
        public int EmpCostcenterId { get; set; }

        [XmlElement("EmpTeamId")]
        public int EmpTeamId { get; set; }

        [XmlElement("empEmploymentType")]
        public int empEmploymentType { get; set; }
        [XmlElement("empDOJ")]
        public string empDOJ { get; set; }
        [XmlElement("empConfirmedOn")]
        public string empConfirmedOn { get; set; }
        [XmlElement("empEmail1")]
        public string empEmail1 { get; set; }
        [XmlElement("empEmail2")]
        public string empEmail2 { get; set; }

        [XmlElement("empPhone1")]
        public string empPhone1 { get; set; }
        [XmlElement("empPhoneExt1")]
        public string empPhoneExt1 { get; set; }
        [XmlElement("empPhone2")]
        public string empPhone2 { get; set; }
        [XmlElement("empPhone3")]
        public string empPhone3 { get; set; }
        [XmlElement("empPhone4")]
        public string empPhone4 { get; set; }
        [XmlElement("empMobile1")]
        public string empMobile1 { get; set; }
        [XmlElement("empMobile2")]
        public string empMobile2 { get; set; }
        [XmlElement("empCurStatus")]
        public int empCurStatus { get; set; }
        [XmlElement("empDesignation")]
        public int empDesignation { get; set; }
        [XmlElement("empBranch")]
        public int empBranch { get; set; }
        [XmlElement("empCompany")]
        public int empCompany { get; set; }
        //[XmlElement("EmpReportTo")]
        //public int EmpReportTo { get; set; }

        [XmlElement("empReportTo")]//Get
        public int empReportTo { get; set; }
        [XmlElement("empReportTo1")]//Get
        public int? empReportTo1 { get; set; }



        [XmlElement("empAddress1")]
        public string empAddress1 { get; set; }
        [XmlElement("empAddress2")]
        public string empAddress2 { get; set; }
        [XmlElement("empAddress3")]
        public string empAddress3 { get; set; }
        [XmlElement("empCity1")]
        public string empCity1 { get; set; }
        [XmlElement("empState1")]
        public string empState1 { get; set; }
        [XmlElement("empCountry1")]
        public int empCountry1 { get; set; }
        [XmlElement("empZip1")]
        public string empZip1 { get; set; }
        [XmlElement("empNationality1")]
        public int empNationality1 { get; set; }
        [XmlElement("empAddress4")]
        public string empAddress4 { get; set; }
        [XmlElement("empAddress5")]
        public string empAddress5 { get; set; }


        [XmlElement("empAddress6")]
        public string empAddress6 { get; set; }
        [XmlElement("empCity2")]
        public string empCity2 { get; set; }
        [XmlElement("empState2")]
        public string empState2 { get; set; }
        [XmlElement("empCountry2")]
        public int empCountry2 { get; set; }

        [XmlElement("empStatePK1")]
        public string empStatePK1 { get; set; }
        [XmlElement("empStatePK1_TEXT")]
        public string empStatePK1_TEXT { get; set; }
        [XmlElement("empStatePK2")]
        public string empStatePK2 { get; set; }
        [XmlElement("empStatePK2_TEXT")]
        public string empStatePK2_TEXT { get; set; }

        [XmlElement("empZip2")]
        public string empZip2 { get; set; }
        [XmlElement("empNationality2")]
        public int empNationality2 { get; set; }
        [XmlElement("empFatherName")]
        public string empFatherName { get; set; }
        [XmlElement("empMotherName")]
        public string empMotherName { get; set; }
        [XmlElement("empSpouseName")]
        public string empSpouseName { get; set; }
        [XmlElement("empSpouseTaxNo")]
        public string empSpouseTaxNo { get; set; }

        [XmlElement("empSpouseIsWorking")]
        public int empSpouseIsWorking { get; set; }
        [XmlElement("empAlternateNum")]
        public string empAlternateNum { get; set; }


        [XmlElement("empBloodGroup")]
        public int empBloodGroup { get; set; }
        [XmlElement("empNoOfChildren")]
        public string empNoOfChildren { get; set; }
        [XmlElement("empProfession")]
        public int empProfession { get; set; }


        [XmlElement("empJobLevel")]
        public int empJobLevel { get; set; }
        [XmlElement("empSkillLevel")]
        public int empSkillLevel { get; set; }
        [XmlElement("empJobCategory")]
        public int empJobCategory { get; set; }
        [XmlElement("empJobStream")]
        public int empJobStream { get; set; }
        [XmlElement("empDistrict1")]
        public string empDistrict1 { get; set; }
        [XmlElement("empDistrict2")]
        public string empDistrict2 { get; set; }
        [XmlElement("empPlaceOfBirth")]
        public string empPlaceOfBirth { get; set; }

        [XmlElement("empReligion")]
        public int empReligion { get; set; }
        [XmlElement("empSubReligion")]
        public int empSubReligion { get; set; }





        [XmlElement("empActive")]
        public int empActive { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("empDept")]
        public int empDept { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        //Employee Photo

        [XmlElement("empPhotoPK")]
        public int empPhotoPK { get; set; }
        [XmlElement("empPhotoName")]
        public string empPhotoName { get; set; }
        [XmlElement("empPhotoFileName")]
        public string empPhotoFileName { get; set; }
        [XmlElement("empPhotoFileType")]
        public string empPhotoFileType { get; set; }
        [XmlElement("empPhotoFilePath")]
        public string empPhotoFilePath { get; set; }


        //get EmployeeID XML

        [XmlElement("empDepartmentText")]
        public string empDepartmentText { get; set; }
        [XmlElement("EmpCostcenterText")]
        public string EmpCostcenterText { get; set; }
        [XmlElement("EmpTeamText")]
        public string EmpTeamText { get; set; }
        [XmlElement("empDepartmentCode")]
        public string empDepartmentCode { get; set; }
        [XmlElement("empDesignationText")]
        public string empDesignationText { get; set; }
        [XmlElement("empCountry1Text")]
        public string empCountry1Text { get; set; }
        [XmlElement("empCountry2Text")]
        public string empCountry2Text { get; set; }
        [XmlElement("empNationality1Text")]
        public string empNationality1Text { get; set; }
        [XmlElement("empNationality2Text")]
        public string empNationality2Text { get; set; }
        [XmlElement("empBranchText")]
        public string empBranchText { get; set; }
        [XmlElement("empBranchCode")]
        public string empBranchCode { get; set; }
        [XmlElement("empReportToText")]
        public string empReportToText { get; set; }
        [XmlElement("empReportTo1Text")]
        public string empReportTo1Text { get; set; }
        [XmlElement("empProfessionText")]
        public string empProfessionText { get; set; }
        [XmlElement("empReligionText")]
        public string empReligionText { get; set; }
        [XmlElement("empSubReligionText")]
        public string empSubReligionText { get; set; }
        [XmlElement("empDOJText")]
        public string empDOJText { get; set; }
        [XmlElement("empDOBText")]
        public string empDOBText { get; set; }
        [XmlElement("empNameText")]
        public string empNameText { get; set; }
        [XmlElement("empConfirmedOnText")]
        public string empConfirmedOnText { get; set; }
        [XmlElement("emsPk")]
        public string emsPk { get; set; }
        [XmlElement("empOldCode")]
        public string empOldCode { get; set; }
        [XmlElement("empBiometricId")]
        public string empBiometricId { get; set; }
        [XmlElement("empPassportText")]
        public string empPassportText { get; set; }
        [XmlElement("EPD_EMP_TYPE_TEXT")]
        public string EPD_EMP_TYPE_TEXT { get; set; }

        //Additional info 14-11-2016
        [XmlElement("empSpouseBranch")]
        public string empSpouseBranch { get; set; } //changed empSpouseBranch to empSpouseSurname
        [XmlElement("empFatherNameId")]
        public string empFatherNameId { get; set; }
        [XmlElement("empMotherNameId")]
        public string empMotherNameId { get; set; }
        [XmlElement("empSpouseNameId")]
        public string empSpouseNameId { get; set; }
        [XmlElement("empFatherofSpouse")]
        public string empFatherofSpouse { get; set; }
        [XmlElement("empFatherofSpouseId")]
        public string empFatherofSpouseId { get; set; }
        [XmlElement("empMotherofSpouse")]
        public string empMotherofSpouse { get; set; }
        [XmlElement("empMotherofSpouseId")]
        public string empMotherofSpouseId { get; set; }
        [XmlElement("empNoOfChildrenId")]
        public string empNoOfChildrenId { get; set; }
        [XmlElement("empTaxPayer")]
        public int empTaxPayer { get; set; }
        [XmlElement("empNoOfChildEdu")]
        public string empNoOfChildEdu { get; set; }
        [XmlElement("empNoOfChildEduId")]
        public string empNoOfChildEduId { get; set; }
        [XmlElement("empDueDate")]
        public string empDueDate { get; set; }
        [XmlElement("MAIL_BEFORE")]
        public int MAIL_BEFORE { get; set; }
        [XmlElement("IS_CONTRACT")]
        public int IS_CONTRACT { get; set; }
        [XmlElement("MAIL_BEFORE_CNFRM_ON")]
        public int MAIL_BEFORE_CNFRM_ON { get; set; }
        [XmlElement("MAIL_BEFORE_DOJ")]
        public int MAIL_BEFORE_DOJ { get; set; }

        [XmlElement("EmpDocDetails")]
        public EmployeeDoc EmployeeDocDetails { get; set; }
    }

    public class EmpTabDetails
    {
        public int Id { get; set; }
        public string NextPageUrl { get; set; }
    }
}
