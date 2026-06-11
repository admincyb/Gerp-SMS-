using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Configurations;
using BusinessLogic.CommonManagement;
using ERP.Utilities;
using GTIService.Constants.Administration.Configurations;
//using gBudget.BO.AdministrationBO;
using System.Web.Security;
//using gBudget.BO.AccountsBO;
//using gBudget.BL.AdministrationBL;
//using gBudget.Utilities;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class Mgtb : ERP.Store.UI.MyBasePage
    {
        #region Properties
        private bool IsValidState
        {
            get { return this.ViewState["IsValidState"] == null ? false : Convert.ToBoolean(this.ViewState["IsValidState"].ToString()); }
            set { this.ViewState["IsValidState"] = value; }
        }
        private string DeleteQuery
        {
            get { return this.ViewState["DeleteQuery"] == null ? string.Empty : this.ViewState["DeleteQuery"].ToString(); }
            set { this.ViewState["DeleteQuery"] = value; }
        }
        private string PKValue
        {
            get { return this.ViewState["PKValue"] == null ? string.Empty : this.ViewState["PKValue"].ToString(); }
            set { this.ViewState["PKValue"] = value; }
        }
        #endregion

        #region VARIABLES

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        StringBuilder sb = new StringBuilder();
        // private IdentityUser currentUser;
        private DataTable dtResult;
        private ActionsEnum commonActions;
        private DataTable dtTable;
        private DataTable dtTBFilterGrid;
        private DataTable dtFieldFilterGrid;
        private DataTable dtViewQueryGrid;

        private string TableName;
        private string QueryTxt;
        private string FldName;
        private string FldType;
        private string FldValue;
        private string ButtonType;
        private string Params;

        private string HeaderText;

        private string[] TypeName;
        private string[] ColName;
        private string[] SplitParams;
        private string[] ColumnNames;
        private string[] Values;
        private BusinessObject.User currentUser;

        List<Operator> operatorList = new List<Operator>();
        List<TableSchema> lstSchema = new List<TableSchema>();
        int result = 0;

        #endregion

        #region PAGE LEVEL EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            string dbName = con.Database.ToString();
            string cons = con.DataSource.ToString();
            PageActionHandler();
        }
        private void SetPageVariables()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        protected void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            SetPageVariables();

            if (!IsSuperAdminUser(currentUser.PKUser))
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                GetFieldValues(ControlsEnum.TABLES);
                SetFeildValues(ControlsEnum.TABLES);
                GetFieldValues(ControlsEnum.OPERATORS);
                string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                string connection = conString.Replace(";", ", ");
                lblConnectionString.Text = GetShortString(connection, 100);
                lblConnectionString.ToolTip = connection;
              //  BindDBDetails();
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (IsValidState)
            {
                divManageTables.Visible = true;
                divUserLogin.Visible = false;
            }
            else
            {
                divManageTables.Visible = false;
                divUserLogin.Visible = true;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedSearch", "ShowHideAdvancedSearch('1');", true);
        }

        #endregion

        #region ACTION HANDLER

        protected void ActionHandler(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }

            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                if (((DropDownList)sender).ID == "ddlTables")
                    commonActions = ActionsEnum.GETFEILD;
                else
                    if (((DropDownList)sender).ID == "ddlFields")
                        commonActions = ActionsEnum.GETOPERATORS;
            }

            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }

            switch (commonActions)
            {
                case ActionsEnum.INSERTQUERY:
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowContainerDiv('#divQueryDetails','" + "Query Window" + "','900','500');", true);//GetLocalResourceObject("ExpenseAccount").ToString()
                    break;
                case ActionsEnum.SUBMITQUERY:
                    if (!string.IsNullOrEmpty(txtQuery.Text.Trim()))
                    {
                        SqlCommand cmdQuery = new SqlCommand(txtQuery.Text, con);
                        con.Open();
                        result = cmdQuery.ExecuteNonQuery();
                        con.Close();
                        if (result >= 1)
                        {
                            txtQuery.Text = string.Empty;
                            litErrorMsg.Text = "Query Executed Successfully.";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = "Please enter the Query";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
                        grdManageTable.DataSource = null;
                        grdManageTable.DataBind();
                    }
                    break;
                case ActionsEnum.VIEWQUERY:
                    if (!string.IsNullOrEmpty(txtQuery.Text.Trim()))
                    {
                        try
                        {
                            SqlCommand cmdQuery = new SqlCommand(txtQuery.Text, con);
                            con.Open();
                            SqlDataAdapter sdaView = new SqlDataAdapter(cmdQuery);
                            dtViewQueryGrid = new DataTable();
                            sdaView.Fill(dtViewQueryGrid);
                            cmdQuery.ExecuteReader();
                            con.Close();

                            if (dtViewQueryGrid != null && dtViewQueryGrid.Rows.Count > 0)
                            {
                                grdManageTable.DataSource = dtViewQueryGrid;
                                grdManageTable.DataBind();
                            }
                            else
                            {
                                BindGrid(ControlsEnum.FIELDS);
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message.ToString();
                            txtQuery.Text = string.Empty;
                            litErrorMsg.Text = message;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
                            grdManageTable.DataSource = null;
                            grdManageTable.DataBind();
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = "Please enter the Query";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);

                        grdManageTable.DataSource = null;
                        grdManageTable.DataBind();
                    }
                    break;
                case ActionsEnum.DELETE:
                    SqlCommand cmd = new SqlCommand(DeleteQuery, con);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                    if (result >= 1)
                    {
                        litErrorMsg.Text = "Row Deleted at Position " + ((string[])ViewState["columnNames"])[0] + " = " + PKValue.ToString(); 
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);

                    }
                    else
                    {
                        litErrorMsg.Text = "Error Deleting!";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);

                    }

                    GetFieldValues(ControlsEnum.BINDGRID);
                    SetFeildValues(ControlsEnum.BINDGRID);
                    break;
                case ActionsEnum.SUBMIT:
                    IsValidState = IsValidUser(txtPassword.Text);
                    break;
                #region SEARCH
                case ActionsEnum.SEARCH:

                    divError.Visible = false;

                    if (ddlTables.SelectedIndex == 0)
                    {
                        if (grdManageTable.DataSource == null)
                        {
                            grdManageTable.DataSource = new string[] { };
                        }
                        grdManageTable.DataBind();
                    }
                    else if (ddlFields.SelectedItem.Text != string.Empty && ddlOperator.SelectedValue != string.Empty && txtQueryField.Text != string.Empty)
                    {
                        if (ddlOperator.SelectedIndex == 0)
                        {
                            litErrorMsg.Text = "Select Operator";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
                            BindGrid(ControlsEnum.FIELDS);
                        }
                        else
                        {
                            divError.Visible = false;
                            grdManageTable.EditIndex = -1;
                            GetFieldValues(ControlsEnum.FEILDGRIDFILTER);
                            SetFeildValues(ControlsEnum.FEILDGRIDFILTER);
                        }
                    }
                    else
                    {
                        divError.Visible = false;
                        grdManageTable.EditIndex = -1;
                        GetFieldValues(ControlsEnum.BINDGRID);
                        SetFeildValues(ControlsEnum.BINDGRID);
                    }

                    if (ddlTables.SelectedIndex != 0 && grdManageTable.Rows.Count >= 1)
                    {
                        btnInsert.Visible = true;
                    }

                    break;
                #endregion

                #region GET FIELDS
                case ActionsEnum.GETFEILD:

                    TableName = ddlTables.SelectedValue;

                    QueryTxt = "SELECT TOP 1 * FROM  " + TableName;

                    con.Open();
                    SqlCommand cmdGet = new SqlCommand(QueryTxt, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmdGet);
                    sda.Fill(dt);
                    cmdGet.ExecuteReader();
                    con.Close();

                    int slno = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        TableSchema objTbl = new TableSchema();
                        objTbl.SlNo = slno++;
                        objTbl.FieldName = dc.ColumnName.ToString();
                        objTbl.FieldType = dc.DataType.Name.ToString();
                        lstSchema.Add(objTbl);
                    }
                    ViewState["lstNameandType"] = lstSchema;

                    BindDropdown(ControlsEnum.FIELDS);
                    BindGrid(ControlsEnum.FIELDS);
                   // ResetFields();
                    btnInsert.Visible = false;
                    divError.Visible = false;

                    break;
                #endregion

                #region GET OPERATORS
                case ActionsEnum.GETOPERATORS:
                    if (ddlFields.SelectedIndex != 0)
                    {
                        FldName = ddlFields.SelectedItem.Text;
                        FldType = ((List<TableSchema>)ViewState["lstNameandType"])[Convert.ToInt32(ddlFields.SelectedValue)].FieldType;
                        operatorList = ViewState["OperatorList"] as List<Operator>;
                        BindDropdown(ControlsEnum.TABLEOPERATORS);
                        BindGrid(ControlsEnum.FIELDS);
                        btnInsert.Visible = false;
                    }
                    else
                    {
                        ddlOperator.Items.Clear();
                        ddlOperator.Items.Insert(0, "--Select Operator--");
                    }
                    break;
                #endregion

                #region INSERT NEW ROW
                case ActionsEnum.INSERTROW:

                    btnInsert.Visible = true;
                    grdManageTable.PageIndex = 0;
                    dt = (DataTable)ViewState["GridBind"];
                    DataRow dr = dt.NewRow();
                    dt.Rows.InsertAt(dr, 0);
                    grdManageTable.EditIndex = 0;
                    grdManageTable.DataSource = dt;
                    grdManageTable.DataBind();
                    ((LinkButton)grdManageTable.Rows[0].Cells[0].Controls[0]).Text = "Insert";
                    btnInsert.Visible = true;

                    break;
                #endregion

                #region CLEAR FIELDS
                case ActionsEnum.CLEAR:

                    grdManageTable.EditIndex = -1;
                    ResetFields();
                    //GetFieldValues(ControlsEnum.BINDGRID);
                    //SetFeildValues(ControlsEnum.BINDGRID);

                    break;
                #endregion
            }
        }

        //ROW-EDIT
        protected void ActionHandler(object sender, GridViewEditEventArgs e)
        {
            grdManageTable.EditIndex = e.NewEditIndex;
            if (txtQueryField.Text != string.Empty)
            {
                GetFieldValues(ControlsEnum.FEILDGRIDFILTER);
                SetFeildValues(ControlsEnum.FEILDGRIDFILTER);
            }
            else
            {
                GetFieldValues(ControlsEnum.BINDGRID);
                SetFeildValues(ControlsEnum.BINDGRID);
            }
            divError.Visible = false;
            btnInsert.Visible = true;
        }

        //ROW-MODIFY
        protected void ActionHandler(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = grdManageTable.Rows[e.RowIndex];
                FldValue = string.Empty;
                TableName = ddlTables.SelectedValue;
                TypeName = ViewState["ColumnTypeName"] as string[];
                ColName = ViewState["columnNames"] as string[];
                QueryTxt = string.Empty;
                int colIndex = 0;

                ButtonType = ((LinkButton)grdManageTable.Rows[0].Cells[0].Controls[0]).Text.ToString();

                switch (ButtonType)
                {
                    #region INSERT
                    case "Insert":
                        foreach (TableCell cell in row.Cells)
                        {
                            if (cell.Controls[0].GetType().Name != "DataControlLinkButton")
                            {
                                switch (TypeName[colIndex])
                                {
                                    case "String":
                                    case "Byte":
                                    case "DateTime":
                                        {
                                            FldValue += "'" + (cell.Controls[0] as TextBox).Text.ToString() + "',";
                                            colIndex++;
                                        }
                                        break;

                                    case "Int32":
                                        {
                                            FldValue += "'" + ((cell.Controls[0] as TextBox).Text).ToString() + "',";
                                            colIndex++;
                                        }
                                        break;
                                    default:
                                        FldValue += (cell.Controls[0].GetType().Name != "CheckBox" ? ("'" + ((cell.Controls[0] as TextBox).Text).ToString() + "',") : ("'" + ((cell.Controls[0] as CheckBox).Checked ? 1 : 0).ToString()) + "',");
                                        colIndex++;
                                        break;
                                }
                            }
                        }

                        Params = FldValue.Trim().TrimEnd(',').ToString();
                        SplitParams = Params.Split(',').ToArray();

                        con.Open();
                        string pkValue = "select " + ((string[])ViewState["columnNames"])[0] + " = isnull(max(" + ((string[])ViewState["columnNames"])[0] + "),0)+1 from " + ddlTables.SelectedValue;
                        SqlCommand cmdPK = new SqlCommand(pkValue, con);
                        string pk = Convert.ToString(cmdPK.ExecuteScalar());
                        con.Close();

                        int i = string.Compare(SplitParams[0], pk);
                        if (i != 1)
                        {
                            SplitParams[0] = pk;
                        }

                        string newParam = string.Join(",", SplitParams);
                        QueryTxt = "INSERT INTO " + TableName + " (" + ViewState["ColumnHeaders"] + ") VALUES (" + newParam + ")";

                        con.Open();
                        SqlCommand cmdInsert = new SqlCommand(QueryTxt, con);
                        result = cmdInsert.ExecuteNonQuery();
                        con.Close();
                        if (result >= 1)
                        {
                            litErrorMsg.Text = "Row Inserted Successfully";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Title_Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = "Error while processing!";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
                        }


                        break;
                    #endregion

                    #region UPDATE
                    case "Edit":
                    case "Update":
                        string cellVal = string.Empty;
                        foreach (TableCell cell in row.Cells) //iterate through the cells
                        {
                            cellVal = string.Empty;
                            if (cell.Controls[0].GetType().Name != "DataControlLinkButton")
                            {
                                cellVal = ((cell.Controls[0] as TextBox).Text).ToString();
                                cellVal = string.IsNullOrEmpty(cellVal) ? DBNull.Value.ToString() : cellVal;
                                switch (TypeName[colIndex])
                                {
                                    case "String":
                                    case "Byte":
                                        {
                                            FldValue += FldValue.Length > 0 ? "," + ColName[colIndex] + "= '" + (cell.Controls[0] as TextBox).Text.ToString() + "'" : ColName[colIndex] + "= '" + (cell.Controls[0] as TextBox).Text.ToString() + "'";
                                            colIndex++;
                                        }
                                        break;
                                    case "DateTime":
                                        {
                                            FldValue += FldValue.Length > 0 ? "," + ColName[colIndex] + "= CONVERT(VARCHAR(30),CONVERT(DATETIME,'" + (cell.Controls[0] as TextBox).Text.ToString() + "'), 121)" : ColName[colIndex] + "= convert(nvarchar,'" + (cell.Controls[0] as TextBox).Text.ToString() + "',120)";
                                            colIndex++;
                                        }
                                        break;

                                    case "Int32":
                                        {
                                            FldValue += FldValue.Length > 0 ? "," + ColName[colIndex] + "= '" + cellVal + "'" : ColName[colIndex] + "=" + cellVal;
                                            colIndex++;
                                        }
                                        break;

                                    default:
                                        FldValue += "," + (cell.Controls[0].GetType().Name != "CheckBox" ? ColName[colIndex] + "=" + ((cell.Controls[0] as TextBox).Text).ToString() : ColName[colIndex] + "=" + ((cell.Controls[0] as CheckBox).Checked ? 1 : 0).ToString());
                                        colIndex++;
                                        break;
                                }
                            }
                        }

                        Params = FldValue.Trim().TrimStart(',').ToString();
                        List<string> ParamArry = Params.Split(',').ToList();
                        string PkValue = ParamArry.First();
                        ViewState["ParamValue"] = PkValue.Split('=')[1]; //To get PK value

                        QueryTxt = "UPDATE " + TableName + " SET " + Params + " WHERE " + ((string[])ViewState["columnNames"])[0] + " = " + ViewState["ParamValue"].ToString() + " ";

                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(QueryTxt, con);
                        result = cmd1.ExecuteNonQuery();
                        con.Close();
                        if (result >= 1)
                        {
                            litErrorMsg.Text = "Row at " + ((string[])ViewState["columnNames"])[0] + " = " + ViewState["ParamValue"].ToString() + " Updated Successfully!";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Title_Information + "');", true);

                        }
                        else
                        {
                            litErrorMsg.Text = "Error while processing!";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);

                        }

                        break;
                    #endregion
                }

                grdManageTable.EditIndex = -1;
                btnInsert.Visible = true;
                GetFieldValues(ControlsEnum.BINDGRID);
                SetFeildValues(ControlsEnum.BINDGRID);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                ViewState["errLog"] = msg.ToString();
            }
            finally
            {
                divError.Visible = false;
                if (ViewState["errLog"] == null)
                {
                    divError.Visible = false;
                    ViewState["errLog"] = null;

                }
                else
                {
                    divError.Visible = true;
                    lblErrMsg.Text = ViewState["errLog"].ToString();
                    ViewState["errLog"] = null;
                }
            }
        }

        //CANCEL - EDIT
        protected void ActionHandler(object sender, GridViewCancelEditEventArgs e)
        {
            grdManageTable.EditIndex = -1;
            if (txtQueryField.Text != string.Empty)
            {
                GetFieldValues(ControlsEnum.FEILDGRIDFILTER);
                SetFeildValues(ControlsEnum.FEILDGRIDFILTER);
            }
            else
            {
                GetFieldValues(ControlsEnum.BINDGRID);
                SetFeildValues(ControlsEnum.BINDGRID);
            }
            divError.Visible = false;
            btnInsert.Visible = true;
        }

        //ROW-DELETE

        protected void ActionHandler(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);

                TableName = ddlTables.SelectedValue;
                PKValue = grdManageTable.Rows[e.RowIndex].Cells[1].Text;
                QueryTxt = "DELETE FROM " + TableName + " WHERE " + ((string[])ViewState["columnNames"])[0] + " = " + PKValue;
                DeleteQuery = QueryTxt;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDeleteConfirm", "ShowDeleteGridConfirm();", true);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                ViewState["errLog"] = msg.ToString();
                litErrorMsg.Text = msg.ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Title_Information + "');", true);
            }
            finally
            {
                if (ViewState["errLog"] == null)
                {
                    divError.Visible = false;
                    ViewState["errLog"] = null;
                }
                else
                {
                    divError.Visible = true;
                    lblErrMsg.Text = ViewState["errLog"].ToString();
                    ViewState["errLog"] = null;
                }
                grdManageTable.DataSource = ViewState["GridBind"];
                grdManageTable.DataBind();
            }
        }

        //PAGING
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            grdManageTable.PageIndex = e.NewPageIndex;
            grdManageTable.EditIndex = -1;
            divError.Visible = false;
            btnInsert.Visible = true;
            GetFieldValues(ControlsEnum.BINDGRID);
            SetFeildValues(ControlsEnum.BINDGRID);
        }

        #endregion

        #region GET FEILD VALUES

        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                string query = string.Empty;
                string table = ddlTables.SelectedValue;

                switch (controlType)
                {
                    #region GET TABLE LIST

                    case ControlsEnum.TABLES:

                        btnInsert.Visible = false;
                        divError.Visible = false;
                        con.Open();
                        query = "SELECT name FROM sys.Tables ORDER BY name ASC ";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        dtTable = new DataTable();
                        sda.Fill(dtTable);
                        con.Close();

                        break;

                    #endregion

                    #region GET OPERATORS

                    case ControlsEnum.OPERATORS:

                        string[] DBOperators = GetGlobalResourceObject("Constants", "DBFieldTypes").ToString().Split(',');
                        for (int i = 0; i < DBOperators.Length; i++)
                        {
                            string[] OperatorResource = DBOperators[i].Split('-');
                            string[] Operators = GetGlobalResourceObject("Constants", OperatorResource[1]).ToString().Split(',');
                            for (int k = 0; k < Operators.Length; k++)
                            {
                                operatorList.Add(new Operator() { FeildType = OperatorResource[0], FeildOperator = Operators[k] });
                            }
                        }
                        ViewState["OperatorList"] = operatorList;

                        break;

                    #endregion

                    #region GET TOP-BOTTOM GRID

                    case ControlsEnum.BINDGRID:

                        divError.Visible = false;

                        BindColumnFields();

                        if (ddlOrder.SelectedValue == "Top" && txtTopBottom.Text != string.Empty)
                        {
                            query = "SELECT TOP " + txtTopBottom.Text + " * FROM  " + table + " ORDER BY " + ((string[])ViewState["columnNames"])[0] + " ASC";
                        }
                        else if (ddlOrder.SelectedValue == "Bottom" && txtTopBottom.Text != string.Empty)
                        {
                            query = "SELECT * FROM ( SELECT TOP " + txtTopBottom.Text + " * FROM  " + table + " ORDER BY " + ((string[])ViewState["columnNames"])[0] + " DESC ) Temp ORDER BY " + ((string[])ViewState["columnNames"])[0] + " ASC";
                        }
                        else
                        {
                            query = "SELECT * FROM  " + table;
                        }

                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(query, con);
                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        dtTBFilterGrid = new DataTable();
                        sda1.Fill(dtTBFilterGrid);
                        ViewState["GridBind"] = dtTBFilterGrid;
                        cmd1.ExecuteReader();
                        con.Close();

                        break;

                    #endregion

                    #region GET FEILD FILTER GRID

                    case ControlsEnum.FEILDGRIDFILTER:

                        divError.Visible = false;

                        string fieldName = ddlFields.SelectedItem.Text;
                        string fldOperator = ddlOperator.SelectedValue;
                        string fldOperatorType = ((List<TableSchema>)ViewState["lstNameandType"])[Convert.ToInt32(ddlFields.SelectedValue)].FieldType;

                        BindColumnFields();

                        txtTopBottom.Text = string.Empty;
                        ddlOrder.SelectedIndex = -1;

                        if (txtQueryField.Text != string.Empty)
                        {
                            if (fldOperatorType == "String" || fldOperatorType == "DateTime")
                            {
                                query = "SELECT * FROM " + table + " WHERE " + fieldName + " " + fldOperator + "  '" + txtQueryField.Text.Trim() + "'";
                            }
                            else
                            {
                                query = "SELECT * FROM " + table + " WHERE " + fieldName + " " + fldOperator + " " + txtQueryField.Text.Trim();
                            }
                        }
                        else
                        {
                            query = "SELECT * FROM " + table;
                        }

                        con.Open();
                        SqlCommand cmd2 = new SqlCommand(query, con);
                        SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                        dtFieldFilterGrid = new DataTable();
                        sda2.Fill(dtFieldFilterGrid);
                        cmd2.ExecuteReader();
                        con.Close();

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

        #region SET FEILD VALUES

        private void SetFeildValues(ControlsEnum controlType)
        {
            try
            {
                string table = ddlTables.SelectedValue;
                string query = string.Empty;

                switch (controlType)
                {
                    #region TABLES DROPDOWN
                    case ControlsEnum.TABLES:

                        BindDropdown(ControlsEnum.TABLES);

                        break;
                    #endregion

                    #region BIND GRID TOP-BOTTOM FILTER
                    case ControlsEnum.BINDGRID:

                        BindGrid(ControlsEnum.BINDGRID);

                        break;
                    #endregion

                    #region BIND FILTER GRID
                    case ControlsEnum.FEILDGRIDFILTER:

                        BindGrid(ControlsEnum.FEILDGRIDFILTER);

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

        #region HELPER METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsValidUser(string password)
        {

            bool result = false;
            dtResult = CommonBL.GetApplicaitonConfiguaration("MAIL STATUS", "MTTB", currentUser.SBUID);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                result = dtResult.Rows[0]["ACF_DATA"].ToString().ToLower() == password.ToLower();
            }
            return result;
        }

        //private void BindDBDetails()
        //{
        //    try
        //    {
        //        System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
        //        builder.ConnectionString = con.ConnectionString.ToString();
        //        lblDatasource.Text = builder["Data Source"] as string;
        //        lblDatabase.Text = builder["Initial Catalog"] as string;
        //        lblUsername.Text = builder["User ID"] as string;
        //        //string password = builder["Password"] as string;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void BindColumnFields()   //Method to get table fields and their datatype
        {
            try
            {
                TableName = ddlTables.SelectedValue;
                QueryTxt = "SELECT * FROM  " + TableName;

                con.Open();
                SqlCommand cmd = new SqlCommand(QueryTxt, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteReader();
                con.Close();

                #region Getting Column names

                ColumnNames = (from dc in dt.Columns.Cast<DataColumn>()
                               select dc.ColumnName).ToArray();

                HeaderText = String.Join(",", ColumnNames); //to get comma separated array values

                #endregion

                #region To get Datatype of columns

                Type[] columnType = (from dc in dt.Columns.Cast<DataColumn>()
                                     select dc.DataType).ToArray();

                for (int i = 0; i < columnType.Length; i++)
                {
                    sb.Append(columnType[i].Name.ToString() + ",");
                }

                Values = sb.ToString().TrimEnd(',').Split(',');

                #endregion

                ViewState["columnNames"] = ColumnNames;
                ViewState["ColumnTypeName"] = Values;
                ViewState["ColumnHeaders"] = HeaderText;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindDropdown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region BIND TABLE DROPDOWN
                    case ControlsEnum.TABLES:

                        if (dtTable != null && dtTable.Rows.Count > 0)
                        {
                            ddlTables.DataTextField = "name";
                            ddlTables.DataSource = dtTable;
                            ddlTables.DataBind();
                            ddlTables.Items.Insert(0, "--Select Table--");
                        }

                        break;
                    #endregion

                    #region TABLE FIELDS
                    case ControlsEnum.FIELDS:
                        if (lstSchema.Count != 0)
                        {
                            ddlFields.DataSource = lstSchema;
                            ddlFields.DataValueField = "SlNo";
                            ddlFields.DataTextField = "FieldName";
                            ddlFields.DataBind();
                            ddlFields.Items.Insert(0, "--Select Field--");
                        }
                        break;
                    #endregion

                    #region TABLE OPERATORS
                    case ControlsEnum.TABLEOPERATORS:

                        string feildType = ((List<TableSchema>)ViewState["lstNameandType"])[Convert.ToInt32(ddlFields.SelectedValue)].FieldType;
                        ddlOperator.DataSource = operatorList.Where(o => o.FeildType == feildType).ToList();
                        ddlOperator.DataTextField = "FeildOperator";
                        ddlOperator.DataBind();
                        ddlOperator.Items.Insert(0, "-Select Operator-");

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region BIND FIELD FILTER GRID

                    case ControlsEnum.FEILDGRIDFILTER:
                        if (dtFieldFilterGrid != null && dtFieldFilterGrid.Rows.Count > 0)
                            grdManageTable.DataSource = dtFieldFilterGrid;
                        else
                            grdManageTable.DataSource = new DataTable();
                        grdManageTable.DataBind();
                        break;

                    #endregion

                    #region BIND TOP-BOTTOM FILTER GRID

                    case ControlsEnum.BINDGRID:

                        if (dtTBFilterGrid != null && dtTBFilterGrid.Rows.Count > 0)
                        {
                            grdManageTable.DataSource = dtTBFilterGrid;
                            grdManageTable.DataBind();
                        }
                        else
                        {
                            BindGrid(ControlsEnum.FIELDS);
                        }

                        break;

                    #endregion

                    #region EMPTY GRID
                    case ControlsEnum.FIELDS:

                        grdManageTable.DataSource = null;
                        grdManageTable.DataBind();

                        break;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetShortString(object evelOrginalString, int limit)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return (orginalString.Length <= limit) ? orginalString : (orginalString.Substring(0, limit) + "..");
        }

        private void ResetFields()
        {
            txtQueryField.Text = string.Empty;
            txtTopBottom.Text = string.Empty;
            ddlTables.SelectedIndex = -1;
            ddlOperator.SelectedIndex = -1;
            ddlOrder.SelectedIndex = -1;
            ddlFields.SelectedIndex = -1;
            grdManageTable.DataSource = null;
            grdManageTable.DataBind();
        }
        /// <summary>
        /// Is Super Admin User
        /// </summary>
        /// <param name="pkUser"></param>
        /// <returns></returns>
        private bool IsSuperAdminUser(int pkUser)
        {
            bool retVal = false;
            DataTable dtResult = UserManagementBL.SuperAdminMstGet(pkUser);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = dtResult.Rows[0]["usrIsSuperAdmin"].ToString() == "1" ? true : false;
            }
            return retVal;
        }
        #endregion

        #region CLASS & ENUM

        [Serializable]
        public class TableSchema
        {
            public int SlNo
            { get; set; }
            public string FieldName
            { get; set; }
            public string FieldType
            { get; set; }
        }

        [Serializable]
        public class Operator
        {
            public string FeildType
            { get; set; }
            public string FeildOperator
            { get; set; }
        }
        public enum ActionsEnum
        {
            GETFEILD,
            GETOPERATORS,
            INSERTQUERY,
            SUBMITQUERY,
            VIEWQUERY,
            DELETE,
            SUBMIT,
            SEARCH,
            CLEAR,
            INSERTROW
        }
        public enum ControlsEnum
        {
            TABLES,
            OPERATORS,
            GETTABLES,
            FIELDS,
            TABLEOPERATORS,
            BINDGRID,
            FEILDGRIDFILTER

        }
        #endregion

    }
}