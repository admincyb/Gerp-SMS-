using System;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class StoreRequisitionSlipDL
    {
        #region Methods

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="sbuPk"></param>
        /// <param name="objUser"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.REQUISITIONSRCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.REQUISITIONSRCHVALUE ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  sbuPk),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.USERPK,  objUser.PKUser),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PROCESSID,  procID)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetRequisitionList(GridPrams grid, int bizUnit, User objUser, int procID, string PageUrl, int DeptPK=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.REQUISITIONLSTSTATUS , grid.SearchBy == "0" || grid.SearchBy == "Date"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.REQUISITIONLSTSRCH , grid.SearchValue == string.Empty ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy==GTIService.Constants.Store.Fields_RequisitionSlip.MRHNO ||  grid.SortBy==GTIService.Constants.Store.Fields_RequisitionSlip.MRHDATE ?GTIService.Constants.Store.Parameters_Requisition.REQUISITIONID:grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,  grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.P_MRH_DEPT , DeptPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE, grid.FromDate==string.Empty ?(object)DBNull.Value:grid.FromDate),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE,grid.ToDate==string.Empty ?(object)DBNull.Value:grid.ToDate),    
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.MRH_STATUS,grid.FilterStatus=="-1"?(object)DBNull.Value:Convert.ToInt16(grid.FilterStatus))    
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETREQUISITIONLISTBYSRH, colParameters);
            return dtRequisition;


        }
        /// <summary>
        /// Delete Requisition Details By MRHPK
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>int= 1(Success)</returns>
        public static int DeleteRequisitionDtls(int MRHPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.REQUISITIONID,  MRHPK == 0 ? (object)DBNull.Value :  MRHPK),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.REQUISITIONRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.DELETEREQUISITIONDETAILS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_Requisition.REQUISITIONRETURNVALUE]).Value);



        }
        /// <summary>
        /// Get Requisition Details By Requisition Id as A Xml Format
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns>Xml Formatted Requisition Details</returns>
        public static string GetRequisitionDetails(int requisitionID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.MRDPK ,  requisitionID),  

                 new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.REQUISITIONRETURNVALUE, 0,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETREQUISITIONDETAILSBYREQUISITIONID, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
                strRetVal += dtRequisition.Tables[0].Rows[i][0].ToString();
            return strRetVal;

            //  return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETREQUISITIONDETAILSBYREQUISITIONID, colParameters));

        }

        public static DataTable GetInvStoreDepartment(bool active, int parent, int type, int category)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.DPT_ACTIVE ,  active),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_PARENT ,  parent),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_TYPE ,  type),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_CATEGORY,  category)
            };
            DataTable dtInvStoreDpt = new DataTable();
            dtInvStoreDpt = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.SPADMDEPTMSTGETKV, colParameters).Tables[0];
            return dtInvStoreDpt;
        }

        #endregion
    }
}
