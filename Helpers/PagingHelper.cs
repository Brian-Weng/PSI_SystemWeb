using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_System.Helpers
{
    public class PagingHelper
    {
        string Url = HttpContext.Current.Request.Url.AbsolutePath;
        public class PagingLink
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public string Title { get; set; }
        }
        public int CalculatePages(int totalSize, int pageSize)
        {
            int pages = totalSize / pageSize;

            if (totalSize % pageSize != 0)
                pages += 1;

            return pages;
        }

        public string GetQueryString(int? pageIndex)
        {
            List<string> conditions = new List<string>();

            if (pageIndex.HasValue)
                conditions.Add("Page=" + pageIndex.Value);

            string retText =
                (conditions.Count > 0)
                    ? "?" + string.Join("&", conditions)
                    : string.Empty;

            return retText;
        }

        public List<PagingLink> RepPagingList(int pages)
        {
            List<PagingLink> pagingList = new List<PagingLink>();
            for (var i = 1; i <= pages; i++)
            {
                pagingList.Add(new PagingLink()
                {
                    Link = Url + this.GetQueryString(i),
                    Name = $"{i}",
                    Title = $"前往第 {i} 頁"
                });
            }
            return pagingList;
        }

        public string GetPageUrl(int pageIndex)
        {
            return Url + this.GetQueryString(pageIndex);
        }
    }
}