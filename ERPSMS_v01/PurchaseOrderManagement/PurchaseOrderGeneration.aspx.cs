using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderGeneration : System.Web.UI.Page
    {
        #region Events

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
            }

        }
        #endregion

        #region Methords
        
        /// <summary>
        ///Methord used to Fill initial data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FillInitialData()
        {
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            POH_CRTD_BY.Value = objUser.PKUser.ToString();
            CreatedBy.Text = objUser.UserName;
            BizUnitPk.Value = objUser.SBUID.ToString();
            UserID.Value = objUser.PKUser.ToString();
            POH_CRTD_BY.Value = objUser.EmpName;
            CreatedBy.Text = objUser.EmpName;
            //DeptPk.Value = objUser.ActiveDepID.ToString();
            
            if (Request.QueryString["POID"] != null)
                FillPODetails(Convert.ToInt32(Request.QueryString["POID"].ToString()));
            else
                FillPODetails(0);

            if (Request.QueryString["RefID"] != null)
                FillWorkFlowDetails(Request.QueryString["RefID"].ToString());
            else
                FillWorkFlowDetails("0");
           // FillPRDetails();
            
        }
        
        /// <summary>
        ///Methord used to Fill Po Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillPoNumber(BusinessObject.User objUser)
        {
            DataTable dtponoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, objUser.ActiveDepID, "PO Number", 0);
            string purOrdNo;
            if (dtponoFormat.Rows.Count > 0)
                purOrdNo = dtponoFormat.Rows[0]["DFT_VALUE"].ToString();
            else
                purOrdNo = GTIService.Constants.Common.CommonConstant.PONUMBERFORMAT;

            MatchCollection matchcol = Regex.Matches(purOrdNo, @"\#[\w]+\#");
            for(int i =0;i<matchcol.Count;i++)
            {
                if (i == 0)
                {
                    purOrdNo = purOrdNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#",string.Empty)));
                }
                else if (i == 1)
                {
                    purOrdNo = purOrdNo.Replace(matchcol[i].ToString(),BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPONumber());
                }

            }
            POH_NO.Value = purOrdNo;
            lblPOH_NO.Text = purOrdNo;

        }

        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillPODetails(int pOID)
        {
            if (pOID != 0)
            {

                MaterialDetails.Value = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPODetails(pOID);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
               
            }
            else
            {
                BusinessObject.PurchaseOrderGeneration.PurchaseOrder poObject = new BusinessObject.PurchaseOrderGeneration.PurchaseOrder();
                poObject.MaterialDetails = new List<BusinessObject.PurchaseOrderGeneration.PurchaseOrderMaterials> { };
                MaterialDetails.Value = Newtonsoft.Json.JsonConvert.SerializeObject(poObject);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                FillPoNumber(((BusinessObject.User)(HttpContext.Current.User.Identity)));
               

            }
        }
        
        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillWorkFlowDetails(string refID)
        {
            int processID;
            WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
            //*************************************RefID is used to get the details of application and its state in Workflow**********************************
            if (refID != "0")
            {

                //***************************Assigning Ref ID To Hidden Field*********************************************************************************
                ReferenceID.Value = refID;
                if (ReferenceID.Value == "0")//**************Means its is a fresh application Need to do the workflow from base*******************************
                {
                    processID = Convert.ToInt32(ProcessID.Value);
                    //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                    //Assign the Task And Action According to Process Intial Task Given By Work Flow
                    DataTable dtTask = obj.GetInitialTaskAction(processID); ;
                    if (dtTask.Rows.Count > 0)
                    {
                        TaskID.Value = dtTask.Rows[0]["Kmsp"].ToString();
                        TaskName.Value = dtTask.Rows[0]["KmsNme"].ToString();
                        ApplicationID.Value = Request.QueryString["POID"] != null ? Request.QueryString["POID"].ToString() : "0";
                        FillActions(dtTask);
                    }

                }
                else//*******************************************************Means its in Workflow and application once Saved*********************************
                {


                    //*********************************************************Request Workflow to Get the Application Satus By Providing the RefID************
                    DataTable dtAppStatus = obj.GetAppStatus(Convert.ToInt32(refID), ((BusinessObject.User)HttpContext.Current.User.Identity).PKUser);
                    if (dtAppStatus.Rows.Count > 0)
                    {
                        DataRow drow = dtAppStatus.Rows[0];
                        TaskID.Value = drow["KdsSkt"].ToString();
                        TaskName.Value = drow["KmsNme"].ToString();
                        FillActions(dtAppStatus);
                        ApplicationID.Value = drow["FmrPpaDi"].ToString();
                        ReferenceID.Value = Request.QueryString["RefID"].ToString();

                    }
                    //used to fill the completed task
                    else
                    {
                        DataTable dtAppID = obj.GetApplicationID(Convert.ToInt32(refID));
                        if (dtAppID.Rows.Count > 0)
                        {
                            DataRow drow = dtAppID.Rows[0];
                            ApplicationID.Value = drow["FmrPpaDi"].ToString();                            
                        }
                    }

                    FillPODetails(Convert.ToInt32(ApplicationID.Value));



                }
            }
            //*******************************Automatically Assigned the referenec No as 0******************************************************
            else
            {
                processID = Convert.ToInt32(ProcessID.Value);
                //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                //Assign the Task And Action According to Process Intial Task Given By Work Flow
                DataTable dtTask = obj.GetInitialTaskAction(processID); ;
                if (dtTask.Rows.Count > 0)
                {
                    TaskID.Value = dtTask.Rows[0]["Kmsp"].ToString();
                    TaskName.Value = dtTask.Rows[0]["KmsNme"].ToString();
                    ApplicationID.Value = Request.QueryString["POID"] != null ? Request.QueryString["POID"].ToString() : "0";
                    FillActions(dtTask);
                }
            }
        }

        //Summary
        //CreatedBy Vineeth Babu
        //CreatedOn 01-April-2011
        //Method Used to fill the Action Drop Down Using the Datatable Provided
        private void FillActions(DataTable dtTask)
        {
            WRKFACT_ID.DataTextField = "NdtNme";
            WRKFACT_ID.DataValueField = "Ndtp";
            WRKFACT_ID.DataSource = dtTask;
            WRKFACT_ID.DataBind();
        }

        private void FillPRDetails()
        {
            PRDetails.Value =  BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.CreatePRData();
        }

        #endregion


    }
}