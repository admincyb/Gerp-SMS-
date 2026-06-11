using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public interface IContainerEvaluationManager
    {
        List<SAL_CONTAINER_EVAL_HDR> GetContainerEvaluationList(SAL_CONTAINER_EVAL_HDR SALCONTAINEREVALHDRobj, ServiceUtility utilityObj);

        int SaveContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SALCONTAINEREVALHDRobj);

        int DeleteContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SalContainerEvelHdrList);

        List<PUR_VENDOR_MST> GetCompany(PUR_VENDOR_MST PUR_VENDOR_MSTobj);
    }
}
