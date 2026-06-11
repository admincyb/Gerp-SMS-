using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class DocumentRevisionBO
    {
        [XmlElement("DRM_PK")]
        public int DRM_PK { get; set; }
        [XmlElement("DRM_DOC_PK")]
        public int DRM_DOC_PK { get; set; }
        [XmlElement("DRM_DOC_NAME")]
        public string DRM_DOC_NAME { get; set; }
        [XmlElement("DRM_DOC_TYPE")]
        public int DRM_DOC_TYPE { get; set; }
        [XmlElement("DRM_DOC_NO")]
        public string DRM_DOC_NO { get; set; }

        [XmlElement("DRM_REVISION")]
        public int DRM_REVISION { get; set; }
        [XmlElement("DRM_REV_DT")]
        public DateTime DRM_REV_DT { get; set; }
        [XmlElement("DRM_DAR_NO")]
        public string DRM_DAR_NO { get; set; }
        [XmlElement("DRM_USER_PK")]
        public int DRM_USER_PK { get; set; } 

        [XmlElement("DRM_FROM_DT")]
        public DateTime DRM_FROM_DT { get; set; }

        [XmlElement("DRM_TO_DT")]
        public DateTime DRM_TO_DT { get; set; }

        [XmlElement("DRM_ACTIVE")]
        public string DRM_ACTIVE { get; set; }

        [XmlElement("DRM_MOD_DT")]
        public DateTime DRM_MOD_DT { get; set; }

        [XmlElement("DRM_MOD_BY")]
        public int DRM_MOD_BY { get; set; }

        [XmlElement("DRM_SL_NO")]
        public int DRM_SL_NO { get; set; }

        [XmlElement("DRM_CRT_BY")]
        public int DRM_CRT_BY { get; set; }

        [XmlElement("DRM_CRT_DT")]
        public DateTime DRM_CRT_DT { get; set; }

        [XmlElement("IS_EDITABLE")]
        public int IS_EDITABLE { get; set; }


    }
}
