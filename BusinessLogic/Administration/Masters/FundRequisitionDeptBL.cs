using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;
using GTIService;
using DataAccess.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
   public class FundRequisitionDeptBL
    {
       public static DataTable GetFundRequisitionDeptList(GridPrams grid, User objUser, string trxNo, int reqDeptPk)
        {
            return FundRequisitionDeptDL.GetFundRequisitionDeptList(grid, objUser, trxNo, reqDeptPk);
        }

        public static int? SaveFundRequisitionDeptDetails(string strxml, out string TrxNo)
        {
            try
            {
                return FundRequisitionDeptDL.SaveFundRequisitionDeptDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static FundRequisitionDeptHeader GetFundRequisitionDeptByPK(int itemPK)
        {
            try
            {
                FundRequisitionDeptHeader fundReqDeptHeader = new FundRequisitionDeptHeader();
                string dtl = FundRequisitionDeptDL.GetFundRequisitionDeptByPK(itemPK);
                if (dtl != string.Empty)
                {
                    fundReqDeptHeader = (FundRequisitionDeptHeader)CommonFunctions.DeserializeObject(dtl, fundReqDeptHeader);
                    return fundReqDeptHeader;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        public static int DeleteFundRequisitionDept(int pk, string lastModifiedDate)
        {
            return FundRequisitionDeptDL.DeleteFundRequisitionDept(pk, lastModifiedDate);
        }
        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetFundRequisitionNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = FundRequisitionDeptDL.GetFundRequisitionNoAutocomplete(searchBy, searchValue, objUser);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Designation.Fields.PK),
                    Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }
        //Fund Requisition Dept Wise Output Report
        public static DataSet GetFundRequisitionDeptReport(int RecPK)
        {
            return FundRequisitionDeptDL.GetFundRequisitionDeptReport(RecPK);
        }

    }
}
