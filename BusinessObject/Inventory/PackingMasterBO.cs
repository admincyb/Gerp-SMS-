using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Inventory
{
    public class PackingMasterBO
    {
        public int APS_PK { get; set; }
        public string APS_CODE { get; set; }
        public string APS_NAME { get; set; }
        public int APS_TYPE { get; set; }
        public double APS_PC_PCS { get; set; }
        public double APS_IB_PCS { get; set; }
        public double APS_IC_PCS { get; set; }
        public double APS_ZB_PCS { get; set; }
        public double APS_MC_PCS { get; set; }
        public double APS_SC_PCS { get; set; }
        public double APS_TOTAL_PCS { get; set; }
        public string APS_DESC { get; set; }
        public int ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public int BIZUNIT { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public string SortBy { get; set; }
        public string ThenBy { get; set; }
        public string SortDirection { get; set; }
        public string ThenDirection { get; set; }
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }

        public double APS_POB_PCS { get; set; }
        public double APS_PRB_PCS { get; set; }
        public double APS_WLT_PCS { get; set; }
    }

    public class Packingmapping
    {
        public int P_PIM_PK { get; set; }

        public int PIM_PACK_SPEC { get; set; }
        public int PIM_CUSTOMER { get; set; }
        public int PIM_CUST_ITEM { get; set; }
        public string PIM_ART_WORK { get; set; }
        public string PIM_DESC { get; set; }
        public int PIM_PC_ITEM { get; set; }
        public int PIM_IB_ITEM { get; set; }
        public int PIM_IC_ITEM { get; set; }
        public int PIM_ZB_ITEM { get; set; }
        public int PIM_MC_ITEM { get; set; }
        public int PIM_SC_ITEM { get; set; }
        public int P_ACTIVE { get; set; }
        public int P_BIZUNIT { get; set; }
        public DateTime P_LAST_MOD_DT { get; set; }
        public int P_USER_PK { get; set; }
        public int PIM_POB_ITEM { get; set; }
        public int PIM_PRB_ITEM { get; set; }
        public int PIM_WLT_ITEM { get; set; }

        public string P_DETAILS { get; set; }

    }

    public class Detail
    {
        public int BMD_PK { get; set; }
        public int BMD_TYPE { get; set; }
        public int BMD_ITEM { get; set; }
        public decimal BMD_QTY { get; set; }
        public int BMD_SEQUENCE { get; set; }
        public int BMD_ACTIVE { get; set; }
    }

}
