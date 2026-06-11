using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;

namespace ERPManager
{
    public class InvItemSpecDtlManager : IInvItemSpecDtlManager
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
        public InvItemSpecDtlManager(ERPEntities currentEntity)
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

        public List<INV_ITEM_SPEC_DTL> GetInvItemSpecDtl(INV_ITEM_SPEC_DTL invItemSpecDtlObj, int sbuPK = 0)
        {
            List<INV_ITEM_SPEC_DTL> invItemSpecDtlList = null;
            IQueryable<INV_ITEM_SPEC_DTL> invItemSpecDtlQry;
            try
            {

                invItemSpecDtlQry = from isd in this.currentEntity.INV_ITEM_SPEC_DTL
                                     where isd.ISD_ACTIVE == invItemSpecDtlObj.ISD_ACTIVE
                                       && isd.INV_ITEM_MST.INV_ITEM_CATEGORY.ITC_VALUE==2
                                       && ((sbuPK > 0) ? isd.INV_ITEM_MST.ITM_BIZUNIT == sbuPK : true) // SBU filteration
                                       && ((invItemSpecDtlObj.ISD_NATURE.HasValue && invItemSpecDtlObj.ISD_NATURE != null) ? isd.ISD_NATURE == invItemSpecDtlObj.ISD_NATURE : true)
                                       && ((invItemSpecDtlObj.ISD_THICKNESS.HasValue && invItemSpecDtlObj.ISD_THICKNESS != null) ? isd.ISD_THICKNESS == invItemSpecDtlObj.ISD_THICKNESS : true)
                                       && ((invItemSpecDtlObj.ISD_PROCESS.HasValue && invItemSpecDtlObj.ISD_PROCESS != null) ? isd.ISD_PROCESS == invItemSpecDtlObj.ISD_PROCESS : true)
                                       && ((invItemSpecDtlObj.ISD_SURFACE.HasValue && invItemSpecDtlObj.ISD_SURFACE != null) ? isd.ISD_SURFACE == invItemSpecDtlObj.ISD_SURFACE : true)
                                       && ((invItemSpecDtlObj.ISD_COLOUR.HasValue && invItemSpecDtlObj.ISD_COLOUR != null) ? isd.ISD_COLOUR == invItemSpecDtlObj.ISD_COLOUR : true)
                                       && ((invItemSpecDtlObj.ISD_GRADE.HasValue && invItemSpecDtlObj.ISD_GRADE != null) ? isd.ISD_GRADE == invItemSpecDtlObj.ISD_GRADE : true)
                                       && ((invItemSpecDtlObj.ISD_SIZE.HasValue && invItemSpecDtlObj.ISD_SIZE != null) ? isd.ISD_SIZE == invItemSpecDtlObj.ISD_SIZE : true)
                                       && ((invItemSpecDtlObj.ISD_LENGTH.HasValue && invItemSpecDtlObj.ISD_LENGTH != null) ? isd.ISD_LENGTH == invItemSpecDtlObj.ISD_LENGTH : true)
                                       && ((invItemSpecDtlObj.ISD_ADNL_SPEC05.HasValue && invItemSpecDtlObj.ISD_ADNL_SPEC05 != null) ? isd.ISD_ADNL_SPEC05 == invItemSpecDtlObj.ISD_ADNL_SPEC05 : true)
                                       && ((invItemSpecDtlObj.ISD_ADNL_SPEC04.HasValue && invItemSpecDtlObj.ISD_ADNL_SPEC04 != null) ? isd.ISD_ADNL_SPEC04 == invItemSpecDtlObj.ISD_ADNL_SPEC04 : true)
                                       && ((invItemSpecDtlObj.ISD_ADNL_SPEC03.HasValue && invItemSpecDtlObj.ISD_ADNL_SPEC03 != null) ? isd.ISD_ADNL_SPEC03 == invItemSpecDtlObj.ISD_ADNL_SPEC03 : true)
                                       && ((invItemSpecDtlObj.ISD_ADNL_SPEC02.HasValue && invItemSpecDtlObj.ISD_ADNL_SPEC02 != null) ? isd.ISD_ADNL_SPEC02 == invItemSpecDtlObj.ISD_ADNL_SPEC02 : true)
                                       && ((invItemSpecDtlObj.ISD_ADNL_SPEC01.HasValue && invItemSpecDtlObj.ISD_ADNL_SPEC01 != null) ? isd.ISD_ADNL_SPEC01 == invItemSpecDtlObj.ISD_ADNL_SPEC01 : true)
                                       && ((invItemSpecDtlObj.ISD_CHLORINATION.HasValue && invItemSpecDtlObj.ISD_CHLORINATION != null) ? isd.ISD_CHLORINATION == invItemSpecDtlObj.ISD_CHLORINATION : true)
                                     select isd;

                if (invItemSpecDtlObj.INV_ITEM_MST != null)
                {
                    invItemSpecDtlQry = invItemSpecDtlQry
                                .Where(x => x.INV_ITEM_MST.ITM_NAME.Contains(string.IsNullOrEmpty(invItemSpecDtlObj.INV_ITEM_MST.ITM_NAME) ? x.INV_ITEM_MST.ITM_NAME : invItemSpecDtlObj.INV_ITEM_MST.ITM_NAME)
                                    && x.INV_ITEM_MST.ITM_CODE.Contains(string.IsNullOrEmpty(invItemSpecDtlObj.INV_ITEM_MST.ITM_CODE) ? x.INV_ITEM_MST.ITM_CODE : invItemSpecDtlObj.INV_ITEM_MST.ITM_CODE)
                                    && invItemSpecDtlObj.INV_ITEM_MST.ITM_PK>0?x.INV_ITEM_MST.ITM_PK==invItemSpecDtlObj.INV_ITEM_MST.ITM_PK:true
                                     && x.INV_ITEM_MST.ITM_ACTIVE == (byte)(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE)
                                    );
                }

                // Apply Paging And Sorting for grid Purpose
                //OBJ_CRM_CUSTOMER_MST_List = OBJ_CRM_CUSTOMER_MST_qry.SortRecords<CRM_CUSTOMER_MST>(serviceUtilityObj).ToList();
                invItemSpecDtlList = invItemSpecDtlQry.ToList();

                return invItemSpecDtlList;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects

            }
        }
        #endregion

       
    }
}
