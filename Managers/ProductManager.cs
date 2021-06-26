using PIS_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PIS_System.Managers
{
    public class ProductManager : DBBase
    {
        public List<ProductModel> GetProducts()
        {
            string dbQuery =
                @"SELECT * FROM Products";

            List<SqlParameter> dbParameters = new List<SqlParameter>();

            var dt = this.GetDataTable(dbQuery, dbParameters);

            List<ProductModel> list = new List<ProductModel>();

            foreach(DataRow dr in dt.Rows)
            {
                ProductModel model = new ProductModel();
                model.ID = (string)dr["ID"];
                model.Category = (string)dr["Category"];
                model.Name = (string)dr["Name"];
                model.UnitPrice = (decimal)dr["UnitPrice"];

                list.Add(model);
            }

            return list;
        }
    }
}