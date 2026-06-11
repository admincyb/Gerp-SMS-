using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.Constants.DA.Administration;
using ERP.Utilities;

namespace DataAccess.Administration.Configurations
{
    public static class ViewMailsDA
    {
        #region Methods
        /// <summary>
        /// method for Get Mail  Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="mailPK"></param>

        public static DataTable GetMailDetails(int costCenterPK, int mailPK)
        {
            DataTable dtMailDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 
                new DBService.Parameters(ViewMail.P_PK ,costCenterPK==0?(object)DBNull.Value:costCenterPK ),
                new DBService.Parameters(ViewMail.P_MLQ_PK , mailPK==0?(object)DBNull.Value:mailPK),
               
            };
            dtMailDetails = dbService.DataAdapter(CommandType.StoredProcedure, ViewMail.SP_GET, colParameters).Tables[0];
            return dtMailDetails;
        }
        #region Methods
        /// <summary>
        /// method for Get Mail  Details for mail send service
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="mailPK"></param>

        public static DataTable GetMailDetails(int costCenterPK, int mailPK, int count)
        {
            DataTable dtMailDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 
                new DBService.Parameters(ViewMail.P_PK ,costCenterPK==0?(object)DBNull.Value:costCenterPK ),
                new DBService.Parameters(ViewMail.P_MLQ_PK , mailPK==0?(object)DBNull.Value:mailPK),
                new DBService.Parameters(ViewMail.P_COUNT , count==0?(object)DBNull.Value:count),
                
            };
            dtMailDetails = dbService.DataAdapter(CommandType.StoredProcedure, ViewMail.SP_GET_AUTO, colParameters).Tables[0];
            return dtMailDetails;
        }

        /// <summary>
        /// SAVING Mail Details DETAILS 
        /// </summary>
        /// <param name="mailStatus"></param>
        /// <param name="mailPK"></param>
        /// <returns></returns>
        public static int SaveMailDetails(int mailStatus, int mailPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( ViewMail.P_MLQ_PK , mailPK),
               new DBService.Parameters( ViewMail.P_MLQ_STATUS , mailStatus),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, ViewMail.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);

        }
        #endregion
        #endregion

        public static DataSet GetPaySlipMailList(int? Pk, int? Status)
        {
            DataSet dsMailDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 
                new DBService.Parameters(ViewMail.P_EPM_PK ,(Pk.HasValue && Pk > 0)?Pk : (object)DBNull.Value),
                new DBService.Parameters(ViewMail.P_EPM_IS_GENERATED , (Status.HasValue)?Status : (object)DBNull.Value)
            };
            dsMailDetails = dbService.DataAdapter(CommandType.StoredProcedure, ViewMail.SPHRM_EMP_PAYROLL_MAIL_DTL_GET, colParameters);
            return dsMailDetails;
        }

        public static int PaySlipMailQueSave(int PaySlipMailPk, string FilePath, string FileType, int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
               new DBService.Parameters( ViewMail.P_EPM_PK , PaySlipMailPk),
               new DBService.Parameters( ViewMail.P_ADD_PATH , FilePath),
               new DBService.Parameters( ViewMail.P_ADD_TYPE , FileType),
               new DBService.Parameters( ViewMail.P_BIZUNIT , BizUnit),
               new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, ViewMail.SPHRM_EMP_PAYROLL_MAIL_DTL_UPDATE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }
    }
}
