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
        private bool _isInsertMode = false;
        private string _queryStr = string.Empty;
        private PO_Manager _po_Manager = new PO_Manager();
        protected void Page_Init(object sender, EventArgs e)
        {
            IsInsertMode();

            var manager = new ProductManager();
            var list = manager.GetProducts();
            this.repProduct.DataSource = list;
            this.repProduct.DataBind();

            if (_isInsertMode)
            {
                this.h2Title.InnerText = "新增進貨單";
            }
            else
            {
                this.h2Title.InnerText = "修改進貨單";
                this.LoadPOAndViewDetail(_queryStr);
            }
           
        }

        private void IsInsertMode()
        {
            string queryStr = Request.QueryString["PID"] as string;
            if (string.IsNullOrEmpty(queryStr))
            {
                _isInsertMode = true;
                return;
            }
            else
            {
                _isInsertMode = false;
                _queryStr = queryStr;
            }

        }

        private void LoadPOAndViewDetail(string PID)
        {
            //讀取資料並放入資料model
            var POModel = _po_Manager.ReadPO(PID);

            //如果讀取不到資料，回到發票總覽頁面
            if (POModel == null)
                Response.Redirect("~/PO_List.aspx");

            //讀取到的資料放入畫面中各個控制項中
            this.txtPID.Text = POModel.PID;
            this.txtDate.Text = POModel.ArrivalTime.ToString("yyyy/MM/dd HH:mm");
            this.lblCreator.Text = "建立者" + POModel.Creator;
            string createTime = "建立時間" + POModel.CreateDate.ToString("yyyy/MM/dd HH:mm:ss");
            this.lblCreateDate.Text = createTime;
            this.lblTotal.Text = POModel.Total.ToString("#,0");

            this.divCreate.Attributes.Remove("style");

            var viewDetailsList = _po_Manager.GetViewPODetails(PID);
            this.repViewPODetail.DataSource = viewDetailsList;
            this.repViewPODetail.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}