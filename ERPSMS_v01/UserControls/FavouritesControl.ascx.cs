using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GTIService.Constants.Common;
using System.Data;
using BusinessLogic.CommonManagement;
using BusinessObject.CommonManagement;
using System.Xml;
using GTIService;

namespace ERPSMS_v01.UserControls
{
    public partial class FavouritesControl : System.Web.UI.UserControl
    {
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private int pageId;
        private int sbu;
        private int userPK;
        BusinessObject.User currentUser = new BusinessObject.User();

        #region Set Page Variables

        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                ListOfFavourites();
                GetFavouriteExist();
            }
        }

        /// <summary>
        /// Action handeler fro button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            switch (commonActions)
            {
                //Add mode to enter details to save
                case ActionsEnum.ADD:
                    AddToFavouritesList();
                    break;
            }
        }

        /// <summary>
        /// Metod for changing url for image favourite(ADD/REMOVE)
        /// </summary>
        /// <param name="type"></param>
        private void ChangeURL(ActionsEnum type)
        {
            switch (type)
            {
                //Add mode to change the image url
                case ActionsEnum.ADDIMAGEURL:
                    //setting url and tootip add image
                    imbFavourites.ImageUrl = CommonConstants.ADDIMAGEURL;
                    imbFavourites.ToolTip = CommonConstants.ADDIMAGETOOLTIP;
                    break;
                case ActionsEnum.REMOVEIMAGEURL:
                    //setting url and tootip remove image
                    imbFavourites.ImageUrl = CommonConstants.REMOVEIMAGEURL;
                    imbFavourites.ToolTip = CommonConstants.REMOVEIMAGETOOLTIP;
                    break;
            }
        }

        /// <summary>
        /// listing all favourites
        /// </summary>
        private void ListOfFavourites()
        {
            DataList dlFavourites;
            DataTable dtList;
            dlFavourites = (DataList)Page.Master.FindControl("DtlstFavourates");
            HiddenField hdfPath = (HiddenField)Page.Master.FindControl("AbsolutePath");
            pageId = 0;
            userPK = currentUser.PKUser;
            sbu = Convert.ToInt32(currentUser.SBUID);
            List<MenuType> lstMenu = new List<MenuType>() { new MenuType() { PK = 2 } };
            XmlDocument xDoc = CommonFunctions.ObjectTOXml(lstMenu);
            dtList = CommonBL.GetFavouritesList(userPK, pageId, sbu, xDoc.InnerXml, hdfPath.Value);
            if (dtList != null && dtList.Rows.Count > 0)
            {
                dlFavourites.DataSource = dtList;
            }
            else
            {
                dlFavourites.DataSource = null;
            }
            dlFavourites.DataBind();
        }

        /// <summary>
        /// checking page already added or not.
        /// </summary>
        private void GetFavouriteExist()
        {
            HiddenField hdnPageID;
            DataTable dtResult;
            hdnPageID = (HiddenField)Page.Master.FindControl("hdnPageID");
            HiddenField hdfPath = (HiddenField)Page.Master.FindControl("AbsolutePath");
            pageId = Convert.ToInt32(hdnPageID.Value);
            userPK = currentUser.PKUser;
            sbu = Convert.ToInt32(currentUser.SBUID);
            List<MenuType> lstMenu = new List<MenuType>() { new MenuType() { PK = 2 } };
            XmlDocument xDoc = CommonFunctions.ObjectTOXml(lstMenu);
            dtResult = CommonBL.GetFavouritesList(userPK, pageId, sbu, xDoc.InnerXml, hdfPath.Value);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                ChangeURL(ActionsEnum.REMOVEIMAGEURL);
            }
            else
            {
                ChangeURL(ActionsEnum.ADDIMAGEURL);
            }
        }

        /// <summary>
        /// Adding page to favourites and changing url image .
        /// </summary>
        private void AddToFavouritesList()
        {
            HiddenField hdnPageID;
            DataTable dtResult;
            hdnPageID = (HiddenField)Page.Master.FindControl("hdnPageID");
            pageId = Convert.ToInt32(hdnPageID.Value);
            userPK = currentUser.PKUser;
            sbu = Convert.ToInt32(currentUser.SBUID);
            if (pageId != 0)
            {
                dtResult = CommonBL.AddToFavouritesList(userPK, pageId, sbu);
                if (Convert.ToInt32(dtResult.Rows[0][0]) > 0)
                {
                    ChangeURL(ActionsEnum.REMOVEIMAGEURL);
                }
                else
                {
                    ChangeURL(ActionsEnum.ADDIMAGEURL);
                }
                ListOfFavourites();
                Response.Redirect("~" + dtResult.Rows[0][1].ToString(), true);
            }
        }

        /// <summary>
        /// Enum properties.
        /// </summary>
        public enum ActionsEnum
        {
            ADD,
            GETSETURL,
            ADDIMAGEURL,
            REMOVEIMAGEURL,
        }
    }
}