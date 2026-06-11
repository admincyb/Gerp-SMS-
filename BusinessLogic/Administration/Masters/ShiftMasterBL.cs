using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
    public class ShiftMasterBL
    {
        public static DataTable   GetShift(int bizUnit,string code,string name, int pageNum, int pageSize,int shiftPK)
        {
            return ShiftMasterDL.GetShift(bizUnit, code,name,pageNum, pageSize,shiftPK);
        }
        public static int SaveShiftDetails(ShiftHeader objShiftHeader)
        {
            return ShiftMasterDL.SaveShiftDetails(objShiftHeader);
        }

        public static DataTable GetShiftType(int? ltmPK, int active, int bizUnit)
        {
            return ShiftMasterDL.GetShiftType(ltmPK, active, bizUnit);
        }

        public static int DeleteShiftDetails(int CurrPK)
        {
            return ShiftMasterDL.DeleteShiftDetails(CurrPK);
        }
    }
}
