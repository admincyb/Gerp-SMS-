using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPManager;
using ERPData;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICommonService" in both code and config file together.
    [ServiceContract]
    public interface ICommonService
    {
        [OperationContract]
        List<DDLMaster> ExecuteQuery(string query);

        [OperationContract]
        List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj);

        [OperationContract]
        List<ADM_APP_TYPE_MST> GetAppTypeValues(ADM_APP_TYPE_MST ADMAPPTYPEMSTObj);

        [OperationContract]
        List<ADM_CONST_MST> GetConstMstValues(int? CON_PK, Int16? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT);

        [OperationContract]
        double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit);

        [OperationContract]
        string GetSubTypeQuery(int SubTypePk);

        [OperationContract]
        List<FIN_COA_SUB_TYPE_CFG> GetSubTypeCfgValues(FIN_COA_SUB_TYPE_CFG FINCOASUBTYPECFGObj);

        [OperationContract]
        object ExecuteSP(string spName, object[] methodParams);

        [OperationContract]
        List<ADM_CONTROLS_CFG> GetControlsList(ADM_CONTROLS_CFG ADMCONTROLSCFGobj);

        [OperationContract]
        List<ADM_CONST_GRP> GetConstGrpList(ADM_CONST_GRP ADMCONSTGRPobj);

        [OperationContract]
        List<ADM_CHECK_LIST_TRX_HDR> GetCheckListTrxHdrList(ADM_CHECK_LIST_TRX_HDR ADMCHECKLISTTRXHDRobj);

        [OperationContract]
        int SaveCheckListTrxHdr(List<ADM_CHECK_LIST_TRX_HDR> ADMCHECKLISTTRXHDRList);

        [OperationContract]
        List<ADM_CHECK_LIST_TRX_DTL> GetCheckListTrxDtlList(ADM_CHECK_LIST_TRX_DTL ADMCHECKLISTTRXDTLobj);

        [OperationContract]
        int SaveCheckListTrxDtl(List<ADM_CHECK_LIST_TRX_DTL> ADMCHECKLISTTRXDTLList, int TrxHdrPK);

        [OperationContract]
        List<ADM_COUNTRY_MST> GetCountry(ADM_COUNTRY_MST ADM_COUNTRY_MSTobj);

        [OperationContract]
        List<ADM_STATE_MST> GetStates(ADM_STATE_MST ADM_STATE_MSTobj);

        [OperationContract]
        List<ADM_APP_SUB_TYPE_MST> GetADMAPPSUBTYPEMST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj);

        [OperationContract]
        List<ADM_APP_CONFIG_MST> GetADM_APP_CONFIG_MST(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj);

        List<SPCRM_CUSTOMER_USER_GET_Result> GetCustomerDetails(int P_USER, int P_BIZUNIT);

        [OperationContract]
        string GetTrxDocNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? CmpanyPK=null);

        [OperationContract]
        List<SAL_DESPATCH_HDR> GetGONHdr(int SPPk);

        [OperationContract]
        List<SpWkfTransactionNewCountGet_Result> GetMessageCount(int UserPK, byte? inboxType);

        [OperationContract]
        List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status);

        [OperationContract]
        List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status, ServiceUtility utilityObj);

        [OperationContract]
        bool GetHasPreviousTrxDiffProcess(int refID);

        [OperationContract]
        List<SPSAL_FORECAST_RPT_Result> GetSalesForecast(string xml, string Status, ServiceUtility utilityObj);

        [OperationContract]
        List<INV_ITEM_MST> GetInvItemMstTypeAutoCompleteList(short? ITM_ACTIVE, int type, ServiceUtility utilityObj);

        [OperationContract]
        List<INV_UOM_MST> GetUOM(int PK);

        [OperationContract]
        List<PUR_VENDOR_MST> GetVendor(int PK);

        [OperationContract]
        List<ADM_APP_CONFIG_MST> GetAlertNotify(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj);

        [OperationContract]
        List<ADM_CURRENCY_MST> GetCurrency(ADM_CURRENCY_MST ADMCURRENCYMSTObj);

        [OperationContract]
        List<INV_ITEM_VENDOR_MAP> GetVendorItems(int venPk, int itemPk);

        [OperationContract]
        List<CRM_CUSTOMER_MST> GetCustomers(int cuspk);
    }
}
