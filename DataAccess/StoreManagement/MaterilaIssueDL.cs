using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class MaterilaIssueDL
    {
        /// <summary>
        /// Function Used To get Material Issue xml details based on the id
        /// </summary>
        /// <param name="miID"></param>
        /// <returns></returns>
        public static string GetMaterialIssueDetails(int miID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK , miID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMATERIALISSUE, colParameters).ToString();
        }


        public static string GetMaterialIssueDetailsForReportDOCNOREVISION(int miID, int reportpk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK , miID),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_AST_PK , reportpk)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMATERIALISSUE, colParameters).ToString();
        }

        public static string GetMaterialAcceptDetails(int miID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK , miID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETMATERIALACCEPT, colParameters).ToString();
        }
        public static string GetMIssueDetails(int miID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.M_ICH_PK , miID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMATERIALISSUEREPORT, colParameters).ToString();
        }
        public static string GetMRIssueDetails(int miID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("ICH_PK" , miID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMRISSUE, colParameters).ToString();
        }
        /// <summary>
        /// Get Requesting Store
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static DataTable GetRequestingStore(int appID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MRH_PK , appID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.ITEM_REQUEST_DTL_GET, colParameters).Tables[0];
            return new DataTable();
        }

        public static DataTable GetMRStore(int appID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters("P_PRH_PK" , appID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_MATERIAL_REQUEST_DTL_GET", colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To save Material Issue
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveMaterialIssue(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEXML , xmlstr),
                  new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),

                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SAVEMATERIALISSUE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string miNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();

            retvals.Add(result);
            retvals.Add(miNo);
            return retvals;
        }
        public static List<object> SaveMaterialIssueWkf(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SAVEMATERIALISSUEWKF, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string miNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);

            retvals.Add(result);
            retvals.Add(miNo);
            retvals.Add(refPK);
            return retvals;
        }

        public static List<object> SaveMRIssue(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_XML" , xmlstr),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_MATERIAL_ISSUE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string miNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();
            string LockDate = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE]).Value.ToString();

            retvals.Add(result);
            retvals.Add(miNo);
            if (!string.IsNullOrEmpty(LockDate))
                retvals.Add(Convert.ToDateTime(LockDate).ToString(ERP.Utilities.CommonConstants.DATEFORMAT));
            return retvals;
        }

        public static List<object> SaveMRIssuePlantToPlant(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_XML" , xmlstr),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar)
            };
            List<object> retvals = new List<object>();
            string SaveSP = GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_MATERIAL_ISSUE_INTER_PLANT_SAVE;
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaveSP, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string miNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();
            string LockDate = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE]).Value.ToString();

            retvals.Add(result);
            retvals.Add(miNo);
            if (!string.IsNullOrEmpty(LockDate))
                retvals.Add(Convert.ToDateTime(LockDate).ToString(ERP.Utilities.CommonConstants.DATEFORMAT));
            return retvals;
        }

        public static DataTable GetPendingWIHSearchAuto(string searchBy, string searchValue, int store, User objUser, int department,int PendingWO)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR , store),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR1 , department),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT,objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_IS_COMPLETED,PendingWO)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_WORK_ORDER_ITEM_AUTO, colParameters).Tables[0];
        }


        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPendingSearchAuto(string searchBy, string searchValue, int store, User objUser, int department)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR , store),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR1 , department),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT,objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETPENDINGSEARCH, colParameters).Tables[0];
        }
        public static DataTable GetMIPendingSearchAuto(string searchBy, string searchValue, User objUser, int department, int MenuType = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVALUE , searchValue),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR , store),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHCORR1 , department),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT,objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_PRH_TRX_TYPE,MenuType > 0 ? MenuType : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_MATERIAL_ISSUE_PEND_AUTO", colParameters).Tables[0];
        }

        public static DataSet GetWOPending(GridPrams grid, int sbuID, int store, int miPK, int dept, int mrhPK,int PendingWO)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MIPK , miPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "MRH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.STORE, store),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.DEPARTMENT, dept),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MRH_PK, mrhPK==0 ? (object)DBNull.Value : mrhPK),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_IS_COMPLETED, PendingWO)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_WORK_ORDER_ITEM_LIST_GET, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetSRSPending(GridPrams grid, int sbuID, int store, int miPK, int dept, int mrhPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MIPK , miPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "MRH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.STORE, store),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.DEPARTMENT, dept),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MRH_PK, mrhPK==0 ? (object)DBNull.Value : mrhPK),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETPENDINGSRS, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="miPK"></param>
        /// <param name="dept"></param>
        /// <param name="mrhPK"></param>
        /// <returns></returns>
        public static DataSet GetMRPending(GridPrams grid, int sbuID, int miPK, int dept, int mrhPK, int MenuType = 0, int CurrDept = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.ICHPK , miPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "PRH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.DEPARTMENT, dept),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PRH_PK, mrhPK==0 ? (object)DBNull.Value : mrhPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_PRH_TRX_TYPE, MenuType == 0 ? (object)DBNull.Value : MenuType),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_ISSUE_DEPT, CurrDept == 0 ? (object)DBNull.Value : CurrDept)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETPENDINGMR, colParameters);
        }


        /// <summary>
        /// function for filling prevoius srs detail in grid.this has to transfer to srs details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetPreviousSRSDetailsView(int srsID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SRSPK, srsID),
            };

            DataSet dtProduct = new DataSet();
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETPREVIOUSSRS, colParameters);
        }
        ///// <summary>
        ///// function for filling prevoius material issue(rijoys form) detail in grid.this has to transfer to material isssue.
        ///// </summary>
        ///// <param name="grid"></param>
        ///// <param name="sbuID"></param>
        ///// <returns></returns>
        //public static DataSet GetPreviousMIDetailsView(int miID, int sbuID)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {            
        //      new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
        //      new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MIPK, miID),
        //    };

        //    DataSet dtProduct = new DataSet();
        //    return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAcceptance.GETPREVIOUSMI, colParameters);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetMIList(GridPrams grid, User objUser, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "MIH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVAL , grid.SearchValue == "" ? "%" : "%" + grid.SearchValue + "%"),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SORTBY ,  grid.SortBy == null ||grid.SortBy == "MIH_DATE"|| grid.SortBy == "MIH_NO" ? GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PROCESSID , GTIService.Constants.Common.CommonConstant.MaterialIssueProcessID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGEURL,pageURL),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MIH_STATUS,grid.FilterStatus=="-1"?(object)DBNull.Value:Convert.ToInt16(grid.FilterStatus))
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMILIST, colParameters);
        }
        public static DataSet GetMRIssueList(GridPrams grid, User objUser, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "ICH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SEARCHVAL , grid.SearchValue == "" ? "%" : "%" + grid.SearchValue + "%"),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SORTBY ,  grid.SortBy == null ||grid.SortBy == "ICH_DATE"|| grid.SortBy == "ICH_NO" ? "ICH_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PROCESSID , GTIService.Constants.Common.CommonConstant.MaterialIssueProcessID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.PAGEURL,pageURL),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_ICH_TRX_TYPE, grid.POH_MENU_TYPE > 0 ? grid.POH_MENU_TYPE : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_MATERIAL_ISSUE_GET_LIST", colParameters);
        }

        /// <summary>
        /// Function Used To get MI no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetMINO()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.GETMINO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK]).Value);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="searchBy"></param>
        ///// <param name="searchValue"></param>
        ///// <param name="objUser"></param>
        ///// <returns></returns>
        public static DataTable GetMISearchValue(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK   ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.MILISTAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miPk"></param>
        /// <returns></returns>
        public static List<object> DeleteMaterialIssue(int miPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.MATERIALISSUEPK ,  miPk == 0 ? (object)DBNull.Value :  miPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.DELETEMATERIALISSUE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string retNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();

            retvals.Add(result);
            retvals.Add(retNo);
            return retvals;

        }
        /// <summary>
        /// Delete MR Issue
        /// </summary>
        /// <param name="miPk"></param>
        /// <returns></returns>
        public static List<object> DeleteMRIssue(int miPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( "P_ICH_PK" ,  miPk == 0 ? (object)DBNull.Value :  miPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, "SPINV_MATERIAL_ISSUE_DELETE", colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string retNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();
            string LockDate = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE]).Value.ToString();

            retvals.Add(result);
            retvals.Add(retNo);
            if (!string.IsNullOrEmpty(LockDate))
                retvals.Add(Convert.ToDateTime(LockDate).ToString(ERP.Utilities.CommonConstants.DATEFORMAT));
            return retvals;

        }
        public static List<object> DeleteMRIssuePlantToPlant(int miPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( "P_ICH_PK" ,  miPk == 0 ? (object)DBNull.Value :  miPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, "SPINV_MATERIAL_ISSUE_INTER_PLANT_DELETE", colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.RETVAL]).Value.ToString());
            string retNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.MINO]).Value.ToString();
            string LockDate = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_RET_LOCK_DATE]).Value.ToString();

            retvals.Add(result);
            retvals.Add(retNo);
            if (!string.IsNullOrEmpty(LockDate))
                retvals.Add(Convert.ToDateTime(LockDate).ToString(ERP.Utilities.CommonConstants.DATEFORMAT));
            return retvals;

        }

        // Sumesh 07112011
        /// <summary>
        /// Method to get All Stores
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="category"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>

        public static DataTable GetAllStores(User objUser, int sbuPk, int category)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {   
              //new DBService.Parameters("DPT_TYPE" ,Convert.ToInt32( GTIService.Constants.Administration.Configurations.Departments.MainDepartments.Inventory)),
              new DBService.Parameters("P_BIZUNIT",  sbuPk),
              new DBService.Parameters("DPT_CATEGORY",  category)
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STORE_ISSUE_DEPT_GET_KV", colParameters).Tables[0];
        }

        public static string GetMaterialIssueForRateAdjustment(string FromDate, string ToDate, int bizunit, int DeptPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.FROMDATE, FromDate),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.TODATE, ToDate),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_DEPT_PK, DeptPK),
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_ITEM_CONS_RATE_DIFF_GET, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int? SaveMaterialIssueRateAdjustment(string xmlDoc, out int refID, out string transNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_ITEM_CONS_RATE_DIFF_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_REF_PK]).Value);
            transNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);
            return result;
        }

        public static DataSet GetRateAdjustmentList(int bizunit, int transPK, string transDate, BusinessObject.GridPrams gridParamObj, int DeptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_IRH_PK, transPK > 0 ? transPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_IRH_DATE, string.IsNullOrEmpty(transDate) ? (Object)DBNull.Value : transDate),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_NUM, gridParamObj.PageNumber),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_SIZE, gridParamObj.PageSize),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_DEPT_PK, DeptPK)
            };

            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_ITEM_CONS_RATE_DIFF_GET_LIST, colParameters);
            return dsResult;
        }

        public static string GetRateAdjustmentByPK(int TranPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.P_IRH_PK, TranPK)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_ITEM_CONS_RATE_DIFF_GET_KV, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static DataTable GetMIRateAdjustNumbers(int bizunit, string searchKey)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_VALUE, searchKey)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialIssue.SPINV_ITEM_CONS_RATE_DIFF_AUTO, colParameters).Tables[0];
            return dtResult;
        }
    }
}
