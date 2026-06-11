namespace BusinessObject.StoreManagement
{
 
    public class MaterialConsumptionDetail
    {
        public string ICD_ITEM_CATEGORY_TEXT
        { get; set; }
        public int ICD_ITEM_CATEGORY
        { get; set; }
        public string ICD_ITEM_TEXT
        { get; set; }
        public string MaterialName
        { get; set; }
        public int ICD_PK
        { get; set; }
        public int ICD_ITEM
        { get; set; }
        public double ICD_QTY_CONSUMED
        { get; set; }
        public double ICD_CURRENT_STK
        { get; set; }
        public string UOM
        { get; set; }
        public int ICD_UOM
        { get; set; }
        public string ICD_REMARKS
        { get; set; }
    }
}
