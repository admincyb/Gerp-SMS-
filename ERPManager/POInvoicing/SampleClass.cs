using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

namespace ERPManager
{
    public class SampleClass
    {  
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Currency Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public SampleClass(ERPEntities currentEntity)
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
        /// Gets list of Tax Group Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="admTaxGroupsMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Tax Group Master</returns>
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

        /// <summary>
        /// Insert or update list of  AdTaxesGroupMstDtl
        /// </summary>
        /// <param name="admTaxGroupsMstList">List of  AdTaxes GroupMstDtl</param>
        /// <returns>primary key of last inserted or updated AdTaxes GroupMstDtl</returns>
        //public short SaveAdmTaxGroupsMst(List<AD_TAX_GROUPS_MST> admTaxGroupsMstList)
        //{
        //    // Gets or sets save status
        //    short retval;

        //    // Gets or sets adTaxesMstpk
        //    int? maxAdTaxesGroupMstpk;

        //    // Gets or sets old AdTaxesGroupMst Used for updating AdTaxesGroupMst and for checking concurrency
        //    AD_TAX_GROUPS_MST oldAdTaxesGroupMstObj;
        //    try
        //    {
        //        // Sets save status zero,save failed
        //        retval = 0;

        //        // Iterating through AdTaxesGroupMst list
        //        foreach (AD_TAX_GROUPS_MST adTaxesGroupMasterObj in admTaxGroupsMstList)
        //        {
        //            // check AdTaxesGroupMst pk is zero,insert AdTaxesGroupMst to db context
        //            if (adTaxesGroupMasterObj.TXG_PK == 0)
        //            {
        //                // Gets last AdTaxesGroupMst pk
        //                maxAdTaxesGroupMstpk = this.currentEntity.AD_TAX_GROUPS_MST.Max(v => (int?)v.TXG_PK);

        //                // Sets return value as next AdTaxesGroupMst pk
        //                retval = Convert.ToInt16((maxAdTaxesGroupMstpk.HasValue ? maxAdTaxesGroupMstpk.Value + 1 : 1));

        //                // Sets next AdTaxesGroupMst pk
        //                adTaxesGroupMasterObj.TXG_PK = retval;

        //                // Sets AdTaxesGroupMst modified date time as current date time
        //                adTaxesGroupMasterObj.TXG_Mod_On = DateTime.Now;

        //                // Sets AdTaxesGroupMst Created date time as current date time
        //                adTaxesGroupMasterObj.TXG_Crtd_On = DateTime.Now;

        //                // Add new AdTaxesGroupMst to the db context
        //                this.currentEntity.AD_TAX_GROUPS_MST.AddObject(adTaxesGroupMasterObj);
        //            }
        //            else
        //            {
        //                // updating AdTaxesGroupMst
        //                // Get current AdTaxesGroupMst using AdTaxesGroupMst pk and last modified date time,used for concurrency checking
        //                oldAdTaxesGroupMstObj = currentEntity.AD_TAX_GROUPS_MST.SingleOrDefault(v => v.TXG_PK == adTaxesGroupMasterObj.TXG_PK
        //                    && v.TXG_Mod_On == adTaxesGroupMasterObj.TXG_Mod_On);

        //                // If oldAdTaxesGroupMstObj is null then,anyone modified or deleted the record
        //                if (oldAdTaxesGroupMstObj != null)
        //                {
        //                    // Update AdTaxesGroupMst
        //                    oldAdTaxesGroupMstObj.TXG_Code = adTaxesGroupMasterObj.TXG_Code;
        //                    oldAdTaxesGroupMstObj.TXG_Name = adTaxesGroupMasterObj.TXG_Name;
        //                    oldAdTaxesGroupMstObj.TXG_Active = adTaxesGroupMasterObj.TXG_Active;
        //                    oldAdTaxesGroupMstObj.TXG_BizUnit = adTaxesGroupMasterObj.TXG_BizUnit;
        //                    oldAdTaxesGroupMstObj.TXG_Crtd_By = adTaxesGroupMasterObj.TXG_Crtd_By;
        //                    oldAdTaxesGroupMstObj.TXG_Dept = adTaxesGroupMasterObj.TXG_Dept;
        //                    oldAdTaxesGroupMstObj.TXG_Desc = adTaxesGroupMasterObj.TXG_Desc;
        //                    oldAdTaxesGroupMstObj.TXG_Mod_By = adTaxesGroupMasterObj.TXG_Mod_By;
        //                    oldAdTaxesGroupMstObj.TXG_Mod_On = DateTime.Now;
        //                    // Sets return value as AdTaxesGroupMst pk
        //                    retval = adTaxesGroupMasterObj.TXG_PK;
        //                }
        //                else
        //                {
        //                    // throws exception already deleted or modified by other user
        //                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
        //                }
        //            }
        //        }

        //        // return AdTaxesGroupMst pk
        //        return retval;
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        // Handler for concurrency exception
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handler for unknown exceptions
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //}

        /// <summary>
        /// Delete list of AdTaxesGroupMstDtl
        /// </summary>
        /// <param name="camAdTaxesMstList">List of AdTaxesGroupMstDtl</param>
        /// <returns>1 if successfully deleted</returns>
        /// 
        //public short DeleteAdmTaxGroupsMst(List<AD_TAX_GROUPS_MST> admTaxGroupsMstList)
        //{
        //    short txgrpPk = 0;
        //    // Gets or sets old AD_TAX_GROUPS_MST
        //    AD_TAX_GROUPS_MST oldAdTaxesGroupMstObj;
        //    try
        //    {
        //        txgrpPk=admTaxGroupsMstList[0].TXG_PK;
        //        IQueryable<AD_TAX_GROUP_MPG> oldMpgList = this.currentEntity.AD_TAX_GROUP_MPG.Where(mpg => mpg.TGM_Tax_Group == txgrpPk);
        //        foreach (AD_TAX_GROUP_MPG oldMpg in oldMpgList)
        //        {
        //            this.currentEntity.AD_TAX_GROUP_MPG.DeleteObject(oldMpg);
        //        }

        //        IQueryable<AD_TAX_GROUP_AIRPORT_MPG> oldAirportMpgList = this.currentEntity.AD_TAX_GROUP_AIRPORT_MPG.Where(mpg => mpg.TGA_Tax_Group == txgrpPk);
        //        foreach (AD_TAX_GROUP_AIRPORT_MPG oldMpg in oldAirportMpgList)
        //        {
        //            this.currentEntity.AD_TAX_GROUP_AIRPORT_MPG.DeleteObject(oldMpg);
        //        }

        //        // Iterating through AD_TAX_GROUPS_MST for delete
        //        foreach (AD_TAX_GROUPS_MST AdTaxesGroupMstObj in admTaxGroupsMstList)
        //        {
        //            // Get current AD_TAX_GROUPS_MST with pk and last modified date time used for concurrency checking
        //            oldAdTaxesGroupMstObj = currentEntity.AD_TAX_GROUPS_MST.SingleOrDefault(v => v.TXG_PK == AdTaxesGroupMstObj.TXG_PK && v.TXG_Mod_On == AdTaxesGroupMstObj.TXG_Mod_On);

        //            // If oldAdTaxesMstObj is null then,anyone modified or deleted the record
        //            if (oldAdTaxesGroupMstObj != null)
        //            {
        //                // Delete AD_TAX_GROUPS_MST details from db context
        //                this.currentEntity.AD_TAX_GROUPS_MST.DeleteObject(oldAdTaxesGroupMstObj);
        //            }
        //            else
        //            {
        //                // Throws already modified or deleted by other user exception
        //                throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
        //            }
        //        }

        //        // Delete successful
        //        return 1;
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        // Handler for concurrency exception
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        // Exception handler for Delete - Delete Concurrency
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Exception handler for Delete - Delete Concurrency
        //        // Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //}


        /// <summary>
        /// Gets list of AD_TAX_GROUPS_MST after filtering,sorting
        /// </summary>
        /// <param name="camVendorContactDtlObj">AD_TAX_GROUPS_MST object</param>
        /// <param name="utilityObj">Service utility object</param>
        /// <returns>List of AD_TAX_GROUPS_MST</returns>
        //public List<AD_TAX_GROUPS_MST> GetAdmTaxGroupsMst(AD_TAX_GROUPS_MST admTaxGroupsMstObj, ServiceUtility utilityObj = null)
        //{
        //    IQueryable<AD_TAX_GROUPS_MST> qry;
        //    System.Data.Objects.ObjectSet<AD_TAX_GROUPS_MST> loadQuery;
        //    string loadName;

        //    try
        //    {
        //        loadQuery = this.currentEntity.AD_TAX_GROUPS_MST;
        //        //For Loading Entities
        //        if (utilityObj.LoadEntities != null && utilityObj.LoadEntities.Count > 0)
        //        {
        //            foreach (LoadEntities load in utilityObj.LoadEntities)
        //            {
        //                if (load == LoadEntities.TaxesConstCfg)
        //                {
        //                    loadName = LoadEntities.TaxesConstCfg.ToString();
        //                }
        //                else
        //                {
        //                    loadName = DataTableRes.ResourceManager.GetObject(load.ToString()).ToString();
        //                }
        //                loadQuery.Include(loadName);
        //            }
        //        }

        //        //Set page size one if not given
        //        utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;

        //        qry = this.currentEntity.AD_TAX_GROUPS_MST;

        //        qry = FilterEntity(admTaxGroupsMstObj, qry, utilityObj);

        //        #region Sorting
        //        // Apply Paging And Sorting For AD_TAX_GROUPS_MST grid Purpose
        //        // Checking sorting criteria is given

        //        #endregion
        //        //return Tax master details;
        //        return qry.SortRecords<AD_TAX_GROUPS_MST>(utilityObj).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        //Disposing objects
        //    }
         
        //}

        //public ServiceUtility GetAdmTaxGroupsMstCount(AD_TAX_GROUPS_MST admTaxGroupsMstObj, ServiceUtility utilityObj = null)
        //{
        //    IQueryable<AD_TAX_GROUPS_MST> qry;

        //    try
        //    {
        //        qry = this.currentEntity.AD_TAX_GROUPS_MST.AsQueryable();

        //        qry = FilterEntity(admTaxGroupsMstObj, qry, utilityObj);
        //        // Sets total AD_TAX_GROUPS_MST count to service utility
        //        utilityObj.TotalRecords = qry.Count();
        //        // Returns service utility object with total AD_TAX_GROUPS_MST count
        //        return utilityObj;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        //Disposing objects
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
