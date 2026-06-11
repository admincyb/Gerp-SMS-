using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;

namespace HRMS.UserControls
{
    public partial class FileUploaderNew : System.Web.UI.UserControl
    {

        public delegate void MultipleFileUploadClick(object sender, FileCollectionEventArgs e);
        public event MultipleFileUploadClick Click;

        public delegate void AfterFileDelete(object sender, FileEventArgs e);
        public event AfterFileDelete AfterDelete;

        //public delegate void AllFilesRemoved(object sender, EventArgs e);
        public event EventHandler AllFilesDeleted;

        public string CommandName { get; set; }

        public int ViewType
        {
            get { return int.Parse(this.ViewState["ViewType"].ToString()); }
            set { this.ViewState["ViewType"] = value; }
        }
        private ActionsEnum commonActions;
        public List<AttachmentBO> Upload
        {
            get;
            set;
        }
        /// <summary>
        /// Base Path + ...
        /// Eg. Upload/ + EDocs/Project1/
        /// </summary>
        public string SubFolderPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager current = ScriptManager.GetCurrent(Page);
            if (current != null)
            {
                current.RegisterPostBackControl(btnUpload);
            }

            if (!IsPostBack)
            {
                ViewAction();
            }
        }

        /// <summary>
        /// Method to View Actions Control in User Controls
        /// </summary>
        public void ViewAction()
        {
            if (ViewType == 1)
            {
                btnUpload.Visible = true;
                FileUploader.Visible = true;
            }
            else if (ViewType == 0)
            {
                FileUploader.Visible = false;
                btnUpload.Visible = false;
            }
        }


        #region Action Handlers

        #region -- For Buttons ---

