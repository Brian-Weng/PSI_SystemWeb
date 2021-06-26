using PIS_System.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIS_System
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new ProductManager();
            var list = manager.GetProducts();
            this.repProduct.DataSource = list;
            this.repProduct.DataBind();
        }
    }
}