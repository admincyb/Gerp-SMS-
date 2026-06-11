using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Production;

namespace DataAccess.Production
{
   public class CompoundPreparationDL
    {
        /// <summary>
        /// Compound Preparation Print Report
        /// </summary>
       public static DataSet GetCmpPreparationReport(int RequistID)
        {
            DataSet dsProcess = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {                
                    new DBService.Parameters(GTIService.Constants.Production.Parameters.COMPOUNDPK, RequistID),  
                };
            dsProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCMPPRINTREPORT, colParameters);
            return dsProcess;
        }

        /// <summary>
        /// Function Used To get the next Batch No
        /// </summary>
        /// <returns></returns>
        public static string GetCompoundBatchNo()
        {
            DBService dbService = new DBService();
            int result;
            DBService.Parameters[] colParameters = null;
             colParameters = new DBService.Parameters[] 
            {   
            
             // new DBService.Parameters( GTIService.Constants.Production.Parameters.COMPMAX, null,ParameterDirection.Output)
              new DBService.Parameters(GTIService.Constants.Production.Parameters.COMPMAX, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
              
            };
             dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCOMPOUNDNO, colParameters);
            result = int.Parse(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters.COMPMAX]).Value.ToString());
            return Convert.ToString(result);
        }

        /// <summary>
        /// Get Compounds  for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCompoundsForCombo(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
      
              
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCOMPOUNDFORCOMBO, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Get Plans  for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlansForCombo(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
      
              
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETPLANSFORCONBO, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Get Tanks  for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTankForCombo(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters("TNK_BIZUNIT", bizUnit)
      
              
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETTANKCOMBO, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Get Tanks  for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTankForComboOnly(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Production.Parameters.BIZUNIT, bizUnit)
      
              
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GET_COMPOUND_TANK, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Get Compound Details By Compound Id as A Xml Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>Xml Formatted Dispersion Details</returns>
        public static string GetCompoundDetails(int compoundID, int deptID, int bizUnit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Production.Parameters.COMPPK ,  compoundID),   
                new DBService.Parameters( GTIService.Constants.Production.Parameters.DEPTPKCMP ,  deptID),  
                new DBService.Parameters(GTIService.Constants.Production.Parameters.BIZUNIT, bizUnit)
            };

            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCOMPDTL, colParameters).ToString();
        }

        /// <summary>
        /// Save Compound Transaction - Save Compound header and  Material Details - Pass as Xml Format
        /// </summary>
        /// <param name="DispersionMaster"></param>
        /// <returns>CompoundTRxPk/ -1 Exception </returns>
        public static List<object> SaveCompoundTrxDetails(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Production.Parameters.STRXML , strxml),  
                 new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                  new DBService.Parameters(GTIService.Constants.Production.Parameters.BATCHNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar)

            };

            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.SAVECOMTRXML, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
            string batchNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters.BATCHNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(batchNo);
            return retvals;
        }

       /// <summary>
        /// Get compound transaction list
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetCompoundTrxList(GridPrams grid, User objUser, int procId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : "%"+ grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? "CTH_PK": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procId ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) )
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCOMPOUNDTRXLIST, colParameters);
        }

        /// <summary>
        /// Get batch lists 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBatchesForItem(int itemCatagory, int itemPk, int batchPK, int compPK, int CompDtlPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMCATEGORY, itemCatagory),
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMPK, itemPk),
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMBATCHPK, batchPK == 0?(object)DBNull.Value : batchPK),
               new DBService.Parameters(GTIService.Constants.Production.Parameters.COMPPK, compPK), 
               new DBService.Parameters(GTIService.Constants.Production.Parameters.DETAILPK, CompDtlPK == 0?(object)DBNull.Value : CompDtlPK)
            };
            DataTable dtBatch = new DataTable();
            dtBatch = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETBATCHFORITEMS, colParameters).Tables[0];
            return dtBatch;
        }

        /// <summary>
        /// Get batch lists 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCategoryItemBatch(int itemCatagory, int itemPK)
        {
            DataTable dtBatch = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMTYPE, itemCatagory),
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMPK, itemPK)
            };
            dtBatch = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCATEGORYITEMBATCH, colParameters).Tables[0];
            return dtBatch;
        }

        /// <summary>
       /// Get Batch details
       /// </summary>
       /// <param name="itemCatagory"></param>
       /// <param name="itemPk"></param>
       /// <param name="batchId"></param>
       /// <returns></returns>
        public static DataTable GetStockValue(int itemCatagory, int itemPk,int  batchId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMCATEGORY, itemCatagory),
               new DBService.Parameters(GTIService.Constants.Production.Parameters.ITEMPK, itemPk),
                new DBService.Parameters(GTIService.Constants.Production.Parameters.BATCHPK, batchId),
      
              
            };

            DataTable dtStock = new DataTable();
            dtStock = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETSTOCKVALUE, colParameters).Tables[0];
            return dtStock;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compoundPk"></param>
        /// <returns></returns>
        public static int DeleteCompopundPreparation(int compoundPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters.COMPOUNDPK,  compoundPk ),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.DELETECOMPOUNDTRX, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL]).Value);

        }

        /// <summary>
        /// 'Get Compound Preparation AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, User objUser, int procId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK   ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procId ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETAUTOCOMPLETECOMPOUNDTRX, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Compound Preparation Details By Compound Id as A Xml Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>Xml Formatted Dispersion Details</returns>
        public static string GetCompoundPreparationDetails(int compoundID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Production.Parameters.COMPOUNDTRXPK ,  compoundID),  

            };

            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETCOMPOUNDTRXDETAIL, colParameters).ToString();
        }


        /// <summary>
        /// GetSelectedTankDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSelectedTankDetails(int tnkPK,int tnkType,int bizUnit,int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters.TNK_PK, tnkPK),
                new DBService.Parameters(GTIService.Constants.Production.Parameters.TNK_TYPE, tnkType==0?(object)DBNull.Value:tnkType),
                new DBService.Parameters(GTIService.Constants.Production.Parameters.TNK_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Production.Parameters.TNK_ACTIVE, active)      
  
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETTANKCOMBO, colParameters).Tables[0];
            return dtDispersions;
        }


    }
}
