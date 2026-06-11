using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using System.Data.Objects.DataClasses;

namespace ERPManager
{
    public class VendorMasterManager:IVendorMasterManager
    {  
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        List<short> vendRole;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Vendor Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public VendorMasterManager(ERPEntities currentEntity)
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
        /// Gets list of vendors
        /// </summary>
        /// <param name="admTaxGroupsMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Tax Group Master</returns>
        public List<PUR_VENDOR_MST> GetVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            List<PUR_VENDOR_MST> PUR_VENDOR_MST_List_Obj = new List<PUR_VENDOR_MST>();
            IQueryable<PUR_VENDOR_MST> purVENDOR_MSTQuery;
            try
            {
                vendRole = purVendorObj.PUR_VENDOR_ROLE_MAP != null && purVendorObj.PUR_VENDOR_ROLE_MAP.Count > 0 ?
                    purVendorObj.PUR_VENDOR_ROLE_MAP.Select(mpg => mpg.VRM_ROLE).ToList() : null;
                purVENDOR_MSTQuery = (from vnd in this.currentEntity.PUR_VENDOR_MST
                                      where vnd.VEN_ACTIVE == purVendorObj.VEN_ACTIVE
                                       //&& vnd.VEN_NAME.StartsWith(utilityObj.FilterValue)
                                       && vnd.VEN_NAME.Contains(utilityObj.FilterValue)
                                       && vnd.VEN_STATUS == 2 && (utilityObj.IsSBUSpecific == true ? vnd.VEN_BIZUNIT == purVendorObj.VEN_BIZUNIT : true)
                                      select vnd);
                if (vendRole != null && vendRole.Count > 0)
                {
                    purVENDOR_MSTQuery = purVENDOR_MSTQuery.Where(itm => (from mpg in itm.PUR_VENDOR_ROLE_MAP
                                                                          join role in vendRole.AsQueryable()
                                                                          on mpg.VRM_ROLE equals role
                                                                          select role).Count() > 0);
                }
                PUR_VENDOR_MST_List_Obj = purVENDOR_MSTQuery.OrderBy(vnd => vnd.VEN_NAME).ToList();
            }
            catch
            {

            }
            return PUR_VENDOR_MST_List_Obj;
        }
        /// <summary>
        /// Gets list of vendors by vendor name or vendor code.
        /// </summary>
        /// <param name="purVendorObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Vendor Master</returns>
        public List<PUR_VENDOR_MST> GetVendorNameCodeAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            List<PUR_VENDOR_MST> PUR_VENDOR_MST_List_Obj = new List<PUR_VENDOR_MST>();
            IQueryable<PUR_VENDOR_MST> purVENDOR_MSTQuery;
            try
            {
                purVENDOR_MSTQuery = (from vnd in this.currentEntity.PUR_VENDOR_MST
                                      where vnd.VEN_ACTIVE == purVendorObj.VEN_ACTIVE
                                       && (vnd.VEN_NAME.Contains(utilityObj.FilterValue) || (vnd.VEN_CODE != null && vnd.VEN_CODE.Contains(utilityObj.FilterValue)))
                                       && vnd.VEN_STATUS == 2 && (utilityObj.IsSBUSpecific == true ? vnd.VEN_BIZUNIT == purVendorObj.VEN_BIZUNIT : true)
                                      select vnd);
                PUR_VENDOR_MST_List_Obj = purVENDOR_MSTQuery.OrderBy(vnd => vnd.VEN_NAME).ToList();
            }
            catch
            {

            }
            return PUR_VENDOR_MST_List_Obj;
        }
        /// <summary>
        /// Gets list of Service vendors
        /// </summary>
        /// <param name="admTaxGroupsMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Tax Group Master</returns>
        public List<PUR_VENDOR_MST> GetServiceVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            List<PUR_VENDOR_MST> PUR_VENDOR_MST_List_Obj = new List<PUR_VENDOR_MST>();

            try
            {

                PUR_VENDOR_MST_List_Obj = (from vnd in this.currentEntity.PUR_VENDOR_MST
                                           where vnd.VEN_ACTIVE == purVendorObj.VEN_ACTIVE
                                            && vnd.VEN_NAME.StartsWith(utilityObj.FilterValue)
                                            && vnd.VEN_STATUS == 2
                                            && vnd.PUR_VENDOR_ROLE_MAP.Any(rol=> rol.VRM_ROLE ==3|| rol.VRM_ROLE==10)
                                           select vnd).OrderBy(vnd => vnd.VEN_NAME).ToList();
            }
            catch
            {

            }

            return PUR_VENDOR_MST_List_Obj;
        }


        /// <summary>
        /// Gets list of vendors
        /// </summary>
        /// <param name="admTaxGroupsMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Tax Group Master</returns>
        public List<PUR_VENDOR_MST> GetVendorRoleListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            List<PUR_VENDOR_MST> PUR_VENDOR_MST_List_Obj = new List<PUR_VENDOR_MST>();

            try
            {
                PUR_VENDOR_MST_List_Obj = (from vnd in this.currentEntity.PUR_VENDOR_MST
                                           join role in this.currentEntity.PUR_VENDOR_ROLE_MAP on vnd.VEN_PK equals role.VRM_VENDOR
                                           where vnd.VEN_ACTIVE == purVendorObj.VEN_ACTIVE
                                            && vnd.VEN_NAME.StartsWith(utilityObj.FilterValue)
                                            && vnd.VEN_STATUS == 2
                                            && role.VRM_ROLE ==10
                                           select vnd).OrderBy(vnd => vnd.VEN_NAME).ToList();
            }
            catch
            {

            }

            return PUR_VENDOR_MST_List_Obj;
        }

        //Sample Code
        //public List<ERPData.AD_TAX_GROUPS_MST> GetAdmTaxGroupsMstAutoCompleteList(ERPData.AD_TAX_GROUPS_MST admTaxGroupsMstObj, ServiceUtility utilityObj)
        //{
        //    List<AD_TAX_GROUPS_MST> admTaxGroupsMstList;
        //    AD_TAX_GROUPS_MST tempadmTaxGroupsMstObj = null;
        //    int pageSize;
        //    try
        //    {
        //        admTaxGroupsMstList = new List<AD_TAX_GROUPS_MST>();
        //        pageSize = Convert.ToInt32(utilityObj.PageSize);

        //        if (utilityObj.FilterBy == DataFieldRes.TaxGroupCode)
        //        {
        //            admTaxGroupsMstList = currentEntity.AD_TAX_GROUPS_MST
        //                                   .Where(txg => txg.TXG_BizUnit == (admTaxGroupsMstObj.TXG_BizUnit <= 0 ? txg.TXG_BizUnit : admTaxGroupsMstObj.TXG_BizUnit)
        //                                    && txg.TXG_Active == 1 && txg.TXG_Code.StartsWith(utilityObj.FilterValue))
        //                                   .OrderBy(txg => txg.TXG_Code).ToList();
        //        }
        //        else
        //        {
        //            admTaxGroupsMstList = currentEntity.AD_TAX_GROUPS_MST
        //                                   .Where(txg => txg.TXG_BizUnit == (admTaxGroupsMstObj.TXG_BizUnit <= 0 ? txg.TXG_BizUnit : admTaxGroupsMstObj.TXG_BizUnit)
        //                                    && txg.TXG_Active == 1 && txg.TXG_Code.StartsWith(utilityObj.FilterValue))
        //                                   .OrderBy(txg => txg.TXG_Code).ToList();
        //        }

        //        if (admTaxGroupsMstList.Count > pageSize)
        //        {
        //            admTaxGroupsMstList = admTaxGroupsMstList.Take(pageSize).ToList();
        //            tempadmTaxGroupsMstObj = new AD_TAX_GROUPS_MST();
        //            tempadmTaxGroupsMstObj.TXG_PK = 0;
        //            tempadmTaxGroupsMstObj.TXG_Code = gComsManagerRes.MoreAutoComplete;
        //            admTaxGroupsMstList.Add(tempadmTaxGroupsMstObj);

        //            return admTaxGroupsMstList;
        //        }
        //        else
        //        {
        //            return admTaxGroupsMstList;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Handler for unknown exceptions
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        // Disposing used objects
        //        admTaxGroupsMstList = null;
        //        tempadmTaxGroupsMstObj = null;
        //    }
        //}

    
        #endregion

        #region Private Methods

        //private static IQueryable<AD_TAX_GROUPS_MST> FilterEntity(AD_TAX_GROUPS_MST adTaxesMstObj, IQueryable<AD_TAX_GROUPS_MST> qry, ServiceUtility utilityObj)
        //{
        //    #region Filtering
        //    if (adTaxesMstObj.TXG_PK != 0)
        //        qry = qry.Where(cnt => cnt.TXG_PK == adTaxesMstObj.TXG_PK);
        //    if (adTaxesMstObj.TXG_BizUnit != -1)
        //    {
        //        qry = qry.Where(cnt => cnt.TXG_BizUnit == adTaxesMstObj.TXG_BizUnit);
        //    }
        //    if (adTaxesMstObj.TXG_PK < 1 && adTaxesMstObj.TXG_Active == 1) //select all active records
        //        qry = qry.Where(cnt => cnt.TXG_Active == 1);
        //    else if (adTaxesMstObj.TXG_PK > 0 && adTaxesMstObj.TXG_Active == 1) // select all active +(union) having given pk
        //        qry = qry.Where(cnt => cnt.TXG_Active == adTaxesMstObj.TXG_Active || cnt.TXG_PK == adTaxesMstObj.TXG_PK);

        //    if (utilityObj.FilterBy != null || utilityObj.FilterValue != null)
        //    {
        //        // Filter AD_TAX_GROUPS_MST with Code
        //        if (utilityObj.FilterBy == DataFieldRes.TaxGroupCode)
        //            qry = qry.Where(jbm => jbm.TXG_Code.Contains(utilityObj.FilterValue));
        //        // Filter AD_TAX_GROUPS_MST with Code
        //        if (utilityObj.FilterBy == DataFieldRes.TaxGroupName)
        //            qry = qry.Where(jbm => jbm.TXG_Name.Contains(utilityObj.FilterValue));

        //        // Filter AD_TAX_GROUPS_MST with Active
        //        if (utilityObj.FilterBy == DataFieldRes.TaxGroupActive)
        //            qry = qry.Where(jbm => jbm.TXG_Active == byte.Parse(utilityObj.FilterValue));
        //    }
        //    #endregion
        //    return qry;
        //}

        #endregion

      
    }
}
