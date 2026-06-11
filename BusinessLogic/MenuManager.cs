using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using DataAccess;

namespace BusinessLogic
{
    public class MenuManager
    {
        /// <summary>
        /// Get Menu List By Menu Name
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public static DataTable GetMenuList(string menuName)
        {
            return MenuDL.GetMenuSearch(menuName);
        }

        /// <summary>
        /// Get Menu List By UserID
        /// </summary>
        /// <param name="usrId"></param>
        /// <returns></returns>
        public  DataTable GetMenu(int usrId)
        {
            return MenuDL.GetMenu(usrId);
        }
        /// <summary>
        /// Get All Menu List
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllMenuDtls(int userPK, int bizUnit)
        {
            return MenuDL.GetMenuDetails( userPK,  bizUnit);
        }
        /// <summary>
        /// Search Menu Details
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public static DataTable GetMenuDetailsSearch(string menuName)
        {
            return MenuDL.GetMenuDetailsSearch(menuName);
        }

        /// <summary>
        /// Get All Menu List
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllMenuDtls(int userPK, int bizUnit, int deptID, string menuType)
        {
            return MenuDL.GetMenuDetails(userPK, bizUnit, deptID, menuType);
        }
    }
}
