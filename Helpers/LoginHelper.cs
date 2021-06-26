using PIS_System.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_System.Helpers
{
    public class LoginHelper
    {
        private const string _sessionKey = "Logined";

        public class LoginInfo
        {
            public int ID { get; set; }

            public string Name { get; set; }

        }

        public static bool HasLogined()
        {
            var val = HttpContext.Current.Session[_sessionKey] as LoginInfo;

            if (val != null)
                return true;
            else
                return false;

        }

        public static bool TryLogin(string account, string pwd)
        {
            if (LoginHelper.HasLogined())
                return true;

            AccountManager manager = new AccountManager();
            var model = manager.GetAccount(account);

            if (model != null && string.Compare(pwd, model.Password, false) == 0)
            {
                //保留狀態
                HttpContext.Current.Session[_sessionKey] = new LoginInfo()
                {
                    ID = model.ID,
                    Name = model.Name,
                };

                return true;
            }
            else
                return false;

        }

        public static void Logout()
        {
            if (!LoginHelper.HasLogined())
                return;

            HttpContext.Current.Session.Remove(_sessionKey);
        }

        public static LoginInfo GetCurrentUserInfo()
        {
            if (!LoginHelper.HasLogined())
                return null;

            return HttpContext.Current.Session[_sessionKey] as LoginInfo;
        }
    }
}