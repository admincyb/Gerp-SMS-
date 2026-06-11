using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using DataAccess.Administration.Configurations;
using BusinessObject.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class CostCenterBL
    {
        /// <summary>
        /// method for Get CostCenter Details bypassing userPK
        /// </summary>
        /// <param name="costCenterPK"></param>

        /// <returns></returns>
        public static DataTable GetCostCenter(int userPK, int locationPK)
        {
            return CostCenterDA.GetCostCenter(userPK, locationPK);
        }
        /// <summary>
        /// method for Get CostCenter Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int costCenterPK, DbActiveStatus status, int sbu, int locationPK)
        {
            return CostCenterDA.GetCostCenter(costCenterPK, status, sbu, locationPK);
        }
        /// <summary>
        /// method for Get CostCenter Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int costCenterPK, DbActiveStatus status, int sbu, int locationPK, string searchKey, string searchVal)
        {
            return CostCenterDA.GetCostCenter(costCenterPK, status, sbu, locationPK, searchKey, searchVal);
        }
        /// <summary>
        /// method for Get Airports By Location
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyByLocation(int locationPK)
        {
            return CostCenterDA.GetCurrencyByLocation(locationPK);
        }
        /// <summary>
        /// method for Get Airports By Location
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetAirportsByLocation(int locationPK, int sbu, int airportPK)
        {
            return CostCenterDA.GetAirportsByLocation(locationPK, sbu, airportPK);
        }
        /// <summary>
        /// method for saving costcenter details
        /// </summary>
        /// <param name="objCostCenter"></param>
        /// <returns></returns>
        public static int SaveCostCenter(CostCenterBO objCostCenter, int user, int sbu)
        {
            return CostCenterDA.SaveCostCenter(objCostCenter, user, sbu);
        }
        /// <summary>
        /// method for delete costcenter details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int DeleteCostCenter(int costCenterPK, int user)
        {
            return CostCenterDA.DeleteCostCenter(costCenterPK, user);
        }
        /// <summary>
        ///Methord used to update status fr the selected country
        /// </summary>
        ///  <param name="costCenterPK"></param>
        /// <param name="dbActiveStatus"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int StatusUpdate(int costCenterPK, DbActiveStatus status, int user, string lastModifiedDate)
        {
            return CostCenterDA.StatusUpdate(costCenterPK, status, user, lastModifiedDate);
        }

        /// <summary>
        /// Methode used to fill the search auto complete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="searchType"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static object GetSearchAuto(string searchKey, string SearchVal, int bizUnit, string Value)
        {
            DataTable dtSearch;
            dtSearch = CostCenterDA.GetSearchAuto(searchKey, SearchVal, bizUnit);
            var searchResult = from rws in dtSearch.AsEnumerable()
                               select new { Key = rws.Field<string>(Value), Value = rws.Field<string>(Value) };
            return searchResult;
        }
    }
}
