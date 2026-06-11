using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinCrDrHdrNoteMpgService" in code, svc and config file together.
    public class FinCrDrHdrNoteMpgService : IFinCrDrHdrNoteMpgService,IFinCrDrHdrNoteMpgManager
    {
         #region Private Variables
        ERPEntities  currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public FinCrDrHdrNoteMpgService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList, byte CrDrType)
        {
            FinCrDrHdrNoteMpgManager objFinCrDrHdrNoteMpgManager;
            try
            {
                objFinCrDrHdrNoteMpgManager = new FinCrDrHdrNoteMpgManager(currentContext);
                return objFinCrDrHdrNoteMpgManager.SaveFinCrDrNoteMpg(finCrDrNoteMpgList,CrDrType);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteMpgManager = null;
            }
        }
        public List<FIN_CRDR_NOTE_MPG> GetFinCrDrNoteMpg(long finCrDrNotePK)
        {
            FinCrDrHdrNoteMpgManager objFinCrDrHdrNoteMpgManager;
            try
            {
                objFinCrDrHdrNoteMpgManager = new FinCrDrHdrNoteMpgManager(currentContext);
                return objFinCrDrHdrNoteMpgManager.GetFinCrDrNoteMpg(finCrDrNotePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteMpgManager = null;
            }
        }

        public List<FIN_INVOICE_VND_HDR> GetFinCrDrNoteMpg(List<long> InvoicePK)
        {
            FinCrDrHdrNoteMpgManager objFinCrDrHdrNoteMpgManager;
            try
            {
                objFinCrDrHdrNoteMpgManager = new FinCrDrHdrNoteMpgManager(currentContext);
                return objFinCrDrHdrNoteMpgManager.GetFinCrDrNoteMpg(InvoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteMpgManager = null;
            }
        }

      
        #endregion



        public long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList)
        {
            throw new NotImplementedException();
        }
    }
}
