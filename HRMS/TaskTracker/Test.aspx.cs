using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.TaskTracker
{
    public partial class Test : System.Web.UI.Page
    {
        #region Event
        //public event EventHandler CreateTask;
        #endregion

        private ControlsEnum commonActions;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
         #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ControlsEnum)(Enum.Parse(typeof(ControlsEnum), ((LinkButton)sender).CommandName));
                }
            switch (commonActions)
            {
                case ControlsEnum.SHOWPOPUP:
                    ucPopUpTask.IsSubtask = 1;
                    ucPopUpTask.TaskPk = 5;  
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','916','400');", true);
                    break;
                case ControlsEnum.SHOWPOPUP2:
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUp2]','Update Status','916','350');", true);
                    break;
            }
        }
        #endregion

        #region ControlsEnum
        private enum ControlsEnum
        {
            SHOWPOPUP,
            SHOWPOPUP2
        }
        #endregion
    }
}