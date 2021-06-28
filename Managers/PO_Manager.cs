using PIS_System.Models;
using PIS_System.VIewModels;
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

        public void UpdatePO(PO_Model model, DataTable dt)
        {
            string dbCommandText =
                $@" UPDATE PurchaseOrders
                    SET Items = @Items, 
                        QTY = @QTY, 
                        ArrivalTime = @ArrivalTime, 
                        Total = @Total, 
                        ModifyDate = @ModifyDate,
                        Modifier = @Modifier
                    WHERE PID = @PID;
                    
                    WITH PO_DetailsT AS (SELECT * FROM PO_Details WHERE PID = @PID)
                    MERGE INTO PO_DetailsT [bt]
                    USING @tvpPO_Details [ut]
                    ON [bt].PID = [ut].PID AND [bt].ID = [ut].ID
                        WHEN MATCHED
                        THEN UPDATE SET QTY = [ut].QTY, Amount = [ut].Amount
                        WHEN NOT MATCHED
                        THEN INSERT VALUES ([ut].PID, [ut].ID, [ut].QTY, [ut].Amount)
                        WHEN NOT MATCHED BY SOURCE
                        THEN DELETE;
                ";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@PID", model.PID),
                new SqlParameter("@Items", model.Items),
                new SqlParameter("@QTY", model.QTY),
                new SqlParameter("@ArrivalTime", model.ArrivalTime),
                new SqlParameter("@Total", model.Total),
                new SqlParameter("@ModifyDate", model.ModifyDate),
                new SqlParameter("@Modifier", model.Modifier),
            };

            SqlParameter dtParam = new SqlParameter("@tvpPO_Details", dt);
            dtParam.SqlDbType = SqlDbType.Structured;
            dtParam.TypeName = "dbo.PO_DetailsTableType";
            parameters.Add(dtParam);

            this.ExecuteNonQuery(dbCommandText, parameters);
        }

        public void DeletePO(string pid)
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
                $@"SELECT TOP {5} * FROM 
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

        public PO_Model ReadPO(string pid)
        {
            string dbQuery =
                @"SELECT * FROM PurchaseOrders
                  WHERE PID = @PID AND Deleter IS NULL";

            List<SqlParameter> dbParameters = new List<SqlParameter>();
            dbParameters.Add(new SqlParameter("@PID", pid));

            var dt = this.GetDataTable(dbQuery, dbParameters);

            PO_Model model = new PO_Model();

            model.PID = (string)dt.Rows[0]["PID"];
            model.Items = (int)dt.Rows[0]["Items"];
            model.QTY = (int)dt.Rows[0]["QTY"];
            model.ArrivalTime = (DateTime)dt.Rows[0]["ArrivalTime"];
            model.Total = (decimal)dt.Rows[0]["Total"];
            model.CreateDate = (DateTime)dt.Rows[0]["CreateDate"];
            model.Creator = (string)dt.Rows[0]["Creator"];
            model.ModifyDate = dt.Rows[0].Field<DateTime?>("ModifyDate");
            model.Modifier = dt.Rows[0]["Modifier"] as string;

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

        public List<PODetailViewModel> GetViewPODetails(string pid)
        {
            string dbQuery =
                $@"
                    SELECT Products.ID, Products.Name, Products.UnitPrice, PO_Details.QTY, PO_Details.Amount
                    FROM Products
                    JOIN 
                    (
                    SELECT ID, QTY, Amount
                    FROM PO_Details
                    WHERE PO_Details.PID = @Pid
                    )AS PO_Details
                    ON Products.ID = PO_Details.ID
                  ";

            List<SqlParameter> dbParameters = new List<SqlParameter>() 
            { 
                new SqlParameter("@Pid", pid)
            };

            var dt = this.GetDataTable(dbQuery, dbParameters);

            List<PODetailViewModel> list = new List<PODetailViewModel>();

            foreach (DataRow dr in dt.Rows)
            {
                PODetailViewModel model = new PODetailViewModel();
                model.ID = (string)dr["ID"];
                model.Name = (string)dr["Name"];
                model.UnitPrice = (decimal)dr["UnitPrice"];
                model.Qty = dr.Field<int?>("QTY");
                model.Amount = dr.Field<decimal?>("Amount");

                list.Add(model);
            }

            return list;
        }
    }
}