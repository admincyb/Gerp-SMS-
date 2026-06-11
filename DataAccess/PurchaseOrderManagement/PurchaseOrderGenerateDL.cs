using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.PurchaseOrderManagement
{
    public class PurchaseOrderGenerateDL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="vendor"></param>
        /// <param name="poID"></param>
        /// <returns></returns>
        public static DataSet GetPendingPurchaseRequest(BusinessObject.GridPrams gridPrams, int bizUnit, int vendor, int poID,int userPK,int prhPK,int processId=0,int type=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK , poID == 0 ? (object) DBNull.Value : poID),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_PRH_PK , prhPK == 0 ? (object) DBNull.Value : prhPK),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.PODEPK , gridPrams.P_DeptPK == 0 ? (object) DBNull.Value :  gridPrams.P_DeptPK),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.USERRPK , userPK),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_SORT_BY , gridPrams.SortBy),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_SORT_DIR, gridPrams.SortDirection),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.SEARCHNAME , gridPrams.SearchBy == "0" || gridPrams.SearchBy == "Date"  ? (object)DBNull.Value : gridPrams.SearchBy ),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.SEARCHVAL , gridPrams.SearchValue == "" ? (object)DBNull.Value : "%" + gridPrams.SearchValue + "%"),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.FIELDS , gridPrams.Fields == "" ? "*" : gridPrams.Fields),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.BIZUNIT, bizUnit==0 ? (object) DBNull.Value : bizUnit  ),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.VENDOR,  vendor == 0 ? (object) DBNull.Value : vendor),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , gridPrams.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(gridPrams.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, gridPrams.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(gridPrams.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PROCESS,  processId > 0 ? processId : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE,  type > 1 ? type : (object)DBNull.Value),//Type 1 represent normal PR and not required type value
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETPENDINGPR, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseRequestItemVendor(int poID, int bizUnit, string xmlPRItem)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK , poID > 0 ? poID : (object)DBNull.Value),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlPRItem), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETVENDORPRITEM, colParameters).Tables[0];
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="poID"></param>
       /// <returns></returns>
       
        public static DataTable GetPOVendor(int poID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK , poID > 0 ? poID : (object)DBNull.Value),  
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPPUR_ORDER_DTL_GET, colParameters).Tables[0];
        }

        public static DataSet GetVendorRates(int vendorID, string xmlItem)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.VENDORPK , vendorID > 0 ? vendorID : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlItem), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETVENDORITEMRATES, colParameters);
        }

        public static DataSet GetGrnQty(int podPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_POD_PK , podPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPPUR_ORDER_GRN_QTY_CHECK, colParameters);
        }
        public static DataSet GetRevisionHistory(int pohPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK , pohPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETREVISIONHISTORY, colParameters);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, BusinessObject.User objUser, int vendor, int poPK, int DeptPK = 0, int processPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE,  searchValue),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.VENDOR, vendor > 0 ? vendor : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK , poPK),
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.PODEPK , DeptPK >0 ? DeptPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objUser.PKUser) ,
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_PROCESS , processPk >0 ? processPk : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETSEARCHAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetConfigMaster(BusinessObject.User objUser,string CfgType,int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK,0),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,  Active),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, CfgType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETCONFIGMASTER, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseOrderTypes(BusinessObject.User objUser, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK,0),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,  Active),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objUser.PKUser)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPADM_PO_TYPE_GET_KV, colParameters).Tables[0];
        }


       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <param name="poNumber"></param>
        /// <returns></returns>
        public static List<object> SavePODetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.XMLPO ,(object)xmlstr,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.RETVAL , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SAVEPURCHASEORDER, colParameters);
            string poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.RETVAL]).Value.ToString();
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(poNumber);
            return retvals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <param name="poNumber"></param>
        /// <returns></returns>
        public static List<object> SavePODetailsWkf(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.XML, xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPPUR_ORDER_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            string poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(poNumber);
            retvals.Add(refPK);
            return retvals;
        }


        public static string GetPurchaseOrderNonStock(int pohPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID, pohPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPURCHASEORDER, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// 
        /// </summary>       
        /// <param name="bizUnit"></param>      
        /// <param name="poID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderMapPR(int bizUnit, int POD_PK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.P_POD_PK , POD_PK == 0 ? (object) DBNull.Value : POD_PK),            
             // new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.BIZUNIT, bizUnit==0 ? (object) DBNull.Value : bizUnit  ),             
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETPURORDERREQMAP, colParameters);
        }

        public static DataTable GetPONonStockHistory(int pohPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.PurchaseOrderGenerates.Parameters.POPK, pohPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.GETREVISIONHISTORY, colParameters).Tables[0];
                      
        }



        /// <summary>
        /// Get PO department PK
        /// </summary>
        /// <param name="CrDrPk"></param>
        /// <returns></returns>
        public static int GetPODepartment(long CrDrPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_CDH_PK ,CrDrPk)                
            };
            int PoDeptPk = Convert.ToInt32(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPFIN_CRDR_INV_DEPT_GET, colParameters));
            return PoDeptPk;
        }

        #region Trading
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <param name="poNumber"></param>
        /// <returns></returns>
        public static List<object> SavePODetailsTrading(string xmlstr)
        {           
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.XML, xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPPUR_ORDER_TRD_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            string poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(poNumber);
            retvals.Add(refPK);
            return retvals;
        } 
        #endregion

        public static string GetVendorPOs(int VendorPk, string PONo)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_VEN_PK, VendorPk == 0 ? (object)DBNull.Value : VendorPk),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_NO, string.IsNullOrEmpty(PONo.Trim()) ? (object)DBNull.Value : PONo.Trim())
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SPPUR_ORDER_VENDOR_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static string GetPODetails(int POPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID, POPk)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SPPUR_ORDER_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetPOListItemDetails(int transTYPE, int transPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_TRX_TYPE, transTYPE),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_TRX_PK, transPK)          
            };
            DataTable dtTable = new DataTable();
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SPPUR_ORDER_LIST_DTL_GET, colParameters);
            if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                dtTable = dsResult.Tables[0];
            return dtTable;
        }

        public static DataTable GetPurchaseOrderProjectBudget(string Investor, int CurrencyPK, string PODate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_INVESTOR, Investor),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_CUR_PK, CurrencyPK),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_DATE, PODate)
            };
            DataTable dtTable = new DataTable();
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SPPUR_ORDER_HDR_PROJECT_BUDGET_BAL_GET, colParameters);
            if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                dtTable = dsResult.Tables[0];
            return dtTable;
        }
    }
}
