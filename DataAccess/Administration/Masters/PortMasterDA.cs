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
    public class PortMasterDA
    {
        #region Methods
        /// <summary>
        /// To save company details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SavePort(PortMasterBO objport, User objUser,int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_PK,objport.PK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_NAME,objport.Name),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_DISPLAY_CODE,objport.DisplayCode),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_TYPE,objport.Type),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_ADDRESS,objport.Address),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_CITY,objport.City),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_COUNTRY,objport.Country),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_STATE,objport.State),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_ZIP,objport.ZipCode),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_PHONE,objport.Phone),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_MOBILE,objport.Mobile),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_FAX,objport.Fax),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_EMAIL,objport.Email),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_IS_PUR_FROM,objport.IsPurFrom),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_IS_PUR_TO,objport.IsPurTo),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_IS_SALES_FROM,objport.IsSalesFrom),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_IS_SALES_TO,objport.IsSalesTo),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,objport.Active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,bizunit),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_USER_PK,objUser.PKUser),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_LAST_MOD_DT,objport.PRM_MOD_DT),
                               
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_PORT_MST_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }

        /// <summary>
        /// To retrieve company details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetPortList(string code, string name,int bizunit)
        {
            DataTable dtCompany;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_DISPLAY_CODE,code==  string.Empty ? (object)DBNull.Value : code),
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_NAME,name==  string.Empty ? (object)DBNull.Value : name),
            new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,bizunit),
            };
            dtCompany = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_PORT_MST_GET_LIST, colParameters).Tables[0];
            return dtCompany;
        }


        /// <summary>
        /// To Delete company details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static int DeletePort(int PortPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_PK,PortPK),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_PORT_MST_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }

        public static DataTable GetPortEdit(int? ltmPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PRM_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE, active),
                //new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                //new DBService.Parameters(Parameters.P_IS_PRODUCTION,(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_PORT_MST_GET_KV, colParameters).Tables[0];
        }

        #endregion
    }
}
