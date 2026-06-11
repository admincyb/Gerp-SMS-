using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IFinYearMstManager
    {
        short? GetFinYear(DateTime TransDate, Int32 BizUnit,int? CompanyPk = null);
        List<FIN_YEAR_MST> GetCurrentFinPeriod(DateTime TransDate, Int32 BizUnit);
    }
}
