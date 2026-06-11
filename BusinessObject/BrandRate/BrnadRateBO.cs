using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.BrandRate
{

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerRateCopyBO
    {
        [XmlElement("P_ACTIVE")]
        public int Active { get; set; }
        [XmlElement("P_BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("P_USER")]
        public string UserPK { get; set; }
        [XmlElement("P_BRH_DATE_FROM")]
        public string DateFrom { get; set; }
        [XmlElement("P_BRH_DATE_TO")]
        public string DateTo { get; set; }
        [XmlElement("P_BRH_PK_SRC")]
        public int BrhPK { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerRateBO
    {
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("LIST_TYPE")]
        public int ListType { get; set; }
        [XmlElement("BRH_PK")]
        public int BrhPK { get; set; }
        [XmlElement("BRH_DATE_FROM")]
        public string  DateFrom { get; set; }
        [XmlElement("BRH_DATE_TO")]
        public string  DateTo { get; set; }
        [XmlElement("BRH_DESC")]
        public string  Description { get; set; }
        [XmlElement("BRH_STATUS")]
        public string  Status { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageIndex { get; set; }
        [XmlElement("PAGE_SIZE")]
        public int PageSize { get; set; }
        [XmlElement("USER_PK")]
        public string UserPK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string  LastModDate { get; set; }
        [XmlElement("ITEM")]
        public List<ItemsListBO> ItemsList { get; set; }
        [XmlElement("CUSTOMER")]
        public List<CustomerListBO> CustomerList { get; set; }
        [XmlElement("DETAIL")]
        public List<DetailsBO> DetailsList { get; set; }

        [XmlElement("PACKSPEC")]
        public List<PackingSpecListBO> PackingSpecList { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerRateNewBO
    {
        [XmlElement("PAGE_SIZE")]
        public int PageSize { get; set; }
        [XmlElement("USER_PK")]
        public string UserPK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModDate { get; set; }
        [XmlElement("Products")]
        public List<ProductDetailsBo> ProductDetailsList { get; set; }
    }

    [Serializable]
    public class ProductDetailsBo
    {
        //[XmlElement("ROW_NO")]
        //public int ROW_NO { get; set; }
        //[XmlElement("BPR_PK")]
        //public int BPR_PK { get; set; }
       
        [XmlElement("ROW_COUNT")]
        public int ROW_COUNT { get; set; }
        [XmlElement("BPR_CUST_ITEM")]
        public int BPR_CUST_ITEM { get; set; }
        [XmlElement("BPR_ITEM")]
        public int BPR_ITEM { get; set; }
        [XmlElement("BPR_ITEM_CODE")]
        public string BPR_ITEM_CODE { get; set; }
        [XmlElement("BPR_ITEM_NAME")]
        public string BPR_ITEM_NAME { get; set; }
        [XmlElement("BPR_ITEM_TEXT")]
        public string BPR_ITEM_TEXT { get; set; }
        [XmlElement("APS_TEXT")]
        public string APS_TEXT { get; set; }

        [XmlElement("Inter_State")]
        public string Inter_State { get; set; }
        [XmlElement("Intra_State")]
        public string Intra_State { get; set; }
        [XmlElement("Overseas")]
        public string Overseas { get; set; }

        [XmlElement("BPR_DATE")]
        public DateTime BPR_DATE { get; set; }

        [XmlElement("CIM_BRAND_TEXT")]
        public string CIM_BRAND_TEXT { get; set; }
        [XmlElement("ProductRate")]
        public List<ProductRateBO> ProductRateList { get; set; }
    }

    [Serializable]
    public class ProductRateBO
    {
        [XmlElement("BPR_PK")]
        public int BrandPK { get; set; }
        [XmlElement("BPR_CURRENCY")]
        public int CurrencyPK { get; set; }
        [XmlElement("BPR_RATE")]
        public double BandRate { get; set; }
        [XmlElement("BPR_SPECIAL_CAT")]
        public int CategoryPK { get; set; }
        [XmlElement("ROW_NUMBER")]
        public int RowNumber { get; set; }
     



        //[XmlElement("BRD_RATE")]
        //public double BandRate { get; set; }
        //[XmlElement("BRD_CUST_ITEM")]
        //public int BrandPK { get; set; }
        //[XmlElement("BRD_CURRENCY")]
        //public int CurrencyPK { get; set; }
        [XmlElement("BRD_CURRENCY_NAME")]
        public string CurrencyName { get; set; }
        //[XmlElement("BRD_CATEGORY")]
        //public int CategoryPK { get; set; }
        [XmlElement("BRD_CATEGORY")]
        public string CategoryName { get; set; }
        //[XmlElement("ROW_NUMBER")]
        //public int RowNumber { get; set; }
    }

    [Serializable]
    [XmlRoot]
    public class BrandRatesBo
    {
        [XmlElement("ROW_NO")]
        public int ROW_NO { get; set; }
        [XmlElement("BPR_CURRENCY")]
        public int BPR_CURRENCY { get; set; }
        [XmlElement("BPR_RATE")]
        public double BPR_RATE { get; set; }   
    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerProductFilterBO
    {
        [XmlElement("BizUnit")]
        public int UserPK { get; set; }
        //[XmlElement("CATEGORY")]
        //public int FinishedGoods { get; set; }
        [XmlElement("ACTIVE")]
        public int SelectValue { get; set; }
        [XmlElement("TYPE")]
        public int Type { get; set; }
        [XmlElement("SIZE")]
        public int Size { get; set; }
        [XmlElement("COLOR_CATEGORY")]
        public int ColorCategory { get; set; }
        [XmlElement("FORMER_TYPE")]
        public int FormerType { get; set; }
        [XmlElement("FROMER_SIZE")]
        public int FormerSize { get; set; }
        [XmlElement("COLOR")]
        public int Color { get; set; }
        [XmlElement("PRINT_TYPE")]
        public int PrintType { get; set; }

        [XmlElement("SUB_CATEGORY_TYPE")]
        public int SubCategoryType { get; set; }


        [XmlElement("PACKSPEC")]
        public List<PackingSpecListBO> PackingSpecList { get; set; }

        [XmlElement("SUBCATEGORY")]
        public List<SubCategoryListBO> SubCategoryList { get; set; }

        [XmlElement("PRODUCTS")]
        public List<ProductsListBO> ProductsList { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerProductRateBO
    {
        [XmlElement("PRODUCTRATE")]
        public List<ProductRateBO> ProductRateList { get; set; }
    }

    

    [Serializable]
    [XmlRoot("ROOT")]
    public class ItemBO
    {
        [XmlElement("ITEM")]
        public List<ItemsListBO> ItemsList { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
    }
    [Serializable]
    public class ItemsListBO
    {
        [XmlElement("ITM_PK")]
        public int itemPK { get; set; }
    }

    [Serializable]
    public class DetailsBO
    {
        [XmlElement("BRD_CUST_ITEM")]
        public int BrandPK { get; set; }
        [XmlElement("BRD_CURRENCY")]
        public int CurrencyPK { get; set; }
        [XmlElement("BRD_RATE")]
        public double Rate { get; set; }
        [XmlElement("BRD_ACTIVE")]
        public int Active { get; set; }
    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerBO
    {
        [XmlElement("CUSTOMER")]
        public List<CustomerListBO> CustomerList { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
    }
    [Serializable]
    public class CustomerListBO
    {
        [XmlElement("CUS_PK")]
        public int cusPK { get; set; }
    }

    [Serializable]
    public class PackingSpecListBO
    {
        [XmlElement("APS_PK")]
        public int APS_PK { get; set; }
    }

    [Serializable]
    public class SubCategoryListBO
    {
        [XmlElement("CFG_VALUE")]
        public int CFG_VALUE { get; set; }
    }

    [Serializable]
    public class ProductsListBO
    {
        [XmlElement("PRD_PK")]
        public int PRD_PK { get; set; }
    }
}
