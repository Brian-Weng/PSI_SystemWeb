using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PIS_System.Helpers;
using PIS_System.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIS_System
{
    public partial class PO_List : System.Web.UI.Page
    {
        private const int _pageSize = 5;

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadRepeater();
        }

        private void LoadRepeater()
        {
            string page = Request.QueryString["Page"];
            int pIndex = 0;
            if (string.IsNullOrEmpty(page))
                pIndex = 1;
            else
            {
                int.TryParse(page, out pIndex);

                if (pIndex <= 0)
                    pIndex = 1;
            }

            int totalSize = 0;

            var manager = new PO_Manager();
            var list = manager.ReadPOs(out totalSize, pIndex, _pageSize);
            var pageHelper = new PagingHelper();
            int pages = pageHelper.CalculatePages(totalSize, _pageSize);
            var pagelist = pageHelper.RepPagingList(pages);

            this.repPage.DataSource = pagelist;
            this.repPage.DataBind();

            this.repList.DataSource = list;
            this.repList.DataBind();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PO_Detail.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            var manager = new PO_Manager();
            var dt = manager.GetPOTable();
            ReportDocument crp = new ReportDocument();
            crp.Load(Server.MapPath("~/PurchaseOrderCR.rpt"));
            crp.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = crp;

            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PurchaseOrders");
        }

        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string cmdName = e.CommandName;
            string arg = e.CommandArgument.ToString();
            var manager = new PO_Manager();

            if (cmdName == "DeleteItem")
            {
                var currentUser = LoginHelper.GetCurrentUserInfo();
                string userName = currentUser.Name;
                manager.DeletePO(arg, userName);
                this.LoadRepeater();
            }

            if (cmdName == "UpdateItem")
            {
                string targetUrl = "~/PO_Detail.aspx?PID=" + arg;
                Response.Redirect(targetUrl);
            }
        }
    }
}