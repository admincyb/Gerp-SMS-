<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpSalaryControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.EmpSalaryControl" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/Admin/Masters/UserControls/FormulaMaster.ascx" TagName="FormulaMaster"
    TagPrefix="ucgti" %>
<script type="text/javascript">
    function SerialNoOrdering() {
        var totalEarnRows = $('[id*=grdEarnings] tr').length - 1;   //Total Row Count
        //for hide up and down arrow in  Earnings detials grid
        $('[id*=grdEarnings] tr:nth-child(2)').find("[id*=imbRuleUpEarn]").hide(); //Hide first rows Up arrow
        $('[id*=grdEarnings] tr:nth-child(' + totalEarnRows + ')').find("[id*=imbRuleDownEarn]").hide(); //Hide last rows Down Arrow 


        var totalDeduRows = $('[id*=grdDeductions] tr').length - 1;   //Total Row Count
        //for hide up and down arrow in  Deduction detials grid
        $('[id*=grdDeductions] tr:nth-child(2)').find("[id*=imbRuleUpDedu]").hide(); //Hide first rows Up arrow
        $('[id*=grdDeductions] tr:nth-child(' + totalDeduRows + ')').find("[id*=imbRuleDownDedu]").hide(); //Hide last rows Down Arrow 
    }

    function ShowHideEarnings(flag) {
        if (flag == 1) {
            $("[id$=divEarnings]").show();
            $("[id$=imbShowEarnings]").hide();
            $("[id$=imbHideEarnings]").show();
        }
        else {
            $("[id$=divEarnings]").hide();
            $("[id$=imbShowEarnings]").show();
            $("[id$=imbHideEarnings]").hide();
        }
        return false;
    }

    function ShowHideDeductions(flag) {
        if (flag == 1) {
            $("[id$=divDeductions]").show();
            $("[id$=imbShowDeductions]").hide();
            $("[id$=imbHideDeductions]").show();
        }
        else {
            $("[id$=divDeductions]").hide();
            $("[id$=imbShowDeductions]").show();
            $("[id$=imbHideDeductions]").hide();
        }
        return false;
    }

    function CalculateTotalEarnings(ctrl) {
        var TotalAmount = 0;
        var TotalDedctn = 0;
        var NetSalry = 0;
        var TotalAmountCTC = 0;
        var TotalDedctnCTC = 0;
        var NetCTC = 0;
        var TotalAmountGross = 0;
        var TotalDedctnGross = 0;
        var NetGross = 0;

        var DiffAmountCTC = 0;
        var DiffGross = 0;
        var DiffNetSalry = 0;
        var OldAmountCTC = 0;
        var OldGross = 0;
        var OldNetSalry = 0;

        if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
            DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
        }

        $("#[id*=grdEarnings] input[type=text][id*=txtEarnAmount]").each(function (index) {
            var amount = 0;
            var pelInSalary = 0;
            var pelInCTC = 0;
            var amountCTC = 0;
            var pelInGross = 0;
            var amountGross = 0;
            var eidIsDelete = $(this).closest('tr').find("#[id*=hdfErnEID_IS_DELETE]").val();
            if (!isNaN(eidIsDelete) && eidIsDelete == 0) //Calculate Total if the Pay Element is Deleted or not (Employee Appraisal Page)
            {
                pelInSalary = $(this).closest('tr').find("#[id*=hdfEarnPayElmtInSalary]").val();
                if (!isNaN(pelInSalary) && pelInSalary == 1) {//Calculate Total if the Pay Element included in Salary               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amount = parseFloat($(this).val());
                            TotalAmount = TotalAmount + amount;
                        }
                    }
                }

                pelInCTC = $(this).closest('tr').find("#[id*=hdfErnPartofCTC]").val();
                if (!isNaN(pelInCTC) && pelInCTC == 1) {//Calculate Total if the Pay Element included in CTC               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amountCTC = parseFloat($(this).val());
                            TotalAmountCTC = TotalAmountCTC + amountCTC;
                        }
                    }
                }

                pelInGross = $(this).closest('tr').find("#[id*=hdfErnPartofGross]").val();
                if (!isNaN(pelInGross) && pelInGross == 1) {//Calculate Total if the Pay Element included in Gross               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amountGross = parseFloat($(this).val());
                            TotalAmountGross = TotalAmountGross + amountGross;
                        }
                    }
                }
            }
        });

        if ($("#[id*=hdfIsGridRecord]").val() > 0) {
            $("#[id*=grdEarnings] [id*=lblTotalEarnAmount]").html(TotalAmount.toFixed(DecimalDigits));
            if ($("#[id*=grdDeductions]").find("[id*=lblTotalDeductAmount]").length > 0)
                TotalDedctn = parseFloat($("#[id*=grdDeductions]").find("[id*=lblTotalDeductAmount]").html().replace(/[^0-9\.]+/g, ""));
            NetSalry = TotalAmount - TotalDedctn;
            $("#[id*=txtEmpNetSalary]").val(NetSalry.toFixed(DecimalDigits));

            $("#[id*=grdEarnings] [id*=hdfTotalErnCTC]").val(TotalAmountCTC.toFixed(DecimalDigits));
            if ($("#[id*=grdDeductions]").find("[id*=hdfTotalDedCTC]").length > 0)
                TotalDedctnCTC = parseFloat($("#[id*=grdDeductions]").find("[id*=hdfTotalDedCTC]").val().replace(/[^0-9\.]+/g, ""));
            NetCTC = TotalAmountCTC - TotalDedctnCTC;
            $("#[id*=txtTotalCTC]").val(NetCTC.toFixed(DecimalDigits));

            $("#[id*=grdEarnings] [id*=hdfTotalErnGross]").val(TotalAmountGross.toFixed(DecimalDigits));
            if ($("#[id*=grdDeductions]").find("[id*=hdfTotalDedGross]").length > 0)
                TotalDedctnGross = parseFloat($("#[id*=grdDeductions]").find("[id*=hdfTotalDedGross]").val().replace(/[^0-9\.]+/g, ""));
            NetGross = TotalAmountGross - TotalDedctnGross;
            $("#[id*=txtTotalGross]").val(NetGross.toFixed(DecimalDigits));

            // Emp Salary  Appraisal Section
            if (!isNaN(parseFloat($("#[id*=txtOldNetSalary]").val())))
                OldNetSalry = $("#[id*=txtOldNetSalary]").val()
            DiffNetSalry = NetSalry - OldNetSalry;
            $("#[id*=txtDiffNetSalary]").val(DiffNetSalry.toFixed(DecimalDigits));


            if (!isNaN(parseFloat($("#[id*=txtOldCTC]").val())))
                OldAmountCTC = $("#[id*=txtOldCTC]").val()
            DiffAmountCTC = NetCTC - OldAmountCTC;
            $("#[id*=txtDiffCTC]").val(DiffAmountCTC.toFixed(DecimalDigits));


            if (!isNaN(parseFloat($("#[id*=txtOldGrossSalary]").val())))
                OldGross = $("#[id*=txtOldGrossSalary]").val()
            DiffGross = NetGross - OldGross;
            $("#[id*=txtDiffGrossSalary]").val(DiffGross.toFixed(DecimalDigits));
        }
    }

    function CalculateTotalDeductions(ctrl) {
        var TotalAmount = 0;
        var TotalEarn = 0;
        var NetSalry = 0;
        var TotalAmountCTC = 0;
        var TotalEarnCTC = 0;
        var NetCTC = 0;
        var TotalAmountGross = 0;
        var TotalEarnGross = 0;
        var NetGross = 0;

        var DiffAmountCTC = 0;
        var DiffGross = 0;
        var DiffNetSalry = 0;
        var OldAmountCTC = 0;
        var OldGross = 0;
        var OldNetSalry = 0;

        if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
            DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
        }
        $("#[id*=grdDeductions] input[type=text][id*=txtDeductAmount]").each(function (index) {
            var amount = 0;
            var pelInSalary = 0;
            var amountCTC = 0;
            var pelInCTC = 0;
            var amountGross = 0;
            var pelInGross = 0;

            var eidIsDelete = $(this).closest('tr').find("#[id*=hdfDedEID_IS_DELETE]").val();
            if (!isNaN(eidIsDelete) && eidIsDelete == 0) //Calculate Total if the Pay Element is Deleted or not (Employee Appraisal Page)
            {
                pelInSalary = $(this).closest('tr').find("#[id*=hdfDedPayElmtInSalary]").val();
                if (!isNaN(pelInSalary) && pelInSalary == 1) {//Calculate Total if the Pay Element included in Salary               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amount = parseFloat($(this).val());
                            TotalAmount = TotalAmount + amount;
                        }
                    }
                }

                pelInCTC = $(this).closest('tr').find("#[id*=hdfDedPartofCTC]").val();
                if (!isNaN(pelInCTC) && pelInCTC == 1) {//Calculate Total if the Pay Element included in Salary               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amountCTC = parseFloat($(this).val());
                            TotalAmountCTC = TotalAmountCTC + amountCTC;
                        }
                    }
                }

                pelInGross = $(this).closest('tr').find("#[id*=hdfDedPartofGross]").val();
                if (!isNaN(pelInGross) && pelInGross == 1) {//Calculate Total if the Pay Element included in Salary               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amountGross = parseFloat($(this).val());
                            TotalAmountGross = TotalAmountGross + amountGross;
                        }
                    }
                }
            }
        });

        if ($("#[id*=hdfIsGridRecord]").val() > 0) {
            $("#[id*=grdDeductions] [id*=lblTotalDeductAmount]").html(TotalAmount.toFixed(DecimalDigits));
            if ($("#[id*=txtEmpNetSalary]").length > 0) {
                if ($("#[id*=grdEarnings]").find("[id*=lblTotalEarnAmount]").length > 0)
                    TotalEarn = parseFloat($("#[id*=grdEarnings]").find("[id*=lblTotalEarnAmount]").html().replace(/[^0-9\.]+/g, ""));
                NetSalry = TotalEarn - TotalAmount;
                $("#[id*=txtEmpNetSalary]").val(NetSalry.toFixed(DecimalDigits));
            }
            $("#[id*=grdDeductions] [id*=hdfTotalDedCTC]").val(TotalAmountCTC.toFixed(DecimalDigits));
            if ($("#[id*=txtTotalCTC]").length > 0) {
                if ($("#[id*=grdEarnings]").find("[id*=hdfTotalErnCTC]").length > 0)
                    TotalEarnCTC = parseFloat($("#[id*=grdEarnings]").find("[id*=hdfTotalErnCTC]").val().replace(/[^0-9\.]+/g, ""));
                NetCTC = TotalEarnCTC - TotalAmountCTC;
                $("#[id*=txtTotalCTC]").val(NetCTC.toFixed(DecimalDigits));
            }
            $("#[id*=grdDeductions] [id*=hdfTotalDedGross]").val(TotalAmountGross.toFixed(DecimalDigits));
            if ($("#[id*=txtTotalGross]").length > 0) {
                if ($("#[id*=grdEarnings]").find("[id*=hdfTotalErnGross]").length > 0)
                    TotalEarnGross = parseFloat($("#[id*=grdEarnings]").find("[id*=hdfTotalErnGross]").val().replace(/[^0-9\.]+/g, ""));
                NetGross = TotalEarnGross - TotalAmountGross;
                $("#[id*=txtTotalGross]").val(NetGross.toFixed(DecimalDigits));
            }


            if (!isNaN(parseFloat($("#[id*=txtOldNetSalary]").val())))
                OldNetSalry = $("#[id*=txtOldNetSalary]").val()
            DiffNetSalry = NetSalry - OldNetSalry;
            $("#[id*=txtDiffNetSalary]").val(DiffNetSalry.toFixed(DecimalDigits));


            if (!isNaN(parseFloat($("#[id*=txtOldCTC]").val())))
                OldAmountCTC = $("#[id*=txtOldCTC]").val()
            DiffAmountCTC = NetCTC - OldAmountCTC;
            $("#[id*=txtDiffCTC]").val(DiffAmountCTC.toFixed(DecimalDigits));


            if (!isNaN(parseFloat($("#[id*=txtOldGrossSalary]").val())))
                OldGross = $("#[id*=txtOldGrossSalary]").val()
            DiffGross = NetGross - OldGross;
            $("#[id*=txtDiffGrossSalary]").val(DiffGross.toFixed(DecimalDigits));
        }
    }


    function ShowDeleteConfirmationMsg(btn, message) {
        var msgTitle;
        var msg;
        msgTitle = '<%= Resources.ErpRes.Title_Information %>';
        msg = message ? message : '<%= Resources.ErpRes.MsgDeleteConfirm %>';
        $("#popupHolder").html("");
        //To set fit to screen
        $('html, body').animate({ scrollTop: '0px' }, 0);
        $('html, body').css('overflow', 'hidden');
        $("#divConfirmation").html(msg).dialog({
            modal: true,
            height: 150,
            width: 350,
            title: msgTitle,
            resizable: false,
            buttons: {
                OK: function (e) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                    $(this).dialog("close");
                    __doPostBack(btn.name, '');
                },
                Cancel: function (e) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
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

    $("[id*=chkErnPayElmDelete]").live("click", function () {
        if (!$(this).is(":checked")) {
            $("td", $(this).closest("tr")).removeClass("selected");
           $(this).closest('tr').find("#[id*=hdfErnEID_IS_DELETE]").val(0);
        } else {
            $("td", $(this).closest("tr")).addClass("selected");
            $(this).closest('tr').find("#[id*=hdfErnEID_IS_DELETE]").val(1);
        }
        CalculateTotalEarnings(this);
    });

    $("[id*=chkDedPayElmDelete]").live("click", function () {
        if (!$(this).is(":checked")) {
            $("td", $(this).closest("tr")).removeClass("selected");
            $(this).closest('tr').find("#[id*=hdfDedEID_IS_DELETE]").val(0);
        } else {
            $("td", $(this).closest("tr")).addClass("selected");
            $(this).closest('tr').find("#[id*=hdfDedEID_IS_DELETE]").val(1);
        }
        CalculateTotalDeductions(this);
    }); 
</script>
<div class="gridwrap">
    <div class="gridwrap floatLeft" style="width: 49%;">
        <div class="split-head">
            <h4 class="check-inline">
                <%= GetGlobalResourceObject("Controls", "Earnings").ToString()%></h4>
            <%-- <asp:ImageButton runat="server" ID="imbShowEarnings" OnClientClick="javascript:return ShowHideEarnings(1);"
                SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="13" />
            <asp:ImageButton runat="server" ID="imbHideEarnings" OnClientClick="javascript:return ShowHideEarnings();"
                Style="display: none" SkinID="imbArrowHide" TabIndex="14" ToolTip="<%$ resources:Controls,HideDetails%>" />--%>
            <asp:Button runat="server" ID="btnAddEarnings" CommandName="ADDNEWEARNINGS" TabIndex="10"
                OnClick="ActionHandler" ToolTip="<%$ resources:Controls,New %>" ValidationGroup="upload"
                Text="<%$ resources:Controls,New %>" SkinID="btnInner-add" Style="float: right;
                margin-right: 0px; margin-bottom: 2px" />
        </div>
        <div class="clear">
        </div>
        <div id="divEarnings">
            <asp:GridView ID="grdEarnings" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                ShowFooter="true" OnRowDataBound="ActionHandler">
                <EmptyDataTemplate>
                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton CssClass="nomargin" ID="imbRuleUpEarn" SkinID="move-up" runat="server"
                                OnClick="ActionHandler" CommandName="MOVEUP" ToolTip="Move Up" CommandArgument='<%# Eval("STS_SL_NO") %>'>
                            </asp:ImageButton>
                            <asp:ImageButton CssClass="nomargin" ID="imbRuleDownEarn" SkinID="move-down" runat="server"
                                OnClick="ActionHandler" CommandName="MOVEDOWN" ToolTip="Move Down" CommandArgument='<%# Eval("STS_SL_NO") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                        <HeaderStyle Width="2%" />
                        <ItemStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Controls,Earnings %>">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfEarnSlNo" runat="server" Value='<%#Eval("STS_SL_NO")%>' />
                            <asp:HiddenField ID="hdfEarnPK" runat="server" Value='<%#Eval("EDP_PK")%>' />
                            <asp:HiddenField ID="hdfEarnPayElementPK" runat="server" Value='<%#Eval("STS_PK")%>' />
                            <asp:HiddenField ID="hdfEarnPayElement" runat="server" Value='<%#Eval("STS_PAY_ELEMENT")%>' />
                            <asp:HiddenField ID="hdfEarnCalcMode" runat="server" Value='<%#Eval("STS_CALC_MODE")%>' />
                            <asp:HiddenField ID="hdfErnHasPayroll" runat="server" Value='<%#Eval("STS_HAS_PAYROLL")%>' />
                            <asp:HiddenField ID="hdfEarnPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                            <asp:HiddenField ID="hdfEarnShowPayElement" runat="server" Value='<%#Eval("PEL_SHOW_IN_EMPMAST")%>' />
                            <asp:HiddenField ID="hdfErnPartofCTC" runat="server" Value='<%#Eval("PEL_IN_CTC")%>' />
                            <asp:HiddenField ID="hdfErnPartofGross" runat="server" Value='<%#Eval("PEL_IN_GROSS")%>' />
                            <asp:HiddenField ID="hdfEID_PK" runat="server" Value='<%#Eval("EID_PK")%>' />
                            <asp:HiddenField ID="hdfEID_APPRAISAL_HDR" runat="server" Value='<%#Eval("EID_APPRAISAL_HDR")%>' />
                            <asp:HiddenField ID="hdfErnEID_IS_DELETE" runat="server" Value='<%#Eval("EID_IS_DELETE")%>' />
                            <asp:Label ID="lblEarnHead" runat="server" Text='<%#Eval("STS_PAY_ELEMENT_TEXT")%>'></asp:Label>
                           
                            <asp:DropDownList ID="ddlEarnPayElement" TabIndex="11" runat="server" CssClass="select-full-a custom-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                            </asp:DropDownList>
                          
                            <asp:RequiredFieldValidator ID="rfvEarnPayElement" CssClass="star" SetFocusOnError="true"
                                InitialValue="-1" ValidationGroup="salary" EnableClientScript="true" runat="server"
                                ControlToValidate="ddlEarnPayElement" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_EarningsPayElmt %>">
                            </asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <HeaderStyle Width="30%" />
                        <ItemStyle Width="30%" Wrap="false" />
                        <FooterStyle Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblEarnTotalTxt" Text="<%$ resources:Controls,TotalEarnings %>"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:Image CssClass="hide"  ID="imgEarnMode" runat="server"  AlternateText=" "  ></asp:Image>--%>
                            <div id="divEarnMode" runat="server" class="hide">
                            </div>
                        </ItemTemplate>
                        <HeaderStyle Width="3%" CssClass="padgrgt3" />
                        <ItemStyle Width="3%" CssClass="padgrgt3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <%--<div style="float: left; width: 95%;">--%>
                            <asp:HiddenField ID="hdfEarnFormulaCode" runat="server" Value='<%#Eval("STS_FORMULA_CODE")%>' />
                            <asp:HiddenField ID="hdfIsEarn" runat="server" Value='<%#Eval("PEL_IS_DEDUCTION")%>' />
                            <asp:HiddenField ID="hdfEarnFormula" runat="server" Value='<%#Eval("STS_VALUE_TEXT")%>' />
                            <asp:Label ID="lblEarnFormula" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("STS_VALUE_TEXT"), 28 )%>'
                                ToolTip='<%#Eval("STS_VALUE_TEXT") + ((Convert.ToDecimal(Eval("STS_MIN_AMT")) > 0 || Convert.ToDecimal(Eval("STS_MAX_AMT")) > 0)? ("(" + Resources.Controls.Min.ToString() + GetFormattedCurrencyWithComma(Eval("STS_MIN_AMT")) +", "+ Resources.Controls.Max.ToString() + GetFormattedCurrencyWithComma(Eval("STS_MAX_AMT"))+")") : "")%>'></asp:Label>
                            <asp:HiddenField ID="hdfEarnCalcValue" runat="server" Value='<%#Eval("STS_CALC_VALUE")%>' />
                            <asp:HiddenField ID="hdfErnMinAmount" runat="server" Value='<%#Eval("STS_MIN_AMT")%>' />
                            <asp:HiddenField ID="hdfErnMaxAmount" runat="server" Value='<%#Eval("STS_MAX_AMT")%>' />
                            <asp:DropDownList ID="ddlErnSlabCustom" runat="server" CssClass="select-full" Visible="false"  TabIndex="10">
                            </asp:DropDownList >
                            <asp:RequiredFieldValidator ID="rfvErnSlabCustom" CssClass="star" SetFocusOnError="true"
                                Enabled="false" InitialValue="-1" ValidationGroup="salary" EnableClientScript="true"
                                runat="server" ControlToValidate="ddlErnSlabCustom" Display="Dynamic" Text="*"
                                ErrorMessage="<%$ resources:ErrorMessages,Err_SelectPayElmtValue %>">
                            </asp:RequiredFieldValidator>
                            <%--</div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="28%" />
                        <ItemStyle Width="28%" CssClass="wordwrap padglft0" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%-- <div style="width:35px; float: left; display:table-cell;">--%>
                            <asp:ImageButton runat="server" ID="imbFormula" SkinID="salary-formula" CssClass="margntop2"
                                ToolTip="<%$resources:Controls,Formula %>" OnClick="ActionHandler" CommandName="EARNFORMULAPOPUP"
                                Visible='<%# (Eval("STS_CALC_MODE").ToString() == "1" && Eval("PEL_IS_FORMULA_EDITABLE").ToString() == "1") ? true : false %>' />
                            <%-- </div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="3%" />
                        <ItemStyle Width="3%" HorizontalAlign="Right" CssClass="padgrgt3" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEarnAmount_OLD" CssClass="lbl-82-2perc numeric" runat="server"
                                Text='<%#GetFormattedCurrency(Eval("STS_VALUE_OLD"))%>' Enabled="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="12%" CssClass="amount-numeric" />
                        <ItemStyle Width="12%" CssClass="amount-numeric" Wrap="false" />
                        <ItemStyle CssClass="amount-numeric padglft0" />
                        <FooterStyle CssClass="amount-numeric" Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblOLDTotalEarnAmount"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Controls,Amount %>">
                        <ItemTemplate>
                            <%-- <div style="width: 90px; float: right;">--%>
                            <asp:TextBox ID="txtEarnAmount" CssClass="input-w80 numeric" runat="server" Text='<%#GetFormattedCurrency(Eval("STS_VALUE"))%>'
                                onkeyup="CalculateTotalEarnings(this);" TabIndex="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEarnAmount" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="txtEarnAmount"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_EarnAmount %>">
                            </asp:RequiredFieldValidator>
                            <cc1:AmountValidation ID="vamEarnAmount" runat="server" ControlToValidate="txtEarnAmount"
                                ErrorMessage="<%$ resources:ErrorMessages,EarnValidAmount %>" NumberDigits="11"
                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="salary"></cc1:AmountValidation>
                            <%-- </div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" CssClass="amount-numeric" />
                        <ItemStyle Width="10%" CssClass="amount-numeric" Wrap="false" />
                        <ItemStyle CssClass="amount-numeric padglft0" />
                        <FooterStyle CssClass="amount-numeric" Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblTotalEarnAmount"></asp:Label>
                            <asp:HiddenField runat="server" ID="hdfTotalErnCTC" />
                            <asp:HiddenField runat="server" ID="hdfTotalErnGross" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEEARNINGS"
                                Visible='<%# Convert.ToInt32(Eval("STS_HAS_PAYROLL")) > 0? false : true %>' SkinID="delete-icon"
                                Style="margin-right: 0px!important;" ToolTip="<%$ resources:Controls,Delete %>"
                                OnClientClick="return ShowDeleteConfirmationMsg(this);" TabIndex="10" />
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Button ID="lnkErnRemove" runat="server" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                Enabled="false" TabIndex="10" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkErnPayElmDelete" ToolTip="<%$ resources:Controls,Delete %>"
                                Checked='<%# Convert.ToInt32(Eval("EID_IS_DELETE")) > 0 ? true : false %>'  TabIndex="10"/>
                        </ItemTemplate>
                        <HeaderStyle Width="2%" Wrap="false" />
                        <ItemStyle Width="2%" Wrap="false" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div style="width: 2%;">
    </div>
    <div class="gridwrap floatRight" style="width: 49%;">
        <div class="split-head">
            <h4 class="check-inline">
                <%= GetGlobalResourceObject("Controls", "Deductions").ToString()%></h4>
            <%-- <asp:ImageButton runat="server" ID="imbShowDeductions" OnClientClick="javascript:return ShowHideDeductions(1);"
                SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="13" />
            <asp:ImageButton runat="server" ID="imbHideDeductions" OnClientClick="javascript:return ShowHideDeductions();"
                Style="display: none" SkinID="imbArrowHide" TabIndex="14" ToolTip="<%$ resources:Controls,HideDetails%>" />--%>
            <asp:Button runat="server" ID="btnAddDeduction" CommandName="ADDNEWDEDUCTION" TabIndex="12"
                OnClick="ActionHandler" ToolTip="<%$ resources:Controls,New %>" ValidationGroup="upload"
                Text="<%$ resources:Controls,New %>" SkinID="btnInner-add" Style="float: right;
                margin-right: 0px; margin-bottom: 2px" />
        </div>
        <div class="clear">
        </div>
        <div id="divDeductions">
            <asp:GridView ID="grdDeductions" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                ShowFooter="true" OnRowDataBound="ActionHandler">
                <EmptyDataTemplate>
                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton CssClass="nomargin" ID="imbRuleUpDedu" SkinID="move-up" runat="server"
                                OnClick="ActionHandler" CommandName="MOVEUP" ToolTip="Move Up" CommandArgument='<%# Eval("STS_SL_NO") %>'>
                            </asp:ImageButton>
                            <asp:ImageButton CssClass="nomargin" ID="imbRuleDownDedu" SkinID="move-down" runat="server"
                                OnClick="ActionHandler" CommandName="MOVEDOWN" ToolTip="Move Down" CommandArgument='<%# Eval("STS_SL_NO") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                        <HeaderStyle Width="2%" />
                        <ItemStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Controls,Deductions %>">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfDeductSlNo" runat="server" Value='<%#Eval("STS_SL_NO")%>' />
                            <asp:HiddenField ID="hdfDeductPK" runat="server" Value='<%#Eval("EDP_PK")%>' />
                            <asp:HiddenField ID="hdfDeductPayElementPK" runat="server" Value='<%#Eval("STS_PK")%>' />
                            <asp:HiddenField ID="hdfDeductPayElement" runat="server" Value='<%#Eval("STS_PAY_ELEMENT")%>' />
                            <asp:HiddenField ID="hdfDeductCalcMode" runat="server" Value='<%#Eval("STS_CALC_MODE")%>' />
                            <asp:HiddenField ID="hdfDeductHasPayroll" runat="server" Value='<%#Eval("STS_HAS_PAYROLL")%>' />
                            <asp:HiddenField ID="hdfDedPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                            <asp:HiddenField ID="hdfDedShowPayElement" runat="server" Value='<%#Eval("PEL_SHOW_IN_EMPMAST")%>' />
                            <asp:HiddenField ID="hdfDedPartofCTC" runat="server" Value='<%#Eval("PEL_IN_CTC")%>' />
                            <asp:HiddenField ID="hdfDedPartofGross" runat="server" Value='<%#Eval("PEL_IN_GROSS")%>' />
                            <asp:Label ID="lblDeductHead" runat="server" Text='<%#Eval("STS_PAY_ELEMENT_TEXT")%>'></asp:Label>
                            <asp:HiddenField ID="hdfDedEID_PK" runat="server" Value='<%#Eval("EID_PK")%>' />
                            <asp:HiddenField ID="hdfDedEID_APPRAISAL_HDR" runat="server" Value='<%#Eval("EID_APPRAISAL_HDR")%>' />
                            <asp:HiddenField ID="hdfDedEID_IS_DELETE" runat="server" Value='<%#Eval("EID_IS_DELETE")%>' />
                            <asp:DropDownList ID="ddlDeductPayElement" TabIndex="11" runat="server" CssClass="select-full-a custom-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDeductPayElement" CssClass="star" SetFocusOnError="true"
                                InitialValue="-1" ValidationGroup="salary" EnableClientScript="true" runat="server"
                                ControlToValidate="ddlDeductPayElement" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_DeductPayElmt %>">
                            </asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <HeaderStyle Width="25%" />
                        <ItemStyle Width="25%" Wrap="false" />
                        <FooterStyle Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblDeductTotalTxt" Text="<%$ resources:Controls,TotalDeduction %>"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div id="divDeductMode" runat="server" class="hide">
                            </div>
                            <%--<asp:Image CssClass="hide" ID="imgDeductMode" runat="server" AlternateText=" "></asp:Image>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="3%" CssClass="padgrgt3" />
                        <ItemStyle Width="3%" CssClass="padgrgt3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <%--  <div style="float: left; width: 95%;">--%>
                            <asp:HiddenField ID="hdfDeductFormulaCode" runat="server" Value='<%#Eval("STS_FORMULA_CODE")%>' />
                            <asp:HiddenField ID="hdfIsDeduct" runat="server" Value='<%#Eval("PEL_IS_DEDUCTION")%>' />
                            <asp:HiddenField ID="hdfDeductFormula" runat="server" Value='<%# Eval("STS_VALUE_TEXT") %>' />
                            <asp:Label ID="lblDeductFormula" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("STS_VALUE_TEXT"),28) %>'
                                ToolTip='<%#Eval("STS_VALUE_TEXT") + ((Convert.ToDecimal(Eval("STS_MIN_AMT")) > 0 || Convert.ToDecimal(Eval("STS_MAX_AMT")) > 0)? ("(" + Resources.Controls.Min.ToString() + GetFormattedCurrencyWithComma(Eval("STS_MIN_AMT")) +", "+ Resources.Controls.Max.ToString() + GetFormattedCurrencyWithComma(Eval("STS_MAX_AMT"))+")") : "")%>'></asp:Label>
                            <asp:HiddenField ID="hdfDeductCalcValue" runat="server" Value='<%#Eval("STS_CALC_VALUE")%>' />
                            <asp:HiddenField ID="hdfDeductMinAmount" runat="server" Value='<%#Eval("STS_MIN_AMT")%>' />
                            <asp:HiddenField ID="hdfDeductMaxAmount" runat="server" Value='<%#Eval("STS_MAX_AMT")%>' />
                            <asp:DropDownList ID="ddlDeductSlabCustom" runat="server" CssClass="select-full" TabIndex="12"
                                Visible="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDeductSlabCustom" CssClass="star" SetFocusOnError="true"
                                Enabled="false" InitialValue="-1" ValidationGroup="salary" EnableClientScript="true"
                                runat="server" ControlToValidate="ddlDeductSlabCustom" Display="Dynamic" Text="*"
                                ErrorMessage="<%$ resources:ErrorMessages,Err_SelectPayElmtValue %>">
                            </asp:RequiredFieldValidator>
                            <%-- </div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="28%" />
                        <ItemStyle Width="28%" CssClass="wordwrap padglft0" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%-- <div style="width: 35px; float: left;">--%>
                            <asp:ImageButton runat="server" ID="imbDeductFormula" SkinID="salary-formula" CssClass="margntop2"
                                ToolTip="<%$resources:Controls,Formula %>" OnClick="ActionHandler" CommandName="DEDUCTFORMULAPOPUP"
                                Visible='<%# (Eval("STS_CALC_MODE").ToString() == "1" && Eval("PEL_IS_FORMULA_EDITABLE").ToString() == "1") ? true : false %>' />
                            <%-- </div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="3%" />
                        <ItemStyle Width="3%" HorizontalAlign="Right" CssClass="padgrgt3" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDeductAmount_OLD" CssClass="lbl-82-2perc numeric" runat="server"
                                Text='<%#GetFormattedCurrency(Eval("STS_VALUE_OLD"))%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="12%" CssClass="amount-numeric" />
                        <ItemStyle Width="12%" CssClass="amount-numeric" Wrap="false" />
                        <ItemStyle CssClass="amount-numeric padglft0" />
                        <FooterStyle CssClass="amount-numeric" Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblOLDTotalDeductAmount"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Controls,Amount %>">
                        <ItemTemplate>
                            <%-- <div style="width: 90px; float: right;">--%>
                            <asp:TextBox ID="txtDeductAmount" CssClass="input-w80 numeric" runat="server" Text='<%#GetFormattedCurrency(Eval("STS_VALUE"))%>'
                                onkeyup="CalculateTotalDeductions(this);"  TabIndex="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDeductAmount" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="txtDeductAmount"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_DeductAmount %>">
                            </asp:RequiredFieldValidator>
                            <cc1:AmountValidation ID="vamDeductAmount" runat="server" ControlToValidate="txtDeductAmount"
                                ErrorMessage="<%$ resources:ErrorMessages,DeductValidAmount %>" NumberDigits="11"
                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="salary"></cc1:AmountValidation>
                            <%--</div>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" CssClass="amount-numeric" />
                        <ItemStyle Width="10%" CssClass="amount-numeric" Wrap="false" />
                        <ItemStyle CssClass="amount-numeric padglft0" />
                        <FooterStyle CssClass="amount-numeric" Wrap="false" />
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lblTotalDeductAmount"></asp:Label>
                            <asp:HiddenField runat="server" ID="hdfTotalDedCTC" />
                            <asp:HiddenField runat="server" ID="hdfTotalDedGross" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEDEDUCTION"
                                Visible='<%# Convert.ToInt32(Eval("STS_HAS_PAYROLL")) > 0? false : true %>' SkinID="delete-icon"
                                Style="margin-right: 0px!important;" ToolTip="<%$ resources:Controls,Delete %>"
                                OnClientClick="return ShowDeleteConfirmationMsg(this);"  TabIndex="12" />
                        </ItemTemplate>
                        <HeaderStyle Width="6%" />
                        <ItemStyle Width="6%" Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate >
                            <asp:Button ID="lnkDedRemove" runat="server" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                Enabled="false"   TabIndex="12"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkDedPayElmDelete" ToolTip="<%$ resources:Controls,Delete %>"
                                Checked='<%# Convert.ToInt32(Eval("EID_IS_DELETE")) > 0 ? true : false %>'  TabIndex="12" />
                        </ItemTemplate>
                        <HeaderStyle Width="2%" Wrap="false" />
                        <ItemStyle Width="2%" Wrap="false" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="diverrormsg" style="display: none">
    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
</div>
<div id="divPopUpFormula" style="display: none">
    <ucgti:FormulaMaster id="ucFormulaMaster" runat="server" afterapply="ucPopUpFormula_AfterApply"
        IsSlab="0" ShowMinMaxAmount="1" />
</div>
<asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
<asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
<asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
<asp:HiddenField ID="hdfIsGridRecord" Value="0" runat="server" />
 <asp:HiddenField runat="server" ID="hdfCurrentDepartment" Value="-1" />
