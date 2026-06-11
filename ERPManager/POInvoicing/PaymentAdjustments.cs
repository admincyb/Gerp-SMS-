using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPManager.POInvoicing
{
    [Serializable]
    public class PaymentAdjustments
    {
        public long PAD_PK { get; set; }
        public long PAD_PAYMENT_TRX { get; set; }
        public long? PAD_ALCN_CDH { get; set; }
        public long? PAD_ALCN_PAYMENT_TRX { get; set; }       
        public decimal PAD_AMOUNT { get; set; }       
        public byte PAD_ACTIVE { get; set; }        
        public string PAD_TRX_NO { get; set; }
        public DateTime PAD_TRX_DATE { get; set; }
        public string PAD_TRX_TYPE { get; set; }
        public decimal PAD_TRX_AMOUNT { get; set; }
        public decimal PAD_ALLOCATED_AMOUNT { get; set; }
        public decimal PAD_BALANCE_AMOUNT { get; set; }     

    }
}
