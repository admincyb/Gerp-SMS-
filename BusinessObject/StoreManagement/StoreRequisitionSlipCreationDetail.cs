namespace BusinessObject.StoreManagement
{
   public class StoreRequisitionSlipCreationDetail
    {
        public string MaterialType
        {  get; set; }
        public int MaterialTypePk
        { get; set; }
        public string MaterialCode
        { get; set; }
        public string MaterialName
        { get; set; }
        public int MRD_PK
        { get; set; }
        public int MRD_ITEM
        { get; set; }
        public double MRD_QTY_REQUESTED
        { get; set; }
        public double CurrentStock
        { get; set; }
        public string UOM
        { get; set; }
        public int MRD_UOM
        { get; set; }
        public string MRD_REMARKS
        { get; set; }
 
    }
}
