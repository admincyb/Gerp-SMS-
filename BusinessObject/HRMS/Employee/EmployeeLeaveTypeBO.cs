using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeLeaveTypeBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmpLeaveType
    {
        [XmlElement("empPK")]
        public int empPK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }     
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<LeaveTypeDetails> EmployeeLeaveTypeList { get; set; }
    }
    [Serializable]
    public class LeaveTypeDetails
    {
        [XmlElement("ELV_PK")]
        public int ELV_PK { get; set; }
        [XmlElement("ELV_LEAVE_TYPE")]
        public int ELV_LEAVE_TYPE { get; set; }
        [XmlElement("ELV_LIMIT")]
        public double ELV_LIMIT { get; set; }
        [XmlElement("ELV_ACTIVE")]
        public short ELV_ACTIVE { get; set; }        
    }
}
