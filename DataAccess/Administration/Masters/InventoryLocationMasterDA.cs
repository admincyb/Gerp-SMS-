using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using System.Data;
using GTIService.Constants.Common;

using GTIService.Constants.Administration.Masters;
using BusinessObject;

namespace DataAccess.Administration.Masters
{
  public  class InventoryLocationMasterDA
    {
  /// <summary>
        /// To save company details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveInvStore(int ? pk,InventoryLocationMasterBO objInvList, User objUser,int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_PK,pk>0 ? pk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_CODE,objInvList.Code),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_NAME,objInvList.Name),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_DESC,objInvList.Desc),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_PARENT,objInvList.Parent),
            //    new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE, objUser.Active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.DPT_BIZUNIT,bizunit),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.USR_PK,objUser.CurrentSBUPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_COMPANY,objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_INV_LOC_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }


        /// <summary>
        /// To retrieve inventory details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetInventoryList(GridPrams grdInvType,  int bizunit)
        {
            DataTable dtCompany;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
             new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PAGE_NUM ,  grdInvType.PageNumber),
              new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PAGE_SIZE ,  grdInvType.PageSize),
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.DPT_BIZUNIT,bizunit),
            };
            dtCompany = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_INV_LOC_GET_LIST, colParameters).Tables[0];
            return dtCompany;
        }



        /// <summary>
        /// To retrieve inventory details based on search
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetInventoryList(string code, string  name,int bizunit)
        {
            DataTable dtCompany;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
             
             new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_CODE,code==  string.Empty ? (object)DBNull.Value : code),
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_NAME,name==  string.Empty ? (object)DBNull.Value : name),
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.DPT_BIZUNIT,bizunit),
            };
            dtCompany = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_INV_LOC_GET_LIST, colParameters).Tables[0];
            return dtCompany;
        }


      
        public static DataTable GetInvEdit(int? ltmPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_ACTIVE, active),
                //new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                //new DBService.Parameters(Parameters.P_IS_PRODUCTION,(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_MST_GET, colParameters).Tables[0];

        }

        /// <summary>
        /// To Delete Inventory details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static int DeleteInvLocation(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_PK,pk),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_MST_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }



    }
}
