using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.VendorManagement
{
    public class Vendor
    {
        public int VEN_PK { get; set; }
        public string VEN_NAME { get; set; }
        public string VEN_CONT_NAME { get; set; }
        public string VEN_PHONE { get; set; }
        public string VEN_EMAIL { get; set; }
        public string VEN_MOBIL { get; set; }
        public string VEN_FAX { get; set; }
        public string VEN_TYPE { get; set; }
        public string VEN_TIN { get; set; }
        public string VEN_ADDR1 { get; set; }
        public string VEN_ADDR2 { get; set; }
        public string VEN_CITY { get; set; }
        public string VEN_STATE { get; set; }
        public string VEN_CNTRY { get; set; }
        public string VEN_CURRENCY { get; set; }

        public string VEN_STATE_TEXT { get; set; }
        public string VEN_CNTRY_TEXT { get; set; }
        public string VEN_CURRENCY_TEXT { get; set; }
        public string VEN_CODE { get; set; }
        public int SBUID { get; set; }
        public int UserPK { get; set; }
        public List<VendorAddressBook> AddressBookDetails { get; set; }
        public List<MaterialDetails> MaterialDetails { get; set; }
        public List<VendorTerms> TermsDetails { get; set; }
        //NewSamples Start
        public List<MaterialSamples> MaterialSamples { get; set; }
        //New End
       // public List<TaxHdr> TaxHdr { get; set; }

    }

    public class VendorAddressBook
    {
        public int AddressID { get; set; }
        public int VNC_PK { get; set; }
        public string VNC_NAME { get; set; }
        public string VNC_CONT_NAME { get; set; }
        public string VNC_ADDR1 { get; set; }
        public string VNC_ADDR2 { get; set; }
        public string VNC_EMAIL { get; set; }
        public string VNC_PHONE { get; set; }
        public bool VNC_DEFAULT { get; set; }
        public string VNC_CITY { get; set; }
        public int VNC_CNTRY { get; set; }
        public int VNC_STATE { get; set; }
        public string VNC_MOBIL { get; set; }
        public string VNC_FAX { get; set; }
    }

    public class MaterialDetails
    {
        public int VendorMaterialID { get; set; }
        public string MaterialTypeText { get; set; }
        public int MaterialType { get; set; }
        public int ITV_ITEM { get; set; }
        public string MaterialCode { get; set; }
        public string ITV_NAME { get; set; }
        public float ITV_PRICE { get; set; }
        public int ITV_CURRENCY { get; set; }
        public string MaterialCurrencyText { get; set; }
        public float ITV_MOQ { get; set; }
        public int ITV_MOQ_UOM { get; set; }
        public string UOMText { get; set; }
        //NewMaterial Start
        public string ITV_DISC_PERC { get; set; }
        //New End
        public string ITV_TAX_PERC { get; set; }
        public int ITV_LEAD_TIME { get; set; }
    }

    public class VendorTerms
    {
        public int VTD_VENDER_TERM { get; set; }
        public int VTD_VALUE { get; set; }

    }

    //NewSamples Start
    public class MaterialSamples
    {
        public int VendorMaterialID { get; set; }
        public string MaterialTypeText { get; set; }
        public int MaterialType { get; set; }
        public int ISV_ITEM { get; set; }
        public string SampleMaterialCode { get; set; }
        public DateTime ISV_RECEIVED_DATE { get; set; }
        public string ISV_QC_TEST { get; set; }
        public string ISV_QC_VALUE { get; set; }
        public string ISV_REMARKS { get; set; }
        public float ISV_QUANTITY { get; set; }
        public string ISV_REFERENCE { get; set; }

        public short ISV_STATUS { get; set; }
        public string ISV_STATUS_TEXT { get; set; }
        public short ISV_ACTIVE { get; set; }
    }
    //New End

    //NewMaterial Start
    public class RateHistory
    {
        public int VIH_PK { get; set; }
        public int VIH_VENDOR { get; set; }
        public string VIH_VENDOR_CODE { get; set; }
        public string VIH_VENDOR_NAME { get; set; }
        public int VIH_ITEM { get; set; }
        public string VIH_ITEM_CODE { get; set; }
        public string VIH_ITEM_NAME { get; set; }
        public int VIH_SL_NO { get; set; }
        public int VIH_UOM { get; set; }
        public string VIH_UOM_TEXT { get; set; }
        public double VIH_RATE { get; set; }
        public string VIH_EFCT_FROM { get; set; }
        public string VIH_EFCT_TO { get; set; }
        public double VIH_TAX_PERC { get; set; }
        public double VIH_DISC_PERC { get; set; }
        public int VIH_LEAD_TIME { get; set; }
        public double VIH_LAST_ORDR_QTY { get; set; }
        public double VIH_LAST_ORDR_RATE { get; set; }
    }
    //New End

    public class LocalAddressDetails
    {
        public int VNC_LC_PK { get; set; }
        public int VNC_LC_VENDOR { get; set; }
        public string VNC_LC_TITTLE { get; set; }
        public string VNC_LC_NAME { get; set; }
        public string VNC_LC_LASTNAME { get; set; }
        public string VNC_LC_ADDR1 { get; set; }
        public string VNC_LC_ADDR2 { get; set; }
        public string VNC_LC_ADDR3 { get; set; }
        public string VNC_LC_CNTRY { get; set; }
        public string VNC_LC_TAX_NO { get; set; }
        public string VNC_LC_POSTAL { get; set; }
        public string VNC_LC_BRANCH { get; set; }
        public int VNC_LC_ACTIVE { get; set; }
        public int VNC_LC_BIZUNIT { get; set; }
        public int VNC_LC_MOD_BY { get; set; }
    }

    public class BankDetails
    {
        //public int VendorMaterialID { get; set; }
        //public string MaterialTypeText { get; set; }
        //public int MaterialType { get; set; }
        public int VBD_PK { get; set; }
        public int VBD_VENDOR { get; set; }
        public string VBD_NAME { get; set; }
        public string VBD_BRANCH { get; set; }
        public string VBD_ADDRESS { get; set; }
        public string VBD_CITY { get; set; }
        public string VBD_COUNTRY { get; set; }
        public string VBD_PHONE { get; set; }
        public string VBD_FAX { get; set; }
        public string VBD_IFSC_CODE { get; set; }
        public string VBD_ACCOUNT_NO { get; set; }
        public string VBD_ACCOUNT_TYPE { get; set; }
        public string VBD_ACCOUNT_TYPE_OTHER { get; set; }
        public string VBD_STATE_OTHER { get; set; }
        public string VBD_ZIP { get; set; }
        public string VBD_MOBILE { get; set; }
        public string VBD_EMAIL { get; set; }
        public string VBD_SWIFT_CODE { get; set; }
        public string VBD_ACCOUNT_NO_CNFM { get; set; }
        public string VBD_CONTACT { get; set; }
        //public string VBD_ACTIVE { get; set; }
        public int VBD_ACTIVE { get; set; }
        public int VBD_MOD_BY { get; set; }
    }

    public class TaxHdr
    {
        public int IVT_PK { get; set; }
        public int IVT_TAX { get; set; }
    }

}
