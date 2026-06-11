using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class InvItemMstManager : IInvItemMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank AdmPackingMaster Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public InvItemMstManager(ERPEntities currentEntity)
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

        public List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj)
        {
            int? invCodePK = null;
            //INV_ITEM_CATEGORY invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG");
            //if (invItemCategoryObj != null)
            //{
            //    invCodePK = invItemCategoryObj.ITC_PK;

            //}

            List<INV_ITEM_MST> INV_ITEM_MST_List_Obj = new List<INV_ITEM_MST>();
            IQueryable<INV_ITEM_MST> INV_ITEM_MST_qry;
            try
            {
                INV_ITEM_MST_qry = (from inv in this.currentEntity.INV_ITEM_MST
                                    where inv.ITM_ACTIVE == InvItemMstObj.ITM_ACTIVE
                                    && inv.ITM_PK == (InvItemMstObj.ITM_PK > 0 ? InvItemMstObj.ITM_PK : inv.ITM_PK)
                                    && inv.ITM_NAME.Contains(InvItemMstObj.ITM_NAME)
                                    && inv.ITM_CODE.Contains(InvItemMstObj.ITM_CODE)
                                    //&& inv.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : inv.ITM_CATEGORY)
                                    && inv.ITM_CATEGORY == (invCodePK.HasValue ? InvItemMstObj.ITM_CATEGORY : inv.ITM_CATEGORY)
                                    select inv);

                if (utilityObj.FilterBy == DataFieldRes.ItemCode)
                    INV_ITEM_MST_qry = INV_ITEM_MST_qry.Where(a => a.ITM_CODE.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ItemName)
                    INV_ITEM_MST_qry = INV_ITEM_MST_qry.Where(a => a.ITM_NAME.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ItemGroup)//Search By Group Name
                    INV_ITEM_MST_qry = INV_ITEM_MST_qry.Where(a => a.INV_ITEM_GROUP_MST.IGM_NAME.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ItemGrade)//Search By ItemGrade
                {
                    INV_ITEM_MST_qry = (from INV in INV_ITEM_MST_qry
                                        join CONFIG in this.currentEntity.ADM_CONFIG_MST on INV.ITM_GRADE equals CONFIG.CFG_VALUE
                                        where CONFIG.CFG_TYPE.Equals("PRODUCT GRADE") && CONFIG.CFG_DATA.Equals(utilityObj.FilterValue)
                                        select INV);
                }

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = INV_ITEM_MST_qry.Count();

                //Apply Paging And Sorting for grid Purpose
                INV_ITEM_MST_List_Obj = INV_ITEM_MST_qry.SortRecords<INV_ITEM_MST>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_MST_List_Obj;
        }

        public List<INV_ITEM_MST> GetInvItemMstAdvSearch(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj, int ItemCategoryPrdn,int IsSBUSpecific,int BizUnit)
        {
            int? invCodePK = null;
            INV_ITEM_CATEGORY invItemCategoryObj = null;
            var conditions = new List<string>();

            if (BizUnit > 0)
            {
                invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG" && itm.ITC_BIZUNIT ==BizUnit);
            }
            else
            {
                invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG");
            }
            if (invItemCategoryObj != null)
            {
                invCodePK = invItemCategoryObj.ITC_PK;
            }
            List<INV_ITEM_MST> INV_ITEM_MST_List_Obj = new List<INV_ITEM_MST>();
            IQueryable<INV_ITEM_MST> INV_ITEM_MST_qry;
            try
            {

                INV_ITEM_MST_qry = (from inv in this.currentEntity.INV_ITEM_MST
                                    join CATEGORY in this.currentEntity.INV_ITEM_CATEGORY on inv.ITM_CATEGORY equals CATEGORY.ITC_PK
                                    where inv.ITM_ACTIVE == InvItemMstObj.ITM_ACTIVE
                                    && inv.ITM_PK == (InvItemMstObj.ITM_PK > 0 ? InvItemMstObj.ITM_PK : inv.ITM_PK)
                                        // && inv.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : inv.ITM_CATEGORY)
                                        && CATEGORY.ITC_VALUE == ItemCategoryPrdn
                                        //&& (InvItemMstObj.ITM_CODE != string.Empty ? inv.ITM_CODE.Contains(InvItemMstObj.ITM_CODE) : true)//Product Code
                                        //&& (InvItemMstObj.ITM_NAME != string.Empty ? inv.ITM_NAME.Contains(InvItemMstObj.ITM_NAME) : true)//Product Name
                                    && (InvItemMstObj.ITM_PK > 0 ? inv.ITM_PK == InvItemMstObj.ITM_PK : true)//Product PK
                                    && (InvItemMstObj.ITM_GROUP > 0 ? inv.ITM_GROUP == InvItemMstObj.ITM_GROUP : true)//Search By Group Name
                                    && (InvItemMstObj.ITM_GRADE > 0 ? inv.ITM_GRADE == InvItemMstObj.ITM_GRADE : true)//Search By Grade
                                    && (InvItemMstObj.ITM_SUB_TYPE > 0 ? inv.ITM_SUB_TYPE == InvItemMstObj.ITM_SUB_TYPE : true)//Search By Sub Category
                                     && (IsSBUSpecific == 1 ? InvItemMstObj.ITM_BIZUNIT == inv.ITM_BIZUNIT : true)//Search By bizunit
                                     && (utilityObj.NeedAdvanceFilter == true ? inv.INV_ITEM_SUB_TYPE_MAP.Count > 0 : inv.INV_ITEM_SUB_TYPE_MAP.Count == 0)//Related prod mapped
                                    select inv);


                //INV_ITEM_MST_qry = (from inv in this.currentEntity.INV_ITEM_MST
                //                    where inv.ITM_ACTIVE == InvItemMstObj.ITM_ACTIVE
                //                    && inv.ITM_PK == (InvItemMstObj.ITM_PK > 0 ? InvItemMstObj.ITM_PK : inv.ITM_PK)
                //                        // && inv.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : inv.ITM_CATEGORY)
                //                        //&& (InvItemMstObj.ITM_CODE != string.Empty ? inv.ITM_CODE.Contains(InvItemMstObj.ITM_CODE) : true)//Product Code
                //                        //&& (InvItemMstObj.ITM_NAME != string.Empty ? inv.ITM_NAME.Contains(InvItemMstObj.ITM_NAME) : true)//Product Name
                //                    && (InvItemMstObj.ITM_PK > 0 ? inv.ITM_PK == InvItemMstObj.ITM_PK : true)//Product PK
                //                    && (InvItemMstObj.ITM_GROUP > 0 ? inv.ITM_GROUP == InvItemMstObj.ITM_GROUP : true)//Search By Group Name
                //                    && (InvItemMstObj.ITM_GRADE > 0 ? inv.ITM_GRADE == InvItemMstObj.ITM_GRADE : true)//Search By Grade
                //                    && (InvItemMstObj.ITM_SUB_TYPE > 0 ? inv.ITM_SUB_TYPE == InvItemMstObj.ITM_SUB_TYPE : true)//Search By Sub Category
                //                    select inv);

                 
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = INV_ITEM_MST_qry.Count();

                //Apply Paging And Sorting for grid Purpose
                INV_ITEM_MST_List_Obj = INV_ITEM_MST_qry.SortRecords<INV_ITEM_MST>(utilityObj).ToList();

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_MST_List_Obj;
        }

        public int SaveInvItemMst(List<INV_ITEM_MST> InvItemMstList,int BizUnit)
        {
            // Gets or sets save status
            int retval;
            int indpk;

            // Gets or sets InvMaterialMst pk
            int? max_INV_ITEM_MST_PK;
            int? max_INV_ITEM_PACK_CMB_PK;
            int? max_INV_ITEM_SPEC_DTL_PK;
            int ProductPk = 0;
            int maxCmbPK = 0;
            // Gets or sets old InvMaterialMst.Used for updating InvMaterialMst and for checking concurrency
            INV_ITEM_MST old_INV_ITEM_MST_Obj;
            INV_ITEM_SPEC_DTL old_INV_ITEM_SPEC_DTL_Obj;


            try
            {
                // Sets save status zero,save failed
                retval = 0;
                INV_ITEM_CATEGORY invItemCategoryObj;
                int invCodePK = 0;
                if (BizUnit > 0)
                {
                     invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG" &&itm.ITC_BIZUNIT==BizUnit);
                }
                else
                {
                     invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG");
                }
                if (invItemCategoryObj != null)
                {
                    invCodePK = invItemCategoryObj.ITC_PK;
                }

                // Iterating through InvMaterialMst list
                foreach (INV_ITEM_MST INV_ITEM_MST_Obj in InvItemMstList)
                {
                    List<INV_ITEM_SPEC_DTL> objItemSpecList = INV_ITEM_MST_Obj.INV_ITEM_SPEC_DTL == null ?
                        new List<INV_ITEM_SPEC_DTL>() : INV_ITEM_MST_Obj.INV_ITEM_SPEC_DTL.ToList();
                    INV_ITEM_MST_Obj.INV_ITEM_SPEC_DTL.Clear(); //For Resolving Issue:"Violation of PRIMARY KEY" error shows while Updating(Scenario:click on item which have no packing spec details, add Spec details and click on save button)

                    List<PRD_PRODUCT_GRADE_MAP> finGrdMapList = INV_ITEM_MST_Obj.PRD_PRODUCT_GRADE_MAP == null ?
                           new List<PRD_PRODUCT_GRADE_MAP>() : INV_ITEM_MST_Obj.PRD_PRODUCT_GRADE_MAP.ToList();
                    INV_ITEM_MST_Obj.PRD_PRODUCT_GRADE_MAP.Clear();

                    List<INV_ITEM_PACK_COMB_DTL> packCombList = INV_ITEM_MST_Obj.INV_ITEM_PACK_COMB_DTL == null ?
                          new List<INV_ITEM_PACK_COMB_DTL>() : INV_ITEM_MST_Obj.INV_ITEM_PACK_COMB_DTL.ToList();
                    INV_ITEM_MST_Obj.INV_ITEM_PACK_COMB_DTL.Clear();

                    // check InvMaterialMst pk is zero,insert InvMaterialMst to db context
                    if (INV_ITEM_MST_Obj.ITM_PK == 0)
                    {
                        // Gets last InvMaterialMst pk
                        max_INV_ITEM_MST_PK = this.currentEntity.INV_ITEM_MST.Max(inv => (int?)inv.ITM_PK);

                        // Sets return value as next InvMaterialMst pk
                        retval = Convert.ToInt32((max_INV_ITEM_MST_PK.HasValue ? max_INV_ITEM_MST_PK.Value + 1 : 1));

                        // Sets next InvMaterialMst pk
                        INV_ITEM_MST_Obj.ITM_PK = retval;

                        //INV_ITEM_MST_Obj.ITM_CATEGORY = invCodePK;

                        // Sets InvMaterialMst created date time as current date time
                        INV_ITEM_MST_Obj.ITM_CRTD_DT = DateTime.Now;

                        // Sets InvMaterialMst modified date time as current date time
                        INV_ITEM_MST_Obj.ITM_MOD_DT = DateTime.Now;

                        INV_ITEM_MST_Obj.ITM_CATEGORY = INV_ITEM_MST_Obj.ITM_CATEGORY;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.INV_ITEM_MST.AddObject(INV_ITEM_MST_Obj);
                    }
                    else
                    {
                        // updating InvMaterialMst
                        // Get current InvMaterialMst using InvMaterialMst pk and last modified date time,used for concurrency checking
                        old_INV_ITEM_MST_Obj = currentEntity.INV_ITEM_MST.SingleOrDefault(inv => inv.ITM_PK == INV_ITEM_MST_Obj.ITM_PK && inv.ITM_MOD_DT == INV_ITEM_MST_Obj.ITM_MOD_DT);

                        // If oldInvMaterialMstObj is null then,anyone modified or deleted the record
                        if (old_INV_ITEM_MST_Obj != null)
                        {
                            // Update InvMaterialMst

                            old_INV_ITEM_MST_Obj.ITM_PACK_SPEC = INV_ITEM_MST_Obj.ITM_PACK_SPEC;
                            old_INV_ITEM_MST_Obj.ITM_CODE = INV_ITEM_MST_Obj.ITM_CODE;
                            old_INV_ITEM_MST_Obj.ITM_NAME = INV_ITEM_MST_Obj.ITM_NAME;
                            old_INV_ITEM_MST_Obj.ITM_DESC = INV_ITEM_MST_Obj.ITM_DESC;
                            old_INV_ITEM_MST_Obj.ITM_REF1 = INV_ITEM_MST_Obj.ITM_REF1;
                            old_INV_ITEM_MST_Obj.ITM_REF2 = INV_ITEM_MST_Obj.ITM_REF2;
                            old_INV_ITEM_MST_Obj.ITM_INTER_STATE = INV_ITEM_MST_Obj.ITM_INTER_STATE;
                            old_INV_ITEM_MST_Obj.ITM_INTRA_STATE = INV_ITEM_MST_Obj.ITM_INTRA_STATE;
                            old_INV_ITEM_MST_Obj.ITM_OTHERS = INV_ITEM_MST_Obj.ITM_OTHERS;
                            old_INV_ITEM_MST_Obj.ITM_EXPORT = INV_ITEM_MST_Obj.ITM_EXPORT;

                            //old_INV_ITEM_MST_Obj.ITM_CATEGORY = invCodePK;
                            old_INV_ITEM_MST_Obj.ITM_TYPE = INV_ITEM_MST_Obj.ITM_TYPE;
                            old_INV_ITEM_MST_Obj.ITM_GROUP = INV_ITEM_MST_Obj.ITM_GROUP;
                            old_INV_ITEM_MST_Obj.ITM_WEIGHT = INV_ITEM_MST_Obj.ITM_WEIGHT;
                            old_INV_ITEM_MST_Obj.ITM_MIN_WEIGHT = INV_ITEM_MST_Obj.ITM_MIN_WEIGHT;
                            old_INV_ITEM_MST_Obj.ITM_MAX_WEIGHT = INV_ITEM_MST_Obj.ITM_MAX_WEIGHT;
                            old_INV_ITEM_MST_Obj.ITM_UOM = INV_ITEM_MST_Obj.ITM_UOM;
                            old_INV_ITEM_MST_Obj.ITM_MIN_STK = INV_ITEM_MST_Obj.ITM_MIN_STK;
                            old_INV_ITEM_MST_Obj.ITM_ROL_STK = INV_ITEM_MST_Obj.ITM_ROL_STK;
                            old_INV_ITEM_MST_Obj.ITM_MAX_STK = INV_ITEM_MST_Obj.ITM_MAX_STK;
                            old_INV_ITEM_MST_Obj.ITM_ACTIVE = INV_ITEM_MST_Obj.ITM_ACTIVE;
                            old_INV_ITEM_MST_Obj.ITM_BIZUNIT = INV_ITEM_MST_Obj.ITM_BIZUNIT;
                            old_INV_ITEM_MST_Obj.ITM_CRTD_BY = INV_ITEM_MST_Obj.ITM_CRTD_BY;
                            old_INV_ITEM_MST_Obj.ITM_CRTD_DT = INV_ITEM_MST_Obj.ITM_CRTD_DT;
                            old_INV_ITEM_MST_Obj.ITM_MOD_BY = INV_ITEM_MST_Obj.ITM_MOD_BY;
                            old_INV_ITEM_MST_Obj.ITM_MOD_DT = DateTime.Now;
                            old_INV_ITEM_MST_Obj.ITM_REF3 = INV_ITEM_MST_Obj.ITM_REF3;
                            old_INV_ITEM_MST_Obj.ITM_REF4 = INV_ITEM_MST_Obj.ITM_REF4;
                            old_INV_ITEM_MST_Obj.ITM_NO_OF_COMP = INV_ITEM_MST_Obj.ITM_NO_OF_COMP;
                            old_INV_ITEM_MST_Obj.ITM_GRADE = INV_ITEM_MST_Obj.ITM_GRADE;
                            old_INV_ITEM_MST_Obj.ITM_SUB_TYPE = INV_ITEM_MST_Obj.ITM_SUB_TYPE;
                            old_INV_ITEM_MST_Obj.ITM_PACK_COMB_BSD_ON = INV_ITEM_MST_Obj.ITM_PACK_COMB_BSD_ON;
                            old_INV_ITEM_MST_Obj.ITM_LINKED_ITEM = INV_ITEM_MST_Obj.ITM_LINKED_ITEM;
                            old_INV_ITEM_MST_Obj.ITM_PLAN_GROUP = INV_ITEM_MST_Obj.ITM_PLAN_GROUP;
                            old_INV_ITEM_MST_Obj.ITM_CATEGORY = INV_ITEM_MST_Obj.ITM_CATEGORY;
                            old_INV_ITEM_MST_Obj.ITM_GST_CLASS = INV_ITEM_MST_Obj.ITM_GST_CLASS;
                            // Sets return value as InvMaterialMst pk
                            retval = INV_ITEM_MST_Obj.ITM_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }

                    objItemSpecList.ForEach(dtl => dtl.ISD_ITEM = retval);
                    foreach (INV_ITEM_SPEC_DTL INV_ITEM_SPEC_DTL_Obj in objItemSpecList)                   
                    {
                        if (INV_ITEM_SPEC_DTL_Obj.ISD_PK == 0)
                        {
                            max_INV_ITEM_SPEC_DTL_PK = this.currentEntity.INV_ITEM_SPEC_DTL.Max(ind => (int?)ind.ISD_PK);
                            indpk = Convert.ToInt32((max_INV_ITEM_SPEC_DTL_PK.HasValue ? max_INV_ITEM_SPEC_DTL_PK.Value + 1 : 1));
                            INV_ITEM_SPEC_DTL_Obj.ISD_PK = indpk;
                            INV_ITEM_SPEC_DTL_Obj.ISD_MOD_DT = DateTime.Now;

                            this.currentEntity.INV_ITEM_SPEC_DTL.AddObject(INV_ITEM_SPEC_DTL_Obj);
                        }
                        else
                        {
                            old_INV_ITEM_SPEC_DTL_Obj = currentEntity.INV_ITEM_SPEC_DTL.SingleOrDefault(ind => ind.ISD_PK == INV_ITEM_SPEC_DTL_Obj.ISD_PK);

                            if (old_INV_ITEM_SPEC_DTL_Obj != null)
                            {
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ITEM = INV_ITEM_SPEC_DTL_Obj.ISD_ITEM;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_NATURE = INV_ITEM_SPEC_DTL_Obj.ISD_NATURE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_THICKNESS = INV_ITEM_SPEC_DTL_Obj.ISD_THICKNESS;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_PROCESS = INV_ITEM_SPEC_DTL_Obj.ISD_PROCESS;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_SURFACE = INV_ITEM_SPEC_DTL_Obj.ISD_SURFACE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_COLOUR = INV_ITEM_SPEC_DTL_Obj.ISD_COLOUR;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_GRADE = INV_ITEM_SPEC_DTL_Obj.ISD_GRADE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_SIZE = INV_ITEM_SPEC_DTL_Obj.ISD_SIZE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_LENGTH = INV_ITEM_SPEC_DTL_Obj.ISD_LENGTH;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_CHLORINATION = INV_ITEM_SPEC_DTL_Obj.ISD_CHLORINATION;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_STERILE = INV_ITEM_SPEC_DTL_Obj.ISD_STERILE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_CLEAN_ROOM = INV_ITEM_SPEC_DTL_Obj.ISD_CLEAN_ROOM;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC01 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC01;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC02 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC02;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC03 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC03;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC04 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC04;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC05 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC05;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC06 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC06;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC07 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC07;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC08 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC08;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC09 = INV_ITEM_SPEC_DTL_Obj.ISD_ADNL_SPEC09;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_REMARKS = INV_ITEM_SPEC_DTL_Obj.ISD_REMARKS;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_ACTIVE = INV_ITEM_SPEC_DTL_Obj.ISD_ACTIVE;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_MOD_BY = INV_ITEM_SPEC_DTL_Obj.ISD_MOD_BY;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_MOD_DT = DateTime.Now;
                                old_INV_ITEM_SPEC_DTL_Obj.ISD_AGRADE_PER = INV_ITEM_SPEC_DTL_Obj.ISD_AGRADE_PER;
                            }
                            else
                            {
                                // throws exception already deleted or modified by other user
                                throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                            }
                        }
                    }
                    //

                    List<PRD_PRODUCT_GRADE_MAP> old_INV_ITEM_GRADES_obj = (from old in this.currentEntity.PRD_PRODUCT_GRADE_MAP
                                                                           where old.PGM_PRODUCT == retval
                                                                           select old).ToList();

                    if (old_INV_ITEM_GRADES_obj != null && old_INV_ITEM_GRADES_obj.Count > 0)
                    {
                        foreach (PRD_PRODUCT_GRADE_MAP PRD_GRADE_Obj in old_INV_ITEM_GRADES_obj)
                        {
                            this.currentEntity.PRD_PRODUCT_GRADE_MAP.DeleteObject(PRD_GRADE_Obj);
                        }
                    }

                   
                    finGrdMapList.ForEach(dtl => dtl.PGM_PRODUCT = retval);

                    foreach (PRD_PRODUCT_GRADE_MAP PROD_GRADE_Obj in finGrdMapList)
                    {
                        this.currentEntity.PRD_PRODUCT_GRADE_MAP.AddObject(PROD_GRADE_Obj);
                    }

                    //Packing Combinations
                    List<INV_ITEM_PACK_COMB_DTL> old_INV_ITEM_PACK_COMB_obj = (from old in this.currentEntity.INV_ITEM_PACK_COMB_DTL
                                                                           where old.IPC_ITEM == retval
                                                                           select old).ToList();

                    if (old_INV_ITEM_PACK_COMB_obj != null && old_INV_ITEM_PACK_COMB_obj.Count > 0)
                    {
                        foreach (INV_ITEM_PACK_COMB_DTL PACK_COMB_Obj in old_INV_ITEM_PACK_COMB_obj)
                        {
                            this.currentEntity.INV_ITEM_PACK_COMB_DTL.DeleteObject(PACK_COMB_Obj);
                        }
                    }

                  
                    packCombList.ForEach(dtl => dtl.IPC_ITEM = retval);
                    // Gets last INV_ITEM_PACK_COMB_DTL pk
                    max_INV_ITEM_PACK_CMB_PK = this.currentEntity.INV_ITEM_PACK_COMB_DTL.Max(inv => (int?)inv.IPC_PK);
                    maxCmbPK = Convert.ToInt32((max_INV_ITEM_PACK_CMB_PK.HasValue ? max_INV_ITEM_PACK_CMB_PK.Value + 1 : 1));

                    foreach (INV_ITEM_PACK_COMB_DTL PACK_COMB_Obj in packCombList)
                    {
                        PACK_COMB_Obj.IPC_PK = maxCmbPK;
                        this.currentEntity.INV_ITEM_PACK_COMB_DTL.AddObject(PACK_COMB_Obj);
                        maxCmbPK++;
                    }
                }

                // return InvMaterialMst pk
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int DeleteInvItemMst(List<INV_ITEM_MST> InvItemMstList)
        {
            int retval = 1;
            INV_ITEM_MST Old_INV_ITEM_MST_Obj;

            try
            {
                foreach (INV_ITEM_MST INV_ITEM_MST_Obj in InvItemMstList)
                {
                    Old_INV_ITEM_MST_Obj = currentEntity.INV_ITEM_MST.SingleOrDefault(sah => sah.ITM_PK == INV_ITEM_MST_Obj.ITM_PK && sah.ITM_MOD_DT == INV_ITEM_MST_Obj.ITM_MOD_DT);
                    if (Old_INV_ITEM_MST_Obj != null)
                    {
                        List<INV_ITEM_SPEC_DTL> invItemSpecDtlList = Old_INV_ITEM_MST_Obj.INV_ITEM_SPEC_DTL.ToList();

                        foreach (INV_ITEM_SPEC_DTL INV_ITEM_SPEC_DTL_Obj in invItemSpecDtlList)
                        {
                            if (INV_ITEM_SPEC_DTL_Obj != null)
                                this.currentEntity.INV_ITEM_SPEC_DTL.DeleteObject(INV_ITEM_SPEC_DTL_Obj);
                        }

                        #region Delete Product Grade Mappings
                        List<PRD_PRODUCT_GRADE_MAP> invItemGradeMapList = Old_INV_ITEM_MST_Obj.PRD_PRODUCT_GRADE_MAP.ToList();
                        if (invItemGradeMapList != null && invItemGradeMapList.Count > 0)
                        {
                            foreach (PRD_PRODUCT_GRADE_MAP PRD_PRODUCT_GRADE_MAP_Obj in invItemGradeMapList)
                            {
                                if (PRD_PRODUCT_GRADE_MAP_Obj != null)
                                    this.currentEntity.PRD_PRODUCT_GRADE_MAP.DeleteObject(PRD_PRODUCT_GRADE_MAP_Obj);
                            }
                        }
                        #endregion

                        this.currentEntity.INV_ITEM_MST.DeleteObject(Old_INV_ITEM_MST_Obj);
                        retval = 1;
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }

                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<INV_UOM_MST> GetUomMst(INV_UOM_MST InvUomMstObj, ServiceUtility utilityObj)
        {
            List<INV_UOM_MST> INV_UOM_MST_Obj = new List<INV_UOM_MST>();

            try
            {
                INV_UOM_MST_Obj = (from uom in this.currentEntity.INV_UOM_MST
                                   where uom.UOM_PK == (InvUomMstObj.UOM_PK > 0 ? InvUomMstObj.UOM_PK : uom.UOM_PK)
                                    && uom.UOM_ACTIVE == InvUomMstObj.UOM_ACTIVE

                                   select uom).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return INV_UOM_MST_Obj;
        }

        public List<INV_UOM_MST> GetUomMstAutoCompleteList(INV_UOM_MST invUOMObj, ServiceUtility utilityObj)
        {
            List<INV_UOM_MST> invUOMList = new List<INV_UOM_MST>();
            try
            {
                if (utilityObj.FilterBy.ToLower() == DataFieldRes.UOMCode.ToLower())
                {
                    invUOMList = (from inv in this.currentEntity.INV_UOM_MST
                                  where (inv.UOM_ACTIVE == (invUOMObj.UOM_ACTIVE > 1 ? inv.UOM_ACTIVE : invUOMObj.UOM_ACTIVE)
                                                   && inv.UOM_CODE.StartsWith(utilityObj.FilterValue)
                                                   && inv.UOM_TYPE == (invUOMObj.UOM_TYPE > 0 ? invUOMObj.UOM_TYPE : inv.UOM_TYPE))
                                  select inv).ToList();
                }
                else if (utilityObj.FilterBy.ToLower() == DataFieldRes.UOMName.ToLower())
                {
                    invUOMList = (from inv in this.currentEntity.INV_UOM_MST
                                  where (inv.UOM_ACTIVE == (invUOMObj.UOM_ACTIVE > 1 ? inv.UOM_ACTIVE : invUOMObj.UOM_ACTIVE)
                                                   && inv.UOM_NAME.StartsWith(utilityObj.FilterValue)
                                                   && inv.UOM_TYPE == (invUOMObj.UOM_TYPE > 0 ? invUOMObj.UOM_TYPE : inv.UOM_TYPE))
                                  select inv).ToList();
                }

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return invUOMList;
        }

        public List<SPADM_CONST_GRP_GET_KV_Result> GetControlsList(int? CngPk, int? CngGrpType, int? CgtVal, int? BizUnit, byte? Active, int? CngParent = null, string splCond = null)
        {
            try
            {
                List<SPADM_CONST_GRP_GET_KV_Result> resultFieldsList;
                resultFieldsList = this.currentEntity.SPADM_CONST_GRP_GET_KV(CngPk, CngGrpType, CgtVal, BizUnit, Active, CngParent, splCond).ToList();
                return resultFieldsList;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<INV_ITEM_GROUP_MST> GetProductGroups(INV_ITEM_GROUP_MST InvItemGroupMstObj)
        {
            List<INV_ITEM_GROUP_MST> INV_ITEM_GROUP_MST_Obj = new List<INV_ITEM_GROUP_MST>();

            try
            {
                if (InvItemGroupMstObj.IGM_GROUP_TYPE == 3)
                {
                    INV_ITEM_GROUP_MST_Obj = (from grp in this.currentEntity.INV_ITEM_GROUP_MST
                                              where grp.IGM_PK == (InvItemGroupMstObj.IGM_PK > 0 ? InvItemGroupMstObj.IGM_PK : grp.IGM_PK)
                                        && grp.IGM_ACTIVE == InvItemGroupMstObj.IGM_ACTIVE
                                        && grp.IGM_GROUP_TYPE == 3
                                              select grp).ToList();
                }
                else
                {
                    INV_ITEM_GROUP_MST_Obj = (from grp in this.currentEntity.INV_ITEM_GROUP_MST
                                              where grp.IGM_PK == (InvItemGroupMstObj.IGM_PK > 0 ? InvItemGroupMstObj.IGM_PK : grp.IGM_PK)
                                        && grp.IGM_ACTIVE == InvItemGroupMstObj.IGM_ACTIVE
                                         && grp.IGM_GROUP_TYPE == 1
                                              select grp).ToList();
                }
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return INV_ITEM_GROUP_MST_Obj;
        }
        #endregion

        /// <summary>
        /// Function Used For Item Code Duplication Checking
        /// </summary>
        /// <param name="invItemMstObj"></param>
        /// <returns></returns>
        public bool IsItemCodeExist(INV_ITEM_MST invItemMstObj,int IsSBUSpecific)
        {
            try
            {
                bool IsExist = false;

                var resultFieldsObj = (from grp in this.currentEntity.INV_ITEM_MST
                                       where grp.ITM_PK != invItemMstObj.ITM_PK && grp.ITM_CODE == invItemMstObj.ITM_CODE 
                                       && (IsSBUSpecific == 1 ? grp.ITM_BIZUNIT==invItemMstObj.ITM_BIZUNIT : true)
                                       select grp).ToList();
                if (resultFieldsObj != null && resultFieldsObj.Count > 0)
                    IsExist = true;
                return IsExist;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<INV_ITEM_REL_MAP> GetRelatedItems(int ItemPk)
        {
            List<INV_ITEM_REL_MAP> INV_ITEM_REL_MAP_Obj = new List<INV_ITEM_REL_MAP>();
            try
            {
                INV_ITEM_REL_MAP_Obj = (from grp in this.currentEntity.INV_ITEM_REL_MAP
                                        where grp.IMR_ITEM == ItemPk
                                        select grp).ToList();
                return INV_ITEM_REL_MAP_Obj;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<INV_ITEM_SUB_TYPE_MAP> GetSubTypeItems(int ItemPk)
        {
            List<INV_ITEM_SUB_TYPE_MAP> INV_ITEM_SUB_TYPE_MAP_Obj = new List<INV_ITEM_SUB_TYPE_MAP>();
            try
            {
                INV_ITEM_SUB_TYPE_MAP_Obj = (from grp in this.currentEntity.INV_ITEM_SUB_TYPE_MAP
                                             where (grp.ISM_ITEM == ItemPk)
                                             select grp).ToList();
                //var RelatedLst = (from itm in this.currentEntity.INV_ITEM_SUB_TYPE_MAP
                //                  where itm.ISM_ITEM == ItemPk
                //                  select new INV_ITEM_SUB_TYPE_MAP
                //                  {
                //                      ISM_PK = itm.ISM_PK,
                //                      ISM_ITEM = itm.ISM_ITEM,
                //                      ISM_REL_ITEM = itm.ISM_REL_ITEM,
                //                      ISM_ITEM_SUB_TYPE = itm.ISM_ITEM_SUB_TYPE,
                //                      ISM_CRTD_BY = itm.ISM_CRTD_BY,
                //                      ISM_CRTD_DT = itm.ISM_CRTD_DT,
                //                      ISM_MOD_BY = itm.ISM_MOD_BY,
                //                      ISM_MOD_DT = itm.ISM_MOD_DT
                //                  });
                //var MappedLst = (from rtm in this.currentEntity.INV_ITEM_SUB_TYPE_MAP
                //                 where rtm.ISM_REL_ITEM == ItemPk
                //                 select new INV_ITEM_SUB_TYPE_MAP
                //                 {
                //                     ISM_PK = rtm.ISM_PK,
                //                     ISM_ITEM = rtm.ISM_REL_ITEM,
                //                     ISM_REL_ITEM = rtm.ISM_ITEM,
                //                     ISM_ITEM_SUB_TYPE = rtm.ISM_ITEM_SUB_TYPE,
                //                     ISM_CRTD_BY = rtm.ISM_CRTD_BY,
                //                     ISM_CRTD_DT = rtm.ISM_CRTD_DT,
                //                     ISM_MOD_BY = rtm.ISM_MOD_BY,
                //                     ISM_MOD_DT = rtm.ISM_MOD_DT
                //                 });

                //var FinalLst = RelatedLst.Union(MappedLst);
                ////var fst = (from c in FinalLst
                ////           select new INV_ITEM_SUB_TYPE_MAP
                ////               {
                ////                   ISM_PK = c.ISM_PK,
                ////                   ISM_ITEM = c.ISM_ITEM,
                ////                   ISM_REL_ITEM = c.ISM_REL_ITEM,
                ////                   ISM_ITEM_SUB_TYPE = c.ISM_ITEM_SUB_TYPE,
                ////                   ISM_CRTD_BY = c.ISM_CRTD_BY,
                ////                   ISM_CRTD_DT = c.ISM_CRTD_DT,
                ////                   ISM_MOD_BY = c.ISM_MOD_BY,
                ////                   ISM_MOD_DT = c.ISM_MOD_DT
                ////               }).ToList();

                //INV_ITEM_SUB_TYPE_MAP_Obj = FinalLst.AsEnumerable().ToList();

                ////INV_ITEM_SUB_TYPE_MAP_Obj = FinalLst.Select(grp => new INV_ITEM_SUB_TYPE_MAP { grp.ISM_PK,
                ////                     grp.ISM_ITEM,
                ////                     grp.ISM_REL_ITEM,
                ////                     grp.ISM_ITEM_SUB_TYPE,
                ////                     grp.ISM_CRTD_BY,
                ////                     grp.ISM_CRTD_DT,
                ////                     grp.ISM_MOD_BY,
                ////                     grp.ISM_MOD_DT}).ToList();

                return INV_ITEM_SUB_TYPE_MAP_Obj;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public List<INV_ITEM_PACK_ITEM_MAP> GetPackMatItems(int ItemPk)
        {
            List<INV_ITEM_PACK_ITEM_MAP> INV_ITEM_PACK_ITEM_MAP_Obj = new List<INV_ITEM_PACK_ITEM_MAP>();
            try
            {
                INV_ITEM_PACK_ITEM_MAP_Obj = (from grp in this.currentEntity.INV_ITEM_PACK_ITEM_MAP
                                             where (grp.IMP_ITEM == ItemPk)
                                             select grp).ToList();

                return INV_ITEM_PACK_ITEM_MAP_Obj;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public INV_ITEM_MST GetInvItemMst(int productPk)
        {
            INV_ITEM_MST INV_ITEM_MST_Obj = new INV_ITEM_MST();
            try
            {
                INV_ITEM_MST_Obj = this.currentEntity.INV_ITEM_MST.SingleOrDefault(r => r.ITM_PK == productPk);
                return INV_ITEM_MST_Obj;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int SaveInvRelatedItemMap(List<INV_ITEM_REL_MAP> invRelItemMapList, int ParentItemPk)
        {
            // Gets or sets save status
            int retval;
            // Gets or sets InvMaterialMst pk
            int? max_INV_ITEM_MST_PK;
            // Gets or sets old InvMaterialMst.Used for updating InvMaterialMst and for checking concurrency
            INV_ITEM_REL_MAP old_IINV_ITEM_REL_MAP_Obj;

            try
            {
                // Sets save status zero,save failed
                retval = 0;
                // Iterating through InvMaterialMst list

                if (invRelItemMapList != null && invRelItemMapList.Count > 0)
                {
                    List<int> pks = (from old1 in invRelItemMapList
                                     select old1.IMR_PK).ToList();

                    List<INV_ITEM_REL_MAP> objItemRellst = this.currentEntity.INV_ITEM_REL_MAP.Where(pk => pk.IMR_ITEM == ParentItemPk && !pks.Contains(pk.IMR_PK)).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_REL_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_REL_MAP objlst = this.currentEntity.INV_ITEM_REL_MAP.SingleOrDefault(pk => pk.IMR_PK == objDtl.IMR_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                }
                else
                {
                    List<INV_ITEM_REL_MAP> objItemRellst = this.currentEntity.INV_ITEM_REL_MAP.Where(pk => pk.IMR_ITEM == ParentItemPk).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_REL_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_REL_MAP objlst = this.currentEntity.INV_ITEM_REL_MAP.SingleOrDefault(pk => pk.IMR_PK == objDtl.IMR_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                    retval = ParentItemPk;
                }

                max_INV_ITEM_MST_PK = this.currentEntity.INV_ITEM_REL_MAP.Max(inv => (int?)inv.IMR_PK);
                foreach (INV_ITEM_REL_MAP INV_ITEM_MST_Obj in invRelItemMapList)
                {
                    // check InvMaterialMst pk is zero,insert InvMaterialMst to db context
                    if (INV_ITEM_MST_Obj.IMR_PK == 0)
                    {
                        // Sets return value as next InvMaterialMst pk
                        retval = Convert.ToInt32((max_INV_ITEM_MST_PK.HasValue ? max_INV_ITEM_MST_PK.Value + 1 : 1));
                        max_INV_ITEM_MST_PK = retval;
                        // Sets next InvMaterialMst pk
                        INV_ITEM_MST_Obj.IMR_PK = retval;

                        // Sets InvMaterialMst created date time as current date time
                        INV_ITEM_MST_Obj.IMR_CRTD_DT = DateTime.Now;

                        // Sets InvMaterialMst modified date time as current date time
                        INV_ITEM_MST_Obj.IMR_MOD_DT = DateTime.Now;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.INV_ITEM_REL_MAP.AddObject(INV_ITEM_MST_Obj);
                    }
                    else
                    {
                        old_IINV_ITEM_REL_MAP_Obj = currentEntity.INV_ITEM_REL_MAP.SingleOrDefault(inv => inv.IMR_PK == INV_ITEM_MST_Obj.IMR_PK);
                        // If oldInvMaterialMstObj is null then,anyone modified or deleted the record
                        if (old_IINV_ITEM_REL_MAP_Obj != null)
                        {
                            // Update InvMaterialMst
                            old_IINV_ITEM_REL_MAP_Obj.IMR_ITEM = INV_ITEM_MST_Obj.IMR_ITEM;
                            old_IINV_ITEM_REL_MAP_Obj.IMR_ITEM_GRADE = INV_ITEM_MST_Obj.IMR_ITEM_GRADE;
                            old_IINV_ITEM_REL_MAP_Obj.IMR_MOD_BY = INV_ITEM_MST_Obj.IMR_MOD_BY;
                            old_IINV_ITEM_REL_MAP_Obj.IMR_MOD_DT = INV_ITEM_MST_Obj.IMR_MOD_DT;
                            old_IINV_ITEM_REL_MAP_Obj.IMR_REL_ITEM = INV_ITEM_MST_Obj.IMR_REL_ITEM;
                            // Sets return value as InvMaterialMst pk
                            retval = INV_ITEM_MST_Obj.IMR_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }

                // return InvMaterialMst pk
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int SaveInvSubTypeItemMap(List<INV_ITEM_SUB_TYPE_MAP> invRelItemMapList, int ParentItemPk)
        {
            // Gets or sets save status
            int retval;
            // Gets or sets InvMaterialMst pk
            int? max_INV_ITEM_MST_PK;
            // Gets or sets old InvMaterialMst.Used for updating InvMaterialMst and for checking concurrency
            INV_ITEM_SUB_TYPE_MAP old_IINV_ITEM_REL_MAP_Obj;

            try
            {
                // Sets save status zero,save failed
                retval = 0;
                // Iterating through InvMaterialMst list

                if (invRelItemMapList != null && invRelItemMapList.Count > 0)
                {
                    List<int> pks = (from old1 in invRelItemMapList
                                     select old1.ISM_PK).ToList();

                    List<INV_ITEM_SUB_TYPE_MAP> objItemRellst = this.currentEntity.INV_ITEM_SUB_TYPE_MAP.Where(pk => pk.ISM_ITEM == ParentItemPk && !pks.Contains(pk.ISM_PK)).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_SUB_TYPE_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_SUB_TYPE_MAP objlst = this.currentEntity.INV_ITEM_SUB_TYPE_MAP.SingleOrDefault(pk => pk.ISM_PK == objDtl.ISM_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                }
                else
                {
                    List<INV_ITEM_SUB_TYPE_MAP> objItemRellst = this.currentEntity.INV_ITEM_SUB_TYPE_MAP.Where(pk => pk.ISM_ITEM == ParentItemPk).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_SUB_TYPE_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_SUB_TYPE_MAP objlst = this.currentEntity.INV_ITEM_SUB_TYPE_MAP.SingleOrDefault(pk => pk.ISM_PK == objDtl.ISM_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                    retval = ParentItemPk;
                }

                max_INV_ITEM_MST_PK = this.currentEntity.INV_ITEM_SUB_TYPE_MAP.Max(inv => (int?)inv.ISM_PK);
                foreach (INV_ITEM_SUB_TYPE_MAP INV_ITEM_MST_Obj in invRelItemMapList)
                {
                    // check InvMaterialMst pk is zero,insert InvMaterialMst to db context
                    if (INV_ITEM_MST_Obj.ISM_PK == 0)
                    {
                        // Sets return value as next InvMaterialMst pk
                        retval = Convert.ToInt32((max_INV_ITEM_MST_PK.HasValue ? max_INV_ITEM_MST_PK.Value + 1 : 1));
                        max_INV_ITEM_MST_PK = retval;
                        // Sets next InvMaterialMst pk
                        INV_ITEM_MST_Obj.ISM_PK = retval;

                        // Sets InvMaterialMst created date time as current date time
                        INV_ITEM_MST_Obj.ISM_CRTD_DT = DateTime.Now;

                        // Sets InvMaterialMst modified date time as current date time
                        INV_ITEM_MST_Obj.ISM_MOD_DT = DateTime.Now;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.INV_ITEM_SUB_TYPE_MAP.AddObject(INV_ITEM_MST_Obj);
                    }
                    else
                    {
                        old_IINV_ITEM_REL_MAP_Obj = currentEntity.INV_ITEM_SUB_TYPE_MAP.SingleOrDefault(inv => inv.ISM_PK == INV_ITEM_MST_Obj.ISM_PK);
                        // If oldInvMaterialMstObj is null then,anyone modified or deleted the record
                        if (old_IINV_ITEM_REL_MAP_Obj != null)
                        {
                            // Update InvMaterialMst
                            old_IINV_ITEM_REL_MAP_Obj.ISM_ITEM = INV_ITEM_MST_Obj.ISM_ITEM;
                            old_IINV_ITEM_REL_MAP_Obj.ISM_ITEM_SUB_TYPE = INV_ITEM_MST_Obj.ISM_ITEM_SUB_TYPE;
                            old_IINV_ITEM_REL_MAP_Obj.ISM_MOD_BY = INV_ITEM_MST_Obj.ISM_MOD_BY;
                            old_IINV_ITEM_REL_MAP_Obj.ISM_MOD_DT = INV_ITEM_MST_Obj.ISM_MOD_DT;
                            old_IINV_ITEM_REL_MAP_Obj.ISM_REL_ITEM = INV_ITEM_MST_Obj.ISM_REL_ITEM;
                            // Sets return value as InvMaterialMst pk
                            retval = INV_ITEM_MST_Obj.ISM_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }

                // return InvMaterialMst pk
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int SaveInvPackMatItemMap(List<INV_ITEM_PACK_ITEM_MAP> invRelItemMapList, int ParentItemPk)
        {
            // Gets or sets save status
            int retval;
            // Gets or sets InvMaterialMst pk
            int? max_INV_ITEM_MST_PK;
            // Gets or sets old InvMaterialMst.Used for updating InvMaterialMst and for checking concurrency
            INV_ITEM_PACK_ITEM_MAP old_IINV_ITEM_REL_MAP_Obj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;
                // Iterating through InvMaterialMst list
                if (invRelItemMapList != null && invRelItemMapList.Count > 0)
                {
                    List<int> pks = (from old1 in invRelItemMapList
                                     select old1.IMP_PK).ToList();

                    List<INV_ITEM_PACK_ITEM_MAP> objItemRellst = this.currentEntity.INV_ITEM_PACK_ITEM_MAP.Where(pk => pk.IMP_ITEM == ParentItemPk && !pks.Contains(pk.IMP_PK)).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_PACK_ITEM_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_PACK_ITEM_MAP objlst = this.currentEntity.INV_ITEM_PACK_ITEM_MAP.SingleOrDefault(pk => pk.IMP_PK == objDtl.IMP_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                }
                else
                {
                    List<INV_ITEM_PACK_ITEM_MAP> objItemRellst = this.currentEntity.INV_ITEM_PACK_ITEM_MAP.Where(pk => pk.IMP_ITEM == ParentItemPk).ToList();
                    if (objItemRellst != null && objItemRellst.Count > 0)
                    {
                        foreach (INV_ITEM_PACK_ITEM_MAP objDtl in objItemRellst)
                        {
                            INV_ITEM_PACK_ITEM_MAP objlst = this.currentEntity.INV_ITEM_PACK_ITEM_MAP.SingleOrDefault(pk => pk.IMP_PK == objDtl.IMP_PK);
                            this.currentEntity.DeleteObject(objlst);
                        }
                    }
                    retval = ParentItemPk;
                }

                max_INV_ITEM_MST_PK = this.currentEntity.INV_ITEM_PACK_ITEM_MAP.Max(inv => (int?)inv.IMP_PK);
                foreach (INV_ITEM_PACK_ITEM_MAP INV_ITEM_MST_Obj in invRelItemMapList)
                {
                    // check InvMaterialMst pk is zero,insert InvMaterialMst to db context
                    if (INV_ITEM_MST_Obj.IMP_PK == 0)
                    {
                        // Sets return value as next InvMaterialMst pk
                        retval = Convert.ToInt32((max_INV_ITEM_MST_PK.HasValue ? max_INV_ITEM_MST_PK.Value + 1 : 1));
                        max_INV_ITEM_MST_PK = retval;
                        // Sets next InvMaterialMst pk
                        INV_ITEM_MST_Obj.IMP_PK = retval;

                        // Sets InvMaterialMst created date time as current date time
                        INV_ITEM_MST_Obj.IMP_CRTD_DT = DateTime.Now;

                        // Sets InvMaterialMst modified date time as current date time
                        INV_ITEM_MST_Obj.IMP_MOD_DT = DateTime.Now;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.INV_ITEM_PACK_ITEM_MAP.AddObject(INV_ITEM_MST_Obj);
                    }
                    else
                    {
                        old_IINV_ITEM_REL_MAP_Obj = currentEntity.INV_ITEM_PACK_ITEM_MAP.SingleOrDefault(inv => inv.IMP_PK == INV_ITEM_MST_Obj.IMP_PK);
                        // If oldInvMaterialMstObj is null then,anyone modified or deleted the record
                        if (old_IINV_ITEM_REL_MAP_Obj != null)
                        {
                            // Update InvMaterialMst
                            old_IINV_ITEM_REL_MAP_Obj.IMP_ITEM = INV_ITEM_MST_Obj.IMP_ITEM;
                            old_IINV_ITEM_REL_MAP_Obj.IMP_QUANITY = INV_ITEM_MST_Obj.IMP_QUANITY;
                            old_IINV_ITEM_REL_MAP_Obj.IMP_MOD_BY = INV_ITEM_MST_Obj.IMP_MOD_BY;
                            old_IINV_ITEM_REL_MAP_Obj.IMP_MOD_DT = INV_ITEM_MST_Obj.IMP_MOD_DT;
                            old_IINV_ITEM_REL_MAP_Obj.IMP_PACK_ITEM = INV_ITEM_MST_Obj.IMP_PACK_ITEM;
                            // Sets return value as InvMaterialMst pk
                            retval = INV_ITEM_MST_Obj.IMP_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }

                // return InvMaterialMst pk
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int DeleteInvRelatedItemMap(INV_ITEM_REL_MAP objInvItemRelMap)
        {
            int retval = 0;
            try
            {
                if (objInvItemRelMap != null)
                {
                    List<INV_ITEM_REL_MAP> Old_INV_ITEM_REL_MAP_Obj = currentEntity.INV_ITEM_REL_MAP.Where(sah => sah.IMR_ITEM == objInvItemRelMap.IMR_ITEM).ToList();
                    if (Old_INV_ITEM_REL_MAP_Obj != null && Old_INV_ITEM_REL_MAP_Obj.Count > 0)
                    {
                        foreach (INV_ITEM_REL_MAP INV_ITEM_REL_MAP_Obj in Old_INV_ITEM_REL_MAP_Obj)
                        {
                            this.currentEntity.INV_ITEM_REL_MAP.DeleteObject(INV_ITEM_REL_MAP_Obj);
                        }
                        retval = 1;
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int DeleteInvSubTypeItemMap(INV_ITEM_SUB_TYPE_MAP objInvItemRelMap)
        {
            int retval = 0;
            try
            {
                if (objInvItemRelMap != null)
                {
                    List<INV_ITEM_SUB_TYPE_MAP> Old_INV_ITEM_REL_MAP_Obj = currentEntity.INV_ITEM_SUB_TYPE_MAP.Where(sah => sah.ISM_ITEM == objInvItemRelMap.ISM_ITEM).ToList();
                    if (Old_INV_ITEM_REL_MAP_Obj != null && Old_INV_ITEM_REL_MAP_Obj.Count > 0)
                    {
                        foreach (INV_ITEM_SUB_TYPE_MAP INV_ITEM_REL_MAP_Obj in Old_INV_ITEM_REL_MAP_Obj)
                        {
                            this.currentEntity.INV_ITEM_SUB_TYPE_MAP.DeleteObject(INV_ITEM_REL_MAP_Obj);
                        }
                        retval = 1;
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public int DeleteInvPackMatItemMap(INV_ITEM_PACK_ITEM_MAP objInvItemRelMap)
        {
            int retval = 0;
            try
            {
                if (objInvItemRelMap != null)
                {
                    List<INV_ITEM_PACK_ITEM_MAP> Old_INV_ITEM_PACK_MAP_Obj = currentEntity.INV_ITEM_PACK_ITEM_MAP.Where(sah => sah.IMP_ITEM == objInvItemRelMap.IMP_ITEM).ToList();
                    if (Old_INV_ITEM_PACK_MAP_Obj != null && Old_INV_ITEM_PACK_MAP_Obj.Count > 0)
                    {
                        foreach (INV_ITEM_PACK_ITEM_MAP INV_ITEM_PACK_MAP_Obj in Old_INV_ITEM_PACK_MAP_Obj)
                        {
                            this.currentEntity.INV_ITEM_PACK_ITEM_MAP.DeleteObject(INV_ITEM_PACK_MAP_Obj);
                        }
                        retval = 1;
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public List<SPPRD_INV_ITEM_PACK_COMB_GET_Result> GetPrdPackingCombinations(int BizUnit, int GroupItem, int prdItem)
        {
            try
            {
                List<SPPRD_INV_ITEM_PACK_COMB_GET_Result> resultFieldsList;
                resultFieldsList = this.currentEntity.SPPRD_INV_ITEM_PACK_COMB_GET(BizUnit, GroupItem, prdItem).ToList();
                return resultFieldsList;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
    }
}