using System;
using System.Data;
using BusinessObject;

namespace DataAccess.VendorManagement
{
   public class VendorEvaluationDL
    {

        /// <summary>
        /// SAVING EVALUATION DETAILS
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns> INT</returns>
       public static int SaveEvaluationDetails(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.EVALDTLXML , strxml),  
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDOREVALSAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Vendor.Parameters.PRETVAL]).Value);
            return result;

        }
        /// <summary>
       /// Get EVALUATION DETAILS
     /// </summary>
     /// <param name="PK"></param>
     /// <returns></returns>
       public static string GetEvaluationDetails(int  pK)
       {

           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDOREVALPK , pK),  
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDOREVALGET, colParameters).ToString();
         

       }
        /// <summary>
        /// Method to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue,int bizUnit)
        {
             
            
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.PGSEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.PGSEARCHVAL ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.PBIZUNIT ,  bizUnit)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOREVALSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get Evaluation list 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="VendorID"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public static DataSet GetEvaluationList(GridPrams grid,int vendorID,int bizUnit,int procID,string  PageUrl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? "VEN_NAME" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "VEH_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDORPK , vendorID==0?(object)DBNull.Value:vendorID),//GTIService.Constants.Vendor.Parameters.VENDRSPK
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOREVALDETILSLIST, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// Function Used To Get all Parameters
       /// </summary>
       /// <param name="BizUnitPk"></param>
       /// <param name="TermPK"></param>
       /// <returns></returns>
        public static DataTable GetParametersList(int bizUnitPk,int termPK)
        {
            DBService dbService = new DBService();
             DBService.Parameters[] colParameters = null;
             colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TEMPLATEPK, termPK)

            };
             return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETPARAMETERS, colParameters).Tables[0];
        }

       //NewEval Start
        /// <summary>
        /// Get Template Group list for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEvalGroupList(int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.GROUPPK, DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnitPk)
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETTERMHDRGETKV, colParameters).Tables[0];
            return dtDispersions;
        }
       //New End
         /// <summary>
        /// Get Template Group list for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetParameterValue(int tmdPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TMDPK, tmdPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TMD_ACTIVE, DBNull.Value)
            };

            DataTable dtParameters = new DataTable();
            dtParameters = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDERTERMDTLGETKV, colParameters).Tables[0];
            return dtParameters;
        }
       


        /// <summary>
       /// Get Evaluation list
       /// </summary>
       /// <param name="parameterID"></param>
       /// <returns>Datatable</returns>
       public static DataTable GetEvaluation(string parameterID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PARAMETERPK, parameterID), 
                
              
            };

           DataTable dtEvaluation = new DataTable();
           dtEvaluation = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETEVALUATIONS, colParameters).Tables[0];
           return dtEvaluation;


       }
       /// <summary>
       /// Get list of supplied materials by vendor
       /// </summary>
       /// <param name="vendorID"></param>
       /// <returns>Datatable</returns>
       public static DataTable GetSuppliedMaterial(string vendorID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRMTRSPK, vendorID), 
                
              
            };

           DataTable dtSuppliedMaterial = new DataTable();
           dtSuppliedMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETESUPPLIEDMATERIAL, colParameters).Tables[0];
           return dtSuppliedMaterial;


       }
       /// <summary>
       /// Delete Evaluation Details By evaluationID
       /// </summary>
       /// <param name="evaluationID"></param>
       /// <returns>int- 1(Success)</returns>
       public static int DeleteEvaluationDtls(int evaluationID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDOREVALPK,  evaluationID == 0 ? (object)DBNull.Value :  evaluationID),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.DELETEEVALUATIONDETAILS, colParameters);

           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Vendor.Parameters.PRETVAL]).Value);



       }
       /// <summary>
       /// Get Application ID against Refid
       /// </summary>
       /// <param name="evaluationID"></param>
       /// <returns>ApplicationID</returns>
       public static int GetAppIDForRefID(int processID, int refID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           int Appid = 0;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.RERFID,  refID == 0 ? (object)DBNull.Value :  refID),
            };
           Appid = Convert.ToInt32(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETAPPIDFORREFID, colParameters));
           return Appid;



       }
       //====================== 30-09-2011  For Process to Process Switching  ===========================================
       /// <summary>
       /// Get Application ID against Refid
       /// </summary>
       /// <param name="evaluationID"></param>
       /// <returns>ApplicationID</returns>
       public static int GetEvalAppIDForRefID(int refID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           int Appid = 0;
           colParameters = new DBService.Parameters[] 
           {   
                new DBService.Parameters("REF_ID",  refID),
           };
           Appid = Convert.ToInt32(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETEVALAPPIDFORREFID, colParameters));
           return Appid;
       }
       /// <summary>
       /// Delete Evaluation Details By evaluationID
       /// </summary>
       /// <param name="evaluationID"></param>
       /// <returns>int- 1(Success)</returns>
       public static int UpdateEvaluationDtls(WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq, User objUser)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
           {   
                new DBService.Parameters("VEH_PK",  wrkfReq.ApplicationID ),
                new DBService.Parameters("VEH_REF_ID",  wrkfReq.ReferenceID ),
                 new DBService.Parameters("P_USER_PK",  objUser.PKUser ),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
           };
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.UPDATEEVALREFID, colParameters);
           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Vendor.Parameters.PRETVAL]).Value);

       }
       /// <summary>
       /// Get EVALUATION DETAILS FOR REPORTING
       /// </summary>
       /// <param name="pK"></param>
       /// <returns></returns>
       public static DataTable GetEvaluationReport(int pK)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDOREVALPK , pK)  
            };
           //return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDOREVALREPORT, colParameters).ToString();
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDOREVALREPORT, colParameters).Tables[0];
       }
         /// <summary>
       /// Get EVALUATION DETAILS FOR REPORTING
       /// </summary>
       /// <param name="pK"></param>
       /// <returns></returns>
       public static DataSet GetPerformanceReport(int pK,int bizUnit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VND_PK , pK),
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PBIZUNIT,bizUnit)  
            };
           //return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDOREVALREPORT, colParameters).ToString();
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORPERFORMREPORT, colParameters);
       }
    }
}
