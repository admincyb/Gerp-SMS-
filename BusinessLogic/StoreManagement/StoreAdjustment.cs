using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
    public class StoreAdjustment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeAuditDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveStoreAuditAdjustmentDetails(string storeAuditDetails, User objUser)
        {
            string purchseRequisitionID = string.Empty;
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(storeAuditDetails);
            objRequest.UserPK = objUser.PKUser;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(storeAuditDetails);
            purchseRequisitionID = DataAccess.StoreManagement.StoreAdjustmentDL.SaveStoreAuditAdjustmentDetails(xmlstr).ToString();
            if (objRequest.ActionID != 0)
            {
                objRequest.ApplicationID = Convert.ToInt32(purchseRequisitionID);
                WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
                int ReferenceID = obj.DoWorkFlow(objRequest);
                if (ReferenceID > 0)
                {
                    BusinessObject.CommonManagement.CommonObject.WorkFlowComment workFlowComment = new BusinessObject.CommonManagement.CommonObject.WorkFlowComment();
                    workFlowComment = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.WorkFlowComment>(storeAuditDetails);
                    workFlowComment.ReferenceID = ReferenceID;
                    workFlowComment.ApplicationID = objRequest.ApplicationID;
                    if (workFlowComment.WrkfComment != string.Empty)
                    {
                        BusinessLogic.CommonManagement.CommonManagement.SaveWrkfCommentList(workFlowComment);
                    }
                }
            }
            return purchseRequisitionID;
        }
    }
}
