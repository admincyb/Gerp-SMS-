using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class EmployeeLeaveMasterBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class EmployeeLeaveMaster
        {
            [XmlElement("EOH_PK")]
            public int EOH_PK { get; set; }
            [XmlElement("EOH_LEAVE_TYPE")]
            public int EOH_LEAVE_TYPE { get; set; }
            [XmlElement("EOH_CREDIT")]
            public float EOH_CREDIT { get; set; }
            [XmlElement("EOH_DATE_FROM")]
            public string EOH_DATE_FROM { get; set; }
            [XmlElement("EOH_DATE_TO")]
            public string EOH_DATE_TO { get; set; }
            [XmlElement("EOH_DESC")]
            public string EOH_DESC { get; set; }
            [XmlElement("EOH_STATUS")]
            public int EOH_STATUS { get; set; }
            //[XmlElement("EOH_YEAR")]
            //public int EOH_YEAR { get; set; }
            [XmlElement("EOH_ACTIVE")]
            public int EOH_ACTIVE { get; set; }
            [XmlElement("EOH_COMPANY")]
            public int EOH_COMPANY { get; set; }
            [XmlElement("EOH_BIZUNIT")]
            public int EOH_BIZUNIT { get; set; }
            [XmlElement("EOH_DEPT")]
            public int EOH_DEPT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("IS_CHECK")]
            public int IS_CHECK { get; set; }
            [XmlElement("Detail")]
            public List<EmployeeLeaveDetail> EmployeeLeaveDetail { get; set; }
        }
         
        [Serializable]
        [XmlRoot("Detail")]
        public class EmployeeLeaveDetail
        {
            [XmlElement("ROW_NO")]
            public int ROW_NO { get; set; }
            [XmlElement("ELH_EMPLOYEE")]
            public int ELH_EMPLOYEE { get; set; }
            [XmlElement("ELH_EMPLOYEE_TEXT")]
            public string ELH_EMPLOYEE_TEXT { get; set; }
            [XmlElement("ELH_PK")]
            public int ELH_PK { get; set; }
            [XmlElement("ELH_LEAVE_TYPE")]
            public int ELH_LEAVE_TYPE { get; set; }
            [XmlElement("ELH_LEAVE_TYPE_TEXT")]
            public string ELH_LEAVE_TYPE_TEXT { get; set; }
            [XmlElement("ELH_LEAVE_TYPE_CODE_TEXT")]
            public string ELH_LEAVE_TYPE_CODE_TEXT { get; set; }
            [XmlElement("ELH_LEAVE_BAL")]
            public decimal ELH_LEAVE_BAL { get; set; }
            [XmlElement("ELH_REMARKS")]
            public string ELH_REMARKS { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("ELH_DATE")]
            public string ELH_DATE { get; set; }
            [XmlElement("ELH_ACTIVE")]
            public int ELH_ACTIVE { get; set; }
            [XmlElement("IS_DELETED")]
            public int IS_DELETED { get; set; }
            [XmlElement("ELH_LEAVE_HDR")]
            public int ELH_LEAVE_HDR { get; set; }

            [XmlElement("empDepartment_Text")]
            public string empDepartment_Text { get; set; }            
            [XmlElement("empDepartment")]
            public int empDepartment { get; set; }

            [XmlElement("EmpBranch")]
            public int EmpBranch { get; set; }
            [XmlElement("EmpBranch_Text")]
            public string EmpBranch_Text { get; set; }
            [XmlElement("EPD_EMP_TYPE")]
            public int EPD_EMP_TYPE { get; set; }
            [XmlElement("emptype_text")]
            public string emptype_text { get; set; }

            

            //[XmlElement("ELH_YEAR")]
            //public int ELH_YEAR { get; set; }
            [XmlElement("ELH_DEPT")]
            public int ELH_DEPT { get; set; }
            [XmlElement("ELH_COMPANY")]
            public int ELH_COMPANY { get; set; }
            [XmlElement("ELH_BIZUNIT")]
            public int ELH_BIZUNIT { get; set; }


        }   

        //[Serializable]
        //public class EmployeeLeaveData
        //{
        //    //private decimal leaveBalance;
        //    private DateTime dt;

        //    [XmlElement("ROW_NO")]
        //    public int ROW_NO { get; set; }
        //    [XmlElement("ELH_EMPLOYEE")]
        //    public int ELH_EMPLOYEE { get; set; }
        //    [XmlElement("ELH_EMPLOYEE_TEXT")]
        //    public string ELH_EMPLOYEE_TEXT { get; set; }
        //    [XmlElement("ELH_PK")]
        //    public int ELH_PK { get; set; }
        //    [XmlElement("ELH_LEAVE_TYPE")]
        //    public int ELH_LEAVE_TYPE { get; set; }
        //    [XmlElement("ELH_LEAVE_TYPE_TEXT")]
        //    public string ELH_LEAVE_TYPE_TEXT { get; set; }
        //    [XmlElement("ELH_LEAVE_TYPE_CODE_TEXT")]
        //    public string ELH_LEAVE_TYPE_CODE_TEXT { get; set; }
        //    [XmlElement("ELH_LEAVE_BAL")]
        //    public decimal ELH_LEAVE_BAL
        //    {
        //        get;
        //        set;
        //    }
        //    [XmlElement("ELH_REMARKS")]
        //    public string ELH_REMARKS { get; set; }
        //    [XmlElement("LAST_MOD_DT")]
        //    public string LAST_MOD_DT { get; set; }
        //    [XmlElement("ELH_DATE")]
        //    public string ELH_DATE
        //    {
        //        get
        //        {
        //            return this.dt.ToString("dd-MMM-yyyy");
        //        }
        //        set
        //        {
        //            this.dt = Convert.ToDateTime(value);
        //        }
        //    }
        //    //{ get; set; }
        //    [XmlElement("ELH_ACTIVE")]
        //    public int ELH_ACTIVE { get; set; }
        //    [XmlElement("IS_DELETED")]
        //    public int IS_DELETED { get; set; }
        //}

        //[Serializable]
        //[XmlRoot("Root")]
        //public class EmployeeLeaveDataRoot
        //{
        //    [XmlElement("Detail")]
        //    public List<EmployeeLeaveData> EmployeeLeaveDatas { get; set; }
        //}
    }
}
