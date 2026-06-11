using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IAdmFormTabCfgManager
    {
        List<SPADM_FORM_TAB_CFG_GET_Result> GetFormTabList(string formCode);
    }
}
