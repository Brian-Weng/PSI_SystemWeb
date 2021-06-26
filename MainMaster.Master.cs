using PIS_System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIS_System
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!LoginHelper.HasLogined())
                Response.Redirect("~/LoginPage.aspx");

            var currentUser = LoginHelper.GetCurrentUserInfo();
            this.lblName.Text = $"Hi! {currentUser.Name}";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            LoginHelper.Logout();

            Response.Redirect("~/LoginPage.aspx");
        }
    }
}