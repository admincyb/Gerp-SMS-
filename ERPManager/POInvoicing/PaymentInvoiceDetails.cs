using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPManager.POInvoicing
{
    [Serializable]
    public class PaymentInvoiceDetails
    {        
        public long PVM_INVOICE_HDR { get; set; }        
        public decimal PVM_PAYNOW_AMOUNT { get; set; }
        public decimal PVM_ADJ_AMOUNT { get; set; }
        public decimal PVM_CRDR_AMOUNT { get; set; }       
        public decimal PVM_BALANCE_TO_PAY { get; set; }
        public decimal PVM_OTHER_CHARGE { get; set; }
        public decimal PVM_TAX_AMOUNT { get; set; }
        public decimal PVM_REDUCTION_AMOUNT { get; set; }     
    }
}
