using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    
    public interface ICommonFunctionsManager
    {
        string GetTrxDocNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? companyPk=null, int? bizunit=null);

        double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit);

        List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj);

        List<ADM_APP_TYPE_MST> GetAppTypeValues(ADM_APP_TYPE_MST ADMCONFIGMSTObj);

        List<ADM_CONST_MST> GetConstMstValues(int? CON_PK, Int16? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT);

        string GetSubTypeQuery(int SubTypePk);

        List<FIN_COA_SUB_TYPE_CFG> GetSubTypeCfgValues(FIN_COA_SUB_TYPE_CFG FINCOASUBTYPECFGObj);

        List<ADM_CONTROLS_CFG> GetControlsList(ADM_CONTROLS_CFG ADMCONTROLSCFGobj);

        List<ADM_CONST_GRP> GetConstGrpList(ADM_CONST_GRP ADMCONSTGRPobj);

        List<ADM_CHECK_LIST_TRX_HDR> GetCheckListTrxHdrList(ADM_CHECK_LIST_TRX_HDR ADMCHECKLISTTRXHDRobj);

        int SaveCheckListTrxHdr(List<ADM_CHECK_LIST_TRX_HDR> ADMCHECKLISTTRXHDRList);

        List<ADM_CHECK_LIST_TRX_DTL> GetCheckListTrxDtlList(ADM_CHECK_LIST_TRX_DTL ADMCHECKLISTTRXDTLobj);

        int SaveCheckListTrxDtl(List<ADM_CHECK_LIST_TRX_DTL> ADMCHECKLISTTRXDTLList, int TrxHdrPK);

        List<ADM_COUNTRY_MST> GetCountry(ADM_COUNTRY_MST ADM_COUNTRY_MSTobj);

        List<ADM_STATE_MST> GetStates(ADM_STATE_MST ADM_STATE_MSTobj);

        List<ADM_APP_SUB_TYPE_MST> GetADMAPPSUBTYPEMST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj);

        List<ADM_APP_CONFIG_MST> GetADM_APP_CONFIG_MST(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj);

        List<SPCRM_CUSTOMER_USER_GET_Result> GetCustomerDetails(int P_USER, int P_BIZUNIT);

        List<SAL_DESPATCH_HDR> GetGONHdr(int SPPk);

        List<SpWkfTransactionNewCountGet_Result> GetMessageCount(int UserPK, byte? inboxType);

        List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status);

        List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status, ServiceUtility utilityObj);

        bool GetHasPreviousTrxDiffProcess(int refID);

        List<SPSAL_FORECAST_RPT_Result> GetSalesForecast(string xml, string Status, ServiceUtility utilityObj);

        List<INV_ITEM_MST> GetInvItemMstTypeAutoCompleteList(short? ITM_ACTIVE, int type, ServiceUtility utilityObj);

        List<INV_UOM_MST> GetUOM(int PK);

        List<PUR_VENDOR_MST> GetVendor(int PK);

        List<ADM_APP_CONFIG_MST> GetAlertNotify(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj);

        List<ADM_CURRENCY_MST> GetCurrency(ADM_CURRENCY_MST ADMCURRENCYMSTObj);

        List<INV_ITEM_VENDOR_MAP> GetVendorItems(int venPk, int itemPk);

        List<CRM_CUSTOMER_MST> GetCustomers(int cuspk);
    }
}
