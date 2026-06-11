using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Common
{
    public class ProductDetailsBO
    {
        public int ProductPK { get; set; }
        public int ProductCategoryPK { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Polymer { get; set; }
        public int Size { get; set; }
        public int ActiveStatus { get; set; }
        public string LastModDate { get; set; }
        public int Created_By { get; set; }     
        public int BizUnit { get; set; }
        public int ParentId { get; set; }
        public string ProductCategoryName { get; set; } 
    }
}
