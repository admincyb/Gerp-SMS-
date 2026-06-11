
namespace BusinessObject.StoreManagement
{
    public class NewItemRequestBO
    {
        public int ITR_PK
        {
            get;
            set;
        }
        public string NIR_NO
        {
            get;
            set;
        }
        public int MRH_DEPT_STR
        {
            get;
            set;
        }
        public int ActionID
        {
            get;
            set;
        }
        public int DeptPk
        {
            get;
            set;
        }
        public string NIR_SUBMITTED_DATE
        {
            get;
            set;
        }
        public string NIR_REQUIRED_DATE
        {
            get;
            set;
        }
        public int MaterialType
        {
            get;
            set;
        }
        public string NameTitle
        {
            get;
            set;
        }
        public int RequestFrequency
        {
            get;
            set;
        }
        public string FRD
        {
            get;
            set;
        }
        public float QtyRequired
        {
            get;
            set;
        }
        public int UOM
        {
            get;
            set;
        }
        public string KnownVendors
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Purpose
        {
            get;
            set;
        }
        public string CommercialDetails
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        public string SearchValue
        {
            get;
            set;
        }
        public string SearchType
        {
            get;
            set;
        }
       
        public int UserPk
        {
            get;
            set;
        }
        public int BizUnitPk
        {
            get;
            set;
        }
    }
}
