using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Finance
{
    public sealed class AssetFilterForDepreParameterBinder
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime PurchaseDateFrom { get; set; }
        public DateTime PurchaseDateTo { get; set; }
        public DateTime DepreMonth { get; set; }
        public int AssetCategory { get; set; }
        public int Location { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string DepreciationMethod { get; set; }
        public int? AssetType{ get; set; }
        public int? PType { get; set; }
        public int? Plant { get; set; }
        public int? CfgType { get; set; }
    }
}
