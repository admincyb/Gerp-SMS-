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
    public class CostCenterDA
    {
        /// <summary>
        /// method for Get CostCenter Details bypassing userPK
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="locationPK"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int userPK, int locationPK)
        {
            DataTable dtCostCenter;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              
                new DBService.Parameters(UserDetail.P_USERPK ,userPK<=0?0:userPK ),
                new DBService.Parameters(CostCenters.P_LOCATION_PK , locationPK==0?(object)DBNull.Value:locationPK)

            };
            dtCostCenter = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GET_BY_USER, colParameters).Tables[0];
            return dtCostCenter;
        }
        /// <summary>
        /// method for Get Cost Center  Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        public static DataTable GetCostCenter(int costCenterPK, DbActiveStatus status, int sbu, int locationPK)
        {
            DataTable dtCostCenter;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(CostCenters.P_PK ,costCenterPK<=0?0:costCenterPK ),
                new DBService.Parameters(CostCenters.P_LOCATION_PK , locationPK<=0?(object)DBNull.Value:locationPK),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, status == DbActiveStatus.ALL ? (object)DBNull.Value:Convert.ToInt32(Enum.Parse(typeof(DbActiveStatus),status.ToString()))),
            };
            dtCostCenter = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GET, colParameters).Tables[0];
            return dtCostCenter;
        }

        /// <summary>
        /// method for Get Cost Center  Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        public static DataTable GetCostCenter(int costCenterPK, DbActiveStatus status, int sbu, int locationPK, string searchKey, string searchVal)
        {
            DataTable dtCostCenter;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT, sbu),                
                new DBService.Parameters(CostCenters.P_PK, (object)DBNull.Value),
                new DBService.Parameters(CostCenters.P_LOCATION_PK, locationPK == 0 ? (object)DBNull.Value : locationPK),
                new DBService.Parameters(CommonConstants.SEARCH_KEY, searchKey),
                new DBService.Parameters(CommonConstants.SEARCH_VALUE, searchVal)
            };
            dtCostCenter = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GET, colParameters).Tables[0];
            return dtCostCenter;
        }

        /// <summary>
        /// method for Get Airports By Locationpk
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="sbu"></param>
        public static DataTable GetAirportsByLocation(int locationPK, int sbu, int airportPK)
        {
            DataTable dtAirports;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),         
                new DBService.Parameters( CostCenters.P_APT_PK , airportPK==0?(object)DBNull.Value:airportPK),        
                new DBService.Parameters(CostCenters.P_LOCATION_PK , locationPK==0?(object)DBNull.Value:locationPK),
              
            };
            dtAirports = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GETAIRPORTBYLOCATION, colParameters).Tables[0];
            return dtAirports;
        }
        /// <summary>
        /// method for Get Currency By Locationpk
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="sbu"></param>
        public static DataTable GetCurrencyByLocation(int locationPK)
        {
            DataTable dtCurrency;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                           
                new DBService.Parameters(CostCenters.P_LOCATION_PK , locationPK==0?(object)DBNull.Value:locationPK),
              
            };
            dtCurrency = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GETCURRENCYLOCATION, colParameters).Tables[0];
            return dtCurrency;
        }
        /// <summary>
        /// method for saving costcenter details
        /// </summary>
        /// <param name="objCostCenter"></param>
        /// <param name="user"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static int SaveCostCenter(CostCenterBO objCostCenter, int user, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CostCenters.P_PK ,objCostCenter.PK),
                new DBService.Parameters(CostCenters.P_NAME ,objCostCenter.Name),
                new DBService.Parameters(CostCenters.P_CODE ,objCostCenter.Code),
                new DBService.Parameters(CostCenters.P_LOCATION ,objCostCenter.LocationPK),
                new DBService.Parameters(CostCenters.P_ACCOUNT1, objCostCenter.Account1),
                new DBService.Parameters(CostCenters.P_ACCOUNT2, objCostCenter.Account2),
                new DBService.Parameters(CostCenters.P_ACCOUNT3, objCostCenter.Account3),
                 new DBService.Parameters(CostCenters.P_OTRate, objCostCenter.OtRates),
                new DBService.Parameters(CostCenters.P_MAXOTHrs, objCostCenter.MxOtHrs),
                new DBService.Parameters(CostCenters.P_AIRPORT, objCostCenter.AirportPK==0? (object)DBNull.Value: objCostCenter.AirportPK),
                new DBService.Parameters(CostCenters.P_CURRENCY ,objCostCenter.CurrencyPK),
                new DBService.Parameters(CostCenters.P_DESCRIPTION ,objCostCenter.Description),
                new DBService.Parameters(CostCenters. P_LASTMODDATETIME ,   objCostCenter.LastModByDate==string.Empty? (object)DBNull.Value:Convert.ToDateTime(objCostCenter.LastModByDate)),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS ,1),
                new DBService.Parameters(CommonConstants.BIZUNIT ,sbu),
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, CostCenters.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// method used to delete cost Center details by passing the costCenterPK 
        /// </summary>
        /// <param name="currencyPK" Type=int></param>
        /// <returns>int</returns>
        public static int DeleteCostCenter(int costCenterPK, int user)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CostCenters.P_PK ,costCenterPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),                             
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, CostCenters.SP_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        ///Method used to update status for the selected CostCenters
        /// </summary>
        /// <param name="costCeneterPK"></param>
        /// <param name="dbActiveStatus"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int StatusUpdate(int costCeneterPK, DbActiveStatus status, int user, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CostCenters.P_PK ,costCeneterPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),    
                new DBService.Parameters(CostCenters. P_LASTMODDATETIME ,lastModifiedDate==string.Empty? (object)DBNull.Value: lastModifiedDate),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS,Convert.ToInt32(Enum.Parse(typeof(DbActiveStatus),status.ToString()))), 
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, CostCenters.SP_STATUSCHANGE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// Methode used to fill the search auto complete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="searchType"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetSearchAuto(string searchKey, string searchVal, int bizUnit)
        {
            DataTable dtCostCenter;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT, bizUnit),                
                new DBService.Parameters(CommonConstants.SEARCH_KEY, searchKey),
                new DBService.Parameters(CommonConstants.SEARCH_VALUE, searchVal)
            };
            dtCostCenter = dbService.DataAdapter(CommandType.StoredProcedure, CostCenters.SP_GET, colParameters).Tables[0];
            return dtCostCenter;
        }
    }
}
