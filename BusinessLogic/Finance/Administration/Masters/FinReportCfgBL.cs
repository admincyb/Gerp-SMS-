using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Finance.Administration.Masters;

namespace BusinessLogic.Finance.Administration.Masters
{
    public class FinReportCfgBL
    {
        public static int? SaveFinReportCfg(string xmlDoc)
        {
            return FinReportCfgDL.SaveFinReportCfg(xmlDoc);
        }
        public static DataTable GetKVFinReportCfg(int pk, byte active, byte template, byte? group, int bizUnit, int? type, byte? level, string name, int? parent, string dispName)
        {
            return FinReportCfgDL.GetKVFinReportCfg(pk, active, template, group, bizUnit, type, level, name, parent, dispName);
        }
    }
}
