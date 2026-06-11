using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;

namespace BusinessLogic.Finance
{
    public class YearEndVoucherBL
    {
        /// <summary>
        /// Get Transactions
        /// </summary>
        /// <param name="ApplicationTypePk"></param>
        /// <param name="Active"></param>
        /// <param name="SplCondition"></param>
        /// <returns></returns>
        public static DataTable GetTransactions(int ApplicationTypePk, int Active, string SplCondition)
        {
            return YearEndVoucherDL.GetTransactions(ApplicationTypePk, Active, SplCondition);
        }

        public static DataTable GetPartyOrVoucherDDL(int TransactionType, int VoucherType, DateTime AsOnDate, int Status, int BizUnit, int VoucherPk, int PartyPk, string Field)
        {
            return YearEndVoucherDL.GetPartyOrVoucherDDL(TransactionType,  VoucherType,  AsOnDate,  Status,  BizUnit, VoucherPk, PartyPk, Field);
        }
        public static DataTable GetVoucherTypes(int ConPk, int ConActive, int ConGroupTypeVal, int GroupConVal)
        {
            return YearEndVoucherDL.GetVoucherTypes(ConPk, ConActive, ConGroupTypeVal, GroupConVal);
        }

        public static DataTable GetVoucherList(int TransactionType, int VoucherType, DateTime AsOnDate, int Status, int BizUnit, int VoucherPk, int PartyPk,int PNO, int PSize)
        {
            return YearEndVoucherDL.GetVoucherList(TransactionType, VoucherType, AsOnDate, Status, BizUnit, VoucherPk,PartyPk, PNO,  PSize);
        }

        public static int? YearEndVoucher(string xmlDoc)
        {
            return YearEndVoucherDL.YearEndVoucher(xmlDoc);
        }

        public static int? CancelVoucher(int VoucherPk, int RefId)
        {
            return YearEndVoucherDL.YearEndVoucher(VoucherPk, RefId);
        }

        public static int YearClosingSave(DateTime VoucherDate, User objUser, int CompanyPk)
        {
            return YearEndVoucherDL.YearClosingSave(VoucherDate, objUser, CompanyPk);
        }
    }
}
