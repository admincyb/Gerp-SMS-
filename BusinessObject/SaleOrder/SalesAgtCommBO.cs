using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.SaleOrder
{
    public class SalesAgtCommBO
    {

    }

    [Serializable]
    [XmlRoot("Root")]
    public class AgtCommGetKVBO
    {
        [XmlElement("InvoiceCus")]
        public List<InvoiceCus> InvoiceCusComm { get; set; }
    }

    [Serializable]
    public class InvoiceCus
    {
        [XmlElement("ICH_PK")]
        public int ICH_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class AgtCommBO
    {

        [XmlElement("IVH_DATE")]
        public string IVH_DATE { get; set; }
        [XmlElement("IVH_CURRENCY")]
        public int IVH_CURRENCY { get; set; }

        [XmlElement("IVH_BASE_CURR")]
        public int IVH_BASE_CURR { get; set; }
        [XmlElement("IVH_EXCHG_RATE")]
        public double IVH_EXCHG_RATE { get; set; }

        [XmlElement("IVH_VENDOR")]
        public int IVH_VENDOR { get; set; }

        [XmlElement("IVH_PK")]
        public int IVH_PK { get; set; }
        [XmlElement("IVH_VERSION")]
        public int IVH_VERSION { get; set; }
        [XmlElement("IVH_NO")]
        public string IVH_NO { get; set; }
        [XmlElement("IVH_TYPE")]
        public int IVH_TYPE { get; set; }

        [XmlElement("IVH_GROUP")]
        public int IVH_GROUP { get; set; }
        [XmlElement("IVH_AMOUNT_NET_BC")]
        public double IVH_AMOUNT_NET_BC { get; set; }
        [XmlElement("IVH_AMOUNT_TC")]
        public double IVH_AMOUNT_TC { get; set; }
        [XmlElement("IVH_DISCOUNT_TC")]
        public double IVH_DISCOUNT_TC { get; set; }
        [XmlElement("IVH_TAX_TC")]
        public double IVH_TAX_TC { get; set; }
        [XmlElement("IVH_AMOUNT_NET_TC")]
        public double IVH_AMOUNT_NET_TC { get; set; }

        [XmlElement("IVH_DATE_PAY_BY")]
        public string IVH_DATE_PAY_BY { get; set; }


        [XmlElement("ACTIVE")]
        public byte ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("IVH_COMPANY")]
        public int IVH_COMPANY { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("IVH_DEPT")]
        public int IVH_DEPT { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("AST_VALUE")]
        public string AST_VALUE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("IVH_BRANCH_TYPE")]
        public int IVH_BRANCH_TYPE { get; set; }
        [XmlElement("IVH_TAX_ID")]
        public string IVH_TAX_ID { get; set; }
        [XmlElement("IVH_BRANCH_TEXT")]
        public string IVH_BRANCH_TEXT { get; set; }
        [XmlElement("IVH_BRANCH_NAME")]
        public string IVH_BRANCH_NAME { get; set; }

 

        [XmlElement("IVH_VENDOR_INV_NO")]
        public string IVH_VENDOR_INV_NO { get; set; }
        [XmlElement("IVH_ALLOW_DUP_INV_NO")]
        public byte IVH_ALLOW_DUP_INV_NO { get; set; }
        [XmlElement("IVH_VENDOR_INV_DATE")]
        public string IVH_VENDOR_INV_DATE { get; set; }
        [XmlElement("IVH_CREDIT_DAYS")]
        public string IVH_CREDIT_DAYS { get; set; }
        [XmlElement("IVH_TRANSPORT")]
        public string IVH_TRANSPORT { get; set; } 
        [XmlElement("IVH_REMARKS")]
        public string IVH_REMARKS { get; set; }
 
        [XmlElement("IVH_DATE_RECEIVED")]
        public string IVH_DATE_RECEIVED { get; set; }

        [XmlElement("IVH_IS_SETTLED")]
        public int IVH_IS_SETTLED { get; set; }
        
         

        [XmlElement("Detail")]
        public List<AgentCommInvSaveDetails> agtCommInvDetail { get; set; }
    }
    [Serializable]
    public class AgentCommInvSaveDetails
    {
        [XmlElement("AVD_INV_CUS_DTL")]
        public int AVD_INV_CUS_DTL { get; set; }

        [XmlElement("AVD_CUSTOMER")]
        public int AVD_CUSTOMER { get; set; }

        [XmlElement("AVD_CUST_ITEM")]
        public string AVD_CUST_ITEM { get; set; }

        [XmlElement("AVD_COMMISSION_TYPE")]
        public string AVD_COMMISSION_TYPE { get; set; }
        [XmlElement("AVD_COMMISSION_RATE")]
        public double AVD_COMMISSION_RATE { get; set; }
        [XmlElement("AVD_COMMISSION_AMT")]
        public double AVD_COMMISSION_AMT { get; set; }

        [XmlElement("VID_SL_NO")]
        public int VID_SL_NO { get; set; }
        [XmlElement("VID_QTY_INVOICED")]
        public double VID_QTY_INVOICED { get; set; }
        [XmlElement("VID_UOM")]
        public int VID_UOM { get; set; }
        [XmlElement("VID_RATE")]
        public double VID_RATE { get; set; }
        [XmlElement("VID_AMOUNT")]
        public double VID_AMOUNT { get; set; }
        [XmlElement("VID_DISCOUNT")]
        public double VID_DISCOUNT { get; set; }
        [XmlElement("VID_TAX")]
        public double VID_TAX { get; set; }
        [XmlElement("VID_NET_AMOUNT")]
        public double VID_NET_AMOUNT { get; set; }
        [XmlElement("VID_BRANCH_TYPE")]
        public int VID_BRANCH_TYPE { get; set; }

        [XmlElement("AVD_COMMISSION_FORMULA")]
        public string AVD_COMMISSION_FORMULA { get; set; }

        [XmlElement("AVD_COMMISSION_FORMULA_TEXT")]
        public string AVD_COMMISSION_FORMULA_TEXT { get; set; }
      
    }
}
