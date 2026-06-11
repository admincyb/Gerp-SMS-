using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Finance;
using System.Data;
using BusinessObject.Finance;
namespace BusinessLogic.Finance
{
    public class COAFinGrpMapBL
    {
        public static DataTable GetCOAFinGrpList(int type, int pk, int template, int mapped, int isGroup, int sbuID)
        {
            return COAFinGrpMapDL.GetCOAFinGrpList(type, pk, template, mapped, isGroup, sbuID);
        }
        public static int SaveCOAFinGrp(COAFinGrpMapBO COAFinGrpMapObj)
        {
            return COAFinGrpMapDL.SaveCOAFinGrp(COAFinGrpMapObj);
        }
    }
}
