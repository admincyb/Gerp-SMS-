using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.PurchaseRequestManagement
{
    public class PurchaseRequest
    {
        public int PRH_PK
        { get; set; }
        public string PRH_NO
        { get; set; }
        public List<PurchaseRequestDetailsList> PurchaseRequestList
        { get; set; }
        public string UserPk
        { get; set; }
    }

    public class PurchaseRequestDetailsList
    {
        public int PRD_ITEM
        { get; set; }
        public int PRD_UOM
        { get; set; }
        public int PRD_BIZUNIT
        { get; set; }
        public int PRD_DEPT
        { get; set; }
        public string ITM_NAME
        { get; set; }
        public string ITC_CODE
        { get; set; }
        public string UOM
        { get; set; }
        public double PRD_QTY_REQUESTED
        { get; set; }
        public string UserPk
        { get; set; }
    }
}
