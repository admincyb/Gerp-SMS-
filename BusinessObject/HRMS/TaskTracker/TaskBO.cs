using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.TaskTracker
{
    public class TaskBO
    {
        public int? ROW_NO { get; set; }
        public int? TSK_PK { get; set; }
        public int? TSK_PARENT { get; set; }
        public DateTime? TSK_DATE { get; set; }
        public string TSK_NO { get; set; }
        public DateTime? TSK_EXP_DATE { get; set; }
        public DateTime? TSK_EST_DATE { get; set; }
        public DateTime? TSK_CMP_DATE { get; set; }
        public int TSK_CATEGORY { get; set; }
        public string TSK_CATEGORY_TEXT { get; set; }
        public string TSK_NAME { get; set; }
        public string TSK_DESC { get; set; }
        public int? TSK_ASSIGN_TO { get; set; }
        public string TSK_ASSIGN_TO_TEXT { get; set; }
        public int? TSK_TRX_STATUS { get; set; }
        public int? TSK_ACTIVE { get; set; }
        public int? TSK_STATUS { get; set; }
        public int? REC_COUNT { get; set; }
        public string TSK_STATUS_TEXT { get; set; }
        public int TSK_BIZUNIT { get; set; }
        public int TSK_DEPT { get; set; }
        public int TSK_COMPANY { get; set; }
        public int TSK_CRTD_BY { get; set; }
        public DateTime? TSK_CRTD_DT { get; set; }
        public int TSK_MOD_BY { get; set; }
        public DateTime? TSK_MOD_DT { get; set; }
    }

    public class SingleTaskAndSubTasks
    {
        public TaskBO MainTask { get; set; }
        public List<TaskBO> SubTasks { get; set; }
    }

    public class TaskHistory
    {
    }

    //Nidhin-For Save Task Information
    [Serializable]
    [XmlRoot("Root")]
    public class TaskInfo
    {
        [XmlElement("TSK_PK")]
        public int TSK_PK { get; set; }
        [XmlElement("TSK_PARENT")]
        public string TSK_PARENT { get; set; }
        [XmlElement("TSK_DATE")]
        public string TSK_DATE { get; set; }
        [XmlElement("TSK_NO")]
        public string TSK_NO { get; set; }
        [XmlElement("TSK_EXP_DATE")]
        public string TSK_EXP_DATE { get; set; }
        [XmlElement("TSK_EST_DATE")]
        public string TSK_EST_DATE { get; set; }
        [XmlElement("TSK_CMP_DATE")]
        public string TSK_CMP_DATE { get; set; }
        [XmlElement("TSK_CATEGORY")]
        public string TSK_CATEGORY { get; set; }
        [XmlElement("TSK_NAME")]
        public string TSK_NAME { get; set; }
        [XmlElement("TSK_DESC")]
        public string TSK_DESC { get; set; }
        [XmlElement("TSK_ASSIGN_TO")]
        public string TSK_ASSIGN_TO { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("TSK_CATEGORY_GROUP")]
        public int TSK_CATEGORY_GROUP { get; set; }
 
    }

     //Nidhin-For Save Status
    [Serializable]
    [XmlRoot("Root")]
    public class TaskStatus
    {
        [XmlElement("TKS_PK")]
        public int TKS_PK { get; set; }
        [XmlElement("TKS_TASK")]
        public string TKS_TASK { get; set; }
        [XmlElement("TKS_DATE")]
        public string TKS_DATE { get; set; }
        [XmlElement("TKS_TRX_STATUS")]
        public string TKS_TRX_STATUS { get; set; }
        [XmlElement("TKS_REMARKS")]
        public string TKS_REMARKS { get; set; }
        [XmlElement("TKS_EXP_DATE")]
        public string TKS_EXP_DATE { get; set; }
        [XmlElement("TKS_STATUS")]
        public string TKS_STATUS { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public string BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public string ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public string USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
    }

    //Nidhin For
    [Serializable]
    [XmlRoot("Root")]
    public class TaskCategoryDetails
    {
        [XmlElement("TCT_PK")]
        public int TCT_PK { get; set; }
        [XmlElement("TCT_CODE")]
        public string TCT_CODE { get; set; }
        [XmlElement("TCT_NAME")]
        public string TCT_NAME { get; set; }
        [XmlElement("TCT_DESC")]
        public string TCT_DESC { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("TCT_GROUP")]
        public string TCT_GROUP { get; set; }

        [XmlElement("Detail")]
        public List<TaskDetail> Detail { get; set; }
    }

    [Serializable]
    public class TaskDetail
    {
        [XmlElement("TCI_PK")]
        public int TCI_PK { get; set; }
        [XmlElement("TCI_ITEM")]
        public string TCI_ITEM { get; set; }
        [XmlElement("TCI_DESC")]
        public string TCI_DESC { get; set; }
        [XmlElement("TCI_DURATION")]
        public short TCI_DURATION { get; set; }
        [XmlElement("TCI_SEQUENCE")]
        public short TCI_SEQUENCE { get; set; }
        [XmlElement("TCI_ACTIVE")]
        public byte TCI_ACTIVE { get; set; }                                     

    }
}
