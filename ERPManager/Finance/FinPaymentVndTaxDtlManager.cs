using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data.Objects;
using System.Data;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using ERPManager.Finance;
using System.Threading;

namespace ERPManager.Finance
{
    public class FinPaymentVndTaxDtlManager : IFinPaymentVndtaxDtlManager
    {

        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;

        #endregion

        #region Manager Methods
        /// <summary>
        /// FinPaymentVndtaxDtl Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>

        public FinPaymentVndTaxDtlManager(ERPEntities currentEntity)
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

        public long? SavePaymentTax(List<FIN_PAYMENT_VND_TAX_DTL> finPaymentVndTaxDtlList, ref long? maxID, int? poHdr, decimal splitTaxAMOUNT, Byte Category, long? invoiceHdr, decimal? discTotAmount, int? woPK)
        {
            long? retval;
            retval = 0;

            long? maxPK;
            maxPK = maxID;
            List<FIN_PAYMENT_VND_TAX_DTL> Old_finPaymentVndTaxDtlList;

            PUR_ORDER_HDR obj_PUR_ORDER_HDR;
            INV_WORK_ORDER_ITEM_HDR obj_INV_WORK_ORDER_ITEM_HDR;

            FIN_PAYMENT_VND_HDR obj_FIN_PAYMENT_VND_HDR;
            List<PUR_ORDER_DTL> PUR_ORDER_DTLList;
            List<INV_WORK_ORDER_ITEM_DTL> INV_WORK_ORDER_ITEM_DTLList;

            List<PUR_ORDER_TAX_DTL> PUR_ORDER_TAX_DTLList;
            List<PUR_ORDER_TAX_HDR> PUR_ORDER_TAX_HDRList;
            FIN_PAYMENT_VND_TAX_DTL FIN_PAYMENT_VND_TAX_DTL_obj;

            FIN_INVOICE_VND_HDR obj_FIN_INVOICE_VND_HDR;
            FIN_PAYMENT_VND_TRX_MPG obj_FIN_PAYMENT_VND_TRX_MPG;
            List<FIN_INVOICE_VND_DTL> FIN_INVOICE_VND_DTLList;
            List<FIN_INVOICE_VND_TAX_DTL> FIN_INVOICE_VND_TAX_DTLList;
            List<FIN_INVOICE_VND_TAX_HDR> FIN_INVOICE_VND_TAX_HDRList;

            try
            {
                if (finPaymentVndTaxDtlList.Count > 0)
                {

                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;
                    if (!maxPK.HasValue || maxPK.Value == 0)
                        maxPK = currentEntity.FIN_PAYMENT_VND_TAX_DTL.Max(v => (long?)v.PDT_PK);
                    maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;
                    //  Delete Removed Entries

                    List<int> pks = (from old1 in finPaymentVndTaxDtlList
                                     select old1.PDT_PK).ToList();
                    /**/
                    long PDT_PAYMENT_TRX_MPG = finPaymentVndTaxDtlList[0].PDT_PAYMENT_TRX;

                    Old_finPaymentVndTaxDtlList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_TAX_DTL
                                                   where oldp.PDT_PAYMENT_TRX == PDT_PAYMENT_TRX_MPG
                                                   //&& !pks.Contains(oldp.PDT_PK)
                                                   select oldp).ToList();
                    //Delete existing records against receipt
                    foreach (FIN_PAYMENT_VND_TAX_DTL Old_finPaymentVndTaxDtlObj in Old_finPaymentVndTaxDtlList)
                    {
                        this.currentEntity.FIN_PAYMENT_VND_TAX_DTL.DeleteObject(Old_finPaymentVndTaxDtlObj);
                    }

                    List<FIN_PAYMENT_VND_TAX_DTL> FIN_PAYMENT_VND_TAX_DTL_List = new List<FIN_PAYMENT_VND_TAX_DTL>(); ;
                    foreach (FIN_PAYMENT_VND_TAX_DTL FIN_PAYMENT_VND_TAX_DTL_obj1 in finPaymentVndTaxDtlList)
                    {
                        decimal Totaltax = 0;
                        decimal totalLineitemTax = 0;
                        decimal totalHdritemTax = 0;
                        decimal TaxApplcableinLine = 0;
                        decimal TaxApplcableinHDR = 0;
                        decimal TotalDisc = 0;
                        decimal totalLineitemDisc = 0;
                        decimal totalHdritemDisc = 0;
                        decimal discApplcableinLine = 0;
                        decimal discApplcableinHDR = 0;
                        decimal splitDiscAMOUNT = (decimal)discTotAmount;
                        #region Adv Inv
                        if (Category != (Byte)(POInvoiceCategory.Invoice))
                        {

                            if (poHdr.HasValue)
                            {
                                #region PO Header
                                obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == poHdr);
                                PUR_ORDER_DTLList = obj_PUR_ORDER_HDR.PUR_ORDER_DTL.ToList();
                                Totaltax = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == 1).Sum(s => s.POT_TAX_AMT)) + obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == 1).Sum(o => o.PTH_TAX_AMT);
                                totalLineitemTax = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == 1).Sum(s => s.POT_TAX_AMT));
                                totalHdritemTax = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == 1).Sum(o => o.PTH_TAX_AMT);
                                if (Totaltax > 0)
                                {
                                    TaxApplcableinLine = (totalLineitemTax / Totaltax) * splitTaxAMOUNT;
                                    TaxApplcableinHDR = (totalHdritemTax / Totaltax) * splitTaxAMOUNT;
                                }

                                //Discount
                                TotalDisc = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.POT_TAX_AMT)) + obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.PTH_TAX_AMT);
                                totalLineitemDisc = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.POT_TAX_AMT));
                                totalHdritemDisc = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.PTH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                                if (TotalDisc > 0)
                                {
                                    discApplcableinLine = (totalLineitemDisc / TotalDisc) * splitDiscAMOUNT;
                                    discApplcableinHDR = (totalHdritemDisc / TotalDisc) * splitDiscAMOUNT;

                                }

                                if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PK == 0) //  INSERT NEW RECORD
                                {

                                    if (PUR_ORDER_DTLList.Count > 0)
                                    {

                                        PUR_ORDER_DTLList.ForEach(dtl =>
                                        {

                                            PUR_ORDER_TAX_DTLList = dtl.PUR_ORDER_TAX_DTL.Where(k => (k.POT_TAX_CATEGORY == (byte)TaxType.Tax || k.POT_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                            if (PUR_ORDER_TAX_DTLList.Count > 0)
                                            {
                                                PUR_ORDER_TAX_DTLList.ForEach(dtlTax =>
                                                {
                                                    FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_POD = dtl.POD_PK;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_VND_PO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_VND_PO_MPG;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.POT_TAX;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.POT_TAX_CATEGORY;
                                                    if (dtlTax.POT_TAX_CATEGORY != (byte)TaxType.Discount && (totalLineitemTax > 0))
                                                    {
                                                        decimal TotalTaxPerAmt = dtlTax.POT_TAX_AMT / totalLineitemTax;
                                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinLine * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                    }
                                                    else if (dtlTax.POT_TAX_CATEGORY == (byte)TaxType.Discount && (totalLineitemDisc > 0))
                                                    {
                                                        decimal TotalDiscPerAmt = dtlTax.POT_TAX_AMT / totalLineitemDisc;
                                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinLine * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                                    }
                                                    FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                                    retval = maxPK;
                                                    maxPK++;

                                                });
                                            }
                                        });
                                    }

                                    PUR_ORDER_TAX_HDRList = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(k => (k.PTH_TAX_CATEGORY == (byte)TaxType.Tax || k.PTH_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                    if (PUR_ORDER_TAX_HDRList.Count > 0)
                                    {
                                        PUR_ORDER_TAX_HDRList.ForEach(dtlTax =>
                                        {
                                            FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                            //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_SOD = dtl.SOD_PK;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_VND_PO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_VND_PO_MPG;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.PTH_TAX;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.PTH_TAX_CATEGORY;
                                            if (dtlTax.PTH_TAX_CATEGORY != (byte)TaxType.Discount && (totalHdritemTax > 0))
                                            {
                                                decimal TotalTaxPerAmt = dtlTax.PTH_TAX_AMT / totalHdritemTax;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinHDR * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                            }
                                            else if (dtlTax.PTH_TAX_CATEGORY == (byte)TaxType.Discount && (totalHdritemDisc > 0))
                                            {
                                                decimal TotalDiscPerAmt = dtlTax.PTH_TAX_AMT / totalHdritemDisc;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinHDR * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                            }

                                            FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                            retval = maxPK;
                                            maxPK++;

                                        });
                                    }


                                }//else //UPDATE EXISTING RECORD
                                #endregion
                            }
                            if (woPK.HasValue) //Currently detail level tax not exist. If need to detail level tax, rework the commented code accordingly
                            {
                                #region Work Order Section - Commented
                                //obj_INV_WORK_ORDER_ITEM_HDR = currentEntity.INV_WORK_ORDER_ITEM_HDR.SingleOrDefault(a => a.WIH_PK == woPK);
                                //INV_WORK_ORDER_ITEM_DTLList = obj_INV_WORK_ORDER_ITEM_HDR.INV_WORK_ORDER_ITEM_DTL.ToList();
                                //Totaltax = INV_WORK_ORDER_ITEM_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == 1).Sum(s => s.POT_TAX_AMT)) + obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == 1).Sum(o => o.PTH_TAX_AMT);
                                //totalLineitemTax = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == 1).Sum(s => s.POT_TAX_AMT));
                                //totalHdritemTax = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == 1).Sum(o => o.PTH_TAX_AMT);
                                //if (Totaltax > 0)
                                //{
                                //    TaxApplcableinLine = (totalLineitemTax / Totaltax) * splitTaxAMOUNT;
                                //    TaxApplcableinHDR = (totalHdritemTax / Totaltax) * splitTaxAMOUNT;
                                //}

                                ////Discount
                                //TotalDisc = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.POT_TAX_AMT)) + obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.PTH_TAX_AMT);
                                //totalLineitemDisc = PUR_ORDER_DTLList.Sum(o => o.PUR_ORDER_TAX_DTL.Where(w => w.POT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.POT_TAX_AMT));
                                //totalHdritemDisc = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(g => g.PTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.PTH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                                //if (TotalDisc > 0)
                                //{
                                //    discApplcableinLine = (totalLineitemDisc / TotalDisc) * splitDiscAMOUNT;
                                //    discApplcableinHDR = (totalHdritemDisc / TotalDisc) * splitDiscAMOUNT;

                                //}

                                //if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PK == 0) //  INSERT NEW RECORD
                                //{

                                //    if (PUR_ORDER_DTLList.Count > 0)
                                //    {

                                //        PUR_ORDER_DTLList.ForEach(dtl =>
                                //        {

                                //            PUR_ORDER_TAX_DTLList = dtl.PUR_ORDER_TAX_DTL.Where(k => (k.POT_TAX_CATEGORY == (byte)TaxType.Tax || k.POT_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                //            if (PUR_ORDER_TAX_DTLList.Count > 0)
                                //            {
                                //                PUR_ORDER_TAX_DTLList.ForEach(dtlTax =>
                                //                {
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_POD = dtl.POD_PK;
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_VND_PO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_VND_PO_MPG;
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.POT_TAX;
                                //                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.POT_TAX_CATEGORY;
                                //                    if (dtlTax.POT_TAX_CATEGORY != (byte)TaxType.Discount && (totalLineitemTax > 0))
                                //                    {
                                //                        decimal TotalTaxPerAmt = dtlTax.POT_TAX_AMT / totalLineitemTax;
                                //                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinLine * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                //                    }
                                //                    else if (dtlTax.POT_TAX_CATEGORY == (byte)TaxType.Discount && (totalLineitemDisc > 0))
                                //                    {
                                //                        decimal TotalDiscPerAmt = dtlTax.POT_TAX_AMT / totalLineitemDisc;
                                //                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinLine * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                //                    }
                                //                    FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                //                    retval = maxPK;
                                //                    maxPK++;

                                //                });
                                //            }
                                //        });
                                //    }

                                //    PUR_ORDER_TAX_HDRList = obj_PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.Where(k => (k.PTH_TAX_CATEGORY == (byte)TaxType.Tax || k.PTH_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                //    if (PUR_ORDER_TAX_HDRList.Count > 0)
                                //    {
                                //        PUR_ORDER_TAX_HDRList.ForEach(dtlTax =>
                                //        {
                                //            FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                //            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                //            //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_SOD = dtl.SOD_PK;
                                //            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                //            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_VND_PO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_VND_PO_MPG;
                                //            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.PTH_TAX;
                                //            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.PTH_TAX_CATEGORY;
                                //            if (dtlTax.PTH_TAX_CATEGORY != (byte)TaxType.Discount && (totalHdritemTax > 0))
                                //            {
                                //                decimal TotalTaxPerAmt = dtlTax.PTH_TAX_AMT / totalHdritemTax;
                                //                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinHDR * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                //            }
                                //            else if (dtlTax.PTH_TAX_CATEGORY == (byte)TaxType.Discount && (totalHdritemDisc > 0))
                                //            {
                                //                decimal TotalDiscPerAmt = dtlTax.PTH_TAX_AMT / totalHdritemDisc;
                                //                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinHDR * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                //            }

                                //            FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                //            retval = maxPK;
                                //            maxPK++;

                                //        });
                                //    }


                                //}//else //UPDATE EXISTING RECORD
                                #endregion
                            }

                        }
                        #endregion
                        #region Inv
                        else if (Category == (Byte)(POInvoiceCategory.Invoice))
                        {
                            obj_FIN_INVOICE_VND_HDR = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(a => a.IVH_PK == invoiceHdr);
                            FIN_INVOICE_VND_DTLList = obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.ToList();
                            Totaltax = FIN_INVOICE_VND_DTLList.Sum(o => o.FIN_INVOICE_VND_TAX_DTL.Where(w => w.VTL_TAX_CATEGORY == (byte)TaxType.Tax).Sum(s => s.VTL_TAX_AMT)) + obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(g => g.VTH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(o => o.VTH_TAX_AMT);
                            totalLineitemTax = FIN_INVOICE_VND_DTLList.Sum(o => o.FIN_INVOICE_VND_TAX_DTL.Where(w => w.VTL_TAX_CATEGORY == (byte)TaxType.Tax).Sum(s => s.VTL_TAX_AMT));
                            totalHdritemTax = obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(g => g.VTH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(o => o.VTH_TAX_AMT);
                            if (Totaltax > 0)
                            {
                                TaxApplcableinLine = (totalLineitemTax / Totaltax) * splitTaxAMOUNT;
                                TaxApplcableinHDR = (totalHdritemTax / Totaltax) * splitTaxAMOUNT;
                            }

                            //Disc
                            TotalDisc = FIN_INVOICE_VND_DTLList.Sum(o => o.FIN_INVOICE_VND_TAX_DTL.Where(w => w.VTL_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.VTL_TAX_AMT)) + obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(g => g.VTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.VTH_TAX_AMT);
                            totalLineitemDisc = FIN_INVOICE_VND_DTLList.Sum(o => o.FIN_INVOICE_VND_TAX_DTL.Where(w => w.VTL_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.VTL_TAX_AMT));
                            totalHdritemDisc = obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(g => g.VTH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.VTH_TAX_AMT);
                            if (TotalDisc > 0)
                            {
                                discApplcableinLine = (totalLineitemDisc / TotalDisc) * splitDiscAMOUNT;
                                discApplcableinHDR = (totalHdritemDisc / TotalDisc) * splitDiscAMOUNT;
                            }


                            if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PK == 0) //  INSERT NEW RECORD
                            {

                                if (FIN_INVOICE_VND_DTLList.Count > 0)
                                {

                                    FIN_INVOICE_VND_DTLList.ForEach(dtl =>
                                    {

                                        FIN_INVOICE_VND_TAX_DTLList = dtl.FIN_INVOICE_VND_TAX_DTL.Where(k => (k.VTL_TAX_CATEGORY == (byte)TaxType.Tax || k.VTL_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();

                                        if (FIN_INVOICE_VND_TAX_DTLList.Count > 0)
                                        {
                                            FIN_INVOICE_VND_TAX_DTLList.ForEach(dtlTax =>
                                            {
                                                FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                                //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_SOD = dtl.SOD_PK;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_INVOICE_VND_DTL = dtl.VID_PK;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                                //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_CUS_SO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_CUS_SO_MPG;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.VTL_TAX;
                                                FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.VTL_TAX_CATEGORY;
                                                if (dtlTax.VTL_TAX_CATEGORY != (byte)TaxType.Discount && (totalLineitemTax > 0))
                                                {
                                                    decimal TotalTaxPerAmt = dtlTax.VTL_TAX_AMT / totalLineitemTax;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinLine * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                }
                                                else if (dtlTax.VTL_TAX_CATEGORY == (byte)TaxType.Discount && (totalLineitemDisc > 0))
                                                {
                                                    decimal TotalDiscPerAmt = dtlTax.VTL_TAX_AMT / totalLineitemDisc;
                                                    FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinLine * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                                }


                                                FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                                retval = maxPK;
                                                maxPK++;

                                            });
                                        }
                                    });
                                }
                                FIN_INVOICE_VND_TAX_HDRList = obj_FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(k => (k.VTH_TAX_CATEGORY == (byte)TaxType.Tax) || (k.VTH_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                if (FIN_INVOICE_VND_TAX_HDRList.Count > 0)
                                {
                                    FIN_INVOICE_VND_TAX_HDRList.ForEach(dtlTax =>
                                    {
                                        FIN_PAYMENT_VND_TAX_DTL_obj = new FIN_PAYMENT_VND_TAX_DTL();

                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PK = Convert.ToInt32(maxPK.Value);
                                        //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_SOD = dtl.SOD_PK;
                                        //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_INVOICE_CUS_DTL = dtlTax.CID_PK;   
                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_PAYMENT_TRX;
                                        //FIN_PAYMENT_VND_TAX_DTL_obj.PDT_CUS_SO_MPG = FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_CUS_SO_MPG;
                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX = dtlTax.VTH_TAX;

                                        FIN_PAYMENT_VND_TAX_DTL_obj.PDT_TAX_CATEGORY = dtlTax.VTH_TAX_CATEGORY;
                                        if (dtlTax.VTH_TAX_CATEGORY != (byte)TaxType.Discount && (totalHdritemTax > 0))
                                        {
                                            decimal TotalTaxPerAmt = dtlTax.VTH_TAX_AMT / totalHdritemTax;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinHDR * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                        }
                                        else if (dtlTax.VTH_TAX_CATEGORY == (byte)TaxType.Discount && (totalHdritemDisc > 0))
                                        {
                                            decimal TotalDiscPerAmt = dtlTax.VTH_TAX_AMT / totalHdritemDisc;
                                            FIN_PAYMENT_VND_TAX_DTL_obj.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinHDR * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                        }


                                        FIN_PAYMENT_VND_TAX_DTL_List.Add(FIN_PAYMENT_VND_TAX_DTL_obj);
                                        retval = maxPK;
                                        maxPK++;

                                    });
                                }
                                //////////////////////////////////////////////////////////////////////////////////////////////////


                            }//else //UPDATE EXISTING RECORD
                        }
                        #endregion


                        FIN_PAYMENT_VND_TAX_DTL_List.ForEach(dtl => currentEntity.FIN_PAYMENT_VND_TAX_DTL.AddObject(dtl));

                    }
                }
                maxID = maxPK;
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

    }
}
