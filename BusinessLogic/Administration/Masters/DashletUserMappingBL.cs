using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;
using BusinessObject.Administration.Masters;
using ERP.Utilities;

namespace BusinessLogic.Administration.Masters
{
    public class DashletUserMappingBL
    {

        public static DataTable GetDashletUserMappingList(int bizUnit,  int pageNo, int pageSize, string name,int type,int module, int currPk=0)
        {
            return DashletUserMappingDL.GetDashletUserMappingList(bizUnit, pageNo, pageSize, name, type, module, currPk);
        }

        public static DataTable GetUserRoles(int deptPk, int userPk=0)
        {
            return DashletUserMappingDL.GetUserRoles(deptPk, userPk);
        }


        public static int? SaveDashletUserDetails(string strxml)
        {
            return DashletUserMappingDL.SaveDashletUserDetails(strxml);
        }

        public static DashletUserMappingHeader DashletUserRolesByPK(int currPK)
        {
            try
            {
                DashletUserMappingHeader objDashletUserMappingHeader = new DashletUserMappingHeader();
                string dtl = DashletUserMappingDL.DashletUserRolesByPK(currPK);
                if (dtl != string.Empty)
                {
                    objDashletUserMappingHeader = (DashletUserMappingHeader)CommonFunctions.DeserializeObject(dtl, objDashletUserMappingHeader);
                    return objDashletUserMappingHeader;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
