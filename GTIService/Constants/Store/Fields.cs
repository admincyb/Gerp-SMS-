using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Store
{
    public class Fields
    {
        public const string STORESEARCHTEXTFIELD = "LocNem";
        public const string STORESEARCHVALUEFIELD = "LocP";

         public const string DPT_PK = "DPT_PK";
         public const string DPT_NAME = "DPT_NAME";

       
        public const string STOREPK = "Ersp";
        public const string STORENAME = "ErsNem";
        public const string GRH_GRN_REF_NO = "GRH_GRN_REF_NO";
        public const string GRH_PK = "GRH_PK";
    }
    public class Fields_RequisitionSlip
    {
        public const string SEARCHTEXTFIELD = "ITM_CODE";
        public const string SEARCHVALUEFIELD = "ITM_PK";
        public const string STRDPTTEXTFIELD = "DPT_NAME";
        public const string STRDPTVALUEFIELD = "DPT_PK";
        public const string REQUISITIONSEARCHTEXTFIELD = "PK";
        public const string REQUISITIONSEARCHVALUEFIELD = "VALUE";
        public const string MRHNO = "MRH_NO";
        public const string MRHDATE = "MRH_DATE";   
           
    }
    public class Fields_MaterialConsumption
    {
        public const string SEARCHTEXTFIELD = "ITM_CODE";
        public const string SEARCHVALUEFIELD = "ITM_PK";
        public const string STRDPTTEXTFIELD = "DPT_NAME";
        public const string STRDPTVALUEFIELD = "DPT_PK";
        public const string REQUISITIONSEARCHTEXTFIELD = "PK";
        public const string REQUISITIONSEARCHVALUEFIELD = "VALUE";
        public const string ICHNO = "ICH_NO";
        public const string ICHDATE = "ICH_DATE";

    }
    public class Fields_NewItemRequest
    {
        public const string SEARCHTEXTFIELD = "VALUE";
        public const string SEARCHVALUEFIELD = "PK";
        public const string ITEMPK = "ITR_PK";
        public const string NIRNO = "ITR_NO";
        public const string NIRDATE = "ITR_REQD_DATE";
    }
}
