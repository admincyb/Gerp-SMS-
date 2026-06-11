using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class StockHeader
    {
        [XmlElement("LSH_PK")]
        public int LSH_PK { get; set; }
        [XmlElement("LSH_NO")]
        public string LSH_NO { get; set; }
        [XmlElement("LSH_DATE")]
        public string LSH_DATE { get; set; }
        [XmlElement("LSH_AS_ON_DATE")]
        public string LSH_AS_ON_DATE { get; set; }
        [XmlElement("LSH_DESC")]
        public string LSH_DESC { get; set; }
        [XmlElement("LSH_TRX_CURR")]
        public int LSH_TRX_CURR { get; set; }
        [XmlElement("LSH_TRX_CURR_TEXT")]
        public string LSH_TRX_CURR_TEXT { get; set; }
        [XmlElement("LSH_BASE_CURR")]
        public int LSH_BASE_CURR { get; set; }
        [XmlElement("LSH_BASE_CURR_TEXT")]
        public string LSH_BASE_CURR_TEXT { get; set; }
        [XmlElement("LSH_EXCHG_RATE")]
        public double LSH_EXCHG_RATE { get; set; }
        [XmlElement("LSH_HAS_JRNL_ENTRY")]
        public byte LSH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement("LSH_STATUS")]
        public string LSH_STATUS { get; set; }
        [XmlElement("LSH_DEL_STATUS")]
        public byte LSH_DEL_STATUS { get; set; }
        [XmlElement("LSH_DEPT")]
        public int LSH_DEPT { get; set; }
        [XmlElement("LSH_COMPANY")]
        public int LSH_COMPANY { get; set; }
        [XmlElement("LSH_BIZUNIT")]
        public int LSH_BIZUNIT { get; set; }
        [XmlElement("LSH_CRTD_BY")]
        public int LSH_CRTD_BY { get; set; }
        [XmlElement("LSH_CRTD_DT")]
        public string LSH_CRTD_DT { get; set; }
        [XmlElement("LSH_MOD_BY")]
        public int LSH_MOD_BY { get; set; }
        [XmlElement("LSH_ACTIVE")]
        public int LSH_ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }      
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }   
        [XmlElement("Detail")]
        public List<StockItems> StockItemList { get; set; }
        
    }
    [Serializable]
    public class StockItems
    {
        [XmlElement("LSD_PK")]
        public int LSD_PK { get; set; }
        [XmlElement("LSD_ITEM_CAT")]
        public int LSD_ITEM_CAT { get; set; }
        [XmlElement("ITC_NAME")]
        public string ITC_NAME { get; set; }
        [XmlElement("LSD_ACCOUNT_PUR")]
        public int LSD_ACCOUNT_PUR { get; set; }
        [XmlElement("LSD_ACCOUNT_INV")]
        public string LSD_ACCOUNT_INV { get; set; }
        [XmlElement("LSD_PURCHASE")]
        public double LSD_PURCHASE { get; set; }
        [XmlElement("LSD_STOCK")]
        public double LSD_STOCK { get; set; }
        [XmlElement("LSD_CLOSING")]
        public double LSD_CLOSING { get; set; }       
        [XmlElement("LSD_ACTIVE")]
        public int LSD_ACTIVE { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ClosingStockBO
  {
        [XmlElement("IOH_FIN_YEAR")]
        public int Ioh_Fin_YearPK { get; set; }
        [XmlElement("IOH_BIZUNIT")]
        public int Ioh_Bizunit { get; set; }
        [XmlElement("IOH_PK")]
        public int Ioh_PK { get; set; }
        [XmlElement("IOH_DEPT")]
        public int Ioh_DeptPK { get; set; }

        [XmlElement("USER_PK")]
        public int UserPK { get; set; }
        [XmlElement("WKF_FLAG")]
        public int Wkf_flag { get; set; }

        [XmlElement("IOH_DATE")]
        public DateTime Ioh_Date { get; set; }
        [XmlElement("Dept")]
        public List<StockDepartment> lstDepartments { get; set; }
    }
    [Serializable]
    public class StockDepartment
    {
        
        [XmlElement("P_IOH_PK")]
        public int Ioh_pk { get; set; }
        [XmlElement("IOS_DEPT")]
        public int Dept_pk { get; set; }
        [XmlElement("IOS_DEPT_TEXT")]
        public string Dept_Text { get; set; }
        [XmlElement("IOS_BIZUNIT")]
        public int Bizunit { get; set; }
        [XmlElement("IOS_FIN_YEAR")]
        public int FinYr_pk { get; set; }
        [XmlElement("IOS_FIN_YEAR_TEXT")]
        public string FinYr_Text { get; set; }

        //[XmlElement("WKF_FLAG")]
        //public int WKF_FLAG { get; set; }

        [XmlElement("IOS_NO")]
        public string Trn_No { get; set; }
        
        [XmlElement("ItemCategory")]
        public List<StockItemCategory> lstItemCategory { get; set; }

    }
    [Serializable]
    public class StockItemCategory
    {
        public int Dept_pk { get; set; }
        [XmlElement("IOS_CATEGORY")]
        public int ItemCat_Pk { get; set; }
        [XmlElement("IOS_CATEGORY_TEXT")]
        public string ItemCat_Text { get; set; }

        [XmlElement("Item")]
        public List<StockItem> lstItem { get; set; }

    }
    [Serializable]
    public class StockItem
    {
        [XmlElement("IOS_ITEM")]
        public int Item_pk { get; set; }
        [XmlElement("IOS_ITEM_TEXT")]
        public string Item_Text { get; set; }
        [XmlElement("IOS_OPENING_STK")]
        public double Opening_Stock { get; set; }
        
        [XmlElement("IOS_OPENING_VAL")]
        public double Item_rate { get; set; }
        

    }
    }
