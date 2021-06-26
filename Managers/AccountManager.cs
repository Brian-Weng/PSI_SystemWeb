using PIS_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PIS_System.Managers
{
    public class AccountManager : DBBase
    {
        public AccountModel GetAccount(string account)
        {
            string dbquery =
                @"SELECT * FROM Accounts
                  WHERE Account = @Account";

            List<SqlParameter> dbParameters = new List<SqlParameter>();
            dbParameters.Add(new SqlParameter("@Account", account));

            var dt = this.GetDataTable(dbquery, dbParameters);

            AccountModel model = new AccountModel();
            model.ID = (int)dt.Rows[0]["ID"];
            model.Account = (string)dt.Rows[0]["Account"];
            model.Password = (string)dt.Rows[0]["Password"];
            model.Name = (string)dt.Rows[0]["Name"];

            return model;
        }
    }
}