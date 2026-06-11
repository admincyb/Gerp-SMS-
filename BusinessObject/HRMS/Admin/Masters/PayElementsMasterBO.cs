using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class PayElementsMasterBO
    {
        public int PayElmntPk { get; set; }
        public string PayElmntCode { get; set; }
        public string PayElmntName { get; set; }
        public string P_PEL_CODE_LL { get; set; }
        public string P_PEL_NAME_LL { get; set; }
        public string PayElmntClass { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public string Parent { get; set; }
        public int PaySlip { get; set; }
        public int Recurring { get; set; }
        public int CTC { get; set; }
        public int isTaxable { get; set; }
        public string AccountCode { get; set; }
        public int Active { get; set; }
        public int UserPk { get; set; }
        public int BizUnit { get; set; }
        public DateTime LastModDate { get; set; }
        public int isDeduct{ get; set; }
        public string FormulaCode { get; set; }
        public string PayType { get; set; }
        public int PEL_USD_IN_FMLA { get; set; }
        public int IncludeInSalary { get; set; }
        public int IsEditable { get; set; }
        public int PartofGross { get; set; }
        public string PayElmntDesc { get; set; }
        public int FormulaEditable { get; set; }
        public int RoundoffRequired { get; set; }
        public int Sequence { get; set; }
        public int ShowReport { get; set; }
        public int ShowReport1 { get; set; }
        public int ShowInEmp { get; set; }
    }
}
