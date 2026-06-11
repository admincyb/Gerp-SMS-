using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.SaleOrder;
using GTIService;
using System.Data;
using BusinessObject; 

namespace BusinessLogic.Sales
{
   public class SaleOrderForAgtCommBL
    {
       public static DataSet GetAgentCommInvoiceList(GridPrams grid, User objUser, string pageUrl, int? Status = null, string SP_NAME = null, int? AgentCMSelectType = null)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAgentCommInvoiceList(grid, objUser, pageUrl, Status = null, SP_NAME, AgentCMSelectType);
        }
        /// <summary>
        /// Get Users
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static DataTable GetUsers(string value, int bizunit, int SEARCHBY)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetUsers(value, bizunit, SEARCHBY);
        }

        public static DataTable GetAutoCusScInvForAgentComm(string value, int bizunit, int SEARCHBY)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAutoCusScInvForAgentComm(value, bizunit, SEARCHBY);
        }
        /// <summary>
        /// Get Users for agt comm Transaction
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static DataTable GetAgtCommAuto(string value, int bizunit, int SEARCHBY)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAgtCommAuto(value, bizunit, SEARCHBY);
        }
       /// <summary>
        /// Get agt comm Invoice Details
        /// </summary>
        /// <param name="soPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static AgentCommInvHeader GetAgentCommInvHeader(string XML, int venPK)
        {
            try
            {
                AgentCommInvHeader AgtComminvHeaderObj = new AgentCommInvHeader();
                string AgtComminvoice = DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAgentCommInvHeader(XML, venPK);
                if (AgtComminvoice != string.Empty)
                {
                    AgtComminvHeaderObj = (AgentCommInvHeader)CommonFunctions.DeserializeObject(AgtComminvoice, AgtComminvHeaderObj);
                    return AgtComminvHeaderObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Save Sales Invoice agt comm
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveSalesInvoiceAgtComm(string strxml)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.SaveSalesInvoiceAgtComm(strxml);
        }


          public static int CheckInvoiceAgtComm(int ICHPK)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.CheckInvoiceAgtComm(ICHPK);
        }
        /// <summary>
        /// Get PO Invoice List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetAGTList(GridPrams grid, User objUser, string Customer, int status = 0, byte? pending = null)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAGTList(grid, objUser, Customer, status, pending);
        }
   
    /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteAgtCommDetails(int ivhPK, DateTime lastModDate,string appType,int userPK)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.DeleteAgtCommDetails(ivhPK, lastModDate,appType,userPK);
        }
        /// <summary>
        /// Get AgentCommInvoice Print
        /// </summary>       
        /// <returns>DataSet</returns>
        public static DataSet GetAgentCommInvoicePrint(int ivhPK)
        {
            return DataAccess.SaleOrder.SaleOrderForAgtCommDL.GetAgentCommInvoicePrint(ivhPK);
        }
        
   }
}
