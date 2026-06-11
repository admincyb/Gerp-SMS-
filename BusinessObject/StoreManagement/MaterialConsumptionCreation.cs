using System.Collections.Generic;


namespace BusinessObject.StoreManagement
{
   public class MaterialConsumptionCreation
    {
        public int UserPk
        {
            get;
            set;
        }
        public int DeptPk
        { get; set; }
        public int ICH_PK
        { get; set; }

        public int ICH_DEPT_TEXT
        { get; set; }
        public int ICH_STATUS
        { get; set; }
        public string ICH_DATE
        { get; set; }
        public string ICH_NO
        { get; set; }
        public List<MaterialConsumptionDetail> ConsumptionDetailsList
        { get; set; }
    }
   
}
