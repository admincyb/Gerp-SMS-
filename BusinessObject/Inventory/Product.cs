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
    public class ProductBO
    {
        [XmlElement("Detail")]
        public List<DetailsBO> DetailList { get; set; }
    }
    [Serializable]
    public class DetailsBO
    {
        [XmlElement("PGP_PRODUCT_GROUP")]
        public int? ProductGroupPK { get; set; }
        [XmlElement("PGP_PRODUCT")]
        public int ProductPK { get; set; }
        [XmlElement("PGP_ACTIVE")]
        public int Active { get; set; }

    }

    [Serializable]
    [XmlRoot("Root")]
    public class ProductLineBO
    {
        [XmlElement("Plant")]
        public List<PlantListBO> Plants { get; set; }
    }

    [Serializable]
    public class PlantListBO
    { 
        [XmlElement("PLT_PK")]
        public int PLT_PK { get; set; }
        [XmlElement("PLT_NAME")]
        public string PLT_NAME { get; set; }
        [XmlElement("PLT_CODE")]
        public string PLT_CODE { get; set; }

        [XmlElement("Line")]
        public List<PlantLineBO> Line { get; set; }
    }

    [Serializable]
    public class PlantLineBO
    {
        [XmlElement("LNE_PK")]
        public int LNE_PK { get; set; }
        [XmlElement("LNE_NAME")]
        public string LNE_NAME { get; set; }
        [XmlElement("LNE_CODE")]
        public string LNE_CODE { get; set; }
        [XmlElement("LNE_MAPPED")]
        public int LNE_MAPPED { get; set; }
        [XmlElement("PLM_PK")]
        public int PLM_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ProductLineSaveBO
    {
        [XmlElement("PLM_PRODUCT")]
        public int PLM_PRODUCT { get; set; }
        [XmlElement("Details")]
        public List<ProductLineSaveDtlsBO> Lines { get; set; }
    }

    [Serializable]
    public class ProductLineSaveDtlsBO
    { 
        [XmlElement("PLM_LINE")]
        public int PLM_LINE { get; set; } 
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ProductStoreSaveBO
    {
        [XmlElement("PSM_PRODUCT")]
        public int PSM_PRODUCT { get; set; }

        [XmlElement("Bizunit")]
        public int Bizunit { get; set; }

        [XmlElement("UserPk")]
        public int UserPk { get; set; }

        [XmlElement("Details")]
        public List<ProductStoreSaveDtlsBO> Stores { get; set; }
    }

    [Serializable]
    public class ProductStoreSaveDtlsBO
    {
        [XmlElement("STORE_PK")]
        public int STORE_PK { get; set; }
    }



    [Serializable]
    [XmlRoot("Root")]
    public class PlanningBO
    {
        [XmlElement("PIG_PK")]
        public int? PIG_PK { get; set; }

        [XmlElement("PIG_CODE")]
        public string PIG_CODE { get; set; }

        [XmlElement("PIG_NAME")]
        public string PIG_NAME { get; set; }

        [XmlElement("PIG_DESC")]
        public string PIG_DESC { get; set; }

        [XmlElement("ACTIVE")]
        public string ACTIVE { get; set; }

        [XmlElement("BIZUNIT")]
        public string BIZUNIT { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }

        [XmlElement("Detail")]
        public List<PlanningDetailsBO> DetailList { get; set; }
    }
    [Serializable]
    public class PlanningDetailsBO
    {
        [XmlElement("ITM_PK")]
        public int? ITM_PK { get; set; }
        [XmlElement("IS_MAPPED")]
        public int? IS_MAPPED { get; set; }
    }
    public class PlanningHeader
    {
        public int PIG_PK { get; set; }
        public string PIG_CODE { get; set; }
        public string PIG_NAME { get; set; }
        public string PIG_DESC { get; set; }
        public string PIG_ACTIVE { get; set; }
        public string PIG_BIZUNIT { get; set; }
        public int USER_PK { get; set; }
        public string LAST_MOD_DT { get; set; }
    }
    [Serializable]
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
}
