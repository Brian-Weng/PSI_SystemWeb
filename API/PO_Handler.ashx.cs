using Newtonsoft.Json;
using PIS_System.Helpers;
using PIS_System.Managers;
using PIS_System.Models;
using System.Web.SessionState;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PIS_System.API
{
    /// <summary>
    /// PO_Handler 的摘要描述
    /// </summary>
    public class PO_Handler : IHttpHandler, IRequiresSessionState
    {
        
        public void ProcessRequest(HttpContext context)
        {
            var manager = new PO_Manager();
            var model = new PO_Model();

            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string date = context.Request.Form["ArrivalTime"];
            string pid = context.Request.Form["PID"];
            string tableJSON = context.Request.Form["PO_Detail"];
            var currentUser = LoginHelper.GetCurrentUserInfo();
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(tableJSON);
            DataColumn newColumn = new DataColumn("PID", typeof(string));

            if (pid == "create")
            {
                string newPID = manager.GetNewPID();
                newColumn.DefaultValue = newPID;

                dt.Columns.Add(newColumn);
                dt.Columns["PID"].SetOrdinal(0);

                var query =
                    (from dr in dt.AsEnumerable()
                     group dr by dr.Field<string>("PID") into row
                     select new
                     {
                         Items = dt.AsEnumerable().Count(),
                         QTY = row.Sum(x => int.Parse(x.Field<string>("QTY"))),
                         Total = row.Sum(x => decimal.Parse(x.Field<string>("Amount")))
                     }).ToList();

                model.PID = newPID;
                model.Items = (int)query[0].Items;
                model.QTY = (int)query[0].QTY;
                model.ArrivalTime = DateTime.Parse(date);
                model.Total = (decimal)query[0].Total;
                model.CreateDate = DateTime.Now;
                model.Creator = (string)currentUser.Name;

                manager.CreatePO(model, dt);
            }
            else
            {
                newColumn.DefaultValue = pid;

                dt.Columns.Add(newColumn);
                dt.Columns["PID"].SetOrdinal(0);

                var query =
                    (from dr in dt.AsEnumerable()
                     group dr by dr.Field<string>("PID") into row
                     select new
                     {
                         Items = dt.AsEnumerable().Count(),
                         QTY = row.Sum(x => int.Parse(x.Field<string>("QTY"))),
                         Total = row.Sum(x => decimal.Parse(x.Field<string>("Amount")))
                     }).ToList();
                model = manager.ReadPO(pid);

                model.Items = (int)query[0].Items;
                model.QTY = (int)query[0].QTY;
                model.ArrivalTime = DateTime.ParseExact(date, "yyyy/MM/dd HH:mm", null);
                model.Total = (decimal)query[0].Total;
                model.ModifyDate = DateTime.Now;
                model.Modifier = (string)currentUser.Name;

                manager.UpdatePO(model, dt);
            }

            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}