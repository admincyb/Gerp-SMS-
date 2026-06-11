using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using BusinessObject.CommonManagement;

namespace ERPManager
{

    public interface IContainerInspectionManager
    {
       

        List<SAL_CONTAINER_INSP_HDR> GetContainerInspectionList(SAL_CONTAINER_INSP_HDR SALCONTAINERINSPHDRobj, ServiceUtility utilityObj);

        int SaveContainerInspection(List<SAL_CONTAINER_INSP_HDR> SALCONTAINERINSPHDRobj);

        int DeleteContainerInspection(List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList);

        List<SAL_DESPATCH_HDR> GetDeliveryOrders(SAL_DESPATCH_HDR SAL_DESPATCH_HDRobj);

    }
}
