using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Finance;
using BusinessObject;
namespace BusinessLogic.Finance
{
   public class BadDebitsBL
    {
        /// <summary>
        /// Get Bad Debits 
        /// </summary>
        /// <param name="ApplicationTypePk"></param>
        /// <param name="Active"></param>
        /// <param name="SplCondition"></param>
        /// <returns></returns>
       public static DataSet GetBadDebits(GridPrams grid, User objUser, int MonthInterval)
        {
            return BadDebitDL.GetBadDebits(grid, objUser, MonthInterval);
        }

       /// <summary>
       /// Get Bad Debit Relief Claimable List
       /// </summary>
       /// <param name="ApplicationTypePk"></param>
       /// <param name="Active"></param>
       /// <param name="SplCondition"></param>
       /// <returns></returns>
       public static DataSet GetReliefList(GridPrams grid, User objUser,int IBD_PK,int Active)
       {
           return BadDebitDL.GetReliefList(grid, objUser, IBD_PK,Active);
       }

       /// <summary>
       /// Get Bad Debit Relief Claimable Details of each Record
       /// </summary>
       /// <param name="ApplicationTypePk"></param>
       /// <param name="Active"></param>
       /// <param name="SplCondition"></param>
       /// <returns></returns>
       public static DataSet GetReliefSpecificDetails(GridPrams grid, User objUser, int IBD_PK, int Active)
       {
           return BadDebitDL.GetReliefSpecificDetails(grid, objUser, IBD_PK, Active);
       }

       /// <summary>
       /// Save PO Invoice
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns></returns>
       public static int? SaveBadDebitDetails(string strxml)
       {
           return BadDebitDL.SaveBadDebitDetails(strxml);
       }
    
       /// <summary>
       /// Delete BadDebitReliefDetails
       /// </summary>
       /// <param name="invPK"></param>
       /// <param name="lastModDate"></param>
       /// <returns></returns>
       public static int DeleteBadDebitReliefDetails(int IBD_PK, DateTime lastModDate, string appType, string currentUser)
       {
           return BadDebitDL.DeleteBadDebitReliefDetails(IBD_PK, lastModDate, appType, currentUser);
       }
    }
}
