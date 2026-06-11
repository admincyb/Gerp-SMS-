using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Dispersion
{
    public class Parameters
    {

        public const string PK = "pKp"; //general PK
        public const string MaterialPK = "pKp"; // material PK 

        public const string xmlStringDispersionSave = "P_DSP_XML";//"@pTlxmld";//XML data for saving Dispersion details
        public const string Retval = "P_RET_VAL"; //return value

        public const string DISPERSIONID = "DSP_PK";
        public const string DEPARTMENT_PK = "P_DEPT";


        //  for  GETDISPERSIONDETAILS
        public const string DISPERSIONPK = "DSP_PK";
        public const string PAGENO="@pPegNmu";
        public const string PAGESIZE="@pPegSez";
        public const string FIELDLIST="@pFdls";
        public const string SORTBY="@pStrYb";
        public const string SORTDIRECTION="@pStrdri";
        public const string SEARCHTYPE = "@pLocNem";
        public const string SEARCHVALUE = "@pLocLuv";

        //for getting UOM

        public const string UOMPK = "@Meup";
        public const string UOMTYPEPK = "@pEpt";

       
        
    }

    
}
