using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.UOM
{
    public class Parameters
    {
        public const string UOMTYPEPK = "UMT_PK";
        public const string UOMTYPE = "UOM_TYPE";
        public const string UOMSBU = "UMT_BIZUNIT";

        public const string UOMXML = "P_UOM_XML";
        public const string RETVAL = "P_RTN_VAL";

        public const string UOMPK = "UOM_PK";
        public const int ACTIVESTATUS = 1;
        public const string FIELDNAME= "P_FLD_NAME";
        public const string FIELDVALUE= "P_VALUE";
        public const string ITEMPK = "ITM_PK";


        public const string UOMSTATUS = "UOM_STATUS";
        public const string UOMBIZUNIT = "P_BIZUNIT";
        public const string UOMTYPENAME = "UOM_TYPE_NAME";

        public const string P_UOM_TYPE = "P_UOM_TYPE";
        public const string P_PK = "P_PK";
        public const string P_UOM = "P_UOM";


        public class UOMType_Parameters
        {
            public const string UMTPK = "UMT_PK";
            public const string UMTNAME = "UMT_NAME";
            public const string UMTCODE = "UMT_CODE";// 1 Default
            public const string ACTIVSTATUS = "UMT_ACTIVE";
            public const string BIZUNIT = "UMT_BIZUNIT";
            public const string CREATEDBY = "UMT_CRTD_BY";
            public const string MODBY = "UMT_MOD_BY";
            public const string RETVAL = "P_RET_VAL";

        }

        public class UOM_Search
        {
            public const string SEARCHNAME =  "P_SER_NAME";
            public const string SEARCHVAL = "P_SER_VAL"; 
            public const string FIELDS = "P_FIELDS"; 
            public const string PAGENO = "P_PAGE_NO"; 
            public const string PAGESIZE =  "P_PAGE_SIZE";
            public const string SORTBY = "P_SORT_BY";
            public const string SORTDIRC = "P_SORT_DIR";
            
        }
        public class UOMTYpe
        {
            public const string UOMTYPEPK = "UMT_PK";
            public const string FIELDS = "P_FIELDS";
            public const string PAGENO = "P_PAGE_NO";
            public const string PAGESIZE = "P_PAGE_SIZE";
            public const string SORTBY = "P_SORT_BY";
            public const string SORTDIRC = "P_SORT_DIR";
           

        } 
    }

    
}
