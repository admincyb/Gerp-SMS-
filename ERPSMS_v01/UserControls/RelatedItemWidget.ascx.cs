using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.UserControls
{
    public partial class RelatedItemWidget : System.Web.UI.UserControl
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Request.QueryString["RefID"] != null)
                {
                    int refId = int.Parse(Request.QueryString["RefID"]);
                    int appId = 0;
                    WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                    DataTable dtApplication = wrkfService.GetApplicationID(refId);
                    if (dtApplication != null)
                    {
                        if (dtApplication.Rows.Count > 0)
                        {
                            appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                        }
                    }
                    string path = "";
                    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                    else
                        path = Request.Url.AbsolutePath.ToLower();
                    if (path.ToLower().Contains("purchaserequestcreation.aspx") || path.ToLower().Contains("purchaseordergenerate.aspx") ||
                        path.ToLower().Contains("grncreate.aspx") || path.ToLower().Contains("gincreate.aspx") || path.ToLower().Contains("stocktransfer.aspx"))
                    {
                        FillRelatedItemWidget(appId);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillRelatedItemWidget(int appId)
        {
            if (appId > 0)
            {
                string path = "";
                int processID = 0;
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    processID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                }
                DataTable dtRelatedWidget = CommonBL.GetRelatedItemsWidget(processID, appId, path);
                trvRelatedWidget.Nodes.Clear();
                if (dtRelatedWidget != null && dtRelatedWidget.Rows.Count > 0)
                {
                    divReferenceRecords.Visible = true;
                    TreeNode trNodePR = new TreeNode("PR", "0");
                    trNodePR.SelectAction = TreeNodeSelectAction.None;
                    var PRList = from lst in dtRelatedWidget.AsEnumerable()
                                 where lst.Field<int>("ITM_SEQUENCE") == 1
                                 select lst;

                    TreeNode trChildNode;
                    foreach (var item in PRList)
                    {
                        trChildNode = new TreeNode(item.Field<object>("ITM_APP_NO").ToString(), item.Field<object>("ITM_REF_ID").ToString());
                        trNodePR.ChildNodes.Add(trChildNode);
                    }

                    TreeNode trNodePO = new TreeNode("PO", "0");
                    trNodePO.SelectAction = TreeNodeSelectAction.None;
                    var POList = from lst in dtRelatedWidget.AsEnumerable()
                                 where lst.Field<int>("ITM_SEQUENCE") == 2
                                 select lst;
                    foreach (var item in POList)
                    {
                        trChildNode = new TreeNode(item.Field<object>("ITM_APP_NO").ToString(), item.Field<object>("ITM_REF_ID").ToString());
                        trNodePO.ChildNodes.Add(trChildNode);
                    }

                    TreeNode trNodeGRN = new TreeNode("GRN", "0");
                    trNodeGRN.SelectAction = TreeNodeSelectAction.None;
                    var GRNList = from lst in dtRelatedWidget.AsEnumerable()
                                  where lst.Field<int>("ITM_SEQUENCE") == 3
                                  select lst;
                    foreach (var item in GRNList)
                    {
                        trChildNode = new TreeNode(item.Field<object>("ITM_APP_NO").ToString(), item.Field<object>("ITM_REF_ID").ToString());
                        trNodeGRN.ChildNodes.Add(trChildNode);
                    }

                    TreeNode trNodeGIN = new TreeNode("GIN", "0");
                    trNodeGIN.SelectAction = TreeNodeSelectAction.None;
                    var GINList = from lst in dtRelatedWidget.AsEnumerable()
                                  where lst.Field<int>("ITM_SEQUENCE") == 4
                                  select lst;
                    foreach (var item in GINList)
                    {
                        trChildNode = new TreeNode(item.Field<object>("ITM_APP_NO").ToString(), item.Field<object>("ITM_REF_ID").ToString());
                        trNodeGIN.ChildNodes.Add(trChildNode);
                    }

                    TreeNode trNodeST = new TreeNode("ST", "0");
                    trNodeST.SelectAction = TreeNodeSelectAction.None;
                    var STList = from lst in dtRelatedWidget.AsEnumerable()
                                  where lst.Field<int>("ITM_SEQUENCE") == 5
                                  select lst;
                    foreach (var item in STList)
                    {
                        trChildNode = new TreeNode(item.Field<object>("ITM_APP_NO").ToString(), item.Field<object>("ITM_REF_ID").ToString());
                        trNodeST.ChildNodes.Add(trChildNode);
                    }

                    trvRelatedWidget.Nodes.Add(trNodePR);
                    trvRelatedWidget.Nodes.Add(trNodePO);
                    trvRelatedWidget.Nodes.Add(trNodeGRN);
                    trvRelatedWidget.Nodes.Add(trNodeGIN);
                    trvRelatedWidget.Nodes.Add(trNodeST);
                    if (path.ToLower().Contains("purchaserequestcreation.aspx"))
                    {
                        trNodePR.ExpandAll();
                    }
                    else if (path.ToLower().Contains("purchaseordergenerate.aspx"))
                    {
                        trNodePO.ExpandAll();
                    }
                    else if (path.ToLower().Contains("grncreate.aspx"))
                    {
                        trNodeGRN.ExpandAll();
                    }
                    else if (path.ToLower().Contains("gincreate.aspx"))
                    {
                        trNodeGIN.ExpandAll();
                    }
                    else if (path.ToLower().Contains("stocktransfer.aspx"))
                    {
                        trNodeST.ExpandAll();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            string path = "";
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            if (int.Parse(trvRelatedWidget.SelectedValue) > 0)
            {
                if (trvRelatedWidget.SelectedNode.Parent != null)
                {
                    if (trvRelatedWidget.SelectedNode.Parent.Text == "PR")
                    {
                        if (path.ToLower().Contains("purchaserequestcreation.aspx"))
                        {
                            Response.Redirect("PurchaseRequestCreation.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        else
                        {
                            Response.Redirect("../PurchaseRequestManagement/PurchaseRequestCreation.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                    }
                    else if (trvRelatedWidget.SelectedNode.Parent.Text == "PO")
                    {
                        if (path.ToLower().Contains("purchaseordergenerate"))
                        {
                            Response.Redirect("PurchaseOrderGenerate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        else
                        {
                            Response.Redirect("../PurchaseOrderManagement/PurchaseOrderGenerate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                    }
                    else if (trvRelatedWidget.SelectedNode.Parent.Text == "GRN")
                    {
                        if (path.ToLower().Contains("grncreate.aspx"))
                        {
                            Response.Redirect("GRNCreate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        else
                        {
                            Response.Redirect("../StoreManagement/GRNCreate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        
                    }
                    else if (trvRelatedWidget.SelectedNode.Parent.Text == "GIN")
                    {
                        if (path.ToLower().Contains("gincreate.aspx"))
                        {
                            Response.Redirect("GINCreate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        else
                        {
                            Response.Redirect("../StoreManagement/GINCreate.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                    }
                    else if (trvRelatedWidget.SelectedNode.Parent.Text == "ST")
                    {
                        if (path.ToLower().Contains("stocktransfer.aspx"))
                        {
                            Response.Redirect("StockTransfer.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                        else
                        {
                            Response.Redirect("../StoreManagement/StockTransfer.aspx?RefID=" + trvRelatedWidget.SelectedValue + "&Status=1");
                        }
                    }
                }    
            }
        }
    }
}