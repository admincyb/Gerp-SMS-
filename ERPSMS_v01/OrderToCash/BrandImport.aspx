<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" 
    AutoEventWireup="true" CodeBehind="BrandImport.aspx.cs" Inherits="ERPSMS_v01.OrderToCash.BrandImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ShowDeleteConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = "<%= Resources.ErpRes.Title_Information %>";
            msg = message ? message : "<%= Resources.ErpRes.MsgDeleteConfirm %>";
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
        <ContentTemplate>
            <div class="fixed-buttons" id="divFixedTab">
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
                <asp:TableRow ID="OpsPlan_Entry" runat="server">
                    <%--Align table cell according to design--%>
                    <asp:TableCell>
                        <div class="contentwrapper">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                  <div class="divcolmiddle-S">
                                    <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFile %>" AssociatedControlID="fupImport"></asp:Label>
                                    <asp:FileUpload ID="fupImport" runat="server" TabIndex="7" />
                                    <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                    </asp:RequiredFieldValidator>
                                    <asp:Button Text="<%$ resources: Import %>" ToolTip="<%$ resources: Import %>" runat="server" ID="btnImport" CommandName="SAVE"
                                            OnClick="ActionHandler" ValidationGroup="Land" TabIndex="8" OnClientClick="javascript:ValidatePageNow('upload')" />
                                    <div style="font-weight: bold; margin-bottom: 3px;">
                                        <asp:Literal ID="lblNote" runat="server" Visible="false" EnableTheming="false"></asp:Literal>
                                    </div>
                                    <div >
                                        
                                       <%-- <asp:Button Text="<%$ resources: Cancel %>" TabIndex="9" runat="server" ID="btCancel" />--%>
                                    </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnImport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:TableCell>
                    
                </asp:TableRow>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vvsLanding" ValidationGroup="upload" runat="server" />
                <asp:ValidationSummary ID="vvsLandingDtl" ValidationGroup="LandDtl" runat="server" />
                <%--  <asp:CustomValidator ID="cusLanding" OnServerValidate="LandingServerValidate" Display="Dynamic"
            ValidationGroup="LandDtl" runat="server" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
