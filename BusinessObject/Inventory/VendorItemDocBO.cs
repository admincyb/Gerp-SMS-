using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.Inventory
{
    [Serializable]
    [XmlRoot("Root")]
    public class VendorItemDocBO
    {
        [XmlElement("ITV_PK")]
        public string ITV_PK { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }

        [XmlElement("Detail")]
        public List<VendorDetailsBO> DetailList { get; set; }
    }

    [Serializable]
    public class VendorDetailsBO
    {
        [XmlElement("IVD_PK")]
        public int IVD_PK { get; set; }

        [XmlElement("IVD_DOC_CATEGORY")]
        public string IVD_DOC_CATEGORY { get; set; }

        [XmlElement("IVD_DOC_CATEGORY_TEXT")]
        public string IVD_DOC_CATEGORY_TEXT { get; set; }
          
        [XmlElement("IVD_DOC_TITLE")]
        public string IVD_DOC_TITLE { get; set; }
										
        [XmlElement("IVD_DOC_NAME")]
        public string IVD_DOC_NAME { get; set; }
										
        [XmlElement("IVD_VERSION")]
        public string IVD_VERSION { get; set; }
										
        [XmlElement("IVD_DESC")]
        public string IVD_DESC { get; set; }
										
        [XmlElement("IVD_DOC_PATH")]
        public string IVD_DOC_PATH { get; set; }
										
        [XmlElement("IVD_DOC_TYPE")]
        public string IVD_DOC_TYPE { get; set; }

        [XmlElement("IVD_ACTIVE")]
        public int IVD_ACTIVE { get; set; }

        [XmlElement("IVD_SL_NO")]
        public int ListSlNo { get; set; }

        public string AttachmentFileName { get; set; } 
    }

     
    public class VendorFileDetails
    {
        public int SlNo
        {
            get;
            set;
        }
        public HttpPostedFile VendorFile
        {
            get;
            set;
        }

    }
}
