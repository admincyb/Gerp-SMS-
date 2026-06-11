using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Sales
{
    public class ContainerInspection
    {
        public int CVH_PK { get; set; }
        public string CVH_NO { get; set; }
        public DateTime CVH_DATE { get; set; }
        public string CVH_CONTAINER_NO { get; set; }
        public string CVH_SERIAL_NO { get; set; }
        public string CVH_REMARKS { get; set; }
        public int CVH_CHECK_LIST_GROUP { get; set; }
        public byte CVH_STATUS { get; set; }
        public byte CVH_ACTIVE { get; set; }
        public int CVH_BIZUNIT { get; set; }
        public int CVH_CRTD_BY { get; set; }
        public DateTime  CVH_CRTD_DT { get; set; }
        public int CVH_MOD_BY { get; set; }
        public DateTime  CVH_MOD_DT { get; set; }
        public string Status_text { get; set; }
    }

    public enum WorkFlowStatus
    {
        DRAFT = 0,
        APPROVED = 2,
    }

    public class WorkFlowStatusText
    {
        public const string DRAFT = "Drafted";
        public const string APPROVED = "Approved";
    }


}