        /// <summary>
        /// For Button And ImageButton Click (Save/Delete And Edit/Delete GO (Grid))
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvrCurrentRow;
            string fileID = string.Empty;
            if (sender is Button)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender is ImageButton)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonActions = ActionsEnum.CHANGE;
            }
            switch (commonActions)
            {
                case ActionsEnum.ADD:
                    Click(this, new FileCollectionEventArgs(this.Request));
                    break;

                case ActionsEnum.VIEW:

                    gvrCurrentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
                    fileID = (gvrCurrentRow.FindControl("hdfActName") as HiddenField).Value;
                    DownloadFile(fileID);
                    break;

                case ActionsEnum.DELETE_ACTION:

                    gvrCurrentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
                    fileID = (gvrCurrentRow.FindControl("hdfActName") as HiddenField).Value;
                    DeleteFile(fileID);
                    if (AfterDelete != null)
                    {
                        FileEventArgs evArgs = new FileEventArgs();
                        evArgs.FileID = fileID;
                        evArgs.FileName = (gvrCurrentRow.FindControl("lblFilePath") as Label).Text;
                        AfterDelete(sender, evArgs);
                    }
                    if (this.Upload.Count == 0)
                        AllFilesDeleted(this, EventArgs.Empty);
                    break;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// For RowDataBound Events - For Hide / Show Active & Incativate Buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            DataControlRowType DataRowType = DataControlRowType.DataRow;

            if (e.Row.RowType == DataRowType)
            {
                GridViewRow CurrentRow;
                CurrentRow = (GridViewRow)(e.Row);
                //ImageButton imbView = (ImageButton)CurrentRow.FindControl("imbView");
                ImageButton imbDelete = (ImageButton)CurrentRow.FindControl("imbDelete");
                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                {
                    //current.RegisterPostBackControl(imbView);
                }
                if (ViewType == 0)
                {
                    imbDelete.Visible = false;
                }
            }
        }

        /// <summary>
        /// Binds the file link string with Grid's link button
        /// </summary>
        /// <param name="myEvalObj"></param>
        /// <returns></returns>
        public string BindFileLink(object myEvalObj)
        {
            try
            {
                string link = myEvalObj.ToString();
                string FilePath = "/Upload/";
                if (!string.IsNullOrWhiteSpace(this.SubFolderPath))
                {
                    FilePath += SubFolderPath;
                }
                FileInfo file = new FileInfo(link);
                string virtulaDir = string.Empty;
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"] != string.Empty)
                {
                    virtulaDir = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"];
                    link = virtulaDir + FilePath + file.Name;
                }
                else
                {
                    link = FilePath + file.Name;
                }
                return link;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Binds the file link string with Grid's link button-RawMaterialInspection
        /// </summary>
        /// <param name="myEvalObj"></param>
        /// <returns></returns>
        public string BindFileLinkNew(object myEvalObj)
        {
            try
            {
                string link = myEvalObj.ToString();
                //string FilePath = "../Upload/"; // Commented by Biju
                string FilePath = "~/Upload/";
                if (!string.IsNullOrWhiteSpace(this.SubFolderPath))
                {
                    FilePath += this.SubFolderPath ;
                }
                FileInfo file = new FileInfo(link);
                string virtulaDir = string.Empty;

                link = FilePath + file.Name;
                return link;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Methode Used to Upload file
        /// </summary>
        /// <param name="e"></param>
        public void UploadFile(FileCollectionEventArgs e)
        {
            HttpFileCollection oHttpFileCollection = e.PostedFiles;
            HttpPostedFile oHttpPostedFile = null;
            string uploadPath = Request.MapPath("~/") + "Upload";
            if (!string.IsNullOrWhiteSpace(this.SubFolderPath))
            {
                uploadPath += "/" + this.SubFolderPath;
            }
            string filePath = string.Empty;
            AttachmentBO fileUpload;
            Upload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentBO>>(FileUploadList.Value);
            if (Upload == null)
            {
                Upload = new List<AttachmentBO>();
            }

            string fileKey = oHttpFileCollection.AllKeys.SingleOrDefault(x => x.EndsWith(string.Format("${0}$flpUploader", this.ID)));

            if (e.HasFiles && oHttpFileCollection.Count > 0 && oHttpFileCollection[fileKey].FileName != string.Empty)
            {
                oHttpPostedFile = oHttpFileCollection[fileKey];
                string ext = "." + oHttpPostedFile.FileName.Split('.')[oHttpPostedFile.FileName.Split('.').Length - 1].ToString();
                string fileStatus = CheckValidFileType(ext);
                string actName = Guid.NewGuid().ToString();
                string fileName = oHttpPostedFile.FileName;
                string fileTitle = HttpUtility.HtmlEncode(txtFlpFileTite.Text.Trim());
                string filedescription = HttpUtility.HtmlEncode(txtFlpFileDescription.Text.Trim());
                txtFlpFileTite.Text = string.Empty;
                txtFlpFileDescription.Text = string.Empty;
                if (fileStatus != "Invalid")
                {
                    if (CheckFolderExists(uploadPath))
                    {
                        filePath = uploadPath + "\\" + actName + ext;
                        oHttpPostedFile.SaveAs(filePath);
                        fileUpload = new AttachmentBO();
                        fileUpload.PK = 0;
                        fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1, (fileName.Length - (fileName.LastIndexOf('\\') + 1)));
                        fileUpload.FileName = fileName;
                        fileUpload.FilePath = filePath;
                        fileUpload.Title = fileTitle;
                        fileUpload.FileDescription = filedescription;
                        Upload.Add(fileUpload);
                        grdUpload.DataSource = Upload;
                        grdUpload.DataBind();
                        FileUploadList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Upload);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.Messages.FileUpload_Success) + "','" + Resources.Captions.Information + "');", true);//ganesh
                    }
                    else
                    {
                        string uploadAndSubFolderPath = string.IsNullOrWhiteSpace(this.SubFolderPath)
                            ? string.Empty
                            : "Upload\\" + this.SubFolderPath.Replace("/", "\\");
                        if (CreateFolder(Request.MapPath("~/"), uploadAndSubFolderPath))
                        {
                            filePath = uploadPath + "//" + actName + ext;
                            oHttpPostedFile.SaveAs(filePath);
                            fileUpload = new AttachmentBO();
                            fileUpload.PK = 0;
                            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1, (fileName.Length - (fileName.LastIndexOf('\\') + 1)));
                            fileUpload.FileName = fileName;
                            fileUpload.FilePath = filePath;
                            fileUpload.Title = fileTitle;
                            fileUpload.FileDescription = filedescription;
                            Upload.Add(fileUpload);
                            grdUpload.DataSource = Upload;
                            grdUpload.DataBind();
                            FileUploadList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Upload);
                            //FileUpload_Success
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.Messages.FileUpload_Success) + "','" + Resources.Captions.Information + "');", true);//ganesh
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_NoPermission_Create_Folder) + "','" + Resources.Captions.Information + "');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_Invalid_File) + "','" + Resources.Captions.Information + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_PleaseSelectFile) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="context"></param>
        private void DownloadFile(string fileID)
        {
            FileInfo file = new FileInfo(fileID);
            if (file.Exists)
            {
                HttpContext.Current.Response.ContentType = "image/pjpeg";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                HttpContext.Current.Response.WriteFile(file.FullName);
                HttpContext.Current.Response.End();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide", "if(typeof ShowListing == 'function') { ShowListing(false); }", true);
            }
        }

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="context"></param>
        private void DeleteFile(string fileID)
        {
            Upload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentBO>>(FileUploadList.Value);
            foreach (var item in Upload)
            {
                if (item.FilePath == fileID)
                {
                    Upload.Remove(item);
                    FileInfo file = new FileInfo(fileID);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    break;
                }
            }
            grdUpload.DataSource = Upload;
            grdUpload.DataBind();
            FileUploadList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Upload);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide", "if(typeof ShowListing == 'function') { ShowListing(false); }", true);
        }

        /// <summary>
        /// Check Folder Exists or Not
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <returns>Bool</returns>
        private bool CheckFolderExists(string uploadUrl)
        {
            if (System.IO.Directory.Exists(uploadUrl))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check whether the file type is valid
        /// </summary>
        /// <param name="extn"></param>
        /// <returns></returns>
        private string CheckValidFileType(string extn)
        {
            string retval = "";
            switch (extn.ToLower())
            {
                case ".jpg": break;
                case ".jpeg": break;
                case ".gif": break;
                case ".png": break;
                case ".tiff": break;
                case ".doc": break;
                case ".docx": break;
                case ".xls": break;
                case ".xlsx": break;
                case ".ppt": break;
                case ".pdf": break;
                case ".txt": break;
                default: retval = "Invalid"; break;
            }
            return retval;
        }

        /// <summary>
        /// Create Folder, If Folder Not Exists
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="uploadFolder"></param>
        /// <returns>Bool</returns>
        private bool CreateFolder(string rootPath, string uploadFolder)
        {
            try
            {
                System.IO.Directory.CreateDirectory(rootPath + uploadFolder);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the attached files
        /// </summary>
        /// <param name="extn"></param>
        /// <returns></returns>
        public List<AttachmentBO> GetAttachedFiles()
        {
            Upload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentBO>>(FileUploadList.Value);

            #region Setting Title From Grid
            List<string> titleList = new List<string>();
            List<string> descriptionList = new List<string>();
            foreach (GridViewRow row in grdUpload.Rows)
            {
                TextBox txt = (TextBox)row.FindControl("txtFileTitle");
                if (txt != null)
                    titleList.Add(txt.Text.Trim());
                Label lbl = (Label)row.FindControl("lblFileDescription");
                if (lbl != null)
                    descriptionList.Add(lbl.Text.Trim());
            }

            if (Upload != null && Upload.Count == titleList.Count && Upload.Count == descriptionList.Count)
                for (int indx = 0; indx < titleList.Count; indx++)
                {
                    Upload[indx].Title = titleList[indx];
                    Upload[indx].FileDescription = descriptionList[indx];
                }
            #endregion

            ResetValues();
            return Upload;
        }

        /// <summary>
        /// Set the attached files
        /// </summary>
        /// <param name="extn"></param>
        /// <returns></returns>
        public void SetAttachedFiles(List<AttachmentBO> lstAttachment)
        {
            if (FileUploadList.Value != string.Empty)
            {
                lstAttachment = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentBO>>(FileUploadList.Value);
            }
            grdUpload.DataSource = lstAttachment;
            grdUpload.DataBind();
            FileUploadList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(lstAttachment);
        }

        /// <summary>
        /// Reset Fields
        /// </summary>
        public void ResetValues()
        {
            FileUploadList.Value = string.Empty;
            grdUpload.DataSource = null;
            grdUpload.DataBind();
        }
    }

    [Serializable]
    public class FileEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public string FileID { get; set; }
    }

    [Serializable]
    public class FileCollectionEventArgs : EventArgs
    {
        private HttpRequest _HttpRequest;
        public HttpFileCollection PostedFiles
        {
            get
            {
                return _HttpRequest.Files;
            }
        }
        public int Count
        {
            get { return _HttpRequest.Files.Count; }
        }
        public bool HasFiles
        {
            get { return _HttpRequest.Files.Count > 0 ? true : false; }
        }
        public FileCollectionEventArgs(HttpRequest oHttpRequest)
        {
            _HttpRequest = oHttpRequest;
        }
    }

}