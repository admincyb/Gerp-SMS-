using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.CommonManagement
{
    public class AttachmentBO
    {

        [XmlElement("DOC_PK")]
        public int PK { get; set; }
        [XmlElement("DOC_NAME")]
        public string FileName { get; set; }
        [XmlElement("DOC_TITLE")]
        public string Title { get; set; }

        [XmlElement("DOC_DESC")]
        public string FileDescription { get; set; }

        [XmlElement("DOC_PATH")]
        public string FilePath { get; set; }
        [XmlElement("DOC_MODULE")]
        public int ModuleID { get; set; }
        [XmlElement("DOC_TASK")]
        public int Task { get; set; }
        [XmlElement("DOC_TASK_ID")]
        public int TaskID { get; set; }
        [XmlElement("DOC_SEQ_NO")]
        public int DocumentNo { get; set; }
    }
}
