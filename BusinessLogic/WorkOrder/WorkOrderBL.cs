using BusinessObject;
using BusinessObject.WorkOrder;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.WorkOrder
{
    public class WorkOrderBL
    {
        public static int SaveWorkOrder(string strXml)
        {
            return DataAccess.WorkOrder.WorkOrderDL.SaveWorkOrder(strXml);
        }

        public static int SaveWorkOrderWkf(string strXml, out string woNo)
        {
            return DataAccess.WorkOrder.WorkOrderDL.SaveWorkOrderWkf(strXml, out woNo);
        }

        public static DataSet GetWorkOrderList(GridPrams grid, int subContrPK, int woPK, int woItem, int status, string pageURL)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderList(grid, subContrPK, woPK, woItem, status, pageURL);
        }

        public static DataSet GetWorkOrderDetailsReport(int woID)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderDetailsReport(woID);
        }
        public static DataSet GetWorkOrderDetailsReport(int woID, int version)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderDetailsReport(woID, version);
        }

        public static string GetWorkOrderByPK(int WorkOrderPK)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderByPK(WorkOrderPK);
        }

        public static DataTable GetAutoCompleteWONumber(byte Active, int bizUnit, string searchValue)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetAutoCompleteWONumber(Active, bizUnit, searchValue);
        }

        public static DataTable GetWorkOrderItemsAuto(int Operation, int Type, int Customer, int sbuID, string SearchKey, int BrandPK = 0)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderItemsAuto(Operation, Type, Customer, sbuID, SearchKey, BrandPK);
        }

        public static DataTable GetWorkOrderItemsForFilterAuto(string SearchKey, int SBUID)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderItemsForFilterAuto(SearchKey, SBUID);
        }

        public static int CancelWorkOrder(WorkOrderCancel woCancel)
        {
            return DataAccess.WorkOrder.WorkOrderDL.CancelWorkOrder(woCancel);
        }
       
        public static DataTable GetRevisionHistory(int woPK)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetRevisionHistory(woPK);
        }

        public static int DeleteWODetails(int woID)
        {
            return DataAccess.WorkOrder.WorkOrderDL.DeleteWODetails(woID);
        }

        public static DataTable GetPendingBatches(int SubContractorPK, int ItemPK, int ItemTypePK, int GrnPK = 0, int WOPK = 0, int ReturnWOPK = 0, int IsAfterMulti = 0)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetPendingBatches(SubContractorPK, ItemPK, ItemTypePK, GrnPK, WOPK, ReturnWOPK, IsAfterMulti);
        }

        public static void IsStockExist(int ItemPK, int subContractorPK, out int IsExist)
        {
            DataAccess.WorkOrder.WorkOrderDL.IsStockExist(ItemPK, subContractorPK, out IsExist);
        }

        public static DataTable GetIssueDetails(int WOPK)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetIssueDetails(WOPK);
        }

        public static DataTable GetWOItemList(int TrxType, int WOPK)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWOItemList(TrxType, WOPK);
        }

        public static DataTable TemplateDetailsGetNew(int binID, int sbu, int Type = 0, int DeptPk = 0)
        {
            return DataAccess.WorkOrder.WorkOrderDL.TemplateDetailsGetNew(binID, sbu, Type, DeptPk);
        }

        public static DataTable GetBincardDetails(int binID, string binText, int sbu)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetBincardDetails(binID, binText, sbu);
        }
        #region GetWorkOrderList
        /// <summary>
        /// GetWorkOrderList
        /// </summary>
        /// <param name="workOrderPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataSet GetWorkOrderList(int pageNum, int pageSize, int bizUnit, string fromDate, string toDate, int customerId
            , int typeId, string aptCode, int contractorId, int? projectPk = null, string projectNo = null, string projectCode = null, string refNo = null, int? curUserPk = null, int transactionStatus = -1,string pageurl=null,int status=-1)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderList(pageNum, pageSize, bizUnit, fromDate, toDate, customerId, typeId, aptCode, contractorId, projectPk, projectNo, projectCode, refNo, curUserPk, transactionStatus,pageurl,status);
        }
        #endregion
        #region GetWorkOrder
        /// <summary>
        /// GetWorkOrder
        /// </summary>
        /// <param name="workOrderPk"></param>
        /// <param name="active"></param> 
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetWorkOrder(int workOrderPk, int active, int bizUnit, int customerId, string aptCode, int parentPk, int? curUserPk = null)
        {
            DataTable dtWorkOrders = DataAccess.WorkOrder.WorkOrderDL.GetWorkOrder(workOrderPk, active, bizUnit, customerId, aptCode, parentPk, curUserPk);
            return dtWorkOrders;
        }

        #endregion
        #region SaveProject
        /// <summary>
        /// SaveWorkOrder
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>int</returns>
        public static int Save(ProjectBO project, out string refNo)
        {
            string xmlDoc = CommonFunctions.XmlSerialize<ProjectBO>(project);
            return DataAccess.WorkOrder.WorkOrderDL.SaveProject(xmlDoc, out refNo);
        }
        #endregion
        #region DeleteWorkOrder
        /// <summary>
        /// Delete Work Order
        /// </summary>
        /// <param name="workOrderPk"></param>
        /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteWorkOrder(int workOrderPk, DateTime lastModifiedDate)
        {
            return DataAccess.WorkOrder.WorkOrderDL.DeleteWorkOrder(workOrderPk, lastModifiedDate);
        }
        #endregion
        public static DataTable GetHistory(int pk)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetHistory(pk);
        }
        public static int AmendSave(int pk)
        {
            return DataAccess.WorkOrder.WorkOrderDL.AmendSave(pk);
        }
        public static DataSet GetWorkOrderReport(int pk, int active, int bizunit, int? version = null, int? showHideDesc = null, int? internalFlag = 0)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetWorkOrderReport(pk, active, bizunit, version, showHideDesc, internalFlag);
        }
        public static int CancelProject(ProjectCancel prjcancel)
        {
            return DataAccess.WorkOrder.WorkOrderDL.CancelProject(prjcancel);
        }
        public static int UpdateProjectStatus(int projectPK, int Status, int User, DateTime lastModDate)
        {
            return DataAccess.WorkOrder.WorkOrderDL.UpdateProjectStatus(projectPK, Status, User, lastModDate);
        }

        public static string GetStockAdjustmentBatches(int WOPK)
        {
            return DataAccess.WorkOrder.WorkOrderDL.GetStockAdjustmentBatches(WOPK);
        }

        public static int SaveStockAdjustment(string xml, out string refNo)
        {
            return DataAccess.WorkOrder.WorkOrderDL.SaveStockAdjustment(xml, out refNo);
        }
    }
}
