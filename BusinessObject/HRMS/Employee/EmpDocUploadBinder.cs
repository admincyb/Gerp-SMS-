using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    public class EmpDocUploadBinder
    {

        [XmlElement("DOC_PK")]
        public int DOC_PK { get; set; }

        [XmlElement("DOC_SEQ_NO")]
        public int DOC_SEQ_NO { get; set; }

        /// <summary>
        /// Extension
        /// </summary>
        [XmlElement("DOC_TYPE")]
        public string DOC_TYPE { get; set; }

        /// <summary>
        /// FileNameWithoutExtension
        /// </summary>
        [XmlElement("DOC_TITLE")]
        public string DOC_TITLE { get; set; }

        /// <summary>
        /// FileNameWithExtension
        /// </summary>
        [XmlElement("DOC_NAME")]
        public string DOC_NAME { get; set; }

        /// <summary>
        /// File Path
        /// </summary>
        [XmlElement("DOC_PATH")]
        public string DOC_PATH { get; set; }

        [XmlElement("DOC_ACTIVE")]
        public short DOC_ACTIVE { get; set; }

        public string AttachmentFileName { get; set; }
    }

    public class FileDetails
    {
        public int SlNo { get; set; }
        public HttpPostedFile EmpDocFile { get; set; }
    }
}
