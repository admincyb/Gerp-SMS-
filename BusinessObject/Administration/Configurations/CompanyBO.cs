using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class CompanyBO
    {
        public int PK { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int Currency { get; set; }
        public string TaxNo { get; set; }
        public string Logo { get; set; }
        public string LogoURL { get; set; }
        public int UserPk { get; set; }
        public int BizUnit { get; set; }
        public string Website { get; set; }
        public string CompanyNameLocal { get; set; }
        public string ZipCode { get; set; }
        public int Active { get; set; }
        public string BisRegNo { get; set; }
        public string GSTNo { get; set; }
        public DateTime? FinYearStartDate { get; set; }
        public DateTime? FinYearEndDate { get; set; }
        public string OutputLogo { get; set; }
        public string OutputLogoURL { get; set; }
        public string DisplayCode { get; set; }
        public string DisplayName { get; set; }

        public int? ProductionIn { get; set; }
        public int? NoOfPcsPerBskt { get; set; }
        public int? WeightPerBskt { get; set; }
        public int? NoOfBasket { get; set; }
        public int? TimeReqToPrd { get; set; }
        public int? PrintLabel { get; set; }

        public int Medical { get; set; }
        public int NonMedical { get; set; }
    }
}
