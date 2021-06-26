using Newtonsoft.Json;
using PIS_System.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIS_System
{
    public partial class PO_Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var manager = new ProductManager();
                var list = manager.GetProducts();
                this.repProduct.DataSource = list;
                this.repProduct.DataBind();
            }
           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}