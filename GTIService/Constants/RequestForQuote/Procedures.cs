using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.RequestForQuote
{
    public class Procedures
    {
        // For Listing Page
        public const string GETRFQPRLIST = "SPPUR_RFQ_REQ_MAP_GET_LIST";
        public const string GETRFQVENDORLIST = "SPPUR_RFQ_VENDOR_MAP_GET_KV";
        public const string GETRFQRESPONSE = "SPPUR_RFQ_RESPONSE_GET";
        public const string GETRFQHEADER = "SPPUR_RFQ_HDR_GET_KV";
        public const string GETVENDORBYPR = "SPPUR_RFQ_ITEM_VENDORS_GET_KV";
        public const string GETRFQHDR = "SPPUR_RFQ_GET";
        public const string SAVERFQRESPONSE = "SPPUR_RFQ_RESPONSE_SAVE";
        public const string SAVERFQ = "SPPUR_RFQ_SAVE";
        public const string DELETERFQ = "SPPUR_RFQ_DELETE";
        public const string GET_TRX_DOC_NO = "SPADM_TRX_DOC_NO_GENERATE";
        public const string GET_ITEM_RATES = "SPPUR_VENDOR_ITEM_RATES_GET_LIST";
        public const string RFQGETLIST = "SPPUR_RFQ_GET_LIST";
        public const string SPADM_CURRENCY_CONV_FACT_GET = "SPADM_CURRENCY_CONV_FACT_GET";
        public const string SPFIN_BANK_CUR_GET = "SPFIN_BANK_CUR_GET";
        public const string GETITEMPURCHASEREQUESTAUTO = "SPPUR_RFQ_REQ_MAP_AUTO";

        public const string SPPUR_RFQ_RPT = "SPPUR_RFQ_RPT";
        public const string SPPUR_RFQ_AMT_CMP_RPT = "SPPUR_RFQ_AMT_CMP_RPT";
        public static string GETQUOTATION="SPCRM_QUOTATION_GET";
        public const string SPPUR_RFQ_AUTO="SPPUR_RFQ_AUTO";
    }
}
