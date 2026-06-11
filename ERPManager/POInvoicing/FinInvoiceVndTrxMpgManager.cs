using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

using ERPManager;

namespace ERPManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinInvoiceVndTrxMpgManager" in both code and config file together.
    public class FinInvoiceVndTrxMpgManager : IFinInvoiceVndTrxMpgManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of the InvoiceListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public FinInvoiceVndTrxMpgManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        
        /// <summary>
        /// Saves list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        //public long SaveInvoiceTrxMpg(List<FIN_INVOICE_VND_TRX_MPG> InvoiceTrxMpgList)
        //{
        //    long retval = 0;
        //    long? maxPK;
        //    decimal invoiceamt = 0;
        //    FIN_INVOICE_VND_TRX_MPG OLD_FIN_INVOICE_VND_TRX_MPG_Obj;
        //    PUR_ORDER_HDR PUR_ORDER_HDR_Obj;
        //    try
        //    {
        //        //Set save status zero,save failed
        //        retval = 0;
        //        //Getting last Payment Mpg pk
        //        maxPK = currentEntity.FIN_INVOICE_VND_TRX_MPG.Max(v => (int?)v.IVM_PK );
        //        maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

        //        foreach (FIN_INVOICE_VND_TRX_MPG FIN_INVOICE_VND_TRX_MPG_Obj in InvoiceTrxMpgList)
        //        {
        //            //check invoice Mpg pk is zero,save invoice Mpg as new record
        //            if (FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK == 0)
        //            {
        //                //Set next invoice Mpg pk
        //                FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK = (long)maxPK;

        //                //Add new invoice Mpg to the db context
        //                currentEntity.FIN_INVOICE_VND_TRX_MPG.AddObject(FIN_INVOICE_VND_TRX_MPG_Obj);
        //                maxPK++;
        //                retval = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK;

        //                invoiceamt = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
        //            }
        //            //updating invoice Mpg details
        //            else
        //            {
        //                //Get current invoice Mpg details using invoice Mpg  pk
        //                OLD_FIN_INVOICE_VND_TRX_MPG_Obj = currentEntity.FIN_INVOICE_VND_TRX_MPG.SingleOrDefault(v => v.IVM_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK);
        //                if (OLD_FIN_INVOICE_VND_TRX_MPG_Obj != null)
        //                {
        //                    invoiceamt = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT - OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
      
        //                    //Update invoice Mpg details
        //                    OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
        //                    OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_ACTIVE = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_ACTIVE;

        //                    OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR;
        //                    //OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_TRX_TYPE = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_TRX_TYPE;

        //                    //Set return value as invoice Mpg pk
        //                    retval = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK;
        //                }
        //                else
        //                {
        //                    //throws exception already deleted or modified by other user
        //                    //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
        //                }
        //            }

        //            //Update Invoiced amount of PO : PUR_ORDER_HDR
        //            PUR_ORDER_HDR_Obj = currentEntity.PUR_ORDER_HDR.SingleOrDefault(sah => sah.POH_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR);

        //            if (PUR_ORDER_HDR_Obj != null)
        //            {
        //                PUR_ORDER_HDR_Obj.POH_AMT_INVOICED += invoiceamt;
        //            }
                    
        //        }

        //        //return Invoice Trx Mpg Pk
        //        return retval;
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    //Handler for unknown exceptions
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //}

        /// <summary>
        /// Perform searching,filtering,sorting,and paging on Invoice Trx Mpg;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_INVOICE_VND_TRX_MPG> GetInvoiceTrxMpg(FIN_INVOICE_VND_TRX_MPG InvoiceHdrObj, ServiceUtility utilityObj = null)
        {
            List<FIN_INVOICE_VND_TRX_MPG> InvoiceTrxMpgList = null;
            try
            {

                InvoiceTrxMpgList = (from pvh in this.currentEntity.FIN_INVOICE_VND_TRX_MPG
                                     where pvh.IVM_INVOICE_HDR == (InvoiceHdrObj.IVM_INVOICE_HDR == 0 ? pvh.IVM_INVOICE_HDR : InvoiceHdrObj.IVM_INVOICE_HDR)
                                             select pvh
                             ).ToList();
                //return Invoice Trx Mpg List
                return InvoiceTrxMpgList;
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

        /// <summary>
        /// Saves list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <param name="isWkfSave"></param>
        /// <returns></returns>
        public long SaveInvoiceTrxMpg(List<FIN_INVOICE_VND_TRX_MPG> InvoiceTrxMpgList, bool isWkfSave = false)
        {
            long retval = 0;
            long? maxPK;
            //decimal invoiceamt = 0;
            long InvoiceHdr = 0;

            FIN_INVOICE_VND_TRX_MPG OLD_FIN_INVOICE_VND_TRX_MPG_Obj;
            PUR_ORDER_HDR purOrderHdrObj;

            List<FIN_INVOICE_VND_TRX_MPG> OLD_FIN_INVOICE_VND_TRX_MPG_List_Obj;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxPK = currentEntity.FIN_INVOICE_VND_TRX_MPG.Max(v => (int?)v.IVM_PK);
                maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                if (InvoiceTrxMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in InvoiceTrxMpgList
                                        select old1.IVM_PK).ToList();

                    InvoiceHdr = InvoiceTrxMpgList[0].IVM_INVOICE_HDR;

                    OLD_FIN_INVOICE_VND_TRX_MPG_List_Obj = (from old in this.currentEntity.FIN_INVOICE_VND_TRX_MPG
                                                            where InvoiceHdr == old.IVM_INVOICE_HDR
                                                                && !pks.Contains(old.IVM_PK)
                                                            select old).ToList();

                    foreach (FIN_INVOICE_VND_TRX_MPG old_FIN_INVOICE_VND_TRX_MPG in OLD_FIN_INVOICE_VND_TRX_MPG_List_Obj)
                    {
                        //Reduce old Inviced Amount if Invoice was Submitted
                        if (old_FIN_INVOICE_VND_TRX_MPG.FIN_INVOICE_VND_HDR.IVH_STATUS > 0)
                        {
                            purOrderHdrObj = currentEntity.PUR_ORDER_HDR.SingleOrDefault(sah => sah.POH_PK == old_FIN_INVOICE_VND_TRX_MPG.IVM_PO_HDR);
                            if (purOrderHdrObj != null)
                            {
                                purOrderHdrObj.POH_AMT_INVOICED -= old_FIN_INVOICE_VND_TRX_MPG.IVM_AMOUNT;
                            }
                        }
                        this.currentEntity.FIN_INVOICE_VND_TRX_MPG.DeleteObject(old_FIN_INVOICE_VND_TRX_MPG);
                    }
                }
                

                foreach (FIN_INVOICE_VND_TRX_MPG FIN_INVOICE_VND_TRX_MPG_Obj in InvoiceTrxMpgList)
                {
                    //check invoice Mpg pk is zero,save invoice Mpg as new record
                    if (FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK == 0)
                    {
                        //Set next invoice Mpg pk
                        FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK = (long)maxPK;

                        //Add new invoice Mpg to the db context
                        currentEntity.FIN_INVOICE_VND_TRX_MPG.AddObject(FIN_INVOICE_VND_TRX_MPG_Obj);
                        maxPK++;
                        retval = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK;
                    }
                    //updating invoice Mpg details
                    else
                    {
                        //Get current invoice Mpg details using invoice Mpg  pk
                        OLD_FIN_INVOICE_VND_TRX_MPG_Obj = currentEntity.FIN_INVOICE_VND_TRX_MPG.SingleOrDefault(v => v.IVM_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK);
                        if (OLD_FIN_INVOICE_VND_TRX_MPG_Obj != null)
                        {
                            //Reduce old Inviced Amount if Invoice was Submitted
                            if (OLD_FIN_INVOICE_VND_TRX_MPG_Obj.FIN_INVOICE_VND_HDR.IVH_STATUS > 0)
                            {
                                purOrderHdrObj = currentEntity.PUR_ORDER_HDR.SingleOrDefault(sah => sah.POH_PK == OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR);
                                if (purOrderHdrObj != null)
                                {
                                    purOrderHdrObj.POH_AMT_INVOICED -= OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
                                }
                            }
                            //Update invoice Mpg details
                            OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
                            OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_ACTIVE = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_ACTIVE;
                            OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_OTHER_AMOUNT = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_OTHER_AMOUNT;
                            OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_TAX_AMOUNT = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_TAX_AMOUNT;
                            OLD_FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR;

                            //Set return value as invoice Mpg pk
                            retval = FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PK;
                        }
                        else
                        {
                            //throws exception already deleted or modified by other user
                            //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
                        }
                    }

                    //Update Invoiced amount of PO : PUR_ORDER_HDR
                    FIN_INVOICE_VND_HDR finInvoiceVndHdr = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(inv => inv.IVH_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_INVOICE_HDR);
                    if (isWkfSave || (finInvoiceVndHdr != null && finInvoiceVndHdr.IVH_STATUS > 0))
                    {
                        purOrderHdrObj = currentEntity.PUR_ORDER_HDR.SingleOrDefault(sah => sah.POH_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR);
                        if (purOrderHdrObj != null)
                        {
                            purOrderHdrObj.POH_AMT_INVOICED += FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
                        }
                    }
                }

                //return Invoice Trx Mpg Pk
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
        #endregion

        #region Private Methods
        #endregion
        
    }
}
