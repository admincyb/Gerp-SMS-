using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class BudgetBO : WorkflowBO
    {
        [XmlElement("BGH_PK")]

        public int BGH_PK { get; set; }

        [XmlElement("BGH_NO")]
        public string BGH_NO { get; set; }
        [XmlElement("USER_PK")]
        public int UserPk { get; set; }

        [XmlElement("BGH_YEAR")]
        public int BGH_YEAR { get; set; }
        [XmlElement("BGH_TYPE")]
        public byte BGH_TYPE { get; set; }

        [XmlElement("BGH_VERSION")]
        public int BGH_VERSION { get; set; }

        [XmlElement("BGH_BIZUNIT")]
        public int BGH_BIZUNIT { get; set; }
        [XmlElement("BGH_STATUS")]
        public byte BGH_STATUS { get; set; }
        [XmlElement("BGH_IS_AMEND")]
        public byte BGH_IS_AMEND { get; set; }
        [XmlElement("TOTAL_ROW_COUNT")]
        public int TOTAL_ROW_COUNT { get; set; }
        [XmlElement("BGH_AMOUNT")]
        public decimal BGH_AMOUNT { get; set; }

        [XmlElement("Detail")]
        public List<BudgetDetails> Details { get; set; }
    }

    [Serializable]
    [XmlRoot("Detail")]
    public class BudgetDetails
    {
        [XmlElement("BDG_SL_NO")]
        public int BDG_SL_NO { get; set; }
        [XmlElement("BDG_PK")]
        public int BDG_PK { get; set; }
        [XmlElement("BDG_MONTH")]
        public int BDG_MONTH { get; set; }
        [XmlElement("BDG_PLANT")]
        public int BDG_PLANT { get; set; }
        [XmlElement("BDG_PLANT_NAME")]
        public string BDG_PLANT_NAME { get; set; }
        [XmlElement("BDG_PLANT_CODE")]
        public string BDG_PLANT_CODE { get; set; }
        [XmlElement("BDG_COST_CENTER")]
        public int BDG_COST_CENTER { get; set; }
        [XmlElement("BDG_COA")]
        public int BDG_COA { get; set; }
        [XmlElement("BDG_COST_CENTER_NAME")]
        public string BDG_COST_CENTER_NAME { get; set; }
        [XmlElement("BDG_ACCOUNT_NAME")]
        public string BDG_ACCOUNT_NAME { get; set; }
        [XmlElement("BDG_ACCOUNT_NO")]
        public string BDG_ACCOUNT_NO { get; set; }
        [XmlElement("BDG_AMOUNT")]
        public float BDG_AMOUNT { get; set; }
        [XmlElement("BDG_VERSION")]
        public int BDG_VERSION { get; set; }
        [XmlElement("BDG_BUDGET_DATE")]
        public DateTime BDG_BUDGET_DATE { get; set; }
        [XmlElement("BDG_AMEND_AMOUNT")]
        public float BDG_AMEND_AMOUNT { get; set; }
        [XmlElement("BDG_YEAR")]
        public int BDG_YEAR { get; set; }
        [XmlElement("BDG_MONTH_TEXT")]
        public string BDG_MONTH_TEXT { get; set; }
        [XmlElement("BDG_DEL_STATUS")]
        public int BDG_DEL_STATUS { get; set; }
        [XmlElement("BDG_STATUS_BIT")]
        public int BDG_STATUS_BIT { get; set; }
        
    }
}