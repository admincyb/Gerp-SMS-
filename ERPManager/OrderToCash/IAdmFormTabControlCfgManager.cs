using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IAdmFormTabControlCfgManager
    {
        List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> GetFormTabControlsList(string formCode, string tabCode, int userPk = 0, int sbuPK = 1);
    }
}
