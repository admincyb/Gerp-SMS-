using BusinessLogic.ReportsManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPManager;
using GTIService.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Reports.UserControls
{
    public partial class usrStockCardDataOnly : MISFilterBase
    {
        #region Variables & Properties
        private BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        private DataTable dtResult;
        private TreeNode CurrentNode;
        List<ListItem> selectedItems = new List<ListItem>();
        #region Properties
        private bool usrStockCardDataOnly_IsPostBack
        {
            get
            {
                return this.ViewState["usrStockCardDataOnly_IsPostBack"] == null ? false : (bool)this.ViewState["usrStockCardDataOnly_IsPostBack"];
            }
            set
            {
                this.ViewState["usrStockCardDataOnly_IsPostBack"] = value;
            }
        }
        #endregion
        #endregion

        public override ReportParameters GetReportParameters()
        {
            try
            {
                ReportParameterName prms;
                ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
                tempReportParams.BizUnit = currentUser.SBUID;
                tempReportParams.Dept = currentUser.CurrentDeptPK;
                tempReportParams.UserPK = currentUser.PKUser;
                tempReportParams.RptPK = Session[ERP.Utilities.SessionStrings.REPORTPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK]);
                tempReportParams.Currency = currentUser.BaseCurrency;
                tempReportParams.FromDate = FromDate.Text;
                tempReportParams.ToDate = ToDate.Text;
                tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();

                #region ITM_CATEGORY
                if (GetCheckedItems()!=null&& GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "ITM_CATEGORY";
                    foreach (ListItem item in GetCheckedItems())
                    {
                        prms.Values.Add(
                                   new ReportParameterValues()
                                   {
                                       Value = item.Value
                                   });
                    }
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region ITM_PK
                if (CheckListSearchControlNew.GetCheckedItems() != null && CheckListSearchControlNew.GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "ITM_PK";
                    foreach (ListItem item in CheckListSearchControlNew.GetCheckedItems())
                    {
                        prms.Values.Add(
                                   new ReportParameterValues()
                                   {
                                       Value = item.Value
                                   });
                    }
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region DPT_PK
                if (CheckListSearchControl1.GetCheckedItems() != null && CheckListSearchControl1.GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "DPT_PK";
                    foreach (ListItem item in CheckListSearchControl1.GetCheckedItems())
                    {
                        prms.Values.Add(
                                   new ReportParameterValues()
                                   {
                                       Value = item.Value
                                   });
                    }
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region TRAN_ONLY
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "TRAN_ONLY";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = chkTransaction.Checked ? "1" : "0"
                                });
                tempReportParams.Parameters.Add(prms);
                #endregion
                #region ALL_ITEMS
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "ALL_ITEMS";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = chkAllItems.Checked ? "1" : "0"
                                });
                tempReportParams.Parameters.Add(prms);
                #endregion

                #region IS_RETURN
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "IS_RETURN";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = chkExcludeMatReturn.Checked ? "1" : "0"
                                });
                tempReportParams.Parameters.Add(prms);
                #endregion

                #region IPD_CLASSIFICATION
                if (Convert.ToInt32(ddlClassification.SelectedValue) > 0)
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "IPD_CLASSIFICATION";
                    prms.Values.Add(
                                    new ReportParameterValues()
                                    {
                                        Value = ddlClassification.SelectedValue
                                    });
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region FIN_YEAR
                if (Convert.ToInt32(ddlFinYear.SelectedValue) > 0)
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "FIN_YEAR";
                    prms.Values.Add(
                                    new ReportParameterValues()
                                    {
                                        Value = ddlFinYear.SelectedValue
                                    });
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                return tempReportParams;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region PageLevel Events

        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "usrInitializeComponents", "$(document).ready(function(){usrInitComponents();});", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                if (!usrStockCardDataOnly_IsPostBack)
                {
                    usrStockCardDataOnly_IsPostBack = true;

                    //txtSearchCategory.Text = string.Empty;
                    GetFieldValues(ControlsEnum.ITEMCATEGORY);
                    SetFieldValues(ControlsEnum.ITEMCATEGORY);
                    GetFieldValues(ControlsEnum.STORES);
                    SetFieldValues(ControlsEnum.STORES);
                    GetFieldValues(ControlsEnum.FINYEAR);
                    SetFieldValues(ControlsEnum.FINYEAR);
                    GetFieldValues(ControlsEnum.CURRENTFINYEAR);
                    SetFieldValues(ControlsEnum.CURRENTFINYEAR);
                    GetFieldValues(ControlsEnum.FINYEARDATE);
                    SetFieldValues(ControlsEnum.FINYEARDATE);
                    GetFieldValues(ControlsEnum.TREENODES);
                    SetFieldValues(ControlsEnum.TREENODES);
                    dtResult = null;
                    SetFieldValues(ControlsEnum.CLASSIFICATION);

                    trvCategoryList.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event); postBackByObject();");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlItemCategory")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlFinYear")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGEDRES;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(CheckBoxList)))
                {
                    if (((CheckBoxList)sender).ID == "cblCategoryList")
                    {
                        commonActions = ActionsEnum.CHECKEDCHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.ITEMS);
                        SetFieldValues(ControlsEnum.ITEMS);
                        GetFieldValues(ControlsEnum.CLASSIFICATION);
                        SetFieldValues(ControlsEnum.CLASSIFICATION);
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGEDRES
                    case ActionsEnum.SELECTEDINDEXCHANGEDRES:
                        GetFieldValues(ControlsEnum.FINYEARDATE);
                        SetFieldValues(ControlsEnum.FINYEARDATE);
                        break;
                    #endregion
                    #region CHECKEDCHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                    case ActionsEnum.ADDITEM:
                        GetFieldValues(ControlsEnum.ITEMS);
                        SetFieldValues(ControlsEnum.ITEMS);
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

        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ITEMCATEGORY:
                        dtResult = GenerateReportBL.GetItemCategory(currentUser.SBUID);
                        break;
                    case ControlsEnum.ITEMS:
                        //var test = trvCategoryList.CheckedNodes.Cast<ListItem>().Select(x => x.Value);
                        //Select(x => x.Value);
                        //List<string> selectedPKs = trvCategoryList.CheckedNodes.Cast<ListItem>().Select(x => x.Value).ToList();
                        //trvCategoryList.Nodes.Cast<ListItem>().Where(li => li.Selected).ToList();//Items.Cast<ListItem>().Where(li => li.Selected).Select(s => s.Value).ToList();
                        List<string> selectedPKs = selectedItems.Select(x => x.Value).ToList();
                        string Categories = string.Join(",", selectedPKs);
                        dtResult = GenerateReportBL.GetItemsByCategory(0, Categories);
                        break;
                    case ControlsEnum.STORES:
                        dtResult = GenerateReportBL.GetDeptStores(currentUser.SBUID);
                        break;
                    case ControlsEnum.CLASSIFICATION:
                        //dtResult = GenerateReportBL.GetClassificationByCategory(Convert.ToInt32(ddlItemCategory.SelectedValue));
                        break;
                    case ControlsEnum.FINYEAR:
                        dtResult = GenerateReportBL.GetFinYear(currentUser.SBUID);
                        break;
                    case ControlsEnum.FINYEARDATE:
                        dtResult = GenerateReportBL.GetDateByFinYear(Convert.ToInt32(ddlFinYear.SelectedValue));
                        break;
                    case ControlsEnum.CURRENTFINYEAR:
                        dtResult = GenerateReportBL.GetCurrentFinYear(currentUser.SBUID);
                        break;
                    case ControlsEnum.TREENODES:
                        dtResult = GenerateReportBL.GetCategoryTreeNodes(currentUser.SBUID);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    //case ControlsEnum.ITEMCATEGORY:
                    //    cblCategoryList.DataTextField = "Value";
                    //    cblCategoryList.DataValueField = "PK";
                    //    cblCategoryList.DataSource = dtResult;
                    //    cblCategoryList.DataBind();
                    //    break;
                    case ControlsEnum.ITEMS:
                        var chkItems = dtResult.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(Resources.DataFieldRes.PK),
                            Value = row.Field<string>(Resources.DataFieldRes.Value),
                        }).ToList();
                        CheckListSearchControlNew.ListData = CommonFunctions.HtmlDecode(chkItems, "Value");
                        CheckListSearchControlNew.BindData();
                        break;
                    case ControlsEnum.STORES:
                        var chkStores = dtResult.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(Resources.DataFieldRes.PK),
                            Value = row.Field<string>(Resources.DataFieldRes.Value),
                        }).ToList();
                        CheckListSearchControl1.ListData = CommonFunctions.HtmlDecode(chkStores, "Value");
                        CheckListSearchControl1.BindData();
                        break;
                    case ControlsEnum.CLASSIFICATION:
                        ddlClassification.DataSource = dtResult;
                        ddlClassification.DataTextField = "Value";
                        ddlClassification.DataValueField = "PK";
                        ddlClassification.DataBind();
                        ddlClassification.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.FINYEAR:
                        ddlFinYear.DataSource = dtResult;
                        ddlFinYear.DataTextField = "Value";
                        ddlFinYear.DataValueField = "PK";
                        ddlFinYear.DataBind();
                        ddlFinYear.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.FINYEARDATE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            FromDate.Text = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            ToDate.Text = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);

                            hdfFromDate.Value = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            hdfToDate.Value = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);
                        }
                        break;
                    case ControlsEnum.CURRENTFINYEAR:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlFinYear.SelectedValue = dtResult.Rows[0]["FYR_PK"].ToString();
                        }
                        break;
                    case ControlsEnum.TREENODES:
                        BindTree(null);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        private void BindTree(TreeNode node)
        {
            //if (node == null)
            //{
            //    TreeNode root;
            //    trvCategoryList.Nodes.Clear();
            //    if (treeObj != null)
            //    {
            //        foreach (COATreeNode accounts in treeObj.TreeNodes)
            //        {
            //            root = new TreeNode();
            //            root.Text = HttpUtility.HtmlDecode(accounts.ITC_NAME);
            //            root.ToolTip = HttpUtility.HtmlDecode(accounts.ITC_NAME);
            //            root.Value = accounts.COA_PK.ToString();
            //            if (accounts.COA_IS_GROUP == 1)
            //                root.PopulateOnDemand = true;
            //            root.Collapse();
            //            root.SelectAction = TreeNodeSelectAction.None;
            //            trvCategoryList.Nodes.Add(root);
            //        }
            //    }
            //}
            //else
            //{
            //    TreeNode childNode;
            //    if (treeObj == null) { return; }
            //    foreach (COATreeNode accounts in treeObj.TreeNodes)
            //    {
            //        childNode = new TreeNode();
            //        childNode.Text = HttpUtility.HtmlDecode(accounts.ITC_NAME);
            //        childNode.ToolTip = HttpUtility.HtmlDecode(accounts.ITC_NAME);
            //        childNode.Value = accounts.COA_PK.ToString();
            //        if (accounts.COA_IS_GROUP == 1)
            //            childNode.PopulateOnDemand = true;
            //        childNode.SelectAction = TreeNodeSelectAction.Select;
            //        node.ChildNodes.Add(childNode);
            //    }
            //}
            trvCategoryList.Nodes.Clear();
            TreeNode PRoot;
            TreeNode root;
            if (dtResult != null)
            {
                var dtTree = dtResult.Select("ITC_PARENT_NAME='Root'").CopyToDataTable();
                PRoot = new TreeNode();
                PRoot.Text ="Categories";
                PRoot.Value = "0";
                PRoot.SelectAction = TreeNodeSelectAction.None;

                foreach (DataRow dr in dtTree.Rows)
                {
                    root = new TreeNode();
                    root.Text = dr["ITC_NAME"].ToString();
                    root.Value = dr["ITC_PK"].ToString();
                    root.SelectAction = TreeNodeSelectAction.None;
                    CreateNode(root, dtResult);
                    PRoot.ChildNodes.Add(root);
                    //trvAccounts.Nodes.Add(root);
                }
                trvCategoryList.Nodes.Add(PRoot);
            }
        }

        public void CreateNode(TreeNode node, DataTable dtNode)
        {
            TreeNode childNode;

            if (dtNode
                .AsEnumerable()
                .Where(myRow => myRow.Field<int>("ITC_PARENT") == Convert.ToInt32(node.Value)).Count() > 0)
            {
                var dtChild = dtNode
                  .AsEnumerable()
                  .Where(myRow => myRow.Field<int>("ITC_PARENT") == Convert.ToInt32(node.Value)).CopyToDataTable();


                foreach (DataRow dr in dtChild.Rows)
                {
                    childNode = new TreeNode();
                    childNode.Text = dr["ITC_NAME"].ToString();
                    childNode.Value = dr["ITC_PK"].ToString();
                    childNode.SelectAction = TreeNodeSelectAction.Select;
                    node.ChildNodes.Add(childNode);
                    CreateNode(childNode, dtNode);
                }
            }
            else
                return;
        }

        #region Helper Methods
        private List<ListItem> GetCheckedItems()
        {
            List<ListItem> selected = new List<ListItem>();// trvCategoryList.Nodes.Cast<ListItem>().Where(li => li.Selected).ToList();

            //int size = trvCategoryList.CheckedNodes.Count;
            //TreeNode[] list = new TreeNode[size];
            //trvCategoryList.CheckedNodes.CopyTo(list, 0);

            foreach (TreeNode node in trvCategoryList.CheckedNodes)
            {
                ListItem item = new ListItem();
                item.Text = node.Text;
                item.Value = node.Value;
                selected.Add(item);
            }

            return selected;
        }
        #endregion

        #region Enums
        enum ControlsEnum
        {
            ITEMCATEGORY,
            ITEMS,
            STORES,
            CLASSIFICATION,
            FINYEAR,
            FINYEARDATE,
            CURRENTFINYEAR,
            TREENODES
        }
        #endregion

        protected void ActionHandler(object sender, TreeNodeEventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "usrInitializeComponents", "OnCheckBoxCheckChanged(event);", true);
            selectedItems = new List<ListItem>();

            foreach (TreeNode node in trvCategoryList.CheckedNodes)
            {
                ListItem item = new ListItem();
                item.Text = node.Text;
                item.Value = node.Value;
                selectedItems.Add(item);
            }

            GetFieldValues(ControlsEnum.ITEMS);
            SetFieldValues(ControlsEnum.ITEMS);
        }
    }
}