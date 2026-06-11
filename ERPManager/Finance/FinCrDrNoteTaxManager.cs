using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using System.Data.Objects.DataClasses;

namespace ERPManager
{
    public class FinCrDrNoteTaxManager : IFinCrDrNoteTaxManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// FinCashBankManager Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>

        public FinCrDrNoteTaxManager(ERPEntities currentEntity)
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
        #endregion

        public long? SaveCRDRTaxDetSplit(List<FIN_CRDR_NOTE_TAX_DTL> finCrDrTaxList, ref long? maxID)
        {
            long? retval;
            retval = 0;

            List<FIN_CRDR_NOTE_TAX_DTL> Old_finReceiptCusSOMpgList;
            FIN_CRDR_NOTE_TAX_DTL Old_finReceiptCusSOMpg;           

            try
            {
                if (finCrDrTaxList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_CRDR_NOTE_TAX_DTL.Max(v => (long?)v.NTD_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;                    

                    //  Delete Removed Entries
                    List<long> pks = (from old1 in finCrDrTaxList
                                      select old1.NTD_PK).ToList();
                    /**/
                    long NTD_CRDR_MPG = finCrDrTaxList[0].NTD_CRDR_MPG;
                    long? NTD_CRDR_DTL = finCrDrTaxList[0].NTD_CRDR_DTL;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_CRDR_NOTE_TAX_DTL
                                                  where oldp.NTD_CRDR_MPG == NTD_CRDR_MPG
                                                  && !pks.Contains(oldp.NTD_PK) 
                                                  && oldp.NTD_CRDR_DTL.HasValue
                                                  && oldp.NTD_CRDR_DTL == NTD_CRDR_DTL
                                                  select oldp).ToList();
                    foreach (FIN_CRDR_NOTE_TAX_DTL oldfinCrDrNoteHdrObjObject in Old_finReceiptCusSOMpgList)
                    {
                        this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(oldfinCrDrNoteHdrObjObject);
                    }
                    foreach (FIN_CRDR_NOTE_TAX_DTL FIN_CRDR_NOTE_TAX_DTL_obj in finCrDrTaxList)
                    {
                        if (FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK == 0) //  INSERT NEW RECORD
                        {
                            // The new object is created to avoid multiple reference issue
                            FIN_CRDR_NOTE_TAX_DTL objdtl = new FIN_CRDR_NOTE_TAX_DTL();
                            objdtl.NTD_AMOUNT = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_AMOUNT;
                            objdtl.NTD_CRDR_DTL = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_DTL;
                            objdtl.NTD_CRDR_MPG = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_MPG;
                            objdtl.NTD_TAX = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_TAX;
                            objdtl.NTD_PK = maxID.Value;

                            //FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK = maxID.Value;
                            currentEntity.FIN_CRDR_NOTE_TAX_DTL.AddObject(objdtl);
                            retval = maxID;
                            maxID++;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finReceiptCusSOMpg = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(a => a.NTD_PK == FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK);
                            Old_finReceiptCusSOMpg.NTD_AMOUNT = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_AMOUNT;
                            Old_finReceiptCusSOMpg.NTD_CRDR_DTL = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_DTL;
                            Old_finReceiptCusSOMpg.NTD_CRDR_MPG = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_MPG;
                            Old_finReceiptCusSOMpg.NTD_TAX = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_TAX;
                            retval = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK;
                        }

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

        public long? SaveCRDRTaxHdrSplit(List<FIN_CRDR_NOTE_TAX_DTL> finCrDrTaxList, ref long? maxID)
        {
            long? retval;
            retval = 0;           

            List<FIN_CRDR_NOTE_TAX_DTL> Old_finReceiptCusSOMpgList;
            FIN_CRDR_NOTE_TAX_DTL Old_finReceiptCusSOMpg;           

            try
            {
                if (finCrDrTaxList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_CRDR_NOTE_TAX_DTL.Max(v => (long?)v.NTD_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;                    

                    //  Delete Removed Entries
                    List<long> pks = (from old1 in finCrDrTaxList
                                      select old1.NTD_PK).ToList();
                    /**/
                    long NTD_CRDR_MPG = finCrDrTaxList[0].NTD_CRDR_MPG;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_CRDR_NOTE_TAX_DTL
                                                  where oldp.NTD_CRDR_MPG == NTD_CRDR_MPG
                                                  && !pks.Contains(oldp.NTD_PK) && !oldp.NTD_CRDR_DTL.HasValue
                                                  select oldp).ToList();
                    foreach (FIN_CRDR_NOTE_TAX_DTL oldfinCrDrNoteHdrObjObject in Old_finReceiptCusSOMpgList)
                    {
                        this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(oldfinCrDrNoteHdrObjObject);                        
                    }
                    foreach (FIN_CRDR_NOTE_TAX_DTL FIN_CRDR_NOTE_TAX_DTL_obj in finCrDrTaxList)
                    {
                       
                        if (FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK == 0) //  INSERT NEW RECORD
                        {
                            // The new object is created to avoid multiple reference issue
                            FIN_CRDR_NOTE_TAX_DTL objdtl = new FIN_CRDR_NOTE_TAX_DTL();
                            objdtl.NTD_AMOUNT = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_AMOUNT;
                            objdtl.NTD_CRDR_DTL = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_DTL;
                            objdtl.NTD_CRDR_MPG = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_MPG;
                            objdtl.NTD_TAX = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_TAX;
                            objdtl.NTD_PK = maxID.Value;
                           
                            //FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK = maxID.Value;
                            this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.AddObject(objdtl);
                            retval = maxID;
                            maxID++;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finReceiptCusSOMpg = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(a => a.NTD_PK == FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK);
                            Old_finReceiptCusSOMpg.NTD_AMOUNT = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_AMOUNT;
                            Old_finReceiptCusSOMpg.NTD_CRDR_DTL = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_DTL;
                            Old_finReceiptCusSOMpg.NTD_CRDR_MPG = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_MPG;
                            Old_finReceiptCusSOMpg.NTD_TAX = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_TAX;
                            retval = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK;
                        }

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


        public List<FIN_CRDR_NOTE_TAX_DTL> GetCrDrNoteTaxList(long CrDrPK)
        {
            List<FIN_CRDR_NOTE_TAX_DTL> InvoiceHdrList = null;            
            try
            {

                InvoiceHdrList = (from tx in this.currentEntity.FIN_CRDR_NOTE_TAX_DTL
                                  where tx.FIN_CRDR_NOTE_MPG.CDM_CRDR_NOTE_HDR == CrDrPK
                                  select tx).ToList();
                return InvoiceHdrList;
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
    }
}
