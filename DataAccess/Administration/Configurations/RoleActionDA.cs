using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using System.Data;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Configurations;

namespace DataAccess.Administration.Configurations
{
    public class RoleActionDA
    {
        #region Methods
        /// <summary>
        /// method for Get Role Actions  Details
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        public static DataSet GetRoleActions(int roleActionPK, DbActiveStatus status, int sbu)
        {
            DataSet dsRoleActions;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(RoleActions.PK , roleActionPK==0?(object)DBNull.Value:roleActionPK),
                new DBService.Parameters(RoleActions.P_ACTIVEPAGE, (object)DBNull.Value),
            };
            dsRoleActions = dbService.DataAdapter(CommandType.StoredProcedure, RoleActions.SP_GETTREE, colParameters);
            return dsRoleActions;
        }
        /// <summary>
        /// Methode used for get the role actions.
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static string GetRoleActionsXml(int roleActionPK, DbActiveStatus status, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(RoleActions.PK , roleActionPK==0?(object)DBNull.Value:roleActionPK),
                new DBService.Parameters(RoleActions.P_ACTIVEPAGE, (object)DBNull.Value),
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, RoleActions.SP_GETTREEXML, colParameters).ToString();
        }
        /// <summary>
        /// SAVING Role Action DETAILS 
        /// </summary>
        /// <param name="strxml"></param>
        /// <param name="user"></param>
        /// <returns> INT</returns>
        public static int SaveRoleActions(RoleActionBO objRoleAction, string strxml, int user)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( RoleActions.P_XML  , strxml),
                new DBService.Parameters( RoleActions.P_RAM_Role  , objRoleAction.RolePK),
                //new DBService.Parameters(CommonConstants.CREATEDBY ,user),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, RoleActions.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);

        }
        #endregion
    }
}
