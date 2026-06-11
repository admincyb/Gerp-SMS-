using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.MaterialManagement
{
    public class MaterialCategory
    {
        public int MaterialCategoryPK
        {
            get;
            set;
        }
        public string CategoryCode
        {
            get;
            set;
        }
        public string CategoryName
        {
            get;
            set;
        }
        public int SBU
        {
            get;
            set;
        }
        public string CategoryDesc
        {
            get;
            set;
        }
        public int InactivePeriod { get; set; }
        public int UOMType
        {
            get;
            set;
        }
        public int CategoryType
        {
            get;
            set;
        }
        public int MaterialCategoryParentPK
        {
            get;
            set;
        }

        public string MaterialCategoryParentName
        {
            get;
            set;
        }

        public int UserPk
        {
            get;
            set;
        }

        public string HasChild
        {
            get;
            set;
        }

        public string IsUsing
        {
            get;
            set;
        }

        public string IsDefault
        {
            get;
            set;
        }
        public string PurAccount
        {
            get;
            set;
        }
        public string InvAccount
        {
            get;
            set;
        }

        public string SaleAccount
        {
            get;
            set;
        }

        public string ConsumptionAccount
        {
            get;
            set;
        }
        public string PoCategory
        {
            get;
            set;
        }

        //public string RequireInspection
        //{
        //    get;
        //    set;
        //}
        public string Stock
        {
            get;
            set;
        }
        public Int16 GrnType
        {
            get;
            set;
        }
        public string ReportCategory
        {
            get;
            set;
        }
        public string ITC_IS_SALE
        {
            get;
            set;
        }
        public string ITC_IS_VCH_POST
        {
            get;
            set;
        }
        public List<MaterialCategoryAccounts> MaterialAccountlst
        {
            get;
            set;
        }
    }
    public class MaterialCategoryAccounts
    {
        public int ICC_PK { get; set; }
        public int ICC_COMPANY { get; set; }
        public string ICC_COMPANY_TEXT { get; set; }
        public int ICC_ACCOUNT_TYPE { get; set; }
        public string ICC_ACCOUNT_TYPE_TEXT { get; set; }
        public int ICC_ACCOUNT { get; set; }
        public string ICC_ACCOUNT_TEXT { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class MaterialCategorySaveAccounts
    {
        [XmlElement("Detail")]
        public List<CategoryDetails> MaterialAccountlst
        {
            get;
            set;
        }
    }

    [Serializable]
    public class CategoryDetails
    {
        [XmlElement("ICC_PK")]
        public int ICC_PK { get; set; }
        [XmlElement("ICC_COMPANY")]
        public int ICC_COMPANY { get; set; }
        [XmlElement("ICC_COMPANY_TEXT")]
        public string ICC_COMPANY_TEXT { get; set; }
        [XmlElement("ICC_ACCOUNT_TYPE")]
        public int ICC_ACCOUNT_TYPE { get; set; }
        [XmlElement("ICC_ACCOUNT_TYPE_TEXT")]
        public string ICC_ACCOUNT_TYPE_TEXT { get; set; }
        [XmlElement("ICC_ACCOUNT")]
        public int ICC_ACCOUNT { get; set; }
        [XmlElement("ICC_ACCOUNT_TEXT")]
        public string ICC_ACCOUNT_TEXT { get; set; }
    }
}
