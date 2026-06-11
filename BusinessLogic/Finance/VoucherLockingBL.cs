using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.Finance;

namespace BusinessLogic.Finance
{
    public class VoucherLockingBL
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

        public static int? SaveVoucherLockingDetails(VoucherLockingBO objVoucherLoc)
        {
            return VoucherLockingDL.SaveVoucherLockingDetails(objVoucherLoc);
        }

        public static DataTable GetVoucherLockList(int? PK, int? Active, int BizUnit, int LockMOD)
        {
            return VoucherLockingDL.GetVoucherLockList(PK, Active, BizUnit, LockMOD);
        }

        public static bool IsVoucherLocked(DateTime VoucherDate,int VoucherPk, int BizUnit, ref string LockUptoDate)
        {
            return VoucherLockingDL.IsVoucherLocked(VoucherDate, VoucherPk, BizUnit, ref LockUptoDate);
        }
       
        public static bool IsFinYearLocked(int finYearPK, int BizUnit)
        {
            return VoucherLockingDL.IsFinYearLocked(finYearPK, BizUnit);
        }
    }
}
