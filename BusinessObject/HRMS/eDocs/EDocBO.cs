using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.eDocs
{
    [Serializable]
    [XmlRoot("Root")]
    public class EDocBO
    {
        [XmlElement("DCH_PK")]
        public long DCH_PK { get; set; }

        [XmlElement("DCH_NO")]
        public string DCH_NO { get; set; }

        [XmlElement("DCH_LETTER_NO")]
        public string DCH_LETTER_NO { get; set; }

        [XmlElement("DCH_LETTER_FROM")]
        public string DCH_LETTER_FROM { get; set; }

        [XmlElement("DCH_LETTER_TO")]
        public string DCH_LETTER_TO { get; set; }

        [XmlElement("DCH_DATE")]
        public string DCH_DATE { get; set; }

        [XmlElement("DCH_SITE")]
        public int DCH_SITE { get; set; }

        [XmlElement("DCH_SUBJECT")]
        public string DCH_SUBJECT { get; set; }

        [XmlElement("DCH_TAGS")]
        public string DCH_TAGS { get; set; }

        [XmlElement("DCH_TRX_DEPT")]
        public string DCH_TRX_DEPT { get; set; }

        /// <summary>
        /// 0 Draft, 1 Pending, 2 Complete
        /// </summary>
        [XmlElement("DCH_TRX_STATUS")]
        public int DCH_TRX_STATUS { get; set; }

        [XmlElement("DCT_TO_USER")]
        public int DCT_TO_USER { get; set; }

        [XmlElement("DCH_FOLDER")]
        public string DCH_FOLDER { get; set; }

        /// <summary>
        /// 0 => Normal 1 => Private
        /// </summary>
        [XmlElement("DCT_COMMENT_TYPE")]
        public int DCT_COMMENT_TYPE { get; set; }

        [XmlElement("DCT_COMMENT")]
        public string DCT_COMMENT { get; set; }

        [XmlElement("DCT_DESC")]
        public string DCT_DESC { get; set; }

        [XmlElement("DCH_IS_EDIT")]
        public int DCH_IS_EDIT { get; set; }

        [XmlElement("Detail")]
        public List<EDocDetailBO> Detail { get; set; }

        [XmlElement("HistoryDetail")]
        public List<EDocComment> Comments { get; set; }

        [XmlElement("DCH_DEPT")]
        public int DCH_DEPT { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("DCH_ACTIVE")]
        public int DCH_ACTIVE { get; set; }

        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("UserDetail")]
        public List<EDocSendToBO> EDocSendUsers { get; set; }

    }

    [Serializable]
    public class EDocDetailBO
    {
        [XmlElement("DCD_PK")]
        public int DCD_PK { get; set; }

        [XmlElement("DCD_TITLE")]
        public string DCD_TITLE { get; set; }

        [XmlElement("DCD_DESC")]
        public string DCD_DESC { get; set; }

        [XmlElement("DCD_FILE")]
        public string DCD_FILE { get; set; }

        [XmlElement("DCD_FILE_PATH")]
        public string DCD_FILE_PATH { get; set; }

        [XmlElement("DCD_SEQUENCE")]
        public int DCD_SEQUENCE { get; set; }
    }

    [Serializable]
    public class EDocComment
    {
        [XmlElement("DCT_PK")]
        public int DCT_PK { get; set; }

        [XmlElement("DCT_COMMENT")]
        public string DCT_COMMENT { get; set; }

        [XmlElement("DCT_DESC")]
        public string DCT_DESC { get; set; }        

        [XmlElement("DCT_SEQUENCE")]
        public int DCT_SEQUENCE { get; set; }

        [XmlElement("DCT_MOD_BY")]
        public int DCT_MOD_BY { get; set; }

        [XmlElement("DCT_MODE_TEXT")]
        public string DCT_MODE_TEXT { get; set; }

        [XmlElement("DCT_TO_USER_TEXT")]
        public string DCT_TO_USER_TEXT { get; set; }

        [XmlElement("DCT_MOD_BY_TEXT")]
        public string DCT_MOD_BY_TEXT { get; set; }

        [XmlElement("DCT_MOD_DT")]
        public string DCT_MOD_DT { get; set; }
    }

    [Serializable]
    public class EDocSendToBO
    {
        [XmlElement("DCT_TO_USER_CC")]
        public int DCT_TO_USER_CC { get; set; }

    }

    public struct EDocSearchParameter
    {
        /// <summary>
        /// Given value is search in all fields
        /// </summary>
        public string SearchText { get; set; }
        public string CurrentTab { get; set; }
        public string Comments { get; set; }
        public string Tags { get; set; }
        public int ProjectSite { get; set; }
        public int Department { get; set; }
        public string DocNo { get; set; }
        public string LetterNo { get; set; }
        public string Subject { get; set; }
        public int Status { get; set; } 
        public int DocStatus { get; set; }
        public int BizUnit { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Send { get; set; }
        public int SendTo { get; set; }

        public bool FileMode { get; set; }
        public string FileText { get; set; }
        public long Folder { get; set; }

        public GridPrams GridParams { get; set; }
    }
    public enum CommentType
    {
        Normal,
        Private
    }

    public enum TrxStatus
	{
        None = -1,
        Drafted = 0,
        Pending = 1,
        Completed = 2 
	}
}
