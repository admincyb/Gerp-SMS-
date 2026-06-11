<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormulaMaster.ascx.cs"
    Inherits="HRMS.Admin.Masters.UserControls.FormulaMaster" %>
<script type="text/javascript">
    function FormulaInitComponents() {
        $(document).ready(function () {
            $("[id*=txtFormulaMinAmount]").ForceNumericOnly();
            $("[id*=txtFormulaMaxAmount]").ForceNumericOnly();
        });
    }

    function ValidateNowPopUp(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverrorTask").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
    //    function AfterCloseCallBack() {
    //        ShowContainerDiv('[id$=divPopUpFormula]', 'Formula', '660', '200');
    //    }
    var beginWord = '{';
    var endWord = '}';
    /// For Get current character position in a text box
    jQuery.fn.getSelectionStart = function () {
        if (this.lengh == 0) return -1;
        input = this[0];
        var pos = input.value.length;
        if (input.createTextRange) {
            var r = document.selection.createRange().duplicate();
            r.moveEnd('character', input.value.length);
            if (r.text == '')
                pos = input.value.length;
            pos = input.value.lastIndexOf(r.text);
        } else if (typeof (input.selectionStart) != "undefined")
            pos = input.selectionStart;
        return pos;
    }
    ///For set current cursor position
    jQuery.fn.selectRange = function (start, end) {
        if (end === undefined) {
            end = start;
        }
        return this.each(function () {
            if ('selectionStart' in this) {
                this.selectionStart = start;
                this.selectionEnd = end;
            } else if (this.setSelectionRange) {
                this.setSelectionRange(start, end);
            } else if (this.createTextRange) {
                var range = this.createTextRange();
                range.collapse(true);
                range.moveEnd('character', end);
                range.moveStart('character', start);
                range.select();
            }
        });
    };

    function SetCurIndex(element) {
        //        var curIndex = $(element).getSelectionStart();
        //        $("[id$=hdfCurIndex]").val(curIndex);
        //        return false;
    }
    //For validate formula
    function ValidateFormula(event, element) {
        var chrCur;
        var curIndex;
        var curStr;
        var result = false;
        var keyCode = event.keyCode ? event.keyCode : event.which;
        if (keyCode == 8 || keyCode == 46) {  // Back space  and Delete
            if (keyCode == 46 && (element.selectionEnd > element.selectionStart)) { // for selection delete
                event.returnValue = true;
                return true;
            }
            curIndex = $(element).getSelectionStart();
            curStr = $(element).val();
            chrCur = curStr.charAt(curIndex - 1);
            result = false;
            var expression = "+-*/().0123456789";
            result = (expression.indexOf(chrCur) >= 0) ? true : false;
            if (result == true) {
                event.returnValue = true;
                return true;
            }
            else {
                RemoveItem(curIndex);
                event.returnValue = false;
                return false;
            }
        }
        else  // if (keyCode != 8)
        {
            result = false;
            switch (keyCode) {
                case 46:     // Del
                case 35:     // End
                case 36:     // Home
                case 37:     // Left
                case 39:     // Right
                case 57:     // (
                case 48:     // )
                case 106:    // *
                case 56:     // *
                case 107:    // +
                case 187:    // + 
                case 109:    // -
                case 189:    // -
                case 111:    // /
                case 191:    // /
                case 190:    // .
                case 110:    // .
                case 48:     // 0
                case 96:     // 0
                case 49:     // 1
                case 97:     // 1
                case 50:     // 2
                case 98:     // 2
                case 51:     // 3
                case 99:     // 3
                case 52:     // 4
                case 100:    // 4
                case 53:     // 5
                case 101:    // 5
                case 54:     // 6
                case 102:    // 6
                case 55:     // 7
                case 103:    // 7
                case 56:     // 8
                case 104:    // 8
                case 57:     // 9
                case 105:    // 9
                    result = true;
                    break;
                default:
                    break;
            }
            if (result == true) {
                event.returnValue = true;
                return true;
            }
            else {
                event.returnValue = false;
                return false;
            }
        }

    }

    function RemoveItem(curIndex) {
        beginWord = $("[id$=hdfBeginWord]").val();
        endWord = $("[id$=hdfEndWord]").val();

        var formula = $("#[id$=txtFormula]").val();
        var curStr = $("#[id$=txtFormula]").val();
        var chrCur = curStr.charAt(curIndex - 1);
        switch (chrCur) {
            case endWord:
                for (var i = curIndex - 2; i > -1; i--) {
                    if (curStr.charAt(i) == beginWord) break;
                }
                var result = curStr.substring(i, curIndex)
                $("#[id$=txtFormula]").val(formula.replace(result, ''));
                $("#[id$=txtFormula]").selectRange((curIndex - result.length));
                break;
            default:
        }
    }

    function ResetFormula() {
        $("#[id$=txtFormula]").val('');
    }
</script>
<asp:UpdatePanel runat="server" ID="aupdpopup">
    <ContentTemplate>
        <div class="Button-container-popup">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul>
                            <li>
                                <asp:Button runat="server" ID="btnApplyPopup" SkinID="btnInner-add-dsd" Text="<%$ resources:Controls,Apply %>"
                                    ToolTip="<%$resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="APPLY"
                                    Style="margin-right: 1%!important;" TabIndex="3" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="content-wrapper">
            <div class="detail-poi-co1">
                <%--<div class="divfirstcol-S">--%>
                <asp:Label ID="lblPayElementheaderTxt" runat="server" Text="<%$resources:Controls,PayElement %>"
                    AssociatedControlID="lblPayElement" Font-Bold="true" CssClass="lbl-13perc"></asp:Label>:
                <asp:Label ID="lblPayElement" runat="server"></asp:Label>
                <%--</div>--%>
                <%--<div class="divseccol-S">
                     <asp:Label ID="Label6" runat="server" Text="<<%$resources:Controls,Apply %>" AssociatedControlID="lblDedCurrency"
                        Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDedCurrency" runat="server"></asp:Label>
                </div>--%>
                <div class="clear">
                </div>
            </div>
            <div class="div2col-S">
                <asp:Label runat="server" ID="Label1" Text="<%$resources:Controls,PayElement %>"
                    CssClass="lbl-18-3perc" AssociatedControlID="ddlPayElement"></asp:Label>
                <asp:DropDownList ID="ddlPayElement" runat="server" TabIndex="0" ValidationGroup="Add"
                    AutoPostBack="true" CssClass="select-w33per" OnSelectedIndexChanged="ActionHandler">
                </asp:DropDownList>
                <asp:Label runat="server" ID="lblParam" Text="<%$resources:Controls,Parameters %>"
                    CssClass="lbl-12perc" AssociatedControlID="ddlParameters"></asp:Label>
                <asp:DropDownList ID="ddlParameters" runat="server" TabIndex="1" ValidationGroup="Add"
                    CssClass="select-w24-7per" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqPayElement" CssClass="star" SetFocusOnError="true"
                    runat="server" ControlToValidate="ddlPayElement" Display="Dynamic" Text="*" InitialValue="0"
                    ValidationGroup="Add" ErrorMessage="<%$ resources:ErrorMessages,Msg_SelectPayElement %>">
                </asp:RequiredFieldValidator>
                <%--<asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADD"
                    ValidationGroup="Add" OnClientClick="javascript:ValidateNowPopUp('Add')" TabIndex="1"
                    SkinID="imbaddnew" ToolTip="<%$resources:Controls,Add_Add %>" />--%>
                <div class="clear">
                </div>
                <asp:Label runat="server" ID="Label2" Text="<%$resources:Controls,Formula %>" AssociatedControlID="txtFormula"
                    CssClass="lbl-18-3perc"></asp:Label>
                <asp:TextBox ID="txtFormula" runat="server" TabIndex="2" MaxLength="1000" CssClass="input-w69-5per"
                    onkeydown="return ValidateFormula(event,this);"> </asp:TextBox>
                <%--onkeyup="return SetCurIndex(this);"  onclick="return SetCurIndex(this);"--%>
                <image id="imgClear" onclick="ResetFormula();" class="margntop1 cancel-icon" style="cursor: pointer;"
                    title="<%$resources:Controls,Clear %>" runat="server"></image>
                <%--<asp:ImageButton ID="btnFormulaClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                    ToolTip="<%$ resources:Controls,Clear%>" TabIndex="2" SkinID="clear-ext" CssClass="margntop2 margnbotm0"
                    OnClientClick="javascript:ResetFormula();" src="/Images/Classic/Icons/canceld.png"/>--%>
                <div class="clear">
                </div>
            </div>
            <div class="div2col-S" id="divMinMaxAmnt" runat="server">
                <asp:Label runat="server" ID="lblFMin" Text="<%$resources:Controls,MinAmount %>"
                    AssociatedControlID="txtFormulaMinAmount" CssClass="lbl-18-3perc"></asp:Label>
                <asp:TextBox ID="txtFormulaMinAmount" runat="server" TabIndex="3" MaxLength="15"
                    CssClass="input-small numeric"> </asp:TextBox>
                <asp:Label runat="server" ID="lblFMax" Text="<%$resources:Controls,MaxAmount %>"
                    AssociatedControlID="txtFormulaMaxAmount" CssClass="lbl-31-1perc"></asp:Label>
                <asp:TextBox ID="txtFormulaMaxAmount" runat="server" TabIndex="3" MaxLength="15"
                    CssClass="input-small numeric"> </asp:TextBox>
            </div>
            <asp:HiddenField ID="hdfBeginWord" runat="server" />
            <asp:HiddenField ID="hdfEndWord" runat="server" />
            <asp:HiddenField ID="hdfCurIndex" Value="0" runat="server" />
            <asp:HiddenField ID="hdfFormulaCurrencyFormat" Value="0" runat="server" />
            <asp:HiddenField ID="hdfFormulaDecimalDigits" Value="0" runat="server" />
        </div>
        <div id="diverrorTask" style="display: none">
            <asp:ValidationSummary runat="server" ID="vsAdd" ValidationGroup="Add" />
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
