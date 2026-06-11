using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.ESS
{
    public class EmpLeaveRequestBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class ESSLeaveRequestMaster
        {
            [XmlElement("ESL_PK")]
            public int ESL_PK { get; set; }
            [XmlElement("ESL_DATE")]
            public DateTime ESL_DATE { get; set; }
            [XmlElement("ESL_EMPLOYEE")]
            public int ESL_EMPLOYEE { get; set; }
            [XmlElement("ESL_EMPLOYEE_TEXT")]
            public string ESL_EMPLOYEE_TEXT { get; set; }
            [XmlElement("ESL_LEAVE_TYPE")]
            public int ESL_LEAVE_TYPE { get; set; }
            [XmlElement("ESL_LEAVE_TYPE_TEXT")]
            public string ESL_LEAVE_TYPE_TEXT { get; set; }
            [XmlElement("ESL_LV_FROM_DT")]
            public DateTime ESL_LV_FROM_DT { get; set; }
            [XmlElement("ESL_LV_TO_DT")]
            public DateTime ESL_LV_TO_DT { get; set; }
            [XmlElement("ESL_LV_FROM_HALF")]
            public int ESL_LV_FROM_HALF { get; set; }
            [XmlElement("ESL_LV_TO_HALF")]
            public int ESL_LV_TO_HALF { get; set; }
            [XmlElement("ESL_BIZUNIT")]
            public int ESL_BIZUNIT { get; set; }
            [XmlElement("ESL_COMPANY")]
            public int ESL_COMPANY { get; set; }
            [XmlElement("ESL_BRANCH")]
            public int ESL_BRANCH { get; set; }
            [XmlElement("ESL_DEPT")]
            public int ESL_DEPT { get; set; }
            [XmlElement("ESL_REASON")]
            public string ESL_REASON { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("ESL_STATUS")]
            public int ESL_STATUS { get; set; }
            [XmlElement("VALIDATE_LEAVE")]
            public int VALIDATE_LEAVE { get; set; }
            [XmlElement("LEAVE_SKIP")]
            public int LEAVE_SKIP { get; set; }
            [XmlElement("HOL_SKIP")]
            public int HOL_SKIP { get; set; }
            [XmlElement("OFFDAY_SKIP")]
            public int OFFDAY_SKIP { get; set; }


            [XmlElement("WKF_FLAG")]
            public int WKF_FLAG { get; set; }

            [XmlElement("WKF_REFERENCE")]
            public int WKF_REFERENCE { get; set; }
            [XmlElement("WKF_APPLICATION")]
            public int WKF_APPLICATION { get; set; }
            [XmlElement("WKF_PROCESS")]
            public int WKF_PROCESS { get; set; }
            [XmlElement("WKF_TASK")]
            public int WKF_TASK { get; set; }
            [XmlElement("WKF_TASK_ACTION")]
            public int WKF_TASK_ACTION { get; set; }
            [XmlElement("WKF_COMMENTS")]
            public string WKF_COMMENTS { get; set; }
            [XmlElement("WKF_TRX_FLAG")]
            public int WKF_TRX_FLAG { get; set; }


        }
    }
}
