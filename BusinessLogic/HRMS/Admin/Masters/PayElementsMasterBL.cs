using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Admin.Masters;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class PayElementsMasterBL
    {
        /// <summary>
        /// for get parent elemetns
        /// </summary>
        /// <param name="CurrPK">Pk</param>
        /// <param name="status">status</param>
        /// <param name="bizUnit">biz unit</param>
        /// <returns></returns>
        public static DataTable GetParentElement(int CurrPK, int status, int bizUnit, int excludePK, int isLoanAdv = 0, int pelClass = -1, int? isDeduct = null)
        {
            return PayElementsMasterDL.GetParentElement(CurrPK, status, bizUnit, excludePK, isLoanAdv, pelClass, isDeduct);
        }

        public static DataTable GetEarnDeductPayElements(int payElementPk, int status, int bizUnit, int isDeduct)
        {
            return PayElementsMasterDL.GetEarnDeductPayElements(payElementPk, status, bizUnit, isDeduct);
        }
        /// <summary>
        /// save Pay Elements
        /// </summary>
        /// <param name="objPayElements"></param>
        /// <returns></returns>
        public static int? SavePayElements(PayElementsMasterBO objPayElements)
        {
            return PayElementsMasterDL.SavePayElements(objPayElements);
        }
        /// <summary>
        /// For get  parent elemetns lsit
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="classification"></param>
        /// <param name="status">1:Active records, 0:Inactive records, null:All records</param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static DataTable GetParentElementList(BusinessObject.GridPrams gridParam, int bsu, string code, string name, string classification, int? status,string sortOrder=null)
        {
            return PayElementsMasterDL.GetParentElementList(gridParam, bsu, code, name, classification, status, sortOrder);
        }
        /// <summary>
        /// Delete Pay Elements
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeletePayElement(int CurrPK, DateTime dateTime)
        {
            return PayElementsMasterDL.DeletePayElement(CurrPK, dateTime);
        }

        public static DataTable GetPayElementSearchList(int bizUnit, string filterby, string searchValue)
        {
            return PayElementsMasterDL.GetPayElementSearchList(bizUnit, filterby, searchValue);
        }

        public static int? UpdatePayElementStatus(int PEL_PK, int Status, int UserPk, DateTime? LastModDate)
        {
            return PayElementsMasterDL.UpdatePayElementStatus(PEL_PK, Status, UserPk, LastModDate);
        }

        /// <summary>
        /// Get pay element slab and custom in single list
        /// </summary>
        /// <param name="PayElementPk"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetPayElementValueSingleList(int PayElementPk, int Status,int CurrPK)
        {
            return PayElementsMasterDL.GetPayElementValueSingleList(PayElementPk, Status, CurrPK);
        }

        public static DataTable GetFormulaElement(int CurrPK, int status, int bizUnit, int isDeduction)
        {
            return PayElementsMasterDL.GetFormulaElement(CurrPK, status, bizUnit, isDeduction);
        }
    }
}
