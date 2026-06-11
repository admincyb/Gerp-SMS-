using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERP.Utilities;
using System.Data.Objects;
//using System.Data.Linq;


namespace ERPManager
{
   public class POListManager:IPOListManager
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
        public POListManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
                //     NorthwindDataContext context = new NorthwindDataContext() context.tblCities.MergeOption = MergeOption.NoTracking; 
                currentEntity.PUR_ORDER_HDR.MergeOption = MergeOption.NoTracking; 

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        
        //<summary>
        //Gets list of Approved Pos 
        //</summary>
        //<param name="objPoHeader"></param>
        //<param name="utilityObj"></param>
        //<returns>List of PoHeader</returns>
        public List<PUR_ORDER_HDR> GetPoHeader(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj ,int Status)
        {
            List<PUR_ORDER_HDR> ObjPoHeaderLst = new List<PUR_ORDER_HDR>();
            IQueryable<PUR_ORDER_HDR> PUR_ORDER_HDRQuery;
            

            try
            {
                if (objPoHeader.POH_PK > 0)
                {
                    PUR_ORDER_HDRQuery = (from poh in this.currentEntity.PUR_ORDER_HDR
                                          //join vnd in this.currentEntity.PUR_VENDOR_MST on poh.POH_VENDOR equals vnd.VEN_PK
                                          //join shp in this.currentEntity.ADM_DEPT_MST on poh.POH_SHIPPING equals shp.DPT_PK
                                          //join blg in this.currentEntity.ADM_DEPT_MST on poh.POH_BILLING equals blg.DPT_PK
                                          //join crd in this.currentEntity.WkfUserMsts on poh.POH_CRTD_BY equals crd.usrPK
                                          //join cem in this.currentEntity.EmpEmployeeMsts on crd.usrEmployee equals cem.empPk 
                                          //join ard in this.currentEntity.WkfUserMsts on poh.POH_APPROVED_BY equals ard.usrPK
                                          //join aem in this.currentEntity.EmpEmployeeMsts on ard.usrEmployee equals aem.empPk 
                                          //join acm in this.currentEntity.ADM_CONFIG_MST on poh.POH_TYPE equals acm.CFG_VALUE 
                                          where poh.POH_ACTIVE == objPoHeader.POH_ACTIVE
                                              //&& acm.CFG_TYPE == "PURCHASE TYPE"
                                           && poh.POH_PK ==  objPoHeader.POH_PK                                       
                                              //&& (Status == 0 ? poh.POH_AMT_INVOICED < poh.POH_TOTAL_VALUE : true)    //  Pending                                           
                                              //&& (Status == 2 ? poh.POH_AMT_INVOICED >= poh.POH_TOTAL_VALUE : true)    //  Completed
                                           && (poh.POH_STATUS == 2 || poh.POH_STATUS == 5) //  Approved & Closed
                                               //  Completed
                                          select poh);
                }
                else
                {
                    if (objPoHeader.POH_VENDOR == 0 && utilityObj.FilterDate == null && utilityObj.FilterToDate == null)
                    {
                        PUR_ORDER_HDRQuery = (from poh in this.currentEntity.PUR_ORDER_HDR
                                              where poh.POH_ACTIVE == objPoHeader.POH_ACTIVE
                                               && (objPoHeader.POH_GROUP > 0 ? poh.POH_GROUP == objPoHeader.POH_GROUP : true)
                                               && poh.POH_BIZUNIT == (objPoHeader.POH_BIZUNIT > 0 ? objPoHeader.POH_BIZUNIT : poh.POH_BIZUNIT)
                                               && (Status == 1 ? true : true)    //  All
                                               && (poh.POH_STATUS == 2 || poh.POH_STATUS == 5 || (poh.POH_STATUS == 4 && poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_RECEIVED > 0)))
                                               && (Status == 0 ? (
                                                         poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_REQUESTED > i.POD_QTY_INVOICED)) : true)    //  Pending
                                               && (Status == 2 ? (poh.PUR_ORDER_DTL.All(i => i.POD_QTY_APPROVED <= i.POD_QTY_INVOICED)) : true)    //  Completed
                                              select poh);
                    }
                    else
                    {
                        PUR_ORDER_HDRQuery = (from poh in this.currentEntity.PUR_ORDER_HDR
                                              where poh.POH_ACTIVE == objPoHeader.POH_ACTIVE
                                               && (objPoHeader.POH_VENDOR > 0 ? (poh.POH_VENDOR == objPoHeader.POH_VENDOR) : true)
                                               && (utilityObj.FilterDate != null ? (poh.POH_DATE >= utilityObj.FilterDate) : true)
                                               && (utilityObj.FilterToDate != null ? (poh.POH_DATE <= utilityObj.FilterToDate) : true)
                                               && (objPoHeader.POH_GROUP > 0 ? poh.POH_GROUP == objPoHeader.POH_GROUP : true)
                                               && poh.POH_BIZUNIT == (objPoHeader.POH_BIZUNIT > 0 ? objPoHeader.POH_BIZUNIT : poh.POH_BIZUNIT)
                                               && (Status == 1 ? true : true)    //  All
                                               && (poh.POH_STATUS == 2 || poh.POH_STATUS == 5 || (poh.POH_STATUS == 4 && poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_RECEIVED > 0)))
                                               && (Status == 0 ? (
                                                         poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_REQUESTED > i.POD_QTY_INVOICED)) : true)    //  Pending
                                               && (Status == 2 ? (poh.PUR_ORDER_DTL.All(i => i.POD_QTY_APPROVED <= i.POD_QTY_INVOICED)) : true)    //  Completed
                                              select poh);
                    }


                }
                //Step 1
                //sWatch.Start();

                //utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                //utilityObj.TotalRecords = PUR_ORDER_HDRQuery.Select(x=> x.POH_PK).Count();
                //sWatch.Stop();
                //long x1 = sWatch.ElapsedMilliseconds;
                //MyCompliedQueries.objPoHeader = objPoHeader;
                //MyCompliedQueries.utilityObj = utilityObj;
                //MyCompliedQueries.Status = Status;



               // PUR_ORDER_HDRQuery = MyCompliedQueries.CompliedQueryForPesron.Invoke(this.currentEntity, utilityObj, objPoHeader, Status);
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = PUR_ORDER_HDRQuery.GroupBy(m => m.POH_PK).Count();
                ObjPoHeaderLst = PUR_ORDER_HDRQuery.SortRecords<PUR_ORDER_HDR>(utilityObj).ToList();

            }
            catch
            {
            }

            return ObjPoHeaderLst;
        }
     
      

        //<summary>
        //Gets list of Po details
        //</summary>
        //<param name=" poPK"></param>
        //<param name="utilityObj"></param>
        //<returns>List of PoDetails</returns>
        public List<PUR_ORDER_DTL> GetPoDetails(int poPK, ServiceUtility utilityObj)
        {
            List<PUR_ORDER_DTL> ObjPoDetailsLst = new List<PUR_ORDER_DTL>();
            try
            {
                ObjPoDetailsLst = (from pod in this.currentEntity.PUR_ORDER_DTL
                                   //join itm in this.currentEntity.INV_ITEM_MST on pod.POD_ITEM equals itm.ITM_PK 
                                   //join uom in this.currentEntity.INV_UOM_MST  on pod.POD_UOM  equals uom.UOM_PK 
                                   where pod.POD_PO == poPK
                                   select pod).ToList();
            }
            catch { }

            return ObjPoDetailsLst;
        }

        public List<INV_GRN_HDR> GetGRNDetails(int poPK, ServiceUtility utilityObj)
        {
            List<INV_GRN_HDR> GRNDetailsLstObj = new List<INV_GRN_HDR>();
            try
            {
                GRNDetailsLstObj = (from grh in this.currentEntity.INV_GRN_HDR
                                    join grd in this.currentEntity.INV_GRN_DTL on grh.GRH_PK equals grd.GRD_GR
                                    //join dpt in this.currentEntity.ADM_DEPT_MST on grh.GRH_DEPT equals dpt.DPT_PK
                                    //join crd in this.currentEntity.WkfUserMsts on grh.GRH_SUBMITTED_BY  equals crd.usrPK
                                    //join cem in this.currentEntity.EmpEmployeeMsts on crd.usrEmployee equals cem.empPk
                                    //join ard in this.currentEntity.WkfUserMsts on grh.GRH_APPROVED_BY equals ard.usrPK
                                    //join aem in this.currentEntity.EmpEmployeeMsts on ard.usrEmployee equals aem.empPk 
                                    where grd.GRD_PO == poPK
                                    select grh).Distinct().ToList();
            }
            catch
            {

            }
            return GRNDetailsLstObj;
            //throw new NotImplementedException();
        }
        
        public List<INV_GRN_DTL> GetGRNDetailsList(int poDtlPK, ServiceUtility utilityObj)
        {
            List<INV_GRN_DTL> GRNDetailsLstObj = new List<INV_GRN_DTL>();
            try
            {
                GRNDetailsLstObj = (from grd in this.currentEntity.INV_GRN_DTL
                                    where grd.GRD_PO_DTL == poDtlPK && grd.INV_GRN_HDR.GRH_DEL_STATUS == 0
                                    select grd).ToList();
            }
            catch
            {

            }
            return GRNDetailsLstObj;
            //throw new NotImplementedException();
        }

        public List<INV_GIN_HDR> GetGinDetails(int grnPK, ServiceUtility utilityObj)
        {
            List<INV_GIN_HDR> GINDetailsLstObj = new List<INV_GIN_HDR>();
            try
            {
                GINDetailsLstObj = (from grh in this.currentEntity.INV_GIN_HDR
                                    join grd in this.currentEntity.INV_GIN_DTL on grh.GIH_PK equals grd.GID_GI
                                    //join dpt in this.currentEntity.ADM_DEPT_MST on grh.GIH_DEPT equals dpt.DPT_PK
                                    //join crd in this.currentEntity.WkfUserMsts on grh.GIH_SUBMITTED_BY equals crd.usrPK
                                    //join cem in this.currentEntity.EmpEmployeeMsts on crd.usrEmployee equals cem.empPk
                                    //join ard in this.currentEntity.WkfUserMsts on grh.GIH_APPROVED_BY equals ard.usrPK
                                    //join aem in this.currentEntity.EmpEmployeeMsts on ard.usrEmployee equals aem.empPk
                                    where grd.GID_GRN == grnPK
                                    //group grh by grh.GRH_PK  into grn     :   .Distinct()
                                    select grh).Distinct().ToList();
            }
            catch
            {

            }
            return GINDetailsLstObj;
            //throw new NotImplementedException();
        }

        public List<INV_GIN_DTL> GetGINDetailsList(int grnDtlPK, ServiceUtility utilityObj)
        {
            List<INV_GIN_DTL> GINDetailsLstObj = new List<INV_GIN_DTL>();
            try
            {
                GINDetailsLstObj = (from gid in this.currentEntity.INV_GIN_DTL
                                    where gid.GID_GRN_DTL == grnDtlPK
                                    select gid).Distinct().ToList();
            }
            catch
            {

            }
            return GINDetailsLstObj;
            //throw new NotImplementedException();
        }
        public List<INV_STK_TRAN_HDR> GetStockTransferDetails(int ginDtlPK, ServiceUtility utilityObj)
        {
            List<INV_STK_TRAN_HDR> StockTransferDetailsLstObj = new List<INV_STK_TRAN_HDR>();
            try
            {
                // **Trans fer to link missing
                StockTransferDetailsLstObj = (from sfh in this.currentEntity.INV_STK_TRAN_HDR
                                              join isg in this.currentEntity.INV_STK_TRAN_GIN_MAP on sfh.SFH_PK equals isg.ISG_ST
                                              join gid in this.currentEntity.INV_GIN_DTL on isg.ISG_GIN_DTL equals gid.GID_PK
                                              //join fdt in this.currentEntity.ADM_DEPT_MST on sfh.SFH_DEPT equals fdt.DPT_PK
                                              // join crd in this.currentEntity.WkfUserMsts on sfh.SFH_SUBMITTED_BY equals crd.usrPK
                                              //join cem in this.currentEntity.EmpEmployeeMsts on crd.usrEmployee equals cem.empPk
                                              //join ard in this.currentEntity.WkfUserMsts on sfh.SFH_APPROVED_BY equals ard.usrPK
                                              //join aem in this.currentEntity.EmpEmployeeMsts on ard.usrEmployee equals aem.empPk
                                              where gid.GID_PK == ginDtlPK
                                              select sfh).ToList();

            }
            catch
            {

            }
            //throw new NotImplementedException();
            return StockTransferDetailsLstObj;
        }
        
        /// <summary>
        /// Get PO numbers for autucomplete
        /// </summary>
        /// <param name="objPoHeader"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<PUR_ORDER_HDR> GetPoNumberAutoCompleteList(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj)
        {
            List<PUR_ORDER_HDR> PoHeader_List_Obj = new List<PUR_ORDER_HDR>();

            try
            {
                PoHeader_List_Obj = (from po in this.currentEntity.PUR_ORDER_HDR
                                     where po.POH_ACTIVE == objPoHeader.POH_ACTIVE
                                            && po.POH_NO.StartsWith(utilityObj.FilterValue )
                                            && (utilityObj.IsSBUSpecific==true ? po.POH_BIZUNIT ==objPoHeader.POH_BIZUNIT : true)
                                            && (po.POH_STATUS == 2 || po.POH_STATUS == 5 || po.POH_STATUS == 4)
                                     select po).ToList();
            }
            catch
            {

            }

            return PoHeader_List_Obj;
        }

        /// <summary>
       /// Get Selected POs List
       /// </summary>
       /// <param name="poPkList"></param>
       /// <param name="utilityObj"></param>
       /// <returns></returns>
        public List<PUR_ORDER_HDR> GetSelectedPOs(List<long> poPkList, ServiceUtility utilityObj)
        {
            List<PUR_ORDER_HDR> ObjPoHeaderLst = new List<PUR_ORDER_HDR>();
            try
            {
                ObjPoHeaderLst = (from poh in this.currentEntity.PUR_ORDER_HDR
                                  join vnd in this.currentEntity.PUR_VENDOR_MST on poh.POH_VENDOR equals vnd.VEN_PK
                                  where poPkList.Contains(poh.POH_PK)
                                  select poh).ToList();

            }
            catch
            {
            }

            return ObjPoHeaderLst;
        }

        /// <summary>
       /// Get invoiced Pos for edit and view
       /// </summary>
       /// <param name="invoicePK"></param>
       /// <returns></returns>
        public List<FIN_INVOICE_VND_TRX_MPG> GetInvoicedPOs(long invoicePK)
        {
            List<FIN_INVOICE_VND_TRX_MPG> ObjPoHeaderLst = new List<FIN_INVOICE_VND_TRX_MPG>();
            try
            {
                ObjPoHeaderLst = (from ivd in this.currentEntity.FIN_INVOICE_VND_TRX_MPG 
                                  join poh in this.currentEntity.PUR_ORDER_HDR on ivd.IVM_PO_HDR equals poh.POH_PK 
                                  join vnd in this.currentEntity.PUR_VENDOR_MST on poh.POH_VENDOR equals vnd.VEN_PK
                                  where ivd.IVM_INVOICE_HDR == invoicePK
                                    && ivd.IVM_ACTIVE == 1
                                  select ivd).ToList();

            }
            catch
            {
            }

            return ObjPoHeaderLst;
        }



        /// <summary>
        /// Get PO's Tax Details
        /// </summary>
        /// <param name="POPK"></param>
        /// <returns></returns>
        public DataTable GetPurOrderTaxDetails(long POPK)
        {
            DataTable purorderTaxDetails = new DataTable();
            
            try
            {                

                var POTaxHeaderLst = (from poth in this.currentEntity.PUR_ORDER_TAX_HDR
                                      where poth.PTH_PO == POPK && poth.PTH_TAX_CATEGORY == ((int)BusinessObject.CommonManagement.TaxType.Tax)
                                      select new
                                      {
                                          PTH_PK = poth.PTH_PK,
                                          PTH_PO = poth.PTH_PO,
                                          PTH_TYPE = poth.PTH_TYPE,
                                          PTH_TAX = poth.PTH_TAX,
                                          PTH_TAX_CATEGORY = poth.PTH_TAX_CATEGORY,
                                          PTH_NAME = poth.PTH_NAME,
                                          PTH_TAX_AMT = poth.PTH_TAX_AMT

                                      });
                var POTaxDetailList = (from poth in this.currentEntity.PUR_ORDER_TAX_DTL
                                      where poth.PUR_ORDER_DTL.POD_PO == POPK  && poth.POT_TAX_CATEGORY == ((int)BusinessObject.CommonManagement.TaxType.Tax)
                                      select new
                                      {
                                          PTH_PK = poth.POT_PK,
                                          PTH_PO = poth.POT_PO_DTL,
                                          PTH_TYPE = poth.POT_TYPE,
                                          PTH_TAX = poth.POT_TAX,
                                          PTH_TAX_CATEGORY = poth.POT_TAX_CATEGORY,
                                          PTH_NAME = poth.POT_NAME,
                                          PTH_TAX_AMT = poth.POT_TAX_AMT

                                      });


                var FinalLst = POTaxHeaderLst.Union(POTaxDetailList);
                var groupedFinalLst = FinalLst.GroupBy(f => f.PTH_TAX)
                    .Select(grp => new 
                    {
                        PTH_TAX_AMT = grp.Sum(p => p.PTH_TAX_AMT),
                        PTH_NAME = grp.Min(p => p.PTH_NAME),
                        PTH_TAX = grp.Min(p => p.PTH_TAX),
                        PTH_PK = grp.Min(p => p.PTH_PK)
                    }).ToList();
    
                purorderTaxDetails = groupedFinalLst.ToList().ToDataTable();
                                 
            }
            catch
            {
            }

            return purorderTaxDetails;
        }


        #endregion

        //static class MyCompliedQueries
        //{
          
        //    public static Func<ERPEntities,ServiceUtility,PUR_ORDER_HDR,int,IQueryable<PUR_ORDER_HDR>>
        //    CompliedQueryForPesron = CompiledQuery.Compile<ERPEntities,ServiceUtility,PUR_ORDER_HDR,int,IQueryable<PUR_ORDER_HDR>> ((db,utilityObj,objPoHeader,Status) =>
        //                               //ERPEntities context) =>
        //                                   (from poh in db.PUR_ORDER_HDR
        //                                    where poh.POH_ACTIVE ==  objPoHeader.POH_ACTIVE
        //                                       && (objPoHeader.POH_VENDOR >0 ? (poh.POH_VENDOR == objPoHeader.POH_VENDOR) : true)
        //                                       && (utilityObj.FilterDate != null ? (poh.POH_DATE >= utilityObj.FilterDate) : true)
        //                                       && (utilityObj.FilterToDate != null ? (poh.POH_DATE <= utilityObj.FilterToDate) : true)
        //                                       && (objPoHeader.POH_GROUP > 0 ? poh.POH_GROUP == objPoHeader.POH_GROUP : true)
        //                                       && poh.POH_BIZUNIT == (objPoHeader.POH_BIZUNIT > 0 ? objPoHeader.POH_BIZUNIT : poh.POH_BIZUNIT)
        //                                       && (Status == 1 ? true : true)    //  All
        //                                       && (poh.POH_STATUS == 2 || poh.POH_STATUS == 5 || (poh.POH_STATUS == 4 && poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_RECEIVED > 0)))
        //                                       && (Status == 0 ? (
        //                                                 poh.PUR_ORDER_DTL.Any(i => i.POD_QTY_REQUESTED > i.POD_QTY_INVOICED)) : true)    //  Pending
        //                                       && (Status == 2 ? (poh.PUR_ORDER_DTL.All(i => i.POD_QTY_APPROVED <= i.POD_QTY_INVOICED)) : true)    //  Completed
        //                                      select poh));


        //}
    }
}
