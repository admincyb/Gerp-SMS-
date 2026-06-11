using System;
using System.Data;
using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.StoreManagement
{
   public class StoreRequisitionSlipCreationDL
   {
       #region Methods

       /// <summary>
       /// SAVING REQUISITION DETAILS
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns> INT</returns>
       public static List<object> SaveRequisitionDetails(string strxml)
       {

           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.STOREREQUISITIONXML , strxml),
                  new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.SRSNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
               
                new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.STORERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           List<object> retvals = new List<object>();
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.SAVEREQUISITIONXML, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_Requisition.STORERETURNVALUE]).Value);
           string srsNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_Requisition.SRSNO]).Value.ToString();
           retvals.Add(result);
           retvals.Add(srsNo);
           return retvals;

       }
        /// <summary>
        /// Method to get the Search Values Corresponding to Search Type
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.REQUISITIONSRCHBY ,  GTIService.Constants.Store.Parameters_Requisition.ITEMCODE),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.REQUISITIONSRCHVALUE ,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSEARCHREQUISITIONVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// Function Used To Get all store and store pk
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetStores(User objUser, int sbuPk, int flag, int? storeCategory=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.STOREMODIFIEDBY ,  objUser.PKUser),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.SRSDPTTYPE ,Convert.ToInt32( GTIService.Constants.Administration.Configurations.Departments.MainDepartments.Inventory)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FLAG,  flag),
              new DBService.Parameters("DPT_CATEGORY",  storeCategory)
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSTORES,colParameters).Tables[0];
        }
       /// <summary>
        /// Function Used To Get all store and store pk
       /// </summary>
       /// <param name="objUser"></param>
       /// <param name="sbuPk"></param>
       /// <param name="flag"></param>
       /// <param name="category"></param>
       /// <returns></returns>
        public static DataTable GetStores(User objUser, int sbuPk, int flag, int category)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.STOREMODIFIEDBY ,  objUser.PKUser),
              new DBService.Parameters(  GTIService.Constants.Store.Parameters_Requisition.SRSDPTTYPE ,Convert.ToInt32( GTIService.Constants.Administration.Configurations.Departments.MainDepartments.Inventory)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FLAG,  flag),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CATEGORY,  category)
              
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSTORES, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get SRS No 
        /// </summary>
        /// <returns>string</returns>
        public static string GetSRSNo()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
               
                new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.SRSRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSRSNO, colParameters);

            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_Requisition.SRSRETURNVALUE]).Value);

        }
       /// <summary>
       /// function used to store requisition report
       /// </summary>
       /// <param name="requisitionID"></param>
       /// <returns></returns>
        public static DataSet GetRequisitionReportByReqId(int requisitionID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             new DBService.Parameters( GTIService.Constants.Store.Parameters_Requisition.MRDPK ,  requisitionID),  
             
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETREQUISITIONREPORTBYREQID, colParameters);
            return dtRequisition;


        }
        /// <summary>
        /// function for filling  srs detail in grid. refered by rijoy in MA
        /// </summary>
        /// <param name="srsID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSRSDetails(int srsID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialIssue.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_Requisition.MRDPK, srsID),
            };

            DataSet dtProduct = new DataSet();
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_Requisition.GETSRSDETAILS, colParameters);
        }


        /// Function Used To Get all store and store pk By Type
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetStoresByType(User objUser, int sbuPk, int deptType, int deptPk, int deptCompany=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("DPT_PK" ,  deptPk),
              new DBService.Parameters("P_ACTIVE" ,1),
              new DBService.Parameters("DPT_CATEGORY", deptType ),
              new DBService.Parameters("P_USER",  deptType==1?(object)DBNull.Value:objUser.PKUser), // 1 for scrap Store, it not assigned to any user, its default
              new DBService.Parameters("P_BIZUNIT",   sbuPk),
               new DBService.Parameters("P_DPT_COMPANY",  deptCompany>0?deptCompany:(object)DBNull.Value), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_DEPT_STORE_GET_KV", colParameters).Tables[0];
        }


        /// <summary>
        /// Get All Department Details By UserID and sbupk
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartmentDtls(User objUser, int sbuPk, int?category=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                  new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.USERID  ,objUser.PKUser == 0?(object)DBNull.Value :objUser.PKUser  ),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                  new DBService.Parameters("@P_DPT_CATEGORY",  Convert.ToInt32( GTIService.Constants.Common.StoresGetFlag.GeneralAndCompoundStore)),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETDEPTDTLS, colParameters).Tables[0];
            return dtRunByType;
        }

        public static DataTable FillOtherSBUS(User objUser, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                  new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.P_ACTIVE  ,Active  ),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.SPADM_BIZUNIT_MST_GET_KV, colParameters).Tables[0];
            return dtRunByType;
        }
        #endregion
    }
}
