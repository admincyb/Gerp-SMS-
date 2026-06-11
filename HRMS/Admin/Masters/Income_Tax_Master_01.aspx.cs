using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.HRMS.Payroll;
using BusinessObject.Common;
using BusinessObject.HRMS.Employee;
using BusinessObject;
using System.Data;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using BusinessLogic.HRMS.Payroll;
using BusinessObject.CommonManagement;
using System.IO;
using BusinessObject.HRMS.Admin.Masters;
using System.Web.UI.HtmlControls;

namespace HRMS.Admin.Masters
{
    public partial class Income_Tax_Master_01 : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Variables
        private ActionsEnum commonActions;
        User currentUser;
        #endregion
        #region Properties

        private List<Income_Tax_01_Master_BO.Section> IncomeTax_SectionsList
        {
            get
            {
                return ViewState["IncomeTax_Sections"] == null ? new List<Income_Tax_01_Master_BO.Section>() : (List<Income_Tax_01_Master_BO.Section>)ViewState["IncomeTax_Sections"];
            }
            set
            {
                ViewState["IncomeTax_Sections"] = value;
            }
        }
        private Income_Tax_01_Master_BO.Income_Tax_01 objIncomeTax_Header
        {
            get
            {
                return ViewState["IncomeTax_Header"] == null ? new Income_Tax_01_Master_BO.Income_Tax_01() : (Income_Tax_01_Master_BO.Income_Tax_01)ViewState["IncomeTax_Header"];
            }
            set
            {
                ViewState["IncomeTax_Header"] = value;
            }
        }

