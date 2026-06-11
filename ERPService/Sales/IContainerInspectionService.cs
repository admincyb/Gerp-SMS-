using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using BusinessObject.Inventory;

namespace ERPService.Sales
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContainerInspectionService" in both code and config file together.
    [ServiceContract]
    public  interface IContainerInspectionService
    {
        #region Container Inspection Functions

        [OperationContract]
        string GetContainerInspectionNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null);
        [OperationContract]
        List<SAL_CONTAINER_INSP_HDR> GetContainerInspectionList(SAL_CONTAINER_INSP_HDR SALCONTAINERINSPHDRobj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveContainerInspection(List<SAL_CONTAINER_INSP_HDR> SALCONTAINERINSPHDRobj);
        [OperationContract]
        int DeleteContainerInspection(List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList);
        [OperationContract]
        List<SAL_DESPATCH_HDR> GetDeliveryOrders(SAL_DESPATCH_HDR SAL_DESPATCH_HDRobj);
        #endregion
    }
}
