using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web;

namespace BusinessObject.Shipping
{
    [Serializable]
    [XmlRoot("Root")]
    public class ShippingUploadsBOHeader
    {
        [XmlElement("ShippingUploadsBO")]
        public List<ShippingUploadsBO> ShippingUploadsBOList { get; set; }
    }

    [Serializable]
    public class ShippingUploadsBO
    {
        [XmlElement("SCD_PK")]
        public int SCD_PK { get; set; }
        [XmlElement("SCD_SL_NO")]
        public int SCD_SL_NO { get; set; }
        [XmlElement("SCD_PLAN_HDR")]
        public int SCD_PLAN_HDR { get; set; }
        [XmlElement("SCD_PLAN_HDR_NO")]
        public string SCD_PLAN_HDR_NO { get; set; }
        [XmlElement("SCD_TYPE")]
        public byte SCD_TYPE { get; set; }
        [XmlElement("SCD_ITEM")]
        public int SCD_ITEM { get; set; }
        [XmlElement("SCD_ITEM_Text")]
        public string SCD_ITEM_Text { get; set; }
        [XmlElement("SCD_DATE")]
        public string SCD_DATE { get; set; }
        [XmlElement("SCD_TITLE")]
        public string SCD_TITLE { get; set; }
        [XmlElement("SCD_DESC")]
        public string SCD_DESC { get; set; }
        [XmlElement("SCD_FILE")]
        public string SCD_FILE { get; set; }
        [XmlElement("SCD_FILE_PATH")]
        public string SCD_FILE_PATH { get; set; }
        [XmlElement("SCD_ACTIVE")]
        public byte SCD_ACTIVE { get; set; }
        [XmlElement("SCD_MOD_BY")]
        public int SCD_MOD_BY { get; set; }
        [XmlElement("SCD_MOD_DT")]
        public DateTime SCD_MOD_DT { get; set; }
        [XmlElement("SCD_COMPANY")]
        public int SCD_COMPANY { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
     

        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class BillofLoadingBO
    {
        [XmlElement("BLD_PK")]
        public int BLD_PK { get; set; }
        [XmlElement("BLD_SHIPPING_PLAN")]
        public int BLD_SHIPPING_PLAN { get; set; }
        [XmlElement("BLD_COMPANY")]
        public int BLD_COMPANY { get; set; }
        [XmlElement("BLD_NO")]
        public string  BLD_NO { get; set; }
        [XmlElement("BLD_DATE")]
        public DateTime BLD_DATE { get; set; }
        [XmlElement("BLD_REMARKS")]
        public string BLD_REMARKS { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("BLD_DEPT")]
        public int BLD_DEPT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }

        public string INV_NO { get; set; }
        public string INV_DATE { get; set; }
        public string CONTAINER_NO { get; set; }
        public string SHIP_TO_PORT { get; set; }
        public string IN_TIME { get; set; }
        public string BOOKING_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        [XmlElement("DocDetails")]
        public List<BillofLoadingUploadBO> DocDetails { get; set; }
       
    }

    [Serializable]
    public class BillofLoadingUploadBO
    {
        [XmlElement("SCD_PK")]
        public int SCD_PK { get; set; }
        [XmlElement("SCD_FILE")]
        public string SCD_FILE { get; set; }
        [XmlElement("SCD_TYPE")]
        public int SCD_TYPE { get; set; }
        [XmlElement("SCD_FILE_PATH")]
        public string  SCD_FILE_PATH { get; set; }
        [XmlElement("SCD_DOC_TYPE")]
        public int SCD_DOC_TYPE { get; set; }
        [XmlElement("SCD_DOC_TYPE_TEXT")]
        public string SCD_DOC_TYPE_TEXT { get; set; }
        [XmlElement("SCD_TITLE")]
        public string SCD_TITLE { get; set; }
        [XmlElement("SCD_SEQUENCE")]
        public int SCD_SEQUENCE { get; set; }
        [XmlElement("SCD_ACTIVE")]
        public int SCD_ACTIVE { get; set; }
        public string BLD_REMARKS { get; set; }
        public string BLD_DATE { get; set; }
        public string  BLD_NO { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }


    public class FileDetails
    {
        public int SlNo
        {
            get;
            set;
        }
        public HttpPostedFile ShippingFile
        {
            get;
            set;
        }

    }
}
