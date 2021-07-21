using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PIS_System.Helpers;
using PIS_System.Managers;
using System;
using System.Web.UI.WebControls;

namespace PIS_System
{
    public partial class PO_List : System.Web.UI.Page
    {
        //宣告並設定常數pageSize
        private const int _pageSize = 5;

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadRepeater();
        }

        private void LoadRepeater()
        {
            //抓取當前頁數，假設是空值就設定當前頁數為1
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

            //宣告總資料筆數變數
            int totalSize = 0;

            //從資料庫撈取進貨單表並做資料繫結
            var manager = new PO_Manager();
            var list = manager.ReadPOs(out totalSize, pIndex, _pageSize);
            this.repList.DataSource = list;
            this.repList.DataBind();

            //計算總頁數並生成分頁功能
            var pageHelper = new PagingHelper();
            int pages = pageHelper.CalculatePages(totalSize, _pageSize);
            var pagelist = pageHelper.RepPagingList(pages);
            this.repPage.DataSource = pagelist;
            this.repPage.DataBind();
        }

        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //宣告變數來接Repeater內按鈕控制項傳來的參數
            string cmdName = e.CommandName;
            string arg = e.CommandArgument.ToString();
            var manager = new PO_Manager();

            //依照傳來的參數值，假設是刪除即執行刪除；假設是修改就將變數arg當作QueryString去呼叫進貨單明細Page
            if (cmdName == "DeleteItem")
            {
                var currentUser = LoginHelper.GetCurrentUserInfo();
                string userName = currentUser.Name;
                manager.DeletePO(arg, userName);
                this.lblDelete.Text = $"進貨單{arg}刪除成功";
                this.lblDelete.Visible = true;
                this.LoadRepeater();
            }

            if (cmdName == "UpdateItem")
            {
                string targetUrl = "~/PO_Detail.aspx?PID=" + arg;
                Response.Redirect(targetUrl);
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //跳轉至進貨單明細Page
            Response.Redirect("~/PO_Detail.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //生成水晶報表
            var manager = new PO_Manager();
            var dt = manager.GetPOTable();
            ReportDocument crp = new ReportDocument();
            crp.Load(Server.MapPath("~/PurchaseOrderCR.rpt"));
            crp.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = crp;
            //各參數說明(文件類型,HttpResponse,是否生成附件,附件的名稱)
            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PurchaseOrders");
        }

    }
}