using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Agent
{
    public class Procedures
    {
        public const string GETCURRENCY = "SpGYmntc";
        public const string GETVENDORLIST = "SpGDnvTmHrc";//SpGDnvTm
        //public const string SAVEVENDOR = "SpSDnvTldTm";
        public const string DELVENDOR = "SpDDnvTldTm";

        public const string SEARCHVENDOR = "SpGTimeHrc";//
        public const string SEARCHAUTO = "SpGTimeOta";

        // public const string GETVENDOR = "SpGDnvTldTm";
        public const string GETVENDOR = "SPPUR_VENDOR_MST_GET_KV";
        public const string GETVENDORTYPE = "SPPUR_VENDOR_ROLE_GET_KV";
        public const string GETPURCHASEORDERVENDOR = "SPPUR_VENDOR_GRN_GET_KV";
        public const string GETAgentDTL = "SPFIN_AGENT_MST_GET";
        public const string GETVENDORBANKDTL = "SPPUR_VENDOR_BANK_DTL_GET_KV";

        //sajeer EVALUATIONDETAILS

        // public const string GETPARAMETERS = "SpGRtm";
        public const string GETPARAMETERS = "SPADM_TERM_PARAM_GET";
        public const string GETEVALUATIONS = "SpGLave";
        public const string SAVEEVALUATIONXML = "SpGMtiEpt";
        //public const string GETSEARCHVALUE = "SpGMtiEpt";
        public const string GETEVALUATIONLISTBYSRH = "SpGMtiDnvLnv";
        public const string DELETEEVALUATIONDETAILS = "SPPUR_VENDOR_EVAL_DELETE";
        public const string GETAPPIDFORREFID = "SPWKF_APP_ID_GET";

        // public const string GETESUPPLIEDMATERIAL = "SpGMtiEpt";
        public const string GETESUPPLIEDMATERIAL = "SPINV_ITEM_VENDOR_MAP_GET_ITM";
        public const string SPINV_ITEM_VENDOR_DEPT_GET_ITM = "SPINV_ITEM_VENDOR_DEPT_GET_ITM";

        //daison vendor terms details

        public const string VENDORTERMSAVEMASTERSAVE = "SPPUR_VENDOR_TERM_MST_SAVE";
        public const string VENDORTERMSAVEMASTERGET = "SPPUR_VENDOR_TERM_MST_GET";
        public const string VENDORTERMSDELE = "SPPUR_VENDOR_TERM_MST_DELETE";
        public const string VENDORTERMAUTO = "SPPUR_VENDOR_TERM_MST_AUTO";
        //Daison Vendor Evaluation 
        public const string VENDOREVALSAVE = "SPPUR_VENDOR_EVAL_SAVE";
        public const string VENDOREVALGET = "SPPUR_VENDOR_EVAL_GET";
        public const string GETVENDOREVALDETILSLIST = "SPPUR_VENDOR_EVAL_LIST";
        public const string VENDOREVALREPORT = "SPPUR_VENDOR_EVAL_RPT";
        public const string VENDORPERFORMREPORT = "SPPUR_VENDOR_PERFORM_OP_RPT";

        // Agent Details Save and Get
        public const string SAVEAgent = "SPFIN_AGENT_MST_SAVE";

        public const string SAVEVETAXDISCOUNT = "SPINV_ITEM_VENDOR_TAX_SAVE";
        public const string GETVENDORDETILS = "SPPUR_VENDOR_MST_GET";
        public const string SPINV_ITEM_VENDOR_TAX_GET = "SPINV_ITEM_VENDOR_TAX_GET";
        public const string GETVENDORDETILSREPORT = "SPPUR_VENDOR_DTL_RPT";
        public const string SPPUR_VENDOR_REG_RPT = "SPPUR_VENDOR_REG_RPT";
        public const string SPADM_APP_SUB_TYPE_DATA_GET = "SPADM_APP_SUB_TYPE_DATA_GET";

        public const string GETVENDORPOTYPE = "SPADM_CONFIG_MST_GET_KV";

        //Vineeth Vendor Listing
        //public const string GETVENDORDETILSLIST = "SPPUR_VENDOR_MST_GET_LIST";
        public const string GETAGENTDETILSLIST = "SPFIN_AGENT_MST_GET_LIST";
        public const string SPINV_ITM_VND_GET_LIST = "SPINV_ITEM_VENDOR_GET_LIST";
        public const string GETSEARCHVALUE = "SPPUR_VENDOR_MST_AUTO";
        public const string GETVENDOREVALSEARCHVALUE = "SPPUR_VENDOR_EVAL_AUTO";
        public const string DELETEVENDORS = "SPPUR_VENDOR_MST_DELETE";
        public const string SPINV_ITEM_VENDOR_DELETE = "SPINV_ITEM_VENDOR_DELETE";
        public const string VENDERTERMDTLGETKV = "SPADM_TERM_DTL_GET_KV";
        //PO Material Details for a vendor
        public const string GETMATERIALDETAILS = "SPINV_ITEM_MST_GET_DTL";

        public const string GETVENDORTERMS = "SPPUR_VENDOR_TERM_GET_KV";


        public const string GETEVALAPPIDFORREFID = "SPPUR_PUR_VENDOR_EVAL_REF_ID_GET";
        public const string UPDATEEVALREFID = "SPPUR_PUR_VENDOR_EVAL_REF_ID_SAVE";

        //ADDRESS TYPE
        public const string ADDRESSTYPEGET = "SPADM_CONFIG_MST_GET_KV";
        public const string GETTERMHDRGETKV = "SPADM_TERM_HDR_GET_KV";

        public const string SAVEVENDORBANK = "SPPUR_VENDOR_BANK_DTL_SAVE";
        public const string DELETEVENDORBANK = "SPPUR_VENDOR_BANK_DTL_DELETE";
    }
}
