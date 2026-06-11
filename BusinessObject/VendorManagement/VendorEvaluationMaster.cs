using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.VendorManagement
{
   public class VendorEvaluationMaster
    {


       public int VEH_PK { get; set; }
       public int VEH_VENDOR{ get; set; }
       public int VEH_ITEM { get; set; }
       public int VEH_MAX_POINT { get; set; }
       public int VEH_POINT { get; set; }
       public string VEH_RAT_DESC { get; set; }
       public int UserPK { get; set; }
       public string VEH_PERC { get; set; }


        
        //public string SupplierCode
        //{ get; set; }
        //public string SupplierName
        //{ get; set; }
        //public string Performance
        //{ get; set; }
        //public string Rating
        //{ get; set; }
        //public DateTime Date
        //{ get; set; }
        //public string Product
        //{ get; set; }


        public List<VendorEvaluationDetail> EvalDetailsList
        { get; set; }

    }
}
