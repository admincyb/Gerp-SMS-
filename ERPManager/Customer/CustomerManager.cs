using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

namespace ERPManager
{
    public class CustomerManager : ICustomerManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// customer Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public CustomerManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get List of customers
        /// </summary>
        /// <param name="salCustomerObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<CRM_CUSTOMER_MST> GetCustomerListAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj)
        {
            List<CRM_CUSTOMER_MST> CRM_CUSTOMER_MST_List_Obj = new List<CRM_CUSTOMER_MST>();

            try
            {
                CRM_CUSTOMER_MST_List_Obj = (from cus in this.currentEntity.CRM_CUSTOMER_MST
                                             where cus.CUS_ACTIVE == salCustomerObj.CUS_ACTIVE
                                             && (utilityObj.IsSBUSpecific==true ? cus.CUS_BIZUNIT==salCustomerObj.CUS_BIZUNIT : true)
                                             && cus.CUS_NAME.Contains(utilityObj.FilterValue)
                                             orderby cus.CUS_NAME
                                             select cus).ToList();
            }
            catch
            {

            }

            return CRM_CUSTOMER_MST_List_Obj;

        }

        /// <summary>
        /// Get List of Customers and Vendors
        /// </summary>
        /// <param name="salCustomerObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<BusinessObject.CommonManagement.AutoCompleteBO> GetCustomerVendorAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj)
        {
            List<BusinessObject.CommonManagement.AutoCompleteBO> CRM_CUSTOMER_MST_List_Obj = new List<BusinessObject.CommonManagement.AutoCompleteBO>();          
            try
            {               

                var objCustomerList = (from cus in this.currentEntity.CRM_CUSTOMER_MST
                                       where cus.CUS_ACTIVE == salCustomerObj.CUS_ACTIVE
                                       && (utilityObj.IsSBUSpecific == true ? cus.CUS_BIZUNIT == salCustomerObj.CUS_BIZUNIT : true)
                                       select new BusinessObject.CommonManagement.AutoCompleteBO
                                       {
                                           Name = cus.CUS_NAME.Trim(),
                                           Key = cus.CUS_PK
                                       }).ToList();



                var objVendorList = (from vnd in this.currentEntity.PUR_VENDOR_MST
                                     where vnd.VEN_ACTIVE == salCustomerObj.CUS_ACTIVE && vnd.VEN_STATUS == 2
                                     && (utilityObj.IsSBUSpecific==true ? vnd.VEN_BIZUNIT == salCustomerObj.CUS_BIZUNIT : true)
                                     select new BusinessObject.CommonManagement.AutoCompleteBO
                                        {
                                            Name = vnd.VEN_NAME.Trim(),
                                            Key = vnd.VEN_PK
                                        }).ToList();
                var objResultList = (objCustomerList.Union(objVendorList)).ToList();
                CRM_CUSTOMER_MST_List_Obj = (from c in objResultList where c.Name.ToLower().StartsWith(utilityObj.FilterValue.ToLower()) orderby c.Name select c).ToList();               
            }
            catch
            {

            }
            return CRM_CUSTOMER_MST_List_Obj;

        }



        #endregion

        #region Private Methods

        #endregion

    }
    public class CustomerInvItem
    {
        public int? CIM_PK { get; set; }
        public string CIM_BRAND_CODE { get; set; }
        public string CIM_BRAND_NAME { get; set; }
        public string CIM_CUSTOMER { get; set; }
        public byte? ACTIVE { get; set; }
        public int? USER_PK { get; set; }
        public int? BIZUNIT { get; set; }
        public int? CIM_BIZUNIT { get; set; }

    }
}
