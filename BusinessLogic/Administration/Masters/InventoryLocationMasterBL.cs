using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using BusinessObject.Administration.Masters;
using DataAccess;
using System.Data;
using DataAccess.Administration.Masters;


namespace BusinessLogic.Administration.Masters
{
    public class InventoryLocationMasterBL
    {

        /// <summary>
        /// To save Port details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveInvStore(int ? pk, InventoryLocationMasterBO objInvList, User objUser, int bizunit)
        {
            return InventoryLocationMasterDA.SaveInvStore(pk,objInvList, objUser, bizunit);
        }

        public static DataTable GetInventoryList(GridPrams grdInvType,  int bizunit)
        {
            DataTable dtInventoryList = InventoryLocationMasterDA.GetInventoryList(grdInvType,bizunit);
            return dtInventoryList;
        }

        public static DataTable GetInventoryList(string code, string name,int bizunit)
        {
            DataTable dtInventoryList = InventoryLocationMasterDA.GetInventoryList(code,name,bizunit);
            return dtInventoryList;
        }
        public static DataTable GetInvEdit(int? ltmPK, int active)
        {
            return InventoryLocationMasterDA.GetInvEdit(ltmPK, active);
        }
        public static int DeleteInvLocation(int pk)
        {
            return InventoryLocationMasterDA.DeleteInvLocation(pk);
        }



    }
}
