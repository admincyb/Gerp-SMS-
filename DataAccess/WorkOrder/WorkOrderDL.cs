using BusinessObject;
using BusinessObject.WorkOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.WorkOrder
{
    public class WorkOrderDL
    {
        /// <summary>
        /// SAVING Work Order
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns> INT</returns>
        public static int SaveWorkOrder(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.WORKORDERXML, strXml),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_NO , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_SAVE, colParameters);
            string WONo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_NO]).Value.ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// SAVING Work Order Wkf
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns> INT</returns>
        public static int SaveWorkOrderWkf(string strXml, out string woNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.WORKORDERXML, strXml),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_NO , string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_WKF_SAVE, colParameters);
            woNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_NO]).Value.ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataSet GetWorkOrderList(GridPrams pageParam, int subContrPK, int woPK, int woItem, int status, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_SER_NAME, pageParam.SearchBy == string.Empty ? (Object)DBNull.Value : pageParam.SearchBy),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_SER_VAL, pageParam.SearchValue == string.Empty ? (Object)DBNull.Value : pageParam.SearchValue),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_NO, pageParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_SIZE, pageParam.PageSize),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_SORT_BY, pageParam.SortBy == string.Empty ? (Object)DBNull.Value : pageParam.SortBy),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_FROM_DT, pageParam.FromDate == string.Empty ? (Object)DBNull.Value : pageParam.FromDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_TO_DT, pageParam.ToDate == string.Empty ? (Object)DBNull.Value : pageParam.ToDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_BIZUNIT, pageParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_VEN_PK, subContrPK > 0 ? subContrPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITM_PK, woItem > 0 ? woItem : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_STATUS, pageParam.statusPk < 0 ? (Object)DBNull.Value : pageParam.statusPk),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_FT_STATUS, status),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER_PK, pageParam.UserPK > 0 ? pageParam.UserPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_URL, pageURL == string.Empty ? (Object)DBNull.Value : pageURL)
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.GETWOLIST, colParameters);
            return dsList;
        }

        public static DataSet GetWorkOrderDetailsReport(int woID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.WIH_PK , woID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_GST_GET_RPT, colParameters);
        }
        public static DataSet GetWorkOrderDetailsReport(int woID, int version)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.WIH_PK , woID),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.REVWPK,  version)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ARCHIVE_RPT, colParameters);
        }
        public static string GetWorkOrderByPK(int WorkOrderPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK, WorkOrderPK)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.GetWorkOrder, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static DataTable GetAutoCompleteWONumber(byte Active, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                //new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.P_SER_VAL, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.GetWONumberAuto, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetWorkOrderItemsAuto(int Operation, int Type, int Customer, int sbuID, string SearchKey, int BrandPK = 0)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_OPERATION, Operation),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITEM_TYPE, Type),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_CUS_PK, Customer > 0 ? Customer : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BIZUNIT, sbuID),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_SER_VAL, SearchKey),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BRAND, BrandPK > 0 ? BrandPK : (Object)DBNull.Value)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WO_ITEM_MATERIAL_MAP_AUTO, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetWorkOrderItemsForFilterAuto(string SearchKey, int SBUID)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_VALUE, SearchKey),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BIZUNIT, SBUID),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ACTIVE, 1)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_GET_AUTO, colParameters).Tables[0];
            return dtProcess;
        }

        public static int CancelWorkOrder(WorkOrderCancel woCancel)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK , woCancel.WOID),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_SHORT_CLS_REASON , woCancel.Remarks),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_SHORT_CLS_REFNO , woCancel.RefNo),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER_PK , woCancel.UserPk),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_SHORT_CLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
        }

        public static DataTable GetRevisionHistory(int woPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK , woPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.GETREVISIONHISTORY, colParameters).Tables[0];
        }

        public static int DeleteWODetails(int woID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK , woID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        public static DataTable GetPendingBatches(int SubContractorPK, int ItemPK, int ItemTypePK, int GrnPK = 0, int WOPK = 0, int ReturnWOPK = 0, int IsAfterMulti = 0)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_VENDOR, SubContractorPK > 0 ? SubContractorPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITEM_PK, ItemPK > 0 ? ItemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITEM_TYPE, ItemTypePK > 0 ? ItemTypePK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_GRH_PK, GrnPK > 0 ? GrnPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK, WOPK > 0 ? WOPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_TRN_WO, ReturnWOPK > 0 ? ReturnWOPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_GMR_AFTER_MULTI, IsAfterMulti)
            };
            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_PENDING_BATCH_GET, colParameters);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtProcess = dsResult.Tables[0];
            return dtProcess;
        }

        public static void IsStockExist(int ItemPK, int subContractorPK, out int IsExist)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITM_PK, ItemPK > 0 ? ItemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_VENDOR, subContractorPK > 0 ? subContractorPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL , string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WO_ITEM_MATERIAL_STOCK_EXIST, colParameters);
            IsExist = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
        }

        public static DataTable GetIssueDetails(int WOPK)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.P_WIH_PK, WOPK)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_ISSUE_DTL, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetWOItemList(int TrxType, int WOPK)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.P_WIH_TRX_TYPE, TrxType),
                new DBService.Parameters( GTIService.Constants.WorkOrder.Parameters.P_WIH_TRX_PK, WOPK)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_LIST_DTL_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable TemplateDetailsGetNew(int binID, int sbu, int Type = 0, int DeptPk = 0)
        {
            DataTable dtTemplateList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BCH_PK, binID),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BIZUNIT, sbu),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BCH_TYPE, Type == 0 ? (object)DBNull.Value : Type),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_MNU_DEPT, DeptPk == 0 ? (object)DBNull.Value : DeptPk)
            };
            dtTemplateList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SP_BINCARD_GET_NEW, colParameters).Tables[0];
            return dtTemplateList;
        }

        public static DataTable GetBincardDetails(int binID, string binText, int sbu)
        {
            DataTable dtTemplateList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BCH_NO, string.IsNullOrEmpty(binText)? (object)DBNull.Value:binText),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BCH_TYPE, binID==0? (object)DBNull.Value:binID ),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BIZUNIT, sbu),
            };
            dtTemplateList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SP_BINCARD_GET, colParameters).Tables[0];
            return dtTemplateList;
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_NUM,  pageNum),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_SIZE,  pageSize),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_FROM_DATE, fromDate==string.Empty?(object)DBNull.Value:fromDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_TO_DATE, toDate==string.Empty?(object)DBNull.Value:toDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_CUSTOMER, customerId==0?(object)DBNull.Value:customerId),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_TYPE, typeId<1?(object)DBNull.Value:typeId),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_APT_CODE, aptCode),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_CONTRACTOR, contractorId<1?(object)DBNull.Value:contractorId),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PARENT, projectPk.HasValue ? projectPk >0 ? projectPk : (object)DBNull.Value: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_NO, projectNo == null?(object)DBNull.Value:projectNo),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_CODE, projectCode == null?(object)DBNull.Value:projectCode),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_REF_NO, refNo == null?(object)DBNull.Value:refNo),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER, curUserPk == null?(object)DBNull.Value:curUserPk),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_STATUS ,transactionStatus >= 0 ? transactionStatus : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_PAGE_URL ,pageurl),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ACTIVE,status>=0?status:(object)DBNull.Value),
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_GET_LIST, colParameters);
            return dsList;
            //return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_GET_LIST, colParameters);
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK, workOrderPk),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_CUSTOMER, customerId<1?(object)DBNull.Value:customerId),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_APT_CODE, aptCode),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PARENT, parentPk<1 ?(object)DBNull.Value:parentPk),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER, curUserPk == null?(object)DBNull.Value:curUserPk)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_GET_KV, colParameters).Tables[0];
        }
        #endregion
        #region SaveProject
        /// <summary>
        /// SaveWorkOrder
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>int</returns>
        public static int SaveProject(string xml, out string refNo)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_XML,xml),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            refNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Common.PRETNO]).Value.ToString();
            return result;
        }
        #endregion
        #region DeleteWorkOrder
        /// <summary>
        /// DeleteWorkOrder
        /// </summary>
        /// <param name="workOrderPk"></param>
        /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteWorkOrder(int workOrderPk, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK, workOrderPk),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_LAST_MOD_DT,  lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }
        #endregion
        public static DataTable GetHistory(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK, pk!=null?pk:0),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_VER_GET, colParameters).Tables[0];
        }
        public static int AmendSave(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK, pk!=null?pk:0),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_AMEND_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }
        public static DataSet GetWorkOrderReport(int pk, int active, int bizunit, int? version = null, int? showHideDesc = null, int? internalFlag = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK, pk != null ? pk : 0),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_VERSION, version.HasValue?version != -1 ? version:(object)DBNull.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ITEM_IS_DESC, showHideDesc != null ? showHideDesc : 0),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_IS_INTERNAL, internalFlag),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_RPT, colParameters);
        }
        public static int CancelProject(ProjectCancel prjcancel)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK , prjcancel.P_WOH_PK),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_SHORT_CLS_REASON ,prjcancel.P_WOH_SHORT_CLS_REASON),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_SHORT_CLS_REFNO , prjcancel.P_WOH_SHORT_CLS_REFNO),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER_PK , prjcancel.P_USER_PK),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_SHORT_CLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
        }
 
        public static int UpdateProjectStatus(int projectPK, int Status, int User, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WOH_PK,  projectPK ),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_ACTIVE,  Status),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_USER_PK,  User),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_LAST_MOD_DT,lastModDate==null ? (object)DBNull.Value : lastModDate),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_HDR_ACTIVATE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
        }

        public static string GetStockAdjustmentBatches(int WOPK)
        {
            string strRetVal = string.Empty;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_WIH_PK, WOPK)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_BOM_EXCESS_STK_GET, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int SaveStockAdjustment(string xml, out string refNo)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_XML, xml),
                new DBService.Parameters(GTIService.Constants.WorkOrder.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                //new DBService.Parameters(GTIService.Constants.Common.Common.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.WorkOrder.Procedures.SPINV_WORK_ORDER_ITEM_BOM_EXCESS_STK_ISS_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            refNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Common.PRETNO]).Value.ToString();
            return result;
        }
    }
}
