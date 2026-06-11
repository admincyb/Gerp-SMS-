<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="VendorItemDoc.aspx.cs" Inherits="ERPSMS_v01.Inventory.VendorItemDoc"
    ValidateRequest="false" EnableEventValidation="false" Theme="ClassicExt" %>
    
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url + "?SearchBy=VEN_NAME", "hdfVendor", true, true, "GETMAPPEDITEMVENDOR");
            AutoItem();
            $("[id*=txtVersion]").ForceNumericOnly();
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
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
        function AutoItem() {
            var category = $("[id$='ddlItemCategory']").val() == '-1' ? '' : $("[id$='ddlItemCategory']").val();
            var vendPk = ($("[id$='hdfVendor']").val() == '-1' || $("[id$='hdfVendor']").val() == '0') ? '' : $("[id$='hdfVendor']").val();
            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?SearchBy=ITM_NAME&VendorPk=" + vendPk + "&Type=" + category, "hdfItem", true, true, "GETMAPPEDITEMVENDOR");
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                $("[id$='ddlItemCategory']").val('-1')
                AutoItemChange();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                $("[id$='ddlItemCategory']").val('-1')
                AutoItemChange();
            }
        }
        function AutoItemChange() {
            $("[id$='hdfItem']").val('0');
            $("[id$='txtItem']").val('Select/Type');
            AutoItem();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlProduct" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CLEAR" Text="<%$resources:Controls,Clear %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" ToolTip="<%$ resources:Controls,Clear %>"
                                            OnClick="ActionHandler" TabIndex="6"/>
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <table class="table-devide" style="background: #f2f2f2;">
                    <tr>
                        <td>
                            <div class="div2col-S div-separatn">
                                <asp:Label ID="lblvendor" runat="server" Text="<%$ resources:Controls,Vendor %>"
                                    AssociatedControlID="txtVendor" />
                                <asp:TextBox ID="txtVendor" runat="server" CssClass="input-half" TabIndex="1"/>
                                <asp:HiddenField runat="server" ID="hdfVendor" />
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S div-separatn">
                                <asp:Label ID="lblItemCategory" runat="server" Text="<%$ resources:Controls,MaterialCategory %>"
                                    AssociatedControlID="ddlItemCategory" />
                                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="select-w17per" onchange="AutoItemChange();"  TabIndex="2"/>
                                <asp:Label ID="lblItem" runat="server" Text="<%$ resources:Controls,Item %>" AssociatedControlID="txtItem"
                                    CssClass="lbl-7-6perc" />
                                <asp:TextBox ID="txtItem" runat="server" CssClass="input-w34per"  TabIndex="3"/>
                                <asp:HiddenField runat="server" ID="hdfItem" />
                                <asp:ImageButton ID="btnSearchHdr" runat="server" Text="" ToolTip="<%$ resources:Controls,Search %>"
                                    TabIndex="4" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0"
                                    OnClick="ActionHandler" />
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:GridVendor %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" ToolTip='<%# Eval("VEN_NAME") %>' Text='<%# Eval("VEN_NAME") %>' />
                                                <asp:HiddenField runat="server" ID="hdfMapPK" Value='<%# Eval("ITV_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridCategory %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" ToolTip='<%# Eval("ITM_CATEGORY_TEXT") %>'
                                                    Text='<%# Eval("ITM_CATEGORY_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridItemName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" ToolTip='<%# Eval("ITM_NAME") %>' Text='<%# Eval("ITM_NAME") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridUoM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUom" runat="server" ToolTip='<%# Eval("ITM_UOM_TEXT") %>' Text='<%# Eval("ITM_UOM_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbDownload" runat="server" SkinID="btnattach" OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Attachment %>"
                                                    CommandName="POPUPADD"  TabIndex="5"/>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divDocPopUp" style="display: none;">
                <div class="content-wrapper">
                    <div class="btnwrap-divcol padgrgt0">
                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                            SkinID="btnInner-Save" ToolTip="<%$ resources:Controls,Save %>" OnClick="ActionHandler" TabIndex="17"/>
                    </div>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblVendorPopUp" runat="server" Text="<%$ resources:Controls,Vendor %>"
                                        AssociatedControlID="lblVendorDisplay"></asp:Label>
                                    <asp:Label ID="lblVendorDisplay" runat="server" CssClass="input-half"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblItemCodePopUp" runat="server" Text="<%$ resources:Controls,Item %>"
                                        AssociatedControlID="lblItemCodeDisplay"></asp:Label>
                                    <asp:Label ID="lblItemCodeDisplay" runat="server" CssClass="input-half"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblDocType" runat="server" Text="<%$ resources:Controls,DocType %>"
                                        AssociatedControlID="ddlCategory" />
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select-w61-5per" TabIndex="7"/>
                                    <asp:RequiredFieldValidator ID="vrfCategory" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="upload" InitialValue="-1" EnableClientScript="true" runat="server"
                                        ControlToValidate="ddlCategory" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Category %>">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblTitle" runat="server" Text="<%$ resources:Controls,NameTitle %>"
                                        AssociatedControlID="txtTitle" />
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input-half" MaxLength="50"  TabIndex="8"/>
                                    <asp:RequiredFieldValidator ID="vrfTitle" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="txtTitle"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Enter_Title %>">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblVersion" runat="server" Text="<%$ resources:Controls,Version %>"
                                        AssociatedControlID="txtVersion" />
                                    <asp:TextBox ID="txtVersion" runat="server" CssClass="input-half" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,false);"
                                        MaxLength="8"  TabIndex="9"/>
                                    <asp:CompareValidator ID="vrcVersion" runat="server" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="upload" EnableClientScript="true" ControlToValidate="txtVersion"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Version %>" 
                                        Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Controls,Remarks %>"
                                        AssociatedControlID="txtRemarks"  />
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-half multiline-2line" MaxLength="500"
                                        onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TextMode="MultiLine" TabIndex="10"/>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                    <div class="fileupload-main">
                                        <asp:FileUpload ID="fupUpload" runat="server" CssClass="margn-rgt0 upload-area" TabIndex="11" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clear"></div>
                                    <asp:Label ID="Label1" runat="server" Text="" AssociatedControlID="anchorFile"></asp:Label>
                                    <a id="anchorFile" runat="server" target="_blank" tabindex="5"></a>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Active %>"
                                        AssociatedControlID="chkActive" />
                                    <asp:CheckBox runat="server" ID="chkActive"  TabIndex="12"/>
                                    <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" Style="margin-right: 0px!important;"
                                        OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                        Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" OnClick="ActionHandler"  TabIndex="13"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdDocs" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,DocType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocTypeText" runat="server" ToolTip='<%# Eval("IVD_DOC_CATEGORY_TEXT") %>'
                                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IVD_DOC_CATEGORY_TEXT")),35) %>' />
                                        <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("IVD_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Eval("ListSlNo") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,NameTitle %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocTitle" runat="server" ToolTip='<%# Eval("IVD_DOC_TITLE") %>'
                                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IVD_DOC_TITLE")),25) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Version %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocVersion" runat="server" ToolTip='<%# Eval("IVD_VERSION") %>'
                                            Text='<%# Eval("IVD_VERSION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Remarks %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocDesc" runat="server" ToolTip='<%# Eval("IVD_DESC") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IVD_DESC")),25) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Active %>">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="imgStatus" Enabled="false" alt="" CssClass='<%# Eval("IVD_ACTIVE").ToString() == "0" ? "inactive" : "active" %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a runat="server" id="fileView" class="download-icon nomargin" title="view" target="_blank"
                                            href='<%# Page.ResolveClientUrl(Eval("IVD_DOC_PATH").ToString()) %>' tabindex ="14"></a>
                                        <itemstyle width="1.5%" horizontalalign="Center" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                            SkinID="edit-icon" ToolTip="Edit" Style="margin: 0px !important;"  TabIndex="14"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="1.5%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                            SkinID="delete-icon" ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);"
                                            Style="margin: 0px !important;" TabIndex="14"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="1.5%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdfMapPK" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsEditPermission" Value="0" />
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="upload" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
