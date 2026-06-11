using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.StoreManagement
{
    public class StoreAuditDL
    {
        /// <summary>
        /// Save Store Audit Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveStoreAuditDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.SAXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.SANO , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.PRETTEXT , "0", 500,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                
            };
            List<object> retvals = new List<object>();
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.SAVESTOREAUDIT, colParameters);
            string storeAuditNo  =((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreAudit.SANO]).Value.ToString();
            int result =  Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString());
            string ReturnText = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreAudit.PRETTEXT]).Value.ToString();
            retvals.Add(result);
            retvals.Add(storeAuditNo);
            retvals.Add(ReturnText);
            return retvals;
        }

        /// <summary>
        /// Get Store Audit Number
        /// </summary>
        /// <returns></returns>
        public static string GetStoreAuditNo()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.SAHPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.GETSTOREAUDITNO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreAudit.SAHPK]).Value);
        }

        /// <summary>
        /// Get Store Audit Details By 
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static string GetStoreAuditDetails(int storeAuditID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtxml = new DataTable();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreAudit.SAHPK ,  storeAuditID)
            };
            dtxml =  dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.GETSTOREAUDITDETAILS, colParameters).Tables[0];
            return dtxml.Rows[0][0].ToString();
        }

        /// <summary>
        /// Methord Used to Get Store Material Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="dept"></param>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static DataTable GetItemDetails(int dept, int itemPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.DEPID , dept),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.ITEMID  , itemPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.GETSTOREITEMDETAILS, colParameters).Tables[0];
        }

        /// <summary>
        /// Methord Used to Get Store Material Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="dept"></param>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static DataTable GetCategoryItemDetails(int dept, int categoryPK, int itemPK, int batchPK, int StkbatchPK=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.DEPID, dept),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.CATEGORYTYPE, categoryPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.ITEMID, itemPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.BATCH, batchPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.STKBATCH, StkbatchPK==0?(object)DBNull.Value:StkbatchPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.GETSTORECATEGORYITEMDETAILS, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static DataTable GetItems(int bizUnit, int store)
        {
            DataTable dtItems = new DataTable();
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                 new DBService.Parameters( ""  , bizUnit), 
                 new DBService.Parameters(""  , store), 
            };
            dtItems = dbService.DataAdapter(CommandType.StoredProcedure, "", colParameters).Tables[0];
            return dtItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetStore(int bizUnit)
        {
            DataTable dtStore = new DataTable();
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                 new DBService.Parameters( ""  , bizUnit), 
               
            };
            dtStore = dbService.DataAdapter(CommandType.StoredProcedure, "", colParameters).Tables[0];
            return dtStore;
        }

        public static DataTable GetDamageTypes(int bizUnit)
        {
            DataTable dtDamageTypes = new DataTable();
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit), 
               
            };
            dtDamageTypes = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETDAMAGETYPE, colParameters).Tables[0];
            return dtDamageTypes;
        }
        /// <summary>
        /// Method to get Store Audit Details For Report
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static DataSet GetStoreAuditReportDetails(int storeAuditID)
        {
            DataSet dsStoreAuditDtls = new DataSet();
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            DataTable dtxml = new DataTable();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreAudit.SAHPK ,  storeAuditID)
            };
            dsStoreAuditDtls = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StoreAudit.GETSTOREAUDITREPORT, colParameters);
            return dsStoreAuditDtls;
        }
    }
}