        #endregion
        #endregion
        #region PageEvents
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        #endregion
        #region Page Action Handler
        private void PageActionHandler()
        {
            try
            {
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                } 
                GetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                SetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Page PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
        }
        #endregion
        #region OnLoadComplete
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        #endregion

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {

            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        #endregion


        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int? result = null;
                XmlDocument xmlDoc;
                string redirectUrl = Resources.PageURL.HrmsEmpListing;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        SetUIValuesToObject(ControlsEnum.SAVEINCOMETAXDETAILS);
                        Income_Tax_01_Master_BO.Income_Tax_01 IncomeTax_Data = (Income_Tax_01_Master_BO.Income_Tax_01)SetUIValuesToObject(ControlsEnum.SAVEINCOMETAXDETAILS);
                        if (IncomeTax_Data.Section.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(IncomeTax_Data);
                        string XmlIcomeTax = GetLocalResourceObject("XmlIncomeTaxFile").ToString();
                        FileAttributes attributes = File.GetAttributes(Server.MapPath(XmlIcomeTax));
                        if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            // Make the file RW
                            attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                            File.SetAttributes(Server.MapPath(XmlIcomeTax), attributes);
                        }
                        xmlDoc.Save(Server.MapPath(XmlIcomeTax));
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                        SetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                Panel pnlIncomeTaxDetails = (Panel)item.FindControl("pnlIncomeTaxDetails");
                ImageButton imbHideTaxDetails = (ImageButton)item.FindControl("imbHideTaxDetails");
                ImageButton imbShowTaxDetails = (ImageButton)item.FindControl("imbShowTaxDetails");

               imbHideTaxDetails.Attributes.Add("onclick", string.Format("ImbHideTaxDetails_Click('{0}','{1}','{2}');return false;", pnlIncomeTaxDetails.ClientID, imbHideTaxDetails.ClientID, imbShowTaxDetails.ClientID));
                imbShowTaxDetails.Attributes.Add("onclick", string.Format("ImbShowTaxDetails_Click('{0}','{1}','{2}');return false;", pnlIncomeTaxDetails.ClientID, imbHideTaxDetails.ClientID, imbShowTaxDetails.ClientID));


                Repeater rprIncomeTax = (Repeater)item.FindControl("rprIncomeTax");
                rprIncomeTax.DataSource = IncomeTax_SectionsList[item.ItemIndex].Items;
                rprIncomeTax.DataBind();
            }
        }

        protected void Inner_ActionHandler(object sender, RepeaterItemEventArgs e)
        {
             RepeaterItem item = e.Item;
             HtmlTable sas = (HtmlTable)item.FindControl("tblDetailsSection");


        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        XmlDocument doc = new XmlDocument();
                        string XmlIcomeTax = GetLocalResourceObject("XmlIncomeTaxFile").ToString();
                        doc.Load(Server.MapPath(XmlIcomeTax));
                        string xmlData = doc.InnerXml;
                        if (xmlData == "<Root/>" || xmlData == string.Empty)
                        {
                            IncomeTax_SectionsList = new List<Income_Tax_01_Master_BO.Section>();
                        }
                        else
                        {
                            objIncomeTax_Header = CommonFunctions.XmlDeserialize<Income_Tax_01_Master_BO.Income_Tax_01>(xmlData);
                            IncomeTax_SectionsList = objIncomeTax_Header.Section;
                        }
                        break;
                    #endregion
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
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        BindRepeater(ControlsEnum.GETINCOMETAXDETAILS);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                #region SECTION INCOME TAX DETAILS
                case ControlsEnum.SAVEINCOMETAXDETAILS:
                    foreach (RepeaterItem RepeaterItem_Master in rprIncomeTaxMaster.Items)
                    {
                        Repeater rprIncomeTax = (Repeater)RepeaterItem_Master.FindControl("rprIncomeTax");
                        foreach (RepeaterItem repeaterItem_Sections in rprIncomeTax.Items)
                        {
                            HiddenField hdfItemDbId = (HiddenField)repeaterItem_Sections.FindControl("hdfItemDbId");
                            TextBox txt_Step = (TextBox)repeaterItem_Sections.FindControl("txt_ITCol2");
                            TextBox txt_DefaultAmount = (TextBox)repeaterItem_Sections.FindControl("txt_ITCol3");
                            TextBox txt_Col3Max = (TextBox)repeaterItem_Sections.FindControl("txt_ITCol4");
                            CheckBox chkDefualtValue = (CheckBox)repeaterItem_Sections.FindControl("chkDefualtValue");
                            CheckBox chkAmountHide = (CheckBox)repeaterItem_Sections.FindControl("chkAmountHide");
                            CheckBox chkValueHide = (CheckBox)repeaterItem_Sections.FindControl("chkValueHide");

                            Income_Tax_01_Master_BO.Items tempIncome_Items = new Income_Tax_01_Master_BO.Items();
                            tempIncome_Items = objIncomeTax_Header.Section[RepeaterItem_Master.ItemIndex].Items.Where(e => e.DbId == Convert.ToInt32(hdfItemDbId.Value))
                                       .SingleOrDefault();
                            if (tempIncome_Items != null)
                            {
                                tempIncome_Items.Col2 = txt_Step.Text.HtmlDecode();
                                tempIncome_Items.DefaultAmount = txt_DefaultAmount.Text;
                                tempIncome_Items.Col3_Max = txt_Col3Max.Text;
                                tempIncome_Items.ChkBoxReq = chkDefualtValue.Checked == true ? 1 : 0;
                                tempIncome_Items.Col3_Hide = chkAmountHide.Checked == true ? 1 : 0;
                                tempIncome_Items.Col4_Hide = chkValueHide.Checked == true ? 1 : 0;
                            }
                        }
                    }
                    returnObject = objIncomeTax_Header;
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion

        #region GetUIValuesFromObject
        private void GetUIValuesFromObject(ControlsEnum controlType)
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

        #region Repeater
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindRepeater(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        if (IncomeTax_SectionsList != null && IncomeTax_SectionsList.Count > 0)
                        {
                            rprIncomeTaxMaster.DataSource = IncomeTax_SectionsList;
                            rprIncomeTaxMaster.DataBind();
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    break;
                #endregion
            }
        }
        #endregion

        #region Helper Methods
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            GETINCOMETAXDETAILS,
            COMPANY,
            CLEAR,
            SECTIONINCOMETAXDETAILS,
            ITEMSINCOMETAXDETAILS,
            SAVEINCOMETAXDETAILS
        }
        #endregion
    }
}