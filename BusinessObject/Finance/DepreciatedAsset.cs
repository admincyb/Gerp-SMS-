using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable()]
    public sealed class DepreciatedAsset
    {
        public int FDD_ASSET { get; set; }
        public decimal FDD_AMOUNT { get; set; }
        public string asrCode { get; set; }
        public string asrName { get; set; }
        public string asrNameText { get; set; }
        public int asrCategory { get; set; }
        public string asrCategory_Text { get; set; }
        public DateTime amiDatePur { get; set; }
        public string amiCostPur { get; set; }
        public string amiCurrPur { get; set; }
        public string amiCurrPur_Text { get; set; }
        public string amiDeprPerc { get; set; }
        public decimal BalanceDeprAmt { get; set; }
    }
}
