using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class FinishedGoodsBO
    {
        [XmlElement("PRO_PK")]
        public string ProductPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("PRO_CATEGORY")]
        public int category { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
        [XmlElement("ORDERITEMS")]
        public List<OrderItemsBO> OrderItemsList { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class AllocationDetailsBO
    {
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("DEPT_PK")]
        public int DepaertmentPK { get; set; }
        [XmlElement("USER_PK")]
        public int UserPK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LastModDate { get; set; }
        [XmlElement("MODULE")]
        public int Module { get; set; }
        [XmlElement("MODE")]
        public int Mode { get; set; }
        [XmlElement("ALH_PK")]
        public string AlhPK { get; set; }
        [XmlElement("ALH_NO")]
        public string AlhNo { get; set; }
        [XmlElement("ALH_DATE")]
        public DateTime AlhDate { get; set; }
        [XmlElement("ALH_ALLOCATION_TYPE")]
        public int AllocationType { get; set; }
        [XmlElement("ALH_DESC")]
        public string AlhDescription { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("PRODUCTS")]
        public List<ProductsBO> ProductsList { get; set; }
    }


    [Serializable]
    public class ProductsBO
    {
        [XmlElement("PRODUCT")]
        public List<ProductBO> ProductList { get; set; }
    }

    [Serializable]
    public class ProductBO
    {
        [XmlElement("ALD_PK")]
        public int AldPK { get; set; }
        [XmlElement("SOD_PK")]
        public int SodPK { get; set; }
        [XmlElement("QTY")]
        public Decimal Qty { get; set; }
        [XmlElement("STORE")]
        public int Store { get; set; }
        public bool ItemAdd { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class AllocationdetailsBO
    {
        [XmlElement("PRO_PK")]
        public int ProPk { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("ALH_PK")]
        public string AlhPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class SelectedOrderBO
    {
        [XmlElement("SOD_PK")]
        public int SodPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("SESSION")]
        public string Session { get; set; }
        [XmlElement("ACTIVE")]
        public string Active { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ActivityBO
    {
        [XmlElement("CFG_PK")]
        public int cfgPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("CFG_TYPE")]
        public string  cfgType { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
       
      
    }

    // Former Action 
    [Serializable]
    [XmlRoot("ROOT")]
    public class FormerActionBO
    {
        [XmlElement("CFG_PK")]
        public string  cfgPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("CFG_TYPE")]
        public string cfgType { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }

    }

    //Former Master
    [Serializable]
    [XmlRoot("ROOT")]
    public class FormerMasterBO
    {
        
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PRO_CATEGORY")]
        public int procategory { get; set; }
        [XmlElement("PDT_PK")]
        public string  pdtPK { get; set; }
        [XmlElement("SIZ_PK")]
        public string  sizePK { get; set; }
        [XmlElement("LNE_PK")]
        public int  linePK { get; set; }
    }

   
    [Serializable]
    [XmlRoot("ROOT")]
    public class ProductListBO
    {
        [XmlElement("PRO_PK")]
        public string proPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PRO_CATEGORY")]
        public string  procategory { get; set; }
        [XmlElement("PDT_PK")]
        public string  pdtPK { get; set; }
        [XmlElement("SIZ_PK")]
        public string  sizePK { get; set; }
        [XmlElement("CLR_PK")]
        public string  clrPK { get; set; }
       
        [XmlElement("LNE_PK")]
        public string linePK { get; set; }
        [XmlElement("FMR_PK")]
        public string fmrPK { get; set; }

        [XmlElement("SESSION")]
        public string session { get; set; }
    }


    //Line Activity Save



  






}