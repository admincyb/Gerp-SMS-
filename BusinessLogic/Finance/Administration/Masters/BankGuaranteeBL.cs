using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Finance.Administration.Masters;
using System.Data;
namespace BusinessLogic.Finance.Administration.Masters
{
    public class BankGuaranteeBL
    {

        public static int? SaveBankGuarantee(string xmlDoc)
        {
            return BankGuaranteeDL.SaveBankGuarantee(xmlDoc);
        }
        public static DataTable GetKVBankGuarantee(int pk, int active, int bizUnit, int partyPk, int bankPk, int pageNo, int pageSize, string sortBy, string sortDirection)
        {
            return BankGuaranteeDL.GetKVBankGuarantee(pk, active, bizUnit,partyPk, bankPk, pageNo, pageSize, sortBy, sortDirection);
        }
        public static int DeleteBankGuarantee(int pk, DateTime lastModDate)
        {
            return BankGuaranteeDL.DeleteBankGuarantee(pk, lastModDate);
        }
    }
}
