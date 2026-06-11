using System;
using System.Data;
using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.StoreManagement
{
   public class MaterialConsumptionDL
    {
        //SaveConsumptionDetails
        /// <summary>
        /// SAVING REQUISITION DETAILS
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns> INT</returns>
        public static List<object> SaveConsumptionDetails(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_MaterialComsumption.CONSUMPTIONXML, strxml),
                  new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.SRSNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
               
                new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.CONSUMPTIONRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.SAVECONSUMPTIONXML, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_MaterialComsumption.CONSUMPTIONRETURNVALUE]).Value);
            string srsNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_MaterialComsumption.SRSNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(srsNo);
            return retvals;

        }

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
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  sbuPk)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetConsumptionList(GridPrams grid, int bizUnit, User objUser, int procID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.REQUISITIONLSTSTATUS , grid.SearchBy == ((object)DBNull.Value).ToString()? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.REQUISITIONLSTSRCH , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy==GTIService.Constants.Store.Fields_MaterialConsumption.ICHNO ||  grid.SortBy==GTIService.Constants.Store.Fields_MaterialConsumption.ICHDATE ?GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK:grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,  grid.SortDirection)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETCONSUMPTIONLIST, colParameters);
            return dtRequisition;


        }
       //GetICHNo
        /// <summary>
        /// Function Used To Get SRS No 
        /// </summary>
        /// <returns>string</returns>
        public static string GetICHNo()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
               
                new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK, 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETICHNO, colParameters);

            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK]).Value);

        }

        /// <summary>
        /// Get Requisition Details By Requisition Id as A Xml Format
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns>Xml Formatted Requisition Details</returns>
        public static string GetConsumptionDetails(int consumptionID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK ,  consumptionID)
            };
            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETITEMCONSUMPTION, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
                strRetVal += dtRequisition.Tables[0].Rows[i][0].ToString();
            return strRetVal;

            //return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETITEMCONSUMPTION, colParameters));

        }
        /// <summary>
        /// Delete Requisition Details By MRHPK
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>int= 1(Success)</returns>
        public static int DeleteConsumptionDtls(int ICHPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK,  ICHPK == 0 ? (object)DBNull.Value :  ICHPK),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_MaterialComsumption.REQUISITIONRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.DELETECONSUMPTION, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_Requisition.REQUISITIONRETURNVALUE]).Value);



        }

        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns></returns>
        public static DataSet GetConsumptionReportByReqId(int requistID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             new DBService.Parameters( GTIService.Constants.Store.Parameters_MaterialComsumption.ICHPK ,  requistID),  
             
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETCONSREPORT, colParameters);
            return dtRequisition;


        }


    }
}
