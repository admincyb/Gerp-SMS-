using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class LocationBL
    {
        #region Methods

        /// <summary>
        /// Get Location Details By passing userPK
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetLocation(int userPK)
        {
            return LocationDA.GetLocation(userPK);
        }
        /// <summary>
        /// Methode used get the grade details by search terms
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <param name="searchKey"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static DataTable GetLocation(int locationPK, DbActiveStatus status, int sbu, string searchKey, string searchVal)
        {
            return LocationDA.GetLocation(locationPK, status, sbu, searchKey, searchVal);
        }
        /// <summary>
        /// Get Location Details By Locataion Pk 
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetLocation(int locationPK, DbActiveStatus status, int sbu)
        {
            return LocationDA.GetLocation(locationPK, status, sbu);
        }

        /// <summary>
        /// Save Location Details and Return Location Pk if save success
        /// </summary>
        /// <param name="objLocation"></param>
        /// <param name="user"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static int SaveLocation(LocationBO objLocation, int user, int sbu)
        {



            return LocationDA.SaveLocation(objLocation, user, sbu);
        }

        /// <summary>
        /// Delete Location Details by Location PK : Success return 1 , Fail Return 0
        /// </summary>
        /// <param name="currPK"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int DeleteLocation(int currPK, int user)
        {
            return LocationDA.DeleteLocation(currPK, user);
        }

        /// <summary>
        /// To Update the status of the location details ,1 - Active and 0 fro inactive
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int StatusUpdate(int locationPK, DbActiveStatus status, int user, string lastModDate)
        {
            return LocationDA.StatusUpdate(locationPK, status, user, lastModDate);
        }


        #endregion
    }
}
