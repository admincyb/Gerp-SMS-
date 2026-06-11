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
   public  class PortMasterBL
    {
        /// <summary>
        /// To save Port details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
       public static int SavePort(PortMasterBO objPort, User objUser,int bizunit)
        {
            return PortMasterDA.SavePort(objPort, objUser,bizunit);
        }

       public static DataTable GetPortList(string code, string name, int bizunit)
         
       {
           DataTable dtPortList = PortMasterDA.GetPortList(code,name,bizunit);
           return dtPortList;
       }
       public static int DeletePort(int portPK)
       {
           return PortMasterDA.DeletePort(portPK);
       }

       public static DataTable GetPortEdit(int? ltmPK, int active)
       {
           return PortMasterDA.GetPortEdit(ltmPK, active);
       }
    }
}
