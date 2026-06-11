using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPManager.Sales
{
    [Serializable]
    public class ReceiptCrdrMpg
    {
        public long RNM_PK { get; set; }
        public long RNM_RECEIPT_HDR { get; set; }
        public long RNM_CRDR_HDR { get; set; }
        public long RNM_INVOICE_HDR { get; set; }
        public long RNM_RECEIPT_TRX_MPG { get; set; }
        public long? RNM_CRDR_MPG { get; set; }
        public decimal RNM_PAID_AMOUNT { get; set; }
        public decimal RNM_ADJ_AMOUNT { get; set; }
        public byte RNM_ACTIVE { get; set; }
        public string RNM_CRDR_NO { get; set; }
        public DateTime RNM_CRDR_DATE { get; set; }
        public string RNM_CRDR_CURRENCY_TEXT { get; set; }
        public decimal RNM_CRDR_AMOUNT { get; set; }
        public decimal RNM_ALLOCATED_AMOUNT { get; set; }
        public decimal RNM_BALANCE_AMOUNT { get; set; }    
    }
}
