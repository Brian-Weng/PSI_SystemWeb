using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIS_System.Helpers;

namespace PIS_System
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if(LoginHelper.HasLogined())
                Response.Redirect("~/PO_List.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text;
            string pwd = this.txtPassword.Text.Trim();

            if (LoginHelper.TryLogin(account, pwd))
            {
                Response.Redirect("~/PO_List.aspx");
            }
        }
    }
}