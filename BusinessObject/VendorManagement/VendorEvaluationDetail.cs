using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.VendorManagement
{
  public  class VendorEvaluationDetail
    {
      //NEwEval Start
      public int VED_TERM_HDR{ get; set; }
      public string VED_TERM_HDR_TEXT { get; set; }
      //New End
        public int VED_PK { get; set; }
        public int VED_VENDOR_EVAL { get; set; }
        public int VED_PARAM { get; set; }
        public string VED_PARAM_NAME { get; set; }
        public int VED_POINT { get; set; }
        public string VED_REMARKS { get; set; }
        public int VED_ACTIVE { get; set; }
        

        //public int SupplierMaterialID
        //{ get; set; }
        //public int ParameterID
        //{ get; set; }
        //public int EvaluationID
        //{ get; set; }
        //public string Parameter
        //{ get; set; }
        //public string Evaluation
        //{ get; set; }
        //public int Points
        //{ get; set; }
        //public string Remarks
        //{ get; set; }

    }
}
