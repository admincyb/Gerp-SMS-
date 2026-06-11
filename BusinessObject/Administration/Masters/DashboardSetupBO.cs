using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class DashboardSetupBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class DashboardHeader
    {
        [XmlElement("IND_PK")]
        public int IND_PK { get; set; }
        [XmlElement("IND_CODE")]
        public string IND_CODE { get; set; }
        [XmlElement("IND_NAME")]
        public string IND_NAME { get; set; }
        [XmlElement("IND_MODULE")]
        public int IND_MODULE { get; set; }
        [XmlElement("IND_ACTIVE")]
        public int IND_ACTIVE { get; set; }
        [XmlElement("IND_SEQUENCE")]
        public int IND_SEQUENCE { get; set; }
        [XmlElement("IND_USER_GROUP")]
        public int IND_USER_GROUP { get; set; }
        [XmlElement("IND_MODE")]
        public int IND_MODE { get; set; }
        [XmlElement("IND_DESC")]
        public string IND_DESC { get; set; }
        [XmlElement("IND_TYPE")]
        public string IND_TYPE { get; set; }
        [XmlElement("IND_CLASS")]
        public string IND_CLASS { get; set; }

       
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("IND_USER")]
        public int IND_USER { get; set; }
        [XmlElement("IND_MOD_DT")]
        public DateTime IND_MOD_DT { get; set; }

        [XmlElement("Details")]
        public List<DashboardDetails> DashboardDetails { get; set; }
    }

    [Serializable]
    [XmlRoot("Details")]
    public class DashboardDetails
    {
        [XmlElement("DBD_SL_NO")]
        public int DBD_SL_NO { get; set; }
        [XmlElement("DBD_PK")]
        public int DBD_PK { get; set; }
        [XmlElement("DBD_NAME")]
        public string DBD_NAME { get; set; }
        [XmlElement("DBD_DESC")]
        public string DBD_DESC { get; set; }
        //[XmlElement("DBD_DASH_CFG")]
        //public int DBD_DASH_CFG { get; set; }
        [XmlElement("DBD_SEQUENCE")]
        public int DBD_SEQUENCE { get; set; }
        [XmlElement("DBD_ACTIVE")]
        public int DBD_ACTIVE { get; set; }
        [XmlElement("IS_DELETED")]
        public int IS_DELETED { get; set; }

        [XmlElement("Item_details")]
        public List<DashboardItemDetails> DashboardItemDetails { get; set; }


    }
    [Serializable]
    [XmlRoot("Item_details")]
    public class DashboardItemDetails
    {
        [XmlElement("DBL_PK")]
        public int DBL_PK { get; set; }
        [XmlElement("DBL_SL_NO")]
        public int DBL_SL_NO { get; set; }
        [XmlElement("DBL_SL_NO2")]
        public int DBL_SL_NO2 { get; set; }
        [XmlElement("DBL_NAME")]
        public string DBL_NAME { get; set; }
        [XmlElement("DBL_DASH_DTL")]
        public int DBL_DASH_DTL { get; set; }
        [XmlElement("DBL_MENU_CFG")]
        public int DBL_MENU_CFG { get; set; }
        [XmlElement("DBL_DPT")]
        public int DBL_DPT { get; set; }
        [XmlElement("DBL_DPT_TEXT")]
        public string DBL_DPT_TEXT { get; set; }
        [XmlElement("DBL_LINK")]
        public string DBL_LINK { get; set; }
        [XmlElement("DBL_ACTIVE")]
        public int DBL_ACTIVE { get; set; }
        [XmlElement("DBL_SEQUENCE")]
        public int DBL_SEQUENCE { get; set; }
        [XmlElement("DBL_TYPE")]
        public int DBL_TYPE { get; set; }
        [XmlElement("DBL_DASHLET")]  
        public int DBL_DASHLET { get; set; }
        [XmlElement("DLC_TYPE")]
        public int DLC_TYPE { get; set; }
        [XmlElement("DLC_TYPE_TEXT")]
        public string DLC_TYPE_TEXT { get; set; }
        [XmlElement("DLC_DESC")]
        public string DLC_DESC { get; set; }  
        [XmlElement("DBL_DEF_PK")]
        public string DBL_DEF_PK { get; set; }
        [XmlElement("DBL_DEF_NAME")]
        public string DBL_DEF_NAME { get; set; }
        [XmlElement("DLC_DTL_QUERY")]
        public string DLC_DTL_QUERY { get; set; }
        [XmlElement("Item_trx_details")]
        public List<ItemTrxDetails> lstItemTrxDetails { get; set; }
    }
      [Serializable]
      [XmlRoot("Item_trx_details")]
    public class ItemTrxDetails
    {
          [XmlElement("DBT_PK")]
          public string DBT_PK { get; set; }
          [XmlElement("DBT_TRX_PK")]
          public string DBT_TRX_PK { get; set; }
          [XmlElement("DBT_TRX_NAME")]
          public string DBT_TRX_NAME { get; set; }
          [XmlElement("DBL_SL_NO2")]
          public int DBL_SL_NO2 { get; set; } 
    }




    //[Serializable]
    //[XmlRoot("Root")]
    //public class MenuMappingDetailsHeader
    //{
    //    [XmlElement("MenuDet ails")]
    //    public List<MenuMappingDetails> MenuMappingDetails { get; set; }
    //}

    //[Serializable]
    //[XmlRoot("MenuDetails")]
    //public class MenuMappingDetails
    //{         
    //    [XmlElement("DBL_SL_NO")]
    //    public int DBL_SL_NO { get; set; }
    //    [XmlElement("DBL_PK")]
    //    public int DBL_PK { get; set; }
    //    [XmlElement("MNU_PK")]
    //    public int MNU_PK { get; set; }
    //    [XmlElement("MEBU_DEPT")]
    //    public int MEBU_DEPT { get; set; }
    //    [XmlElement("MEBU_DEPT_TEXT")]
    //    public string MEBU_DEPT_TEXT { get; set; }
    //    [XmlElement("MNU_NAME")]
    //    public string MNU_NAME { get; set; }
    //    [XmlElement("MNU_UNAME")]
    //    public string MNU_UNAME { get; set; }
    //    [XmlElement("MNU_ACTION_URL")]
    //    public string MNU_ACTION_URL { get; set; }
    //    [XmlElement("DBL_Seq")]
    //    public int DBL_Seq { get; set; }
    //    [XmlElement("DBL_ACTIVE")]
    //    public int DBL_ACTIVE { get; set; }
    //}
}
