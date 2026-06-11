using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.MachineryManagement
{
    public class Machinery
    {
        public int MCH_PK { get; set; }
        public string MCH_CODE { get; set; }
        public string MCH_NAME { get; set; }
        public int MCH_TYPE { get; set; }
        public int MCH_LOCATION { get; set; }

        public int SBU { get; set; }
        public int USER_PK { get; set; }

      

        //Purchase Details
        public string MCH_PUR_DT { get; set; }
        public int MCH_VENDOR { get; set; }
        public int MCH_CONDITION { get; set; }
        public double MCH_PUR_PRICE { get; set; }
        public int MCH_PUR_CURR { get; set; }
        public string MCH_EXPR_DT { get; set; }
        public string MCH_PUR_REMARKS { get; set; }
       


        //public int PurchaseMode { get; set; }
        //public string PurchaseAttachment { get; set; }


        // Performance Details
        public string MCH_THROUGHPUT { get; set; }
        public double MCH_AVG_CONS { get; set; }
        public int MCH_AVGC_UOM { get; set; }
        public int MCH_FUEL_TYPE { get; set; }
        public double MCH_MC_USAGE { get; set; }
        public int MCH_MCU_UOM { get; set; }

        public List<MaintenaceInfo> MaintenanceList { get; set; }

        public class MaintenaceInfo
        {
            public int MCM_PK { get; set; }
            public int MCM_TYPE { get; set; }
            public int MCM_FREQUENCY { get; set; }
           
            public string MCM_FROM_DT { get; set; }
            public string MCM_TO_DT { get; set; }

            public string MNT_NAME { get; set; }
            public string FRQ_NAME { get; set; }

            public int SL_NO { get; set; }

        }

        public class LocationMaster
        {
            public int LocationPK { get; set; }
            public string LocationName { get; set; }
            public int UserPK { get; set; }
            public int SBU
            {
                get;
                set;
            }
        }
        public class MachineTypeMaster
        {
            public int MachineTypePK { get; set; }
            public string MachineTypeCode { get; set; }
            public string MachineTypeName { get; set; }
            public int UserPK { get; set; }
            public int SBU { get; set; }
            public int Status { get; set; }

        }
        public class Vendor
        {
            public int vendorPK { get; set; }
            public string VendorName { get; set; }
            public string ContactName { get; set; }
            public string Address { get; set; }
            public string VendorPhone { get; set; }
            public string Email { get; set; }
            public int UserPK { get; set; }
        }
    }
}
   

