using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess;
using BusinessObject.Administration.Masters;
using GTIService;
using BusinessObject.CommonManagement;
using DataAccess.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
    public class AccountMapingBL
    {
        /// <summary>
        /// To Retrive Mapping Type
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetMappingType(int PK,int active)
        {
            return  AccountMapingDA.GetMappingType(PK,active);
        }


        /// <summary>
        /// To Retrive Mapping details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetMappingDetails(int Active,int PK)
        {
            DataTable dtMapping =  AccountMapingDA.GetMappingDetails(Active,PK);
            return dtMapping;
        }
         

        /// <summary>
        /// To Retrive Mapping details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetMappingTypeData(int PK, int Active,int SubType,int Group)
        {
            DataTable dtMapping =  AccountMapingDA.GetMappingTypeData(PK, Active, SubType, Group);
            return dtMapping;
        }
         

        /// <summary>
        /// Passing XML Data for Insert
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int AccountMappingHeader(string pXML)
        {
            return  AccountMapingDA.AccountMappingHeader(pXML);
        }


        /// <summary>
        /// To Retrive Mapping details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int PK)
        {
            DataTable dtResult = AccountMapingDA.GetCostCenter(PK);
            return dtResult;
        }

        /// <summary>
        /// Save Cost Center Allocation Tab
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveCostCenter(string pXML)
        {
            return AccountMapingDA.SaveCostCenter(pXML);
        }
         
       
         
    }
}
