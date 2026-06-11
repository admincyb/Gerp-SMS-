using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;

namespace ERPManager.Finance
{
    public class FinCrDrNoteTaxHdrManager : IFinCrDrNoteTaxHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        public FinCrDrNoteTaxHdrManager()
        {
            try
            {
                currentEntity = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public FinCrDrNoteTaxHdrManager(ERPEntities context)
        {
            try
            {
                currentEntity = context;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public long? SaveCrDrNoteTaxHdr(List<FIN_CRDR_NOTE_TAX_HDR> finCrDrNoteTaxHdr,long? headerPK)
        {
            //Holds save status
            long retval;
            //Holds last Payment Mpg master pk
            long? maxCrDrNoteTaxPK;
            //On update holds old Payment Mpg master details
            FIN_CRDR_NOTE_TAX_HDR oldfinCrDrNoteTaxObj;
            List<FIN_CRDR_NOTE_TAX_HDR> oldfinCrDrNoteTaxListObj;

            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxCrDrNoteTaxPK = currentEntity.FIN_CRDR_NOTE_TAX_HDR.Max(v => (int?)v.NTH_PK);
                maxCrDrNoteTaxPK = (maxCrDrNoteTaxPK.HasValue) ? maxCrDrNoteTaxPK.Value + 1 : 1;
                //maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                //Iterate through vedor list for save


                List<long> pks = (from old1 in finCrDrNoteTaxHdr
                                  select old1.NTH_PK).ToList();
               

                oldfinCrDrNoteTaxListObj = (from old in this.currentEntity.FIN_CRDR_NOTE_TAX_HDR
                                            where !pks.Contains(old.NTH_PK) && headerPK == old.NTH_CRDR_NOTE_HDR 
                                            select old).ToList();
                foreach (FIN_CRDR_NOTE_TAX_HDR oldfinCrDrNoteTaxHdrObj in oldfinCrDrNoteTaxListObj)
                {
                    this.currentEntity.FIN_CRDR_NOTE_TAX_HDR.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                }


                foreach (FIN_CRDR_NOTE_TAX_HDR finCrDrNoteTaxObj in finCrDrNoteTaxHdr)
                {
                    //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                    if (finCrDrNoteTaxObj.NTH_PK == 0)
                    {
                        FIN_CRDR_NOTE_TAX_HDR objTaxHdr = new FIN_CRDR_NOTE_TAX_HDR();

                        //Set next Payment Mpg pk
                        objTaxHdr.NTH_PK = (long)maxCrDrNoteTaxPK;

                        objTaxHdr.NTH_NAME = finCrDrNoteTaxObj.NTH_NAME;
                        objTaxHdr.NTH_TAX = finCrDrNoteTaxObj.NTH_TAX;
                        objTaxHdr.NTH_TAX_AMT = finCrDrNoteTaxObj.NTH_TAX_AMT;
                        objTaxHdr.NTH_TAX_CATEGORY = finCrDrNoteTaxObj.NTH_TAX_CATEGORY;
                        objTaxHdr.NTH_TYPE = finCrDrNoteTaxObj.NTH_TYPE;
                        objTaxHdr.NTH_CRDR_NOTE_HDR = finCrDrNoteTaxObj.NTH_CRDR_NOTE_HDR;

                        //Add new Payment Mpg to the db context
                        currentEntity.FIN_CRDR_NOTE_TAX_HDR.AddObject(objTaxHdr);
                        maxCrDrNoteTaxPK++;
                        retval = finCrDrNoteTaxObj.NTH_PK;
                    }
                    //updating Payment Mpg details
                    else
                    {
                        //Get current Payment Mpg master details using Payment Mpg master pk
                        oldfinCrDrNoteTaxObj = currentEntity.FIN_CRDR_NOTE_TAX_HDR.Single(x => x.NTH_PK == finCrDrNoteTaxObj.NTH_PK);
                        if (oldfinCrDrNoteTaxObj != null)
                        {
                            oldfinCrDrNoteTaxObj.NTH_NAME = finCrDrNoteTaxObj.NTH_NAME;
                            oldfinCrDrNoteTaxObj.NTH_TAX = finCrDrNoteTaxObj.NTH_TAX;
                            oldfinCrDrNoteTaxObj.NTH_TAX_AMT = finCrDrNoteTaxObj.NTH_TAX_AMT;
                            oldfinCrDrNoteTaxObj.NTH_TAX_CATEGORY = finCrDrNoteTaxObj.NTH_TAX_CATEGORY;
                            oldfinCrDrNoteTaxObj.NTH_TYPE = finCrDrNoteTaxObj.NTH_TYPE;
                            retval = finCrDrNoteTaxObj.NTH_PK;
                        }
                    }
                }
                //return Payment Mpg master pk
                return retval;
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
