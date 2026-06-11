<%@ Page Title="<%$ Resources:Captions,Title_EditMenu %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="MenuEdit.aspx.cs"
    EnableEventValidation="false" Inherits="ERPSMS_v01.GeneralAdmin.MenuEdit" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {
            if ($("[id$=grdSectionList]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedSectionItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnMenuGroupDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }

            if ($(row).parent().parent().parent().find("[id$=grdGroupDetails]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf2 = $(row).find("[id*=hdfIsExpandedMenuGroupItem]");
                if (hdf2.val() == "0") {
                    $(row).find("input[id*=btnMenuDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
        }
        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
            }
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
                        //Page_Validators.splice(i, 1);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <contenttemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="gridwrap hierarchical-wrap3" id="divSec_ScrollContainer" grid="grdSectionList">
                    <asp:HiddenField ID="hdfSection_ExpandPosition" runat="server" />
                    <cc1:ExtGridView runat="server" ID="grdSectionList" AutoGenerateColumns="False" Width="100%"
                        PageSize="<%$ Resources:PageSize %>" ExpandButtonCssClass="GridExpandCollapseButton"
                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"  OnRowDataBound="ActionHandler">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:DefaultName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval(Resources.DataFieldRes.SectionName) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.SectionName) %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfIsExpandedSectionItem" Value="0" />
                                    <asp:HiddenField runat="server" ID="hdfSectionPk" Value="<%# Eval(Resources.DataFieldRes.SectionPK) %>" />
                                    <asp:HiddenField runat="server" ID="hdfType" Value="1" />
                                    <asp:HiddenField runat="server" ID="hdfSecActiveStatus" Value="<%# Eval(Resources.DataFieldRes.SectionActive) %>" />
                                    <asp:HiddenField runat="server" ID="hdfSectionModDate" Value="<%# Eval(Resources.DataFieldRes.SectionModDate) %>" />
                                    <asp:Button runat="server" ID="btnMenuGroupDetails" OnClick="ActionHandler" CommandName="MENUGROUPDETAILS"
                                        CommandArgument='<%# Eval(Resources.DataFieldRes.SectionPK) %>' EnableTheming="false"
                                        Style="display: none" />
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:ForeignName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSectionNameForeign" runat="server" Text='<%# Eval(Resources.DataFieldRes.SectionNameForeign) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.SectionNameForeign) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:Description %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSecDescription" runat="server" Text='<%# Eval(Resources.DataFieldRes.SectionDesc) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.SectionDesc) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEditSectionName" runat="server" OnClick="ActionHandler" CommandName="EDIT_ACTION"
                                        TabIndex="18" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" />
                                    <asp:ImageButton ID="imbSectionActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("MNS_ACTIVE").ToString() == "0") ? true  : false %>'
                                        CommandName="ACTIVATE" ToolTip="<%$ resources:Controls,Inactive %>" OnClick="ActionHandler"
                                        CssClass="Active" />
                                    <asp:ImageButton ID="imbSectionInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("MNS_ACTIVE").ToString() == "1") ? true  : false %>'
                                        CommandName="DEACTIVATE" ToolTip="<%$ resources:Controls,Active %>" OnClick="ActionHandler"
                                        CssClass="Active" />
                                </ItemTemplate>
                                <ItemStyle Width="3%" HorizontalAlign="Center" Wrap="false" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="hierarchical-gridwrap">
                                        <cc1:ExtGridView runat="server" ID="grdGroupDetails" AutoGenerateColumns="False"
                                            Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                            AllowPaging="false"  OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblGrpEmty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:DefaultName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMenuGroupName" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuGroupName) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.MenuGroupName) %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfType" Value="2" />
                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedMenuGroupItem" Value="0" />
                                                        <asp:HiddenField runat="server" ID="hdfMenuGroupPk" Value="<%# Eval(Resources.DataFieldRes.MenuGroupPK) %>" />
                                                        <asp:HiddenField runat="server" ID="hdfMenuGroupActiveStatus" Value="<%# Eval(Resources.DataFieldRes.MenuGroupActive) %>" />
                                                        <asp:HiddenField runat="server" ID="hdfMenuGroupModDate" Value="<%# Eval(Resources.DataFieldRes.MenuGroupModDate) %>" />
                                                        <asp:Button runat="server" ID="btnMenuDetails" OnClick="ActionHandler" CommandName="MENUDETAILS"
                                                            CommandArgument='<%# Eval(Resources.DataFieldRes.MenuGroupPK) %>' EnableTheming="false"
                                                            Style="display: none" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:ForeignName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMenuGroupNameForeign" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuGroupNameForeign) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.MenuGroupNameForeign) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMenuGroupDescription" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuGroupDesc) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.MenuGroupDesc) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbEditMenuGroupName" runat="server" OnClick="ActionHandler"
                                                            CommandName="EDIT_ACTION" TabIndex="18" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" />
                                                        <asp:ImageButton ID="imbMenuGroupActive" runat="server" SkinID="btninactive"  Visible='<%# (Eval("MNG_ACTIVE").ToString() == "0") ? true  : false %>'
                                                            CommandName="ACTIVATE" ToolTip="<%$ resources:Controls,Inactive %>" OnClick="ActionHandler"
                                                            CssClass="Active" />
                                                        <asp:ImageButton ID="imbMenuGroupInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("MNG_ACTIVE").ToString() == "1") ? true  : false %>'
                                                            CommandName="DEACTIVATE" ToolTip="<%$ resources:Controls,Active %>" OnClick="ActionHandler"
                                                            CssClass="Active" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" HorizontalAlign="Center" Wrap="false" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="hierarchical-gridwrap">
                                                            <asp:GridView runat="server" ID="grdMenuDetails" AutoGenerateColumns="False" Width="100%"
                                                                EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" OnRowDataBound="ActionHandler">
                                                                <EmptyDataTemplate>
                                                                    <asp:Label ID="lblMenuEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:DefaultName %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMenuName" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuName) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.MenuName) %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="hdfType" Value="3" />
                                                                            <asp:HiddenField runat="server" ID="hdfMenuPk" Value="<%# Eval(Resources.DataFieldRes.MenuPK) %>" />
                                                                            <asp:HiddenField runat="server" ID="hdfMenuActiveStatus" Value="<%# Eval(Resources.DataFieldRes.MenuActive) %>" />
                                                                            <asp:HiddenField runat="server" ID="hdfMenuModDate" Value="<%# Eval(Resources.DataFieldRes.MenuModDate) %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField  HeaderText="<%$ Resources:ForeignName %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMenuNameForeign" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuNameForeign) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.MenuNameForeign) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="<%$ Resources:Description %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMenuDescription" runat="server" Text='<%# Eval(Resources.DataFieldRes.MenuDesc) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.MenuDesc) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="35%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imbEditMenuName" runat="server" OnClick="ActionHandler" CommandName="EDIT_ACTION"
                                                                                TabIndex="18" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" />
                                                                            <asp:ImageButton ID="imbMenuActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("MNU_ACTIVE").ToString() == "0") ? true  : false %>'
                                                                                CommandName="ACTIVATE" ToolTip="<%$ resources:Controls,Inactive %>" OnClick="ActionHandler"
                                                                                CssClass="Active" />
                                                                            <asp:ImageButton ID="imbMenuInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("MNU_ACTIVE").ToString() == "1") ? true  : false %>'
                                                                                CommandName="DEACTIVATE" ToolTip="<%$ resources:Controls,Active %>" OnClick="ActionHandler"
                                                                                CssClass="Active" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="3%" HorizontalAlign="Center" Wrap="false" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="table-thirdlevel" />
                                                                <HeaderStyle CssClass="table-thirdlevela" />
                                                            </asp:GridView>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="table-secondlevel" />
                                            <HeaderStyle CssClass="table-secondlevela" />
                                        </cc1:ExtGridView>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="table-firstlevel" />
                        <HeaderStyle CssClass="table-firstlevela" />
                    </cc1:ExtGridView>
                </div>
                <div id="divSectionEdit" style="display: none;">
                    <div class="content-wrapper">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnSave" SkinID="btnInner-Save" runat="server" Text="<%$ Resources:Controls,Save %>"
                                CommandName="SAVE" OnClick="ActionHandler" TabIndex="252" ValidationGroup="EditPopup"
                                OnClientClick="javascript:ValidatePageNow('EditPopup')" />
                        </div>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <div class="div2col-S">
                                        <label for="txtDefaultName">
                                            <asp:Literal ID="ltDefaultName" runat="server" Text="<%$ Resources:DefaultName %>" />:
                                        </label>
                                        <asp:TextBox runat="server" ID="txtDefaultName" TabIndex="251" ClientIDMode="Static"
                                            CssClass="input-w65per"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtDefaultName"
                                            Text="*" ErrorMessage="<%$ Resources: Err_EmptyName %>" CssClass="star" Display="Dynamic"
                                            ValidationGroup="EditPopup"></asp:RequiredFieldValidator>
                                        <asp:HiddenField runat="server" ID="hdfCurrentPK" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfCurrentActiveStatus" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfCurrentModDate" Value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="div2col-S">
                                        <label for="txtForeignName">
                                            <asp:Literal ID="ltForeignName" runat="server" Text="<%$ Resources:ForeignName %>" />:
                                        </label>
                                        <asp:TextBox runat="server" ID="txtForeignName" TabIndex="252" ClientIDMode="Static"
                                            CssClass="input-w65per"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="2">
                                    <div class="div2col-S">
                                        <label for="lblDescription">
                                            <asp:Literal ID="ltDescription" runat="server" Text="<%$ Resources:Description %>" />:
                                        </label>
                                        <<asp:Label runat="server" ID="lblDescription" CssClass="lbl-65-5perc"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsSave" ValidationGroup="EditPopup" runat="server" />
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
