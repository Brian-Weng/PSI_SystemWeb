using PIS_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PIS_System.Managers
{
    public class PO_Manager : DBBase
    {
        public void CreatePO(PO_Model model, DataTable dt)
        {
            string dbCommandText =
                $@" INSERT INTO PurchaseOrders 
                        (PID, Items, QTY, ArrivalTime, Total, CreateDate, Creator) 
                    VALUES 
                        (@PID, @Items, @QTY, @ArrivalTime, @Total, @CreateDate, @Creator);

                    INSERT INTO PO_Details
                    SELECT PID, ID, QTY, Amount
                    FROM @tvpPO_Details;
                ";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@PID", model.PID),
                new SqlParameter("@Items", model.Items),
                new SqlParameter("@QTY", model.QTY),
                new SqlParameter("@ArrivalTime", model.ArrivalTime),
                new SqlParameter("@Total", model.Total),
                new SqlParameter("@CreateDate", model.CreateDate),
                new SqlParameter("@Creator", model.Creator),
            };

            SqlParameter dtParam = new SqlParameter("@tvpPO_Details", dt);
            dtParam.SqlDbType = SqlDbType.Structured;
            dtParam.TypeName = "dbo.PO_DetailsTableType";
            parameters.Add(dtParam);

            this.ExecuteNonQuery(dbCommandText, parameters);
        }

        public void UpdatePO(PO_Model model)
        {

        }

        public void DeletePO(PO_Model model)
        {

        }

        public DataTable GetPOTable()
        {
            string dbQuery =
                @"SELECT * FROM PurchaseOrders
                  WHERE Deleter IS NULL";

            List<SqlParameter> dbParameters = new List<SqlParameter>();

            var dt = this.GetDataTable(dbQuery, dbParameters);

            return dt;
        }

        public List<PO_Model> ReadPOs(out int totalSize, int currentPage = 1, int pageSize = 5)
        {
            string dbQuery =
                $@"SELECT TOP {2} * FROM 
                   (
                      SELECT
                        ROW_NUMBER() OVER(ORDER BY PID DESC) AS RowNumber,
                        PID,
                        Items,
                        QTY,
                        ArrivalTime,
                        Total,
                        CreateDate,
                        Creator,
                        ModifyDate,
                        Modifier,
                        DeleteDate,
                        Deleter
                      FROM PurchaseOrders
                      WHERE Deleter IS NULL
                   ) AS Temp
                   WHERE RowNumber > {pageSize * (currentPage - 1)}";

            string countQuery =
                @"SELECT COUNT(PID)
                  FROM PurchaseOrders
                  WHERE Deleter IS NULL";

            List<SqlParameter> dbParameters = new List<SqlParameter>();

            var dt = this.GetDataTable(dbQuery, dbParameters);

            List<PO_Model> list = new List<PO_Model>();

            foreach (DataRow dr in dt.Rows)
            {
                PO_Model model = new PO_Model();
                model.PID = (string)dr["PID"];
                model.Items = (int)dr["Items"];
                model.QTY = (int)dr["QTY"];
                model.ArrivalTime = (DateTime)dr["ArrivalTime"];
                model.Total = (decimal)dr["Total"];
                model.CreateDate = (DateTime)dr["CreateDate"];
                model.Creator = (string)dr["Creator"];
                model.ModifyDate = dr["ModifyDate"] as DateTime?;
                model.Modifier = dr["Modifier"] as string;

                list.Add(model);
            }

            int? totalSize2 = this.GetScale(countQuery, dbParameters) as int?;
            totalSize = (totalSize2.HasValue) ? totalSize2.Value : 0;

            return list;
        }

        public PO_Model ReadPO(string Pid)
        {
            string dbQuery =
                @"SELECT * FROM PurchaseOrders
                  WHERE PID = @PID AND Deleter IS NULL";

            List<SqlParameter> dbParameters = new List<SqlParameter>();
            dbParameters.Add(new SqlParameter("@PID", Pid));

            var dt = this.GetDataTable(dbQuery, dbParameters);

            PO_Model model = new PO_Model();

            model.PID = (string)dt.Rows[0]["PID"];
            model.Items = (int)dt.Rows[0]["Items"];
            model.QTY = (int)dt.Rows[0]["QTY"];
            model.ArrivalTime = (DateTime)dt.Rows[0]["ArrivalTime"];
            model.Total = (decimal)dt.Rows[0]["Total"];
            model.CreateDate = (DateTime)dt.Rows[0]["CreateDate"];
            model.Creator = (string)dt.Rows[0]["Creator"];
            model.ModifyDate = (DateTime)dt.Rows[0]["ModifyDate"];
            model.Modifier = (string)dt.Rows[0]["Modifier"];

            return model;
        }

        public string GetNewPID()
        {
            string dbQuery =
                @"SELECT CONCAT( 'ASN-', RIGHT('000' + RTRIM(MAX(CAST(RIGHT(PID, 4) AS INT)) + 1), 4)) AS PID
                  FROM PurchaseOrders";

            List<SqlParameter> dbParameters = new List<SqlParameter>();

            var dt = this.GetDataTable(dbQuery, dbParameters);
            string PID = dt.Rows[0].Field<string>("PID");
            return PID;
        }
    }
}