using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessLogic.BrandRates;
using BusinessObject.OrderToCash;
using BusinessObject.CommonManagement;
using BusinessLogic.OrderToCash;

namespace CustomerPortal.OrderToCash
{
    public partial class BrandCopy : ERP.Store.UI.WorkFlowBasePage
	{
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }      
        #endregion
      
        DataTable dtCustomers;   
        BusinessObject.User currentUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        private BrandCopyHeader objbrandCopyHeader;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();            
        }   
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Method to handle Page Load Action
        /// </summary>
        public void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.CUSTOMERS);
                    BindTree(ControlsEnum.CUSTOMERS);
                    foreach (TreeNode node in trvCustomers.Nodes)
                    {
                        node.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion       

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.CUSTOMERS:                      
                        dtCustomers = BrandRatesBL.GetCustomerList(currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE));
                        break;               
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {                                 
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods       
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null; 
            try
            {
                switch (mode)
                {
                    #region COPY
                    case ActionsEnum.COPY:
                        objbrandCopyHeader = new BrandCopyHeader();
                        objbrandCopyHeader.CUS_PK = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        objbrandCopyHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objbrandCopyHeader.Details = new List<BrandCopyDetails>();
                        List<BrandCopyDetails> detailsList = new List<BrandCopyDetails>();
                                             
                        foreach (TreeNode node in trvCustomers.Nodes)
                        {
                            foreach (TreeNode child1 in node.ChildNodes)
                            {
                                if (child1.Checked)
                                {
                                    BrandCopyDetails objBrandCopyDetails = new BrandCopyDetails();
                                    objBrandCopyDetails.CIM_CUSTOMER = Convert.ToInt32(child1.Value);                                    
                                    detailsList.Add(objBrandCopyDetails);
                                }
                            }
                        }
                        objbrandCopyHeader.Details = detailsList;
                        returnObj = objbrandCopyHeader;
                        break;
                    #endregion                   
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }     
        #region BindTree
        /// <summary>
        /// Bind Tree view
        /// </summary>
        /// <param name="type"></param>
        private void BindTree(ControlsEnum type)
        {
            TreeNode child;
            TreeNode root;
            switch (type)
            {
                #region CUSTOMERS
                case ControlsEnum.CUSTOMERS:
                    trvCustomers.Nodes.Clear();
                    root = new TreeNode(GetLocalResourceObject("AllCustomers").ToString(), "0");
                    root.NavigateUrl = "javascript:return false;";
                    root.ShowCheckBox = true;
                    trvCustomers.Nodes.Add(root);
                    if (dtCustomers != null)
                    {
                        foreach (DataRow row in dtCustomers.Rows)
                        {
                            child = new TreeNode(HttpUtility.HtmlDecode(row["CUS_NAME"].ToString()), row["CUS_PK"].ToString());
                            child.ShowCheckBox = true;
                            child.NavigateUrl = "javascript:return false;";
                            child.ToolTip = HttpUtility.HtmlDecode(row["CUS_NAME"].ToString());
                            root.ChildNodes.Add(child);
                        }
                        root.ExpandAll();
                    }
                    break;
                #endregion
            }
        } 
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    objbrandCopyHeader = null;
                    txtCustomer.Text = string.Empty;
                    hdfCustomerID.Value = "0";
                    ClearTree(ControlsEnum.CUSTOMERS);
                    break;
                #endregion
            }
        }
        #endregion
        /// <summary>
        /// Clear Tree
        /// </summary>
        #region ClearTree
        private void ClearTree(ControlsEnum type)
        {
            switch (type)
            {               
                case ControlsEnum.CUSTOMERS:                   
                    foreach (TreeNode node in trvCustomers.Nodes)
                    {
                        foreach (TreeNode child1 in node.ChildNodes)
                        {
                            child1.Checked = false;
                        }
                    }
                    break;
            }
        } 
        #endregion
        #endregion      

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int? result;   
            try
            {                
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region COPY
                    case ActionsEnum.COPY:                        
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            if (!string.IsNullOrEmpty(hdfCustomerID.Value))
                            {
                                #region COPY Details
                                objbrandCopyHeader = (BrandCopyHeader)SetUIValuesToObject(ActionsEnum.COPY);
                                if (objbrandCopyHeader != null && objbrandCopyHeader.Details.Count > 0)
                                {
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<BrandCopyHeader>(objbrandCopyHeader);
                                    result = BrandCopyBL.SaveBrandCopyDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Brand_Copy_Success").ToString();                          
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                    }
                                    else
                                    {
                                        #region Error/Validation
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BrandCopy + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text  + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BrandCopy + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrandCopy);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Select_To_Customer").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                #endregion
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Customer").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }                                                    
                        }
                        break;
                    #endregion                 
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
            finally
            {

            }
        }
        #endregion

        #region Page PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            SAVE,
            CANCEL,          
            CUSTOMERS,
            CLEAR
        }

        #endregion
	}
    	
}