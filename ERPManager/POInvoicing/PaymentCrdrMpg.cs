using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPManager.POInvoicing
{
    [Serializable]
    public class PaymentCrdrMpg
    {
        public long PNM_PK { get; set; }
        public long PNM_PAYMENT_HDR { get; set; }
        public long PNM_CRDR_HDR { get; set; }
        public long PNM_INVOICE_HDR { get; set; }
        public long PNM_PAYMENT_TRX_MPG { get; set; }
        public long? PNM_CRDR_MPG { get; set; }   
        public decimal PNM_PAID_AMOUNT { get; set; }
        public decimal PNM_ADJ_AMOUNT { get; set; }
        public byte PNM_ACTIVE { get; set; }        
        public string PNM_CRDR_NO { get; set; }
        public DateTime PNM_CRDR_DATE { get; set; }
        public string PNM_CRDR_CURRENCY_TEXT { get; set; }
        public decimal PNM_CRDR_AMOUNT { get; set; }
        public decimal PNM_ALLOCATED_AMOUNT { get; set; }
        public decimal PNM_BALANCE_AMOUNT { get; set; }     

    }
}
