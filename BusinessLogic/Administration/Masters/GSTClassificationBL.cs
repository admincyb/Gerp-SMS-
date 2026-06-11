using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Masters;
using DataAccess.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
    public class GSTClassificationBL
    {
        public static int SaveGSTClassificationDetails(GSTClassificationBO objGSTClassification)
        {
            return GSTClassificationDA.SaveGSTClassificationDetails(objGSTClassification);
        }

        public static System.Data.DataTable GetGstList(string code, string name, int bizUnit, int pageIndex, int pageSize)
        {
            return GSTClassificationDA.GetGstList(code, name, bizUnit,pageIndex,pageSize);
        }

        public static System.Data.DataTable GetGstDetailList(int? gstPK, int active, int bizUnit)
        {
            return GSTClassificationDA.GetGstDetailList(gstPK, active, bizUnit);
        }

        public static int DeleteGstDetails(int CurrPK, DateTime LastModifiedTime)
        {
            return GSTClassificationDA.DeleteGstDetails(CurrPK, LastModifiedTime);
        }
    }
}
