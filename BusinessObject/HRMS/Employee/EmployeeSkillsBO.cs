using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeSkillsBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeSkillsDetails
    {
        [XmlElement("EMPLOYEE_PK")]
        public int EMPLOYEE_PK { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public short BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public byte ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<SkillDetails> listEmployeeSkillsList { get; set; }
    }
    [Serializable]
    public class SkillDetails
    {
        [XmlElement("ESD_PK")]
        public int ESD_PK { get; set; }
        [XmlElement("ESD_SKILL_CATEGORY")]
        public int ESD_SKILL_CATEGORY { get; set; }
        [XmlElement("ESD_SKILL_FLAG")]
        public int ESD_SKILL_FLAG { get; set; }
        [XmlElement("ESD_SKILL")]
        public string ESD_SKILL { get; set; }
        [XmlElement("ESD_SKILL_TEXT")]
        public string ESD_SKILL_TEXT { get; set; }
        [XmlElement("ESD_EXP_YEAR")]
        public string ESD_EXP_YEAR { get; set; }
        //[XmlElement("ESD_EXP_YEAR")]
        //public decimal ESD_EXP_YEAR { get; set; }
        [XmlElement("ESD_EXPERT_LEVEL")]
        public int ESD_EXPERT_LEVEL { get; set; }
        [XmlElement("ESD_RATING")]
        public string ESD_RATING { get; set; }
        [XmlElement("ESD_REMARKS")]
        public string ESD_REMARKS { get; set; }
    }
}
