using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Constants;
using BusinessObject.OrderPlanningBO;
using ERP.Utilities.Constants.DA;

namespace DataAccess.OrderPlanningDA
{
    public class OrderPlanningDA
    {
        #region Methods

        /// <summary>
        /// Method to get Pending Orders
        /// </summary>
        /// <returns></returns>
        public static string GetPendingOrdersList(int Bizunit, string strxml, string ReqDate, int PageIndex = 0, int PageSize = 0, int PlanTrxPK = 0, int WithStock = 0, int BalToAllocate = 0, int BalToPlan = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string strRetVal = "";
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_BIZUNIT , Bizunit),
                new DBService.Parameters(OrderPlanning.P_PAGE_NO , PageIndex > 0 ? PageIndex : 1),
                new DBService.Parameters(OrderPlanning.P_PAGE_SIZE , PageSize> 0 ? PageSize : 2000),
                new DBService.Parameters(CommonConstants.XML, string.IsNullOrEmpty(strxml) ? (object)DBNull.Value : strxml),
                new DBService.Parameters(OrderPlanning.P_SOD_REQUIRED_BY, string.IsNullOrEmpty(ReqDate) ? (object)DBNull.Value : ReqDate),
                new DBService.Parameters(OrderPlanning.P_PNH_PK,PlanTrxPK>0 ? PlanTrxPK : (object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_WITH_STOCK, WithStock),
                new DBService.Parameters(OrderPlanning.P_BAL_TO_ALLOCATE, BalToAllocate),
                new DBService.Parameters(OrderPlanning.P_BAL_TO_PLAN, BalToPlan)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_PEND_SAL_ORDER_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// To get Product Properties with values
        /// </summary>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static string GetProductProperies(int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string stringVal = string.Empty;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_CNG_BIZUNIT, bizunit)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_ITEM_SPEC_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                stringVal += dr[0].ToString();
            }
            return stringVal;
        }

        public static DataTable GetAllLine(int bizUnit, int LinePK = 0, int DeptPk = 0, int Active = 1, int? IsVirtual = null)// to bind shift in Bin List page 
        {
            //Data Table for holding shift reports
            DataTable dtLine;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;


            colParameters = new DBService.Parameters[]
            {

            new DBService.Parameters("P_LNE_ACTIVE",Active),
            new DBService.Parameters("P_BIZUNIT",bizUnit),
            new DBService.Parameters("P_LNE_PK",LinePK),
            new DBService.Parameters("P_MNU_DEPT", DeptPk == 0 ? (object)DBNull.Value : DeptPk),
            new DBService.Parameters("P_LNE_VIRTUAL", IsVirtual== null ? (object)DBNull.Value : IsVirtual)
            };
            dtLine = dbService.DataAdapter(CommandType.StoredProcedure, "SPPRD_LINE_MST_GET_KV", colParameters).Tables[0];

            return dtLine;
        }

        /// <summary>
        /// Save Order Plan
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveOrderPlan(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, strxml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        /// <summary>
        /// Revert Order Plan
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int RevertOrderPlan(int PlanPK, int user, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK),
                new DBService.Parameters(CommonConstants.USERPK, user),
                new DBService.Parameters(CommonConstants.LASTMODDATE,!string.IsNullOrEmpty(lastModDate) ? lastModDate : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_REVERT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }
        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetProductPlanGroups(int PlanGroupPK, int Active, int Bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PIG_PK, PlanGroupPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE, Active),
                new DBService.Parameters(OrderPlanning.P_BIZUNIT, Bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_ITEM_GROUP_MST_GET_KV, colParameters).Tables[0];
            return dtGroups;
        }

        /// <summary>
        /// To get Order Plan 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetOrderPlan(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string stringVal = string.Empty;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK, pk)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                stringVal += dr[0].ToString();
            }
            return stringVal;
        }

        /// <summary>
        /// To get Plan list
        /// </summary>
        /// <param name="Bizunit"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PlanName"></param>
        /// <param name="Version"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static DataTable GetPlanList(int Bizunit, int PageIndex = 0, int PageSize = 0, int PlanPK = 0, int status = -1, string FromDate = null, string ToDate = null, string SortExpression = null, string SortDirection = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK>0 ? PlanPK : (object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_ACTIVE, status == -1? (object)DBNull.Value : status),
                new DBService.Parameters(OrderPlanning.P_FROM_DT, !string.IsNullOrEmpty(FromDate)? FromDate: (object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_TO_DT, !string.IsNullOrEmpty(ToDate)? ToDate: (object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_PAGE_NO, PageIndex > 0 ? PageIndex :(object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_PAGE_SIZE, PageSize> 0 ? PageSize :(object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_BIZUNIT, Bizunit),
                new DBService.Parameters(OrderPlanning.P_SORT_BY, !string.IsNullOrEmpty(SortExpression) ? SortExpression :(object)DBNull.Value),
                new DBService.Parameters(OrderPlanning.P_SORT_DIR, !string.IsNullOrEmpty(SortDirection) ? SortDirection :(object)DBNull.Value)
            };
            DataTable dtList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_GET_LIST, colParameters).Tables[0];
            return dtList;
        }

        /// <summary>
        /// To delete Order Plan Trnasaction
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteOrderPlan(int PlanPK, string lastModDate, int user, int cancelflag = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK),
                new DBService.Parameters(OrderPlanning.P_LAST_MOD_DT, lastModDate),
                new DBService.Parameters(OrderPlanning.P_PNH_IS_CANCEL,cancelflag),
                new DBService.Parameters(CommonConstants.USERPK,user),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Line List Details - From Line Master
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetLinestByProductGroup(int linePK, int active, int sbu, int planGroup = 0, int size = 0)
        {
            DataTable dtLineList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_LINEPK, linePK),
                 new DBService.Parameters(OrderPlanning.P_LNE_ACTIVE, active),
                 new DBService.Parameters(OrderPlanning.P_BIZUNIT, sbu==0?(object)DBNull.Value:sbu),
                 new DBService.Parameters(OrderPlanning.P_ITM_PLAN_GROUP, planGroup > 0 ? planGroup : (object)DBNull.Value),
                 new DBService.Parameters(OrderPlanning.P_ISD_SIZE, size > 0 ? size : (object)DBNull.Value)
            };
            dtLineList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_LINE_GET, colParameters).Tables[0];
            return dtLineList;
        }

        /// <summary>
        /// Method to Get Line List Details - From Line Master
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static string GetLineDetails(string xml)
        {
            DataTable dtLineList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string stringVal = string.Empty;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(CommonConstants.XML, xml)
            };
            dtLineList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_LINE_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtLineList.Rows)
            {
                stringVal += dr[0].ToString();
            }
            return stringVal;
        }

        /// <summary>
        /// To activate/deactivate Order Plan
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="active"></param>
        /// <param name="user"></param>
        /// <param name="lastmodDate"></param>
        /// <returns></returns>
        public static int UpdateOrderPlanStatus(int PlanPK, int active, int user, string lastmodDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK),
                new DBService.Parameters(OrderPlanning.P_ACTIVE, active),
                new DBService.Parameters(CommonConstants.USERPK, user),
                new DBService.Parameters(OrderPlanning.P_LAST_MOD_DT, !string.IsNullOrEmpty(lastmodDate) ? lastmodDate : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_HDR_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        /// <summary>
        /// To get autocomplete for Plan Name/Code
        /// </summary>
        /// <param name="searchField"></param>
        /// <param name="searchval"></param>
        /// <param name="bizunit"></param>
        /// <returns></returns>
        public static DataTable GetPlanNameAuto(string searchField, string searchval, int bizunit, int active)
        {
            DataTable dtSearchList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_FLD_NAME, searchField),
                 new DBService.Parameters(OrderPlanning.P_VALUE,  searchval),
                 new DBService.Parameters(OrderPlanning.P_BIZUNIT, bizunit==0?(object)DBNull.Value:bizunit),
                 new DBService.Parameters(OrderPlanning.P_ACTIVE,active == -1 ? (object)DBNull.Value : active)
            };
            dtSearchList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_AUTO, colParameters).Tables[0];
            return dtSearchList;
        }

        /// <summary>
        /// To get Summary Details
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="type"></param>
        /// <param name="typePK"></param>
        /// <returns></returns>
        public static DataTable GetSummaryDetails(int PlanPK, int type, int typePK = 0)
        {
            DataTable dtData;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK),
                 new DBService.Parameters(OrderPlanning.P_PNH_GROUP, type),
                 new DBService.Parameters(OrderPlanning.P_PNH_GROUP_PK, typePK > 0 ? typePK : (object)DBNull.Value)
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_LINE_SUMM_GET_XML, colParameters).Tables[0];
            return dtData;
        }

        /// <summary>
        /// save allocation 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveAllocation(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, xml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_WIP_ALLOC_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        public static DataTable GetAllocationDetails(string ScDtlPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_SOD_PK, ScDtlPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_ALLOCATION_BIN_SO_MAP_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// save allocation 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveDeAllocation(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, xml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_WIP_DEALLOC_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        /// <summary>
        /// To get Older Versions of a Plan
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="bizunit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetPlanVersions(int PlanPK, int bizunit, int active)
        {
            DataTable dtSearchList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK),
                 new DBService.Parameters(OrderPlanning.P_BIZUNIT, bizunit==0?(object)DBNull.Value:bizunit),
                 new DBService.Parameters(OrderPlanning.P_ACTIVE,active == -1 ? (object)DBNull.Value : active)
            };
            dtSearchList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPARC_PRD_PLAN_TRX_GET_KV, colParameters).Tables[0];
            return dtSearchList;
        }

        /// <summary>
        /// To getLine wise summary of a plan
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <returns></returns>
        public static DataTable GetLineWiseSummary(int PlanPK)
        {
            DataTable dtSearchList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_PNH_PK, PlanPK)
            };
            dtSearchList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_TRX_LINE_SUMM_GET, colParameters).Tables[0];
            return dtSearchList;
        }

        /// <summary>
        /// For Transaction Print OP
        /// </summary>
        /// <param name="planPK"></param>
        /// <returns></returns>
        public static DataSet GetOPReportData(int planPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK,planPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPPRD_PLAN_TRX_RPT, colParameters);
        }

        /// <summary>
        /// For Version wise Print OP
        /// </summary>
        /// <param name="planPK"></param>
        /// <returns></returns>
        public static DataSet GetOPReportDataVersionwise(int planPK, string version)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK,planPK),
                new DBService.Parameters(OrderPlanning.P_PNH_VERSION, version)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SP_SPARC_PRD_PLAN_TRX_RPT, colParameters);
        }

        /// <summary>
        /// To show Production Progress Analysis Report
        /// </summary>
        /// <param name="planPK"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetOProductionProgressReport(int planPK, string date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_PNH_PK,planPK),
                new DBService.Parameters(OrderPlanning.P_TO_DATE, date)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_PRDN_PROG_LINE_RPT, colParameters);
        }

        /// <summary>
        /// Pending Order Details report
        /// </summary>
        /// <param name="planPK"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetPendingOrderDetailsReport(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_BIZUNIT,bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_PEND_SAL_ORDER_GET_RPT, colParameters);
        }

        /// <summary>
        /// To get Sale Contract details
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="bizunit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetSaleContractDetails(GridPram grdParam, int bizUnit, string fromDate, string toDate, int isAllocated, int isProduced, int customerID = 0, int SaleContractID = 0, int ProductID = 0, int PdtGroupID = 0)
        {
            DataTable dtSearchList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_BIZUNIT, bizUnit),
                 new DBService.Parameters(OrderPlanning.P_PAGE_NO, grdParam.PageNumber),
                 new DBService.Parameters(OrderPlanning.P_PAGE_SIZE, grdParam.PageSize),
                 new DBService.Parameters(OrderPlanning.P_FROM_DT,fromDate == string.Empty ? (object)DBNull.Value : fromDate),
                 new DBService.Parameters(OrderPlanning.P_TO_DT,toDate == string.Empty ? (object)DBNull.Value : toDate),
                 new DBService.Parameters(OrderPlanning.P_IS_ALLOCATED,isAllocated),
                 new DBService.Parameters(OrderPlanning.P_IS_PRODUCED,isProduced),
                 new DBService.Parameters(OrderPlanning.P_SOH_CUSTOMER,customerID > 0 ? customerID : (object)DBNull.Value),
                 new DBService.Parameters(OrderPlanning.P_SOH_PK,SaleContractID > 0 ? SaleContractID : (object)DBNull.Value),
                 new DBService.Parameters(OrderPlanning.P_ITM_PK,ProductID > 0 ? ProductID : (object)DBNull.Value),
                 new DBService.Parameters(OrderPlanning.P_ITM_PLAN_GROUP,PdtGroupID > 0 ? PdtGroupID : (object)DBNull.Value)
            };
            dtSearchList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PRDN_ALLOC_GET_LIST, colParameters).Tables[0];
            return dtSearchList;
        }

        /// <summary>
        /// to Get Allocated Qty details by PK
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetAllocatedQtyDetailsByPk(int sodPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_SOD_PK, sodPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, OrderPlanning.SPPRD_PRDN_ALLOC_BIN_ALLOCATED_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// to Get Allocated Qty details by PK
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetProducedQtyDetailsByPk(int sodPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(OrderPlanning.P_SOD_PK, sodPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, OrderPlanning.SPPRD_PRDN_ALLOC_BIN_PRODUCED_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// To get Bin details
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="bizunit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetBinDetails(int sodPk)
        {
            DataTable dtBinList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_SOD_PK,sodPk > 0 ? sodPk : (object)DBNull.Value)
            };
            dtBinList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PRDN_ALLOC_BIN_GET, colParameters).Tables[0];
            return dtBinList;
        }

        public static DataTable GetReleaseScs(int bsu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_BIZUNIT, bsu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_WIP_ALLOC_SC_RELEASE_GET, colParameters).Tables[0];
        }

        public static int SaveRelease(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, xml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SPPRD_WIP_ALLOC_SC_RELEASE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }
        public static DataTable GetSCBinDetails(int ScPk)
        {
            DataTable dtBinList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(OrderPlanning.P_SOD_PK,ScPk)
            };
            dtBinList = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_WIP_ALLOC_SC_RELEASE_BIN_GET, colParameters).Tables[0];
            return dtBinList;
        }
        #endregion

        /// <summary>
        /// save allocation 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveProductionAllocation(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, xml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, OrderPlanning.SPPRD_PRDN_ALLOC_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }


        public static string GetScnarioDetails(string strxml, ref int ReturnVal)
        {
            string stringVal = string.Empty;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, strxml),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            DataSet dtxml = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_SCENARIO_GET, colParameters);
            ReturnVal = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            foreach (DataRow dr in dtxml.Tables[0].Rows)
            {
                stringVal += dr[0].ToString();
            }
            return stringVal;
        }

        public static string GetGroupLines(string strxml)
        {
            string stringVal = string.Empty;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.XML, strxml)
            };
            DataSet dtxml = dbService.DataAdapter(CommandType.StoredProcedure, OrderPlanning.SPPRD_PLAN_PRODUCT_LINE_GET, colParameters);
            foreach (DataRow dr in dtxml.Tables[0].Rows)
            {
                stringVal += dr[0].ToString();
            }
            return stringVal;
        }
    }
}
