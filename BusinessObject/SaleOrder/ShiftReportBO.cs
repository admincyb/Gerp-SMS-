using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class ShiftReportBO
    {
        [XmlElement("SHH_PK")]
        public int SHH_PK { get; set; }
        [XmlElement("SHH_NO")]
        public string  SHH_NO { get; set; }
        [XmlElement("SHH_DATE")]
        public DateTime SHH_DATE { get; set; }
        [XmlElement("SHH_SHIFT")]
        public int SHH_SHIFT { get; set; }
        [XmlElement("SHH_REF_NO")]
        public string SHH_REF_NO { get; set; }
        [XmlElement("SHH_REF_DATE")]
        public string   SHH_REF_DATE { get; set; }
        [XmlElement("SHH_LINE")]
        public int SHH_LINE { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("MODULE")]
        public int MODULE { get; set; }
        [XmlElement("MODE")]
        public int MODE { get; set; }


        [XmlElement("DETAILS")]
        public List<ShiftDetailsBO> ShiftDetailsList { get; set; }
    }

    public class ShiftDetailsBO
    {
        [XmlElement("DETAIL")]
        public List<ShiftDetailBO> ShiftDetailList { get; set; }
    }

    public class ShiftDetailBO
    {
        [XmlElement("SL_NO")]
        public string SL_NO { get; set; }
        [XmlElement("SPD_PK")]
        public int SPD_PK { get; set; }
        [XmlElement("SPD_PRODUCT")]
        public int SPD_PRODUCT { get; set; }
        [XmlElement("PRO_CODE")]
        public string PRO_CODE { get; set; }
        [XmlElement("SPD_QTY_PRODUCED")]
        public decimal SPD_QTY_PRODUCED { get; set; }
        [XmlElement("SPD_QTY_ACTUAL")]
        public decimal SPD_QTY_ACTUAL { get; set; }
        [XmlElement("STORE")]
        public int STORE { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ShiftListingBO
    {
        [XmlElement("SOH_PK")]
        public string sohPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("SHH_SHIFT")]
        public string  Shift { get; set; }
        [XmlElement("SHH_DATE_FROM")]
        public string DateFrom { get; set; }
        [XmlElement("SHH_DATE_TO")]
        public string DateTo { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
       
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ShifMasterDDlBO
    {
        [XmlElement("SHF_PK")]
        public string shiftPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int bizUnit { get; set; }
       

    }

    //[Serializable]
    //[XmlRoot("ROOT")]
    //public class ProductMasterBO
    //{
    //    [XmlElement("PRO_PK>")]
    //    public string productPK { get; set; }
    //    [XmlElement("ACTIVE")]
    //    public int Active { get; set; }
    //    [XmlElement("BIZUNIT")]
    //    public int bizUnit { get; set; }
    //    [XmlElement("PRO_CATEGORY")]
    //    public int ProductCat { get; set; }
    //    [XmlElement("PDT_PK")]
    //    public string pdtPK { get; set; }
    //    [XmlElement("SIZ_PK")]
    //    public string SizePK { get; set; }
    //    [XmlElement("CLR_PK")]
    //    public string clrPK { get; set; }

     
    //}

    [Serializable]
    [XmlRoot("ROOT")]
    public class LineListBO
    {
        [XmlElement("LNE_PK")]
        public string linePK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int bizUnit { get; set; }


    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class NextDocNoBO
    {
        [XmlElement("DOC_TYPE")]
        public int Mode { get; set; }
        [XmlElement("BIZUNIT")]
        public int bizUnit { get; set; }


    }



    [Serializable]
    [XmlRoot("ROOT")]
    public class ShiftHeaderBO
    {
        [XmlElement("SHH_PK")]
        public int shhPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int bizUnit { get; set; }
        [XmlElement("SHH_SHIFT")]
        public string shHdrShift { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
        //


    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class ShiftDetailsGetBO
    {
        [XmlElement("SHH_PK")]
        public int shhPK { get; set; }
        [XmlElement("SPD_PK")]
        public string  spdPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int bizUnit { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }
        //


    }




}