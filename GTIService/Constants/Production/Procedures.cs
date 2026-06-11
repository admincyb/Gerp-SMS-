using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Production
{
    public class Procedures
    {
        public const string GETBINCARDNO = "SPPRD_BIN_CARD_NO_GET";
        public const string SAVEBINCARD = "SPPRD_BIN_CARD_MST_SAVE";
        public const string GETBINCARDDETAILS = "SPPRD_BIN_CARD_MST_GET";

        public const string GETSEARCHVALUE = "SPPRD_BIN_CARD_MST_AUTO";
        public const string GET_COMPOUND_TANK = "SPPRD_TANK_COMP_GET_KV";

        public const string GETBINDETAILSLIST = "SPPRD_BIN_CARD_MST_GET_LIST";
        public const string DELETEBINDETAILS = "SPPRD_BIN_CARD_MST_DELETE";

        //Compound Preparation 
        public const string GETCMPPRINTREPORT = "SPPRD_COMP_TRX_OP_RPT"; //CMP Preparation Print Report
        public const string GETCOMPOUNDNO = "SPPRD_CTH_NO_GET";
        public const string GETCOMPOUNDFORCOMBO = "SPPRD_COMP_MST_GET_KV";
        public const string GETPLANSFORCONBO = "SPPRD_PLAN_MST_GET_KV";
        public const string GETCOMPDTL = "SPPRD_COMP_DTL_GET";
        public const string SAVECOMTRXML = "SPPRD_COMP_TRX_SAVE";
        public const string GETCOMPOUNDTRXLIST = "SPPRD_COMP_TRX_LIST_WRKF";
        public const string GETBATCHFORITEMS = "SPPRD_COMP_TRX_ITEM_DTL_GET";
        public const string GETSTOCKVALUE = "";
        public const string DELETECOMPOUNDTRX = "SPPRD_COMP_TRX_DELETE";
        public const string GETAUTOCOMPLETECOMPOUNDTRX = "SPPRD_COMP_TRX_AUTO";
        public const string GETCOMPOUNDTRXDETAIL = "SPPRD_COMP_TRX_GET";
        public const string GETTANKCOMBO = "SPPRD_TANK_MST_GET_KV";
        public const string GETCATEGORYITEMBATCH = "SPINV_STK_AUD_ITM_BATCH_GET_KV";



    }

    public class Procedures_DispersionPreparation
    {
        public const string SAVEDISPERSIONPREPARATION = "SPPRD_BOM_TRX_SAVE";
        public const string GETDISPERSIONDETAILS = "SPPRD_BOM_DTL_GET";
        public const string GETDISPERSIONPREPARATIONLIST = "SPPRD_BOM_TRX_GET_LIST";
        public const string GETDISPERSIONPREPARATION = "SPPRD_BOM_TRX_GET";
        public const string DELETEDISPERSIONPREP = "SPPRD_BOM_TRX_DELETE";
        public const string GETDISPNO = "SPPRD_DISP_TRX_NO_GET";
        public const string GETDISPRSIONPREPAUTO = "SPPRD_BOM_TRX_AUTO";
        public const string GETINSPECTIONDTLS = "SPQUC_TEST_TRX_ITEM_LIST";
        public const string GETRAWMATERIALINSP = "SPQUC_TEST_TRX_ITEM_GET";
        public const string GETDISPPRINTREPORT = "SPPRD_DISP_TRX_RPT";
    }


    public class Procedure_TopUpRecord
    {


        public const string GETTOPUPRECORDLIST = "SPPRD_TOP_UP_GET_LIST";
        public const string TOPUPAUTO="SPPRD_TOP_UP_AUTO";
        public const string DELETETOPUPRECORD = "SPPRD_TOP_UP_DELETE";

        public const string CHECKSTOCKAVAILABLE = "SPPRD_TOP_UP_STK_CHECK";
        public const string GETTOPUPRECORDDTLS = "SPPRD_TOP_UP_GET";
        public const string SAVETOPUPRECORD = "SPPRD_TOP_UP_SAVE";
        public const string GETTOPUPUOMLIST = "SPPRD_TOP_UP_UOM_LIST_GET_KV";

         public const string GETTOPUPITEMLIST ="SPPRD_TOP_UP_ITM_GET_KV";
    }
}
