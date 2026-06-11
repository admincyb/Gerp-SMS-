using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.Administration.Masters
{
    public class FundRequisitionDeptBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class FundRequisitionDeptHeader
    {
        [XmlElement("DFH_PK")]
        public int DFH_PK { get; set; }
        [XmlElement("DFH_NO")]
        public string DFH_NO { get; set; }
        [XmlElement("DFH_DATE")]
        public DateTime DFH_DATE { get; set; }
        [XmlElement("DFH_REF_NO")]
        public string DFH_REF_NO { get; set; }
        [XmlElement("DFH_REF_DATE")]
        public string DFH_REF_DATE { get; set; }
        [XmlElement("DFH_REQ_DEPT")]
        public int DFH_REQ_DEPT { get; set; }
        [XmlElement("DFH_REQ_DEPT_TEXT")]
        public string DFH_REQ_DEPT_TEXT { get; set; }
        [XmlElement("DFH_DESC")]
        public string DFH_DESC { get; set; }
        [XmlElement("DFH_TOTAL")]
        public decimal DFH_TOTAL { get; set; }
        [XmlElement("DFH_CURR")]
        public int DFH_CURR { get; set; }
        public string DFH_CURR_TEXT { get; set; }
        [XmlElement("DFH_EXCH_RATE")]
        public double DFH_EXCH_RATE { get; set; }
        [XmlElement("DFH_BASE_CURR")]
        public int DFH_BASE_CURR { get; set; } 
        [XmlElement("DFH_STATUS")]
        public int DFH_STATUS { get; set; }
        [XmlElement("DFH_IS_DELETED")]
        public int DFH_IS_DELETED { get; set; }
        [XmlElement("DFH_BIZUNIT")]
        public int DFH_BIZUNIT { get; set; }
        [XmlElement("DFH_DEPT")]
        public int DFH_DEPT { get; set; }
        [XmlElement("DFH_COMPANY")]
        public int DFH_COMPANY { get; set; }

        [XmlElement("DFH_CRTD_BY")]
        public int DFH_CRTD_BY { get; set; }
        [XmlElement("DFH_CRTD_DT")]
        public DateTime DFH_CRTD_DT { get; set; }
        [XmlElement("DFH_MOD_BY")]
        public int DFH_MOD_BY { get; set; }
        [XmlElement("DFH_MOD_DT")]
        public DateTime DFH_MOD_DT { get; set; }   

        [XmlElement("DFH_STATUS_TEXT")]
        public string DFH_STATUS_TEXT { get; set; }
        [XmlElement("DFH_CSS_CLASS")]
        public string DFH_CSS_CLASS { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
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


        [XmlElement("Detail")]
        public List<FundRequisitionDeptDetails> Details { get; set; }

        [XmlElement("FileList")]
        public List<FundRequisitionDeptDetailsUploads> FileList { get; set; }

    }

    [Serializable]
    public class FundRequisitionDeptDetails
    {
        [XmlElement("DFD_PK")]
        public int DFD_PK { get; set; }
        [XmlElement("DFD_HDR_PK")]
        public int DFD_HDR_PK { get; set; }      
        [XmlElement("DFD_HEAD")]
        public int DFD_HEAD { get; set; }
        [XmlElement("DFD_HEAD_TEXT")]
        public string DFD_HEAD_TEXT { get; set; }
        [XmlElement("DFD_DESC")]
        public string DFD_DESC { get; set; }
        [XmlElement("DFD_REQ_AMT")]
        public decimal DFD_REQ_AMT { get; set; }
        [XmlElement("DFD_APP_AMT")]
        public decimal DFD_APP_AMT { get; set; }
        [XmlElement("DFD_REMARKS")]
        public string DFD_REMARKS { get; set; }
        [XmlElement("DFD_SEQUENCE")]
        public int DFD_SEQUENCE { get; set; }      
    }

    public class FileDetails
    {
        public int SlNo
        {
            get;
            set;
        }
        public HttpPostedFile PoFile
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FundRequisitionDeptDetailsUploads
    {
        [XmlElement("DOC_PK")]
        public int DOC_PK { get; set; }
        [XmlElement("DOC_SEQ_NO")]
        public int DOC_SEQ_NO { get; set; }
        [XmlElement("DOC_TITLE")]
        public string DOC_TITLE { get; set; }
        [XmlElement("DOC_NAME")]
        public string DOC_NAME { get; set; }
        [XmlElement("DOC_PATH")]
        public string DOC_PATH { get; set; }
        [XmlElement("DOC_TYPE")]
        public string DOC_TYPE { get; set; }
        [XmlElement("DOC_ACTIVE")]
        public int DOC_ACTIVE { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }
}
