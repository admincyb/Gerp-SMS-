using System.Collections.Generic;


namespace BusinessObject.StoreManagement
{
   public class StoreRequisitionSlipCreation
    {
       public int UserPk
       {
           get;
           set;
       }
       public int DeptPk
       { get; set; }
       public int MRH_PK
       { get; set; }
       
       public int MRH_DEPT_STR
       { get; set; }
       public int MRH_STATUS
       { get; set; }
       public string MRH_SUBMITTED_DATE
       { get; set; }
       public string MRH_DATE
       { get; set; }
       public string MRH_NO
       { get; set; }
       public List<StoreRequisitionSlipCreationDetail> RequisitionDetailsList
       { get; set; }

    }
}
