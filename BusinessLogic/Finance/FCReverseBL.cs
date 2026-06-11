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
    public class FCReverseBL
    {
        /// <summary>
        /// Get PO Invoice List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetFCRList(GridPrams grid, User objUser, string vNo, int Bankpk, int PoPk, int Active = 0, int? Status = null)
        {
            return FCReverseDL.GetFCRList(grid, objUser, vNo, Bankpk, PoPk, Active, Status);
        }
        /// <summary>
        /// Get FCHold Revert Details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="accountPK"></param>
        /// <param name="curPK"></param>
        /// <returns></returns>
        public static DataSet GetFCHoldRevertDetails(int PK, int accountPK, int curPK)
        {
            return FCReverseDL.GetFCHoldRevertDetails(PK, accountPK, curPK);
        }
        /// <summary>
        /// Save FCHold Revert Details 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveFCHoldRevertDetails(string xmlDoc)
        {
            return FCReverseDL.SaveFCHoldRevertDetails(xmlDoc);
        }
        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteFCHoldRevertDetails(int hrhPK, DateTime lastModDate)
        {
            return FCReverseDL.DeleteFCHoldRevertDetails(hrhPK, lastModDate);
        }

        public static DataTable GetFCVoucherNumberAuto(byte Active, int bizUnit, string searchValue)
        {
            return FCReverseDL.GetFCVoucherNumberAuto(Active, bizUnit, searchValue);
        }
    }
}
