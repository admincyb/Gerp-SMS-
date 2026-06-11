<%@ Page Title="<%$ Resources:Captions,Title_PayElementMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="PayElementsMaster.aspx.cs"
    Inherits="HRMS.Admin.Masters.PayElementsMaster" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtEffectiveFrom", "hdfEffectiveFrom", "txtEffectiveTo", "hdfEffectiveTo", false, false);
            ShowHideAdvancedSearch(1);
            GrandScriptUtils.MakeAutoCompleteDDL("txtPayElementCode", url + "?PayElmntValue=PEL_CODE", "hdfPayElementCode", true, true, "PAYELEMENT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtPayElementName", url + "?PayElmntValue=PEL_NAME", "hdfPayElementName", true, true, "PAYELEMENT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtPayElementClass", url + "?PayElmntValue=PEL_CLASS", "hdfPayElementClass", true, true, "PAYELEMENT");

            GrandScriptUtils.MakeAutoCompleteDDL("txtCOA", url + "?AccType=0", "hdfCOA", true, true, "JOURNALACCOUNT");
            $("[id*=txtSeqnc]").ForceNumericOnly();
            //            var searchValue = $("[id$=ddlFilterBy]").val();
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchValue", url + "?PayElmntValue=" + $("[id$='ddlFilterBy']").val(), "hdfPayElmt", true, true, "PAYELEMENT");

            //            $("[id$=ddlFilterBy]").change(function () {
            //                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchValue", url + "?PayElmntValue=" + $("[id$='ddlFilterBy']").val(), "hdfPayElmt", true, true, "PAYELEMENT");
            //            });

            $("[id$=txtFormula]").keyup(function () {
                var yourInput = $(this).val();
                re = /[-` ~!@#$%^&*()|+\=?;:'",.<>\{\}\[\]\\\/]/gi;
                var isSplChar = re.test(yourInput);
                if (isSplChar) {
                    var no_spl_char = yourInput.replace(/[-` ~!@#$%^&*()|+\=?;:'",.<>\{\}\[\]\\\/]/gi, '');
                    $(this).val(no_spl_char);
                }
            });


        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
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
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations

        var validationArrayGroup;

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

        //Validate for Check boxes
        function IsChecked(oSrc, args) {
            if ($("[id$=chkAppearsinPayslip]").attr("checked") == false && $("[id$=chkPartofCTC]").attr("checked") == false)
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }

       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="auplDetailList" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons" style="padding-bottom: 30px !important;">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" OnClick="ActionHandler"
                                            Text="<%$ resources:Controls,Save%>" ToolTip="<%$ resources:Controls,Save%>"
                                            ValidationGroup="save" OnClientClick="javascript:ValidatePageNow('Save')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="21" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="22" OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            CommandName="CANCEL" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            TabIndex="23" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="24" ID="btnNew" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="25" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="9" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="9" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPayElementName" runat="server" AssociatedControlID="txtPayElementName"
                                                Text="<%$ resources:ElementDesp%>"></asp:Label>
                                            <asp:TextBox ID="txtPayElementName" runat="server" TabIndex="1" onkeydown="return Search(event);"
                                                CssClass="select-half margnbotm0" MaxLength="200" />
                                            <asp:HiddenField ID="hdfPayElementName" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPayElementCode" runat="server" AssociatedControlID="txtPayElementCode"
                                                Text="<%$ resources:ElementCode%>" CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox ID="txtPayElementCode" runat="server" onkeydown="return Search(event);"
                                                CssClass="select-small-c1 margnbotm0" MaxLength="50" TabIndex="2" />
                                            <asp:HiddenField ID="hdfPayElementCode" runat="server" />
                                            <asp:Label ID="lblPayElementClass" runat="server" AssociatedControlID="txtPayElementClass"
                                                Text="<%$ resources:Classification%>" CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox ID="txtPayElementClass" runat="server" TabIndex="3" onkeydown="return Search(event);"
                                                CssClass="select-small-c1 margnbotm0" MaxLength="200" />
                                            <asp:HiddenField ID="hdfPayElementClass" runat="server" />
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="4"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="5" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="25"
                                    CssClass="grdTable" AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfPayElemPk" Value='<%# Eval("PEL_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ElementCode %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayElemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PEL_CODE")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEL_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ElementDesp %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayElemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PEL_NAME")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEL_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Classification %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClassText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PEL_CLASS_TEXT")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEL_CLASS_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:COA %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOAText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PEL_ACCOUNT_CODE_TEXT")),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEL_ACCOUNT_CODE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PEL_DESC")),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEL_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %> ">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("PEL_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="<%$ resources:Inactive %>" OnClick="ActionHandler"
                                                    CssClass="Active" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("PEL_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="<%$ resources:Active %>" OnClick="ActionHandler"
                                                    CssClass="Active" />
                                            </ItemTemplate>
                                            <%--CommandName="DEACTIVATE"--%>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCode" runat="server" Text="<%$ resources:PayElementCode %>" CssClass="lbl-30-7perc"
                                                AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox ID="txtCode" runat="server" CssClass="input-medium" MaxLength="40"
                                                TabIndex="6"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ElementCode%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCodeLL" runat="server" Text="<%$ resources:PayElementCodeLL %>"
                                                CssClass="lbl-30-7perc" AssociatedControlID="txtCodeLL"></asp:Label>
                                            <asp:TextBox ID="txtCodeLL" runat="server" CssClass="input-medium" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblClassification" runat="server" Text="<%$ resources:PayClassification %>"
                                                CssClass="lbl-30-7perc" AssociatedControlID="ddlClassification"></asp:Label>
                                            <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-small-e2"
                                                MaxLength="100" TabIndex="8" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvClassification" runat="server" ControlToValidate="ddlClassification"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Classification%>"
                                                InitialValue="-1"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFormula" runat="server" Text="<%$ resources:FormulaCode%>" AssociatedControlID="txtFormula"
                                                CssClass="lbl-30-7perc"></asp:Label>
                                            <asp:Label ID="lblFormulaPrefix" CssClass="small-20" runat="server" /><%--Text='<%$ Resources:ConfigurationsRes, PayElementFormulaConst %>'--%>
                                            <asp:TextBox ID="txtFormula" runat="server" CssClass="input-w21-6per" MaxLength="25"
                                                TabIndex="11"></asp:TextBox>
                                            <asp:Label ID="lblFormulaSufix" runat="server" CssClass="small-20"></asp:Label><%--Text='<%$ Resources:ConfigurationsRes, PayElementFormulaConst %>'--%>
                                            <asp:RequiredFieldValidator ID="rfvFormulaCode" runat="server" ControlToValidate="txtFormula"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_FormulaCode%>"></asp:RequiredFieldValidator>
                                            <%-- <asp:RegularExpressionValidator ID="revFormulaCode" runat="server" ControlToValidate="txtFormula"
                                                ErrorMessage="<%$ Resources:Err_FormulaCodeExpr %>" ValidationExpression="^[0-9A-Za-z]*$"
                                                Text="*" CssClass="star" ValidationGroup="Save"></asp:RegularExpressionValidator>--%>
                                            <asp:Label ID="lblType" runat="server" Text="<%$ resources:ElementType %>" AssociatedControlID="ddlType"
                                                CssClass="lbl-15-1perc "></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="select-w17per" MaxLength="100"
                                                TabIndex="12">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType"
                                                InitialValue="-1" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Type %>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblElementDesp" runat="server" Text="<%$ resources:PayElementName%>"
                                                AssociatedControlID="txtElementDesp" CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:TextBox ID="txtElementDesp" runat="server" CssClass="input-w64per" MaxLength="150"
                                                TabIndex="6"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvElementDesp" runat="server" ControlToValidate="txtElementDesp"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ElementDesp%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblElementDespLL" runat="server" Text="<%$ resources:PayElementNameLL%>"
                                                AssociatedControlID="txtElementDespLL" CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:TextBox ID="txtElementDespLL" runat="server" CssClass="input-w64per" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblEffectiveFrom" runat="server" Text="<%$ resources:EffectiveFrom %>"
                                                AssociatedControlID="txtEffectiveFrom" CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="input-small" MaxLength="100"
                                                TabIndex="9" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdfEffectiveFrom" />
                                            <asp:HiddenField runat="server" ID="hdfEffectiveTo" />
                                            <asp:Label ID="lblEffectiveTo" runat="server" Text="<%$ resources:EffectiveTo%>"
                                                AssociatedControlID="txtEffectiveTo" CssClass="lbl-25-5perc"></asp:Label>
                                            <asp:TextBox ID="txtEffectiveTo" runat="server" CssClass="input-small" MaxLength="100"
                                                TabIndex="10" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lbCOA" runat="server" Text="<%$ resources:COA %>" AssociatedControlID="txtCOA"
                                                CssClass="lbl-31-1perc"></asp:Label>
                                            <%-- <asp:DropDownList ID="ddlCOA" runat="server" CssClass="select-half-b" TabIndex="13">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtCOA" runat="server" CssClass="input-w64per" TabIndex="13" onfocus="this.select();"
                                                onMouseUp="return false;" />
                                            <asp:HiddenField ID="hdfCOA" runat="server" />
                                            <%--<div class="div2col-S">
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:Classification%>" AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox ID="txt" runat="server" CssClass="large" MaxLength="100" TabIndex="1"> </asp:TextBox> 
                                        </div>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription" CssClass="lbl-15-4perc"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="13" TextMode="MultiLine"
                                                onkeypress="return this.value.length<490" onpaste="return this.value.length<490"
                                                Height="40" CssClass="lbl-82-2perc"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblParentElement" runat="server" Text="<%$ resources:ParentElement %>"
                                                CssClass="lbl-30-7perc" AssociatedControlID="ddlParentElement"></asp:Label>
                                            <asp:DropDownList ID="ddlParentElement" runat="server" CssClass="select-w66per" MaxLength="100"
                                                TabIndex="14">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAppearsinPayslip" runat="server" Text="<%$ resources:AppearsinPayslip%>"
                                                AssociatedControlID="lblAppearsinPayslip" CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkAppearsinPayslip" runat="server" TabIndex="15" />
                                            <%-- <asp:Label ID="lblRecurring" runat="server" Text="<%$ resources:Recurring %>" AssociatedControlID="chkRecurring"></asp:Label>
                                            <asp:CheckBox ID="chkRecurring" runat="server" TabIndex="15" />--%>
                                            <asp:Label ID="lblPartofCTC" runat="server" Text="<%$ resources:PartofCTC%>" AssociatedControlID="lblPartofCTC"
                                                CssClass="lbl-28perc"></asp:Label>
                                            <asp:CheckBox ID="chkPartofCTC" runat="server" TabIndex="16" />
                                            <%--<asp:Label ID="lblTaxable" runat="server" Text="<%$ resources:Taxable %>" AssociatedControlID="chkTaxable"></asp:Label>
                                            <asp:CheckBox ID="chkTaxable" runat="server" TabIndex="17" />--%>
                                            <%-- <asp:Label ID="lblIsDeduct" runat="server" Text="<%$ resources:IsDeduct%>" AssociatedControlID="chkIsDeduct"
                                                CssClass="lbl-28perc"></asp:Label>
                                            <asp:CheckBox ID="chkIsDeduct" runat="server" TabIndex="17" />--%>
                                            <asp:Label ID="lblInSalary" runat="server" Text="<%$ resources:IncludeInSalary%>"
                                                AssociatedControlID="lblInSalary" CssClass="lbl-28perc"></asp:Label>
                                            <asp:CheckBox ID="chkIncludeInSalary" Checked="true" runat="server" TabIndex="17" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S h30">
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"
                                                CssClass="lbl-30-7perc"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="18" />
                                            <asp:Label ID="lblRoundoff" runat="server" Text="<%$ resources:Roundoff%>" AssociatedControlID="lblRoundoff"
                                                CssClass="lbl-30-7perc"></asp:Label>
                                            <asp:CheckBox ID="chkRoundoff" runat="server" TabIndex="18" />
                                            <%-- <asp:CustomValidator ID="csvPaySlip" runat="server" Display="None" Text="*" ClientValidationFunction="IsChecked"
                                                ErrorMessage="<%$ resources:Msg_SelectPaySliporCtc %>" ValidationGroup="Save"></asp:CustomValidator>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblEditable" runat="server" Text="<%$ resources:SalaryEditable%>"
                                                AssociatedControlID="lblEditable" CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkEditable" runat="server" TabIndex="19" />
                                            <asp:Label ID="lblPartofGross" runat="server" Text="<%$ resources:PartofGross%>"
                                                AssociatedControlID="lblPartofGross" CssClass="lbl-28perc"></asp:Label>
                                            <asp:CheckBox ID="chkPartofGross" runat="server" TabIndex="20" />
                                            <asp:Label ID="lblFormulaEditable" runat="server" Text="<%$ resources:FormulaEditable%>"
                                                AssociatedControlID="lblFormulaEditable" CssClass="lbl-28perc"></asp:Label>
                                            <asp:CheckBox ID="chkFormulaEditable" runat="server" TabIndex="20" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSeqnc" runat="server" Text="<%$ resources:Sequence%>" CssClass="lbl-30-7perc"
                                                AssociatedControlID="txtSeqnc"></asp:Label>
                                            <asp:TextBox ID="txtSeqnc" runat="server" MaxLength="100" TabIndex="21" Width="60px" CssClass="numeric"></asp:TextBox>
                                            <asp:Label ID="lblShwRpt" runat="server" Text="<%$ resources:ShowInReport%>" AssociatedControlID="lblShwRpt"
                                                CssClass="lbl-22-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkShwRpt" runat="server" TabIndex="21" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblShwRpt1" runat="server" Text="<%$ resources:ShowInReport1%>" AssociatedControlID="lblShwRpt1"
                                                CssClass="lbl-31-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkShwRpt1" runat="server" TabIndex="21" />

                                              <asp:Label ID="lblShwinEmpMaster" runat="server" Text="<%$ resources:ShowInEmpMastSal%>" AssociatedControlID="lblShwinEmpMaster"
                                                CssClass="lbl-27-9perc"></asp:Label>
                                            <asp:CheckBox ID="chkShwEmpMasterSalTemp" runat="server" TabIndex="21" />
                                        </div>

                                    </td>

                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
            <asp:HiddenField ID="hdfFormulaPrefix" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
