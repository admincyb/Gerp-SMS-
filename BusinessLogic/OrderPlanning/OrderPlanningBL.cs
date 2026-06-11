using BusinessObject.CommonManagement;
using BusinessObject.OrderPlanningBO;
using DataAccess.OrderPlanningDA;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogic.OrderPlanning
{
    public class OrderPlanningBL
    {

        #region Methods
        /// <summary>
        /// Method to get Pending Orders
        /// </summary>
        /// <returns></returns>
        public static PendingOrderBO GetPendingOrdersList(int Bizunit, string strxml, string ReqDate, int PageIndex = 0, int PageSize = 0, int PlanTrxPK = 0, int WithStock = 0, int BalToAllocate = 0, int BalToPlan = 0)
        {
            string List = OrderPlanningDA.GetPendingOrdersList(Bizunit, strxml, ReqDate, PageIndex, PageSize, PlanTrxPK, WithStock, BalToAllocate, BalToPlan);
            if (List != string.Empty)
            {
                PendingOrderBO ObjList = new PendingOrderBO();
                ObjList = (PendingOrderBO)CommonFunctions.DeserializeObject(List, new PendingOrderBO());
                return ObjList;
            }
            else
                return null;
        }
        public static DataTable GetAllLine(int bizUnit, int LinePK = 0, int DeptPk = 0, int Active = 1, int? IsVirtual = null)// to bind Line in Bin List pageGetLineMapedTank
        {
            return OrderPlanningDA.GetAllLine(bizUnit, LinePK, DeptPk, Active, IsVirtual);
        }

        /// <summary>
        /// To get Product Properties with values
        /// </summary>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static ProductPropertyBO GetProductProperies(int bizunit)
        {
            string List = OrderPlanningDA.GetProductProperies(bizunit);
            if (List != string.Empty)
            {
                ProductPropertyBO ObjList = new ProductPropertyBO();
                ObjList = (ProductPropertyBO)CommonFunctions.DeserializeObject(List, new ProductPropertyBO());
                return ObjList;
            }
            else
                return null;
        }

        /// <summary>
        /// Save Order Plan
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveOrderPlan(string strxml)
        {
            return OrderPlanningDA.SaveOrderPlan(strxml);
        }

        /// <summary>
        /// Revert Order Plan
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int RevertOrderPlan(int PlanPK, int user, string lastModDate)
        {
            return OrderPlanningDA.RevertOrderPlan(PlanPK, user, lastModDate);
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
            return OrderPlanningDA.GetProductPlanGroups(PlanGroupPK, Active, Bizunit);
        }
        /// <summary>
        /// To ge
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static OrderPlanBO GetOrderPlan(int pk)
        {
            string List = OrderPlanningDA.GetOrderPlan(pk);
            if (List != string.Empty)
            {
                OrderPlanBO ObjList = new OrderPlanBO();
                ObjList = (OrderPlanBO)CommonFunctions.DeserializeObject(List, new OrderPlanBO());
                return ObjList;
            }
            else
                return null;
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
            return OrderPlanningDA.GetPlanList(Bizunit, PageIndex, PageSize, PlanPK, status, FromDate, ToDate, SortExpression, SortDirection);
        }

        /// <summary>
        /// To delete Order Plan Trnasaction
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteOrderPlan(int PlanPK, string lastModDate, int user, int cancelflag = 0)
        {
            return OrderPlanningDA.DeleteOrderPlan(PlanPK, lastModDate, user, cancelflag);
        }

        /// <summary>
        /// To get all lines by product group
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <param name="productPK"></param>
        /// <returns></returns>
        public static DataTable GetLinestByProductGroup(int linePK, int active, int sbu, int planGroup = 0, int size = 0)
        {
            return OrderPlanningDA.GetLinestByProductGroup(linePK, active, sbu, planGroup, size);
        }

        /// <summary>
        /// To get line details
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static PLanLineBO GetLineDetails(string xml)
        {
            string RetString = OrderPlanningDA.GetLineDetails(xml);
            PLanLineBO ObjList = new PLanLineBO();
            if (RetString != string.Empty)
                ObjList = (PLanLineBO)CommonFunctions.DeserializeObject(RetString, new PLanLineBO());
            return ObjList;
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
            return OrderPlanningDA.UpdateOrderPlanStatus(PlanPK, active, user, lastmodDate);
        }

        /// <summary>
        /// To get autocomplete for Plan Name/Code
        /// </summary>
        /// <param name="searchField"></param>
        /// <param name="searchval"></param>
        /// <param name="bizunit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetPlanNameAuto(string searchField, string searchval, int bizunit, int active)
        {
            //List For Storing All values
            List<AutoCompleteBO> lstValue = new List<AutoCompleteBO>();
            //Filling Parameter table with data from DA Layer
            DataTable dtValue = OrderPlanningDA.GetPlanNameAuto(searchField, searchval, bizunit, active);
            foreach (DataRow drValue in dtValue.Rows)
            {
                AutoCompleteBO value = new AutoCompleteBO()
                {
                    Key = (drValue[CommonConstants.F_AUTO_PK] == DBNull.Value) ? -1 : Convert.ToInt32(drValue[CommonConstants.F_AUTO_PK]),
                    Name = (drValue[CommonConstants.F_AUTO_VALUE] == DBNull.Value) ? String.Empty : HttpUtility.HtmlDecode(Convert.ToString(drValue[CommonConstants.F_AUTO_VALUE])),
                };
                //Add Each Category To category list
                lstValue.Add(value);
            }
            return lstValue;
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
            return OrderPlanningDA.GetSummaryDetails(PlanPK, type, typePK);
        }

        /// <summary>
        /// save allocation
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveAllocation(string xml)
        {
            return OrderPlanningDA.SaveAllocation(xml);
        }

        public static DataTable GetAllocationDetails(string ScDtlPk)
        {
            return OrderPlanningDA.GetAllocationDetails(ScDtlPk);
        }

        /// <summary>
        /// save allocation
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveDeAllocation(string xml)
        {
            return OrderPlanningDA.SaveDeAllocation(xml);
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
            return OrderPlanningDA.GetPlanVersions(PlanPK, bizunit, active);
        }

        /// <summary>
        /// To getLine wise summary of a plan
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <returns></returns>
        public static DataTable GetLineWiseSummary(int PlanPK)
        {
            return OrderPlanningDA.GetLineWiseSummary(PlanPK);
        }

        /// <summary>
        /// For Transaction Print OP
        /// </summary>
        /// <param name="planPK"></param>
        /// <returns></returns>
        public static DataSet GetOPReportData(int planPK)
        {
            return OrderPlanningDA.GetOPReportData(planPK);
        }

        /// <summary>
        /// For Version wise Print OP
        /// </summary>
        /// <param name="planPK"></param>
        /// <returns></returns>
        public static DataSet GetOPReportDataVersionwise(int planPK, string version)
        {
            return OrderPlanningDA.GetOPReportDataVersionwise(planPK, version);
        }

        /// <summary>
        /// To show Production Progress Analysis Report
        /// </summary>
        /// <param name="planPK"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetOProductionProgressReport(int planPK, string date)
        {
            return OrderPlanningDA.GetOProductionProgressReport(planPK, date);
        }

        /// <summary>
        /// Pending Order Details report
        /// </summary>
        /// <param name="planPK"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetPendingOrderDetailsReport(int Bizunit)
        {
            return OrderPlanningDA.GetPendingOrderDetailsReport(Bizunit);
        }


        /// <summary>
        /// To get sale contract details
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetSaleContractDetails(GridPram gridparam, int bizUnit, string fromDate, string toDate, int isAllocated, int isProduced, int customerID = 0, int SaleContractID = 0, int ProductID = 0, int PdrGroupID = 0)
        {
            return OrderPlanningDA.GetSaleContractDetails(gridparam, bizUnit, fromDate, toDate, isAllocated, isProduced, customerID, SaleContractID, ProductID, PdrGroupID);
        }

        /// <summary>
        /// Get Direct Allocated Qty details By Pk
        /// </summary>
        /// <param name="grhPk"></param>
        /// <returns>DataTable</returns>
        public static string GetAllocatedQtyDetailsByPk(int sodPk)
        {
            return OrderPlanningDA.GetAllocatedQtyDetailsByPk(sodPk);
        }

        /// <summary>
        /// Get Direct Produced Qty details By Pk
        /// </summary>
        /// <param name="grhPk"></param>
        /// <returns>DataTable</returns>
        public static string GetProducedQtyDetailsByPk(int sodPk)
        {
            return OrderPlanningDA.GetProducedQtyDetailsByPk(sodPk);
        }

        /// <summary>
        /// To get Bin details
        /// </summary>
        /// <param name="linePK"></param>
        /// <param name="sbu"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetBinDetails(int sodPK)
        {
            return OrderPlanningDA.GetBinDetails(sodPK);
        }

        public static DataTable GetReleaseScs(int bsu)
        {
            return OrderPlanningDA.GetReleaseScs(bsu);
        }

        public static int SaveRelease(string xml)
        {
            return OrderPlanningDA.SaveRelease(xml);
        }
        public static DataTable GetSCBinDetails(int ScPk)
        {
            return OrderPlanningDA.GetSCBinDetails(ScPk);
        }

        /// <summary>
        /// Save Order Plan
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveProductionAllocation(string strxml)
        {
            return OrderPlanningDA.SaveProductionAllocation(strxml);
        }

        public static string GetScnarioDetails(string strxml, ref int ReturnVal)
        {
            return OrderPlanningDA.GetScnarioDetails(strxml, ref ReturnVal);
        }

        public static LineBO GetGroupLines(string strxml)
        {
            string List = OrderPlanningDA.GetGroupLines(strxml);
            if (List != string.Empty)
            {
                LineBO ObjList = new LineBO();
                ObjList = (LineBO)CommonFunctions.DeserializeObject(List, new LineBO());
                return ObjList;
            }
            else
                return null;
        }
        #endregion
    }
}
