using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ERPSMS_v01.MachineryManagement
{
    public partial class MachineryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
            }

        }

        private void FillInitialData()
        {

            BusinessObject.MachineryManagement.Machinery ObjMachine = new BusinessObject.MachineryManagement.Machinery();
            ObjMachine.MaintenanceList = new List<BusinessObject.MachineryManagement.Machinery.MaintenaceInfo> { };

                //Assigning initialized Orderobject to hidden field (OrderObject)
            MaintenaceList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ObjMachine);
           
        }
        /// <summary>
        /// Upload Files 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpFileCollection hfc = Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                   FileInfo fInfo = new FileInfo(hpf.FileName);
                   if (!CheckFileExist(fInfo.Name))
                   {
                       hpf.SaveAs(HttpContext.Current.Server.MapPath("..\\Images\\Uploads\\Machinery") + "\\" + System.IO.Path.GetFileName(hpf.FileName));
                       FileList.Value += fInfo.Name+ ",";
                   }
                   else
                   {
                       
                       string[] fileDtls=fInfo.Name.Split('.');
                       string fname = fileDtls[0] + DateTime.Now.ToString("_dd_MM_yyyy_hh_mm_ss.") + fileDtls[1];
                       hpf.SaveAs(HttpContext.Current.Server.MapPath("..\\Images\\Uploads\\Machinery") + "\\" + fname);
                       FileList.Value += fname + ",";

                   }
                    
                }
            }
        }

        /// <summary>
        /// Check File Alreary Exists Or not
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool CheckFileExist(string fileName)
        {
            string filePath = string.Empty;

            filePath = HttpContext.Current.Server.MapPath("..\\Images\\Uploads\\Machinery") + "\\"+fileName;

            try
            {
                if (File.Exists(filePath))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                return true;
            }


        }
    }
}