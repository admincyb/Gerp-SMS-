using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessObject.ProductManagement
{
    public class ProductMaster
    {
        /// <summary>
        /// Product ID or Primary Key
        /// </summary>
        public int ProductID
        {get;set;}

        /// <summary>
        /// Product Name
        /// </summary>
        public string ProductName
        { get; set; }

        /// <summary>
        /// Weight in Grams
        /// </summary>
        public float ProductWeight
        { get; set; }
    }
    public class BrandImportBO
    {
        public DataTable ImportData { get; set; }
    }
}
