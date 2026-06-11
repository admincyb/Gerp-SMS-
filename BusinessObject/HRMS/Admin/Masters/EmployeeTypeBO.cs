using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeTypeBO
    {
        [XmlElement("EMT_PK")]
        public int PK { get; set; }

        [XmlElement("EMT_CODE")]
        public string TypeCode { get; set; }

        [XmlElement("EMT_NAME")]
        public string TypeName { get; set; }

        [XmlElement("EMT_BRANCH")]
        public string BranchOrLocation { get; set; }

        [XmlElement("EMT_LEAVE_TEMP")]
        public string LeaveTemplate { get; set; }

        [XmlElement("EMT_OT_TEMP")]
        public string OTTemplate { get; set; }

        [XmlElement("EMT_OT_AVAILABE")]
        public string OTAvailable { get; set; }

        [XmlElement("EMT_BRANCH_TEXT")]
        public string BranchOrLocationText { get; set; }

        [XmlElement("EMT_DEPT")]
        public int DeptPk { get; set; }

        [XmlElement("EMT_COMPANY")]
        public int Company { get; set; }

        [XmlElement("EMT_LEAVE_TEMP_TEXT")]
        public string LeaveTemplateText { get; set; }

        [XmlElement("EMT_OT_TEMP_TEXT")]
        public string OTTemplateText { get; set; }

        [XmlElement("EMT_WORKING_DAY_TYPE")]
        public short WorkingDayType { get; set; }

        [XmlElement("EMT_WORKING_DAY")]
        public string WorkingDays { get; set; }

        [XmlElement("EMT_WORKING_HRS")]
        public string NormalWorkingHrs { get; set; }

        [XmlElement("EMT_OT_RATE")]
        public string OTRate { get; set; }

        [XmlElement("EMT_BREAK_TIME")]
        public string BreakTime { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public short Active { get; set; }

        [XmlElement("USER_PK")]
        public int User { get; set; }       

        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModifiedDate { get; set; }

        [XmlElement("Details")]
        public List<WorkingHours> WorkingHours { get; set; }
    }
}
