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
using System.Threading;

namespace ERPManager.Sales
{
    public class FinReceiptCustaxDtlManager : IFinReceiptCustaxDtlManager
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

        public FinReceiptCustaxDtlManager(ERPEntities currentEntity)
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

        public long? SaveReceiptTax(List<FIN_RECEIPT_CUS_TAX_DTL> finReceiptCusTaxDtlList, ref long? maxID, int soHdr, decimal splitTaxAMOUNT, Byte Category, long? invoiceHdr, decimal? discTotAmount)
        {
            long? retval;
            retval = 0;

            long? maxPK;
            maxPK = maxID;
            List<FIN_RECEIPT_CUS_TAX_DTL> Old_finReceiptCusTaxDtlList;

            SAL_ORDER_HDR obj_SAL_ORDER_HDR;
            List<SAL_ORDER_DTL> SAL_ORDER_DTLList;
            List<SAL_ORDER_TAX_DTL> SAL_ORDER_TAX_DTLList;
            List<SAL_ORDER_TAX_HDR> SAL_ORDER_TAX_HDRList;
            FIN_RECEIPT_CUS_TAX_DTL FIN_RECEIPT_CUS_TAX_DTL_obj;

            FIN_INVOICE_CUS_HDR obj_FIN_INVOICE_CUS_HDR;//SAL_ORDER_HDR obj_SAL_ORDER_HDR;
            List<FIN_INVOICE_CUS_DTL> FIN_INVOICE_CUS_DTLList;//SAL_ORDER_DTL> SAL_ORDER_DTLList
            List<FIN_INVOICE_CUS_TAX_DTL> FIN_INVOICE_CUS_TAX_DTLList;//            List<SAL_ORDER_TAX_DTL> SAL_ORDER_TAX_DTLList;
            List<FIN_INVOICE_CUS_TAX_HDR> FIN_INVOICE_CUS_TAX_HDRList;//List<SAL_ORDER_TAX_HDR> SAL_ORDER_TAX_HDRList;
            try
            {
                if (finReceiptCusTaxDtlList.Count > 0)
                {

                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;
                    if (!maxPK.HasValue || maxPK.Value == 0)
                        maxPK = currentEntity.FIN_RECEIPT_CUS_TAX_DTL.Max(v => (long?)v.RDT_PK);
                    maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;
                    //  Delete Removed Entries

                    List<int> pks = (from old1 in finReceiptCusTaxDtlList
                                     select old1.RDT_PK).ToList();
                    /**/
                    long RSO_RECEIPT_TRX_MPG = finReceiptCusTaxDtlList[0].RDT_RECEIPT_TRX;

                    Old_finReceiptCusTaxDtlList = (from oldp in this.currentEntity.FIN_RECEIPT_CUS_TAX_DTL
                                                   where oldp.RDT_RECEIPT_TRX == RSO_RECEIPT_TRX_MPG
                                                   //&& !pks.Contains(oldp.RDT_PK)
                                                   select oldp).ToList();
                    //Delete existing records against receipt
                    foreach (FIN_RECEIPT_CUS_TAX_DTL Old_finReceiptCusTaxDtlObj in Old_finReceiptCusTaxDtlList)
                    {
                        this.currentEntity.FIN_RECEIPT_CUS_TAX_DTL.DeleteObject(Old_finReceiptCusTaxDtlObj);
                    }

                    List<FIN_RECEIPT_CUS_TAX_DTL> FIN_RECEIPT_CUS_TAX_DTL_List = new List<FIN_RECEIPT_CUS_TAX_DTL>(); ;
                    //oldReceiptHdrObj.RCH_GROUP = finReceiptCusHdrObj.RCH_GROUP;
                    //obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == old_FIN_RECEIPT_CUS_SO_MPG.RSO_SO_HDR);
                    foreach (FIN_RECEIPT_CUS_TAX_DTL FIN_RECEIPT_CUS_TAX_DTL_obj1 in finReceiptCusTaxDtlList)
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

                        // currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(a => a.FIN_RECEIPT_CUS_TRX_MPG.Any(s => s.RCM_PK == FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX));//.Where(b => b.RCM_PK == FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX)).RCM_RECEIPT_HDR);
                        #region Adv Inv
                        if (Category != (Byte)(SalesInvoiceCategory.Invoice))
                        {
                            obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == soHdr);
                            SAL_ORDER_DTLList = obj_SAL_ORDER_HDR.SAL_ORDER_DTL.ToList();
                            //if (SAL_ORDER_DTLList.Any(d => d.SAL_ORDER_TAX_DTL.Count > 0))
                            //{
                            Totaltax = SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Where(w => w.SLT_TAX_CATEGORY == (byte)TaxType.Tax).Sum(s => s.SLT_TAX_AMT)) + obj_SAL_ORDER_HDR.SAL_ORDER_TAX_HDR.Where(g => g.TSH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(o => o.TSH_TAX_AMT);
                            totalLineitemTax = SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Where(w => w.SLT_TAX_CATEGORY == (byte)TaxType.Tax).Sum(s => s.SLT_TAX_AMT));
                            totalHdritemTax = obj_SAL_ORDER_HDR.SAL_ORDER_TAX_HDR.Where(g => g.TSH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(o => o.TSH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                            if (Totaltax > 0)
                            {
                                //Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                TaxApplcableinLine = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble((totalLineitemTax / Totaltax) * splitTaxAMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                TaxApplcableinHDR = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble((totalHdritemTax / Totaltax) * splitTaxAMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            }

                            TotalDisc = SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Where(w => w.SLT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.SLT_TAX_AMT)) + obj_SAL_ORDER_HDR.SAL_ORDER_TAX_HDR.Where(g => g.TSH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.TSH_TAX_AMT);
                            totalLineitemDisc = SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Where(w => w.SLT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.SLT_TAX_AMT));
                            totalHdritemDisc = obj_SAL_ORDER_HDR.SAL_ORDER_TAX_HDR.Where(g => g.TSH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.TSH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                            if (TotalDisc > 0)
                            {
                                discApplcableinLine = (totalLineitemDisc / TotalDisc) * splitDiscAMOUNT;
                                discApplcableinHDR = (totalHdritemDisc / TotalDisc) * splitDiscAMOUNT;
                            }

                            if (FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_PK == 0) //  INSERT NEW RECORD
                            {

                                if (SAL_ORDER_DTLList.Count > 0)
                                {

                                    SAL_ORDER_DTLList.ForEach(dtl =>
                                    {

                                        SAL_ORDER_TAX_DTLList = dtl.SAL_ORDER_TAX_DTL.Where(k => (k.SLT_TAX_CATEGORY == (byte)TaxType.Tax || k.SLT_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();

                                        if (SAL_ORDER_TAX_DTLList.Count > 0)
                                        {
                                            SAL_ORDER_TAX_DTLList.ForEach(dtlTax =>
                                            {
                                                FIN_RECEIPT_CUS_TAX_DTL_obj = new FIN_RECEIPT_CUS_TAX_DTL();

                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_PK = Convert.ToInt32(maxPK.Value);
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_SOD = dtl.SOD_PK;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_CUS_SO_MPG;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX = dtlTax.SLT_TAX;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX_CATEGORY = dtlTax.SLT_TAX_CATEGORY;
                                                if (dtlTax.SLT_TAX_CATEGORY != (byte)TaxType.Discount && (totalLineitemTax > 0))
                                                {
                                                    if (totalLineitemTax > 0)
                                                    {
                                                        decimal TotalTaxPerAmt = dtlTax.SLT_TAX_AMT / totalLineitemTax;
                                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinLine * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                    }
                                                }
                                                else if (dtlTax.SLT_TAX_CATEGORY == (byte)TaxType.Discount && (totalLineitemDisc > 0))
                                                {
                                                    if (totalLineitemDisc > 0)
                                                    {
                                                        decimal TotalDiscPerAmt = dtlTax.SLT_TAX_AMT / totalLineitemDisc;
                                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinLine * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                    }
                                                }
                                                FIN_RECEIPT_CUS_TAX_DTL_List.Add(FIN_RECEIPT_CUS_TAX_DTL_obj);
                                                retval = maxPK;
                                                maxPK++;

                                            });
                                        }
                                    });
                                }
                                SAL_ORDER_TAX_HDRList = obj_SAL_ORDER_HDR.SAL_ORDER_TAX_HDR.Where(k => (k.TSH_TAX_CATEGORY == (byte)TaxType.Tax || k.TSH_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                if (SAL_ORDER_TAX_HDRList.Count > 0)
                                {
                                    SAL_ORDER_TAX_HDRList.ForEach(dtlTax =>
                                    {
                                        FIN_RECEIPT_CUS_TAX_DTL_obj = new FIN_RECEIPT_CUS_TAX_DTL();

                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_PK = Convert.ToInt32(maxPK.Value);
                                        //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_SOD = dtl.SOD_PK;
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX;
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_CUS_SO_MPG;
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX = dtlTax.TSH_TAX;
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX_CATEGORY = dtlTax.TSH_TAX_CATEGORY;
                                        if (dtlTax.TSH_TAX_CATEGORY != (byte)TaxType.Discount && (totalHdritemTax > 0))
                                        {
                                            if (totalHdritemTax > 0)
                                            {
                                                decimal TotalTaxPerAmt = dtlTax.TSH_TAX_AMT / totalHdritemTax;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinHDR * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                            }
                                        }
                                        else if (dtlTax.TSH_TAX_CATEGORY == (byte)TaxType.Discount && (totalHdritemDisc > 0))
                                        {
                                            if (totalHdritemDisc > 0)
                                            {
                                                decimal TotalDiscPerAmt = dtlTax.TSH_TAX_AMT / totalHdritemDisc;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinHDR * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                            }
                                        }

                                        FIN_RECEIPT_CUS_TAX_DTL_List.Add(FIN_RECEIPT_CUS_TAX_DTL_obj);
                                        retval = maxPK;
                                        maxPK++;

                                    });
                                }
                                //////////////////////////////////////////////////////////////////////////////////////////////////


                            }//else //UPDATE EXISTING RECORD
                            //}
                        }
                        #endregion
                        #region Inv
                        else if (Category == (Byte)(SalesInvoiceCategory.Invoice))
                        {
                            obj_FIN_INVOICE_CUS_HDR = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(a => a.ICH_PK == invoiceHdr);//currentEntity.FIN_RECEIPT_CUS_TRX_MPG.Where(h=>h.RCM_PK==FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX).Any(s=>s.RCM_INVOICE_HDR))// FIN_RECEIPT_CUS_TAX_DTL_obj1.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR);
                            FIN_INVOICE_CUS_DTLList = obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.ToList();
                            //if (FIN_INVOICE_CUS_DTLList.Any(j => j.FIN_INVOICE_CUS_TAX_DTL.Count > 0))
                            //{
                            Totaltax = FIN_INVOICE_CUS_DTLList.Sum(o => o.FIN_INVOICE_CUS_TAX_DTL.Where(w => w.CIT_TAX_CATEGORY == 1).Sum(s => s.CIT_TAX_AMT)) + obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(g => g.ISH_TAX_CATEGORY == 1).Sum(o => o.ISH_TAX_AMT);
                            totalLineitemTax = FIN_INVOICE_CUS_DTLList.Sum(o => o.FIN_INVOICE_CUS_TAX_DTL.Where(w => w.CIT_TAX_CATEGORY == 1).Sum(s => s.CIT_TAX_AMT));
                            totalHdritemTax = obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(g => g.ISH_TAX_CATEGORY == 1).Sum(o => o.ISH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                            if (Totaltax > 0)
                            {
                                TaxApplcableinLine = (totalLineitemTax / Totaltax) * splitTaxAMOUNT;
                                TaxApplcableinHDR = (totalHdritemTax / Totaltax) * splitTaxAMOUNT;
                            }


                            TotalDisc = FIN_INVOICE_CUS_DTLList.Sum(o => o.FIN_INVOICE_CUS_TAX_DTL.Where(w => w.CIT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.CIT_TAX_AMT)) + obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(g => g.ISH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.ISH_TAX_AMT);
                            totalLineitemDisc = FIN_INVOICE_CUS_DTLList.Sum(o => o.FIN_INVOICE_CUS_TAX_DTL.Where(w => w.CIT_TAX_CATEGORY == (byte)TaxType.Discount).Sum(s => s.CIT_TAX_AMT));
                            totalHdritemDisc = obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(g => g.ISH_TAX_CATEGORY == (byte)TaxType.Discount).Sum(o => o.ISH_TAX_AMT);// SAL_ORDER_DTLList.Sum(o => o.SAL_ORDER_TAX_DTL.Sum(s => s.SLT_TAX_AMT));
                            if (TotalDisc > 0)
                            {
                                discApplcableinLine = (totalLineitemDisc / TotalDisc) * splitDiscAMOUNT;
                                discApplcableinHDR = (totalHdritemDisc / TotalDisc) * splitDiscAMOUNT;
                            }

                            if (FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_PK == 0) //  INSERT NEW RECORD
                            {

                                if (FIN_INVOICE_CUS_DTLList.Count > 0)
                                {

                                    FIN_INVOICE_CUS_DTLList.ForEach(dtl =>
                                    {

                                        FIN_INVOICE_CUS_TAX_DTLList = dtl.FIN_INVOICE_CUS_TAX_DTL.Where(k => (k.CIT_TAX_CATEGORY == (byte)TaxType.Tax || k.CIT_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();

                                        if (FIN_INVOICE_CUS_TAX_DTLList.Count > 0)
                                        {
                                            FIN_INVOICE_CUS_TAX_DTLList.ForEach(dtlTax =>
                                            {
                                                FIN_RECEIPT_CUS_TAX_DTL_obj = new FIN_RECEIPT_CUS_TAX_DTL();

                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_PK = Convert.ToInt32(maxPK.Value);
                                                //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_SOD = dtl.SOD_PK;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_INVOICE_CUS_DTL = dtl.CID_PK;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX;
                                                //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_CUS_SO_MPG;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX = dtlTax.CIT_TAX;
                                                FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX_CATEGORY = dtlTax.CIT_TAX_CATEGORY;
                                                if (dtlTax.CIT_TAX_CATEGORY != (byte)TaxType.Discount)
                                                {
                                                    decimal TotalTaxPerAmt = 0;
                                                    if (totalLineitemTax > 0)
                                                    {
                                                        TotalTaxPerAmt = dtlTax.CIT_TAX_AMT / totalLineitemTax;
                                                    }

                                                    FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinLine * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                }
                                                else
                                                {
                                                    decimal TotalDiscPerAmt = 0;
                                                    if (totalLineitemDisc > 0)
                                                    {
                                                        TotalDiscPerAmt = dtlTax.CIT_TAX_AMT / totalLineitemDisc;
                                                    }
                                                    FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinLine * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                                }



                                                FIN_RECEIPT_CUS_TAX_DTL_List.Add(FIN_RECEIPT_CUS_TAX_DTL_obj);
                                                retval = maxPK;
                                                maxPK++;

                                            });
                                        }
                                    });
                                }
                                FIN_INVOICE_CUS_TAX_HDRList = obj_FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(k => (k.ISH_TAX_CATEGORY == (byte)TaxType.Tax || k.ISH_TAX_CATEGORY == (byte)TaxType.Discount)).ToList();
                                if (FIN_INVOICE_CUS_TAX_HDRList.Count > 0)
                                {
                                    FIN_INVOICE_CUS_TAX_HDRList.ForEach(dtlTax =>
                                    {
                                        FIN_RECEIPT_CUS_TAX_DTL_obj = new FIN_RECEIPT_CUS_TAX_DTL();

                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_PK = Convert.ToInt32(maxPK.Value);
                                        //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_SOD = dtl.SOD_PK;
                                        //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_INVOICE_CUS_DTL = dtlTax.CID_PK;   
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_RECEIPT_TRX;
                                        //FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_TAX_DTL_obj1.RDT_CUS_SO_MPG;
                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX = dtlTax.ISH_TAX;

                                        FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_TAX_CATEGORY = dtlTax.ISH_TAX_CATEGORY;
                                        if (dtlTax.ISH_TAX_CATEGORY != (byte)TaxType.Discount)
                                        {
                                            decimal TotalTaxPerAmt = 0;
                                            if (totalLineitemTax > 0)
                                            {
                                                TotalTaxPerAmt = dtlTax.ISH_TAX_AMT / totalHdritemTax;
                                            }

                                            FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(TaxApplcableinHDR * TotalTaxPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                        }
                                        else
                                        {
                                            decimal TotalDiscPerAmt = 0;
                                            if (totalLineitemDisc > 0)
                                            {
                                                TotalDiscPerAmt = dtlTax.ISH_TAX_AMT / totalHdritemDisc;
                                            }
                                            FIN_RECEIPT_CUS_TAX_DTL_obj.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(discApplcableinHDR * TotalDiscPerAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                        }



                                        FIN_RECEIPT_CUS_TAX_DTL_List.Add(FIN_RECEIPT_CUS_TAX_DTL_obj);
                                        retval = maxPK;
                                        maxPK++;

                                    });
                                }
                                //////////////////////////////////////////////////////////////////////////////////////////////////

                            }
                        }//else //UPDATE EXISTING RECORD
                    }
                        #endregion
                    FIN_RECEIPT_CUS_TAX_DTL_List.ForEach(dtl => 
                        {
                            if (dtl.RDT_AMOUNT > 0)
                            currentEntity.FIN_RECEIPT_CUS_TAX_DTL.AddObject(dtl);
                        });
                }
                //}
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
