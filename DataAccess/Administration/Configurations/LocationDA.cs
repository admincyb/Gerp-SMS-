using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERP.Utilities.Constants.DA.Administration;
using BusinessObject.Administration.Configurations;

namespace DataAccess.Administration.Configurations
{
    public class LocationDA
    {
        #region Methods
        /// <summary>
        /// Get Location Details By passing userPK
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns>DataTable</returns>
        /// <summary>
        public static DataTable GetLocation(int userPK)
        {
            DataTable dtCountry;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {               
                new DBService.Parameters(UserDetail.P_USERPK , userPK<=0?0:userPK )
            };
            dtCountry = dbService.DataAdapter(CommandType.StoredProcedure, Locations.SP_GET_BY_USER, colParameters).Tables[0];
            return dtCountry;
        }
        /// <summary>
        /// Methode used get the user details by search terms
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <param name="searchKey"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static DataTable GetLocation(int locationPK, DbActiveStatus status, int sbu, string searchKey, string searchVal)
        {
            DataSet dsUsers;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Locations.P_PK , locationPK <= 0 ? 0 : locationPK),
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),
                new DBService.Parameters(CommonConstants.SEARCH_KEY, searchKey),
                new DBService.Parameters(CommonConstants.SEARCH_VALUE, searchVal)
            };
            dsUsers = dbService.DataAdapter(CommandType.StoredProcedure, Locations.SP_GET, colParameters);
            if (dsUsers.Tables[0] != null)
            {
                return dsUsers.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// Get Location Details By Locataion Pk 
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetLocation(int locationPK, DbActiveStatus status, int sbu)
        {
            DataTable dtCountry;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(Locations.P_PK , locationPK<=0?0:locationPK ),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, status == DbActiveStatus.ALL ? (object)DBNull.Value:Convert.ToInt32(Enum.Parse(typeof(DbActiveStatus),status.ToString()))),
            };
            dtCountry = dbService.DataAdapter(CommandType.StoredProcedure, Locations.SP_GET, colParameters).Tables[0];
            return dtCountry;
        }

        /// <summary>
        /// Save Location Details and Return Location Pk if save success
        /// </summary>
        /// <param name="objLocation"></param>
        /// <param name="user"></param>
        /// <param name="sbu"></param>
        /// <returns>int</returns>
        public static int SaveLocation(LocationBO objLocation, int user, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Locations. P_PK,objLocation.PK),
                new DBService.Parameters(Locations. P_CODE ,objLocation.Code),
                new DBService.Parameters(Locations. P_NAME ,objLocation.Name),
                new DBService.Parameters(Locations. P_COUNTRY,objLocation.Country),
                new DBService.Parameters(Locations. P_REGION, objLocation.AdmReg),
                new DBService.Parameters(Locations. P_CURRENCY,objLocation.Currency==-1? (object)DBNull.Value: objLocation.Currency),
                new DBService.Parameters(Locations. P_SALMONTHS , objLocation.SalMonths),
                new DBService.Parameters(Locations. P_SALMONTH1,objLocation.SalaryMonth1),
                new DBService.Parameters(Locations. P_SALMONTHPERC, objLocation.SalaryMonth1Perc),
                new DBService.Parameters(Locations. P_SALMONTH2 ,objLocation.SalaryMonth2),
                new DBService.Parameters(Locations. P_SALMONTHPERC2  , objLocation.SalaryMonth2Perc),
                new DBService.Parameters(Locations. P_ACCOUNT1  ,objLocation.Account1),
                new DBService.Parameters(Locations. P_ACCOUNT2 , objLocation.Account2),
                new DBService.Parameters(Locations. P_ACCOUNT3  ,objLocation.Account3),
                new DBService.Parameters(Locations. P_DESCRIPTION , objLocation.Description),
                new DBService.Parameters(CommonConstants.LASTMODDATE ,objLocation.LastModDate==string.Empty? (object)DBNull.Value: Convert.ToDateTime(objLocation.LastModDate)),
                new DBService.Parameters(CommonConstants.BIZUNIT ,sbu),
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Locations.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// Delete Location Details by Location PK : Success return 1 , Fail Return 0
        /// </summary>
        /// <param name="currPK"></param>
        /// <param name="user"></param>
        /// <returns>int</returns>
        public static int DeleteLocation(int locationPK, int user)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Locations.P_PK ,locationPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),   
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Locations.SP_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// To Update the status of the location details ,1 - Active and 0 for inactive
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns>int</returns>
        public static int StatusUpdate(int locationPK, DbActiveStatus status, int user, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Locations.P_PK ,locationPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),   
                new DBService.Parameters(CommonConstants.LASTMODDATE  ,lastModDate==string.Empty? (object)DBNull.Value: Convert.ToDateTime(lastModDate)),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS,Convert.ToInt32(Enum.Parse(typeof(DbActiveStatus),status.ToString()))), 
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Locations.SP_STATUSCHANGE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        #endregion
    }
}
