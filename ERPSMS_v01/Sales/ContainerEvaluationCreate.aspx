<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContainerEvaluationCreate.aspx.cs" Inherits="ERPSMS_v01.Sales.ContainerEvaluationCreate" 
    MasterPageFile="~/ERPSMS_2.Master"  Theme="Classic" Title="<%$ Resources:Captions,Title_ContainerEvaluation %>" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/CheckListControl.ascx" TagName="CheckListItems"
    TagPrefix="CL1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
            }
            InitDateComponents();
            return false;
        }


        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
            }
        }
        function InitDateComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCompany", url, "hdfCompany", true, true, "VENDOR");
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerEvaluation')" ValidationGroup="ContainerEvaluation"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="91" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerEvaluation')" ValidationGroup="ContainerEvaluation"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="92" Text="<%$resources:ErpRes,Delete %>" OnClick="ActionHandler"
                                             OnClientClick="return ShowDeleteConfirm(this);" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="93" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
               
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                           
                            <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfEvaluationNo" runat="server" Value="" />
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEvaluationNo" Text="<%$ resources:EvaluationNo %>" AssociatedControlID="lblEvaluationNoTxt"
                                                    TabIndex="1"></asp:Label>
                                                <asp:Label runat="server" ID="lblEvaluationNoTxt"></asp:Label>
                                                <asp:HiddenField ID ="AST_DOC_MODE" runat ="server" Value ="0" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" AssociatedControlID="txtDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDate" TabIndex="2" MaxLength="12" CssClass="Uidate-picker"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="ContainerEvaluation" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="ContainerEvaluation"
                                                        runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td>
                                            <div class="div2col-S">
                                               <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company %>" AssociatedControlID="txtCompany"></asp:Label>
                                              <asp:TextBox ID="txtCompany" runat="server" MaxLength="200"  TabIndex="4" />
                                            <asp:RequiredFieldValidator ID="vrfCompany" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="ContainerEvaluation"
                                                EnableClientScript="true" Display="Dynamic" Text="*" ControlToValidate="txtCompany"
                                                ErrorMessage="<%$ resources:Err_TransCompany %>" />
                                            <asp:HiddenField ID="hdfCompany" runat="server" />
                                              
                                              
                                              <%-- <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="3"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfCompany" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="ContainerEvaluation" EnableClientScript="true" runat="server" ControlToValidate="ddlCompany" InitialValue="-1"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TransCompany %>">
                                                    </asp:RequiredFieldValidator>--%>
                                            </div> 
                                            </td> 
                                           <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo %>" AssociatedControlID="txtContainerNo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtContainerNo" TabIndex="5" MaxLength="200" ></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="vrfContainerNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="ContainerEvaluation" EnableClientScript="true" runat="server" ControlToValidate="txtContainerNo"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ContainerNo %>">
                                                    </asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        </tr>
                                        <tr>
                                          <td colspan ="2">
                                             <div class="divcol-S">
                                                <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>"
                                                AssociatedControlID="txtRemarks" />
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="5" Width="715"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                               
                                             
                                            </div>
                                               <div class="clear">
                                                </div>
                                          </td>
                                        </tr>
                                  
                                </table>
                                <div class="clear">
                                    </div>
                                
                                 <CL1:CheckListItems ID="uclCheckList" runat="server"  TabIndex="6" />

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ContainerEvaluation" runat="server" />
                     <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="ContainerEvaluation" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
