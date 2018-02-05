using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()=> View();
        

        UrlUpdateDbContext db = new UrlUpdateDbContext();

        [HttpPost]
        public ActionResult Parser(string URLs)
        {
            return View(URLs.Split('\n').ToList());
        }

        [HttpGet]
        public JsonResult GetInfo(string URL)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL);
                webRequest.AllowAutoRedirect = false;
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                var status = (int)(response.StatusCode);

                WebClient x = new WebClient();
                string source = x.DownloadString(URL);
                var titleStr = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                AddingToDb(URL, titleStr, status);

                return Json(new { status = status, title = titleStr }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpPost]
        //public ActionResult Parser(string URLs)
        //{
        //    string urlStr;
        //    string titleStr;
        //    int status;
        //    String[] substrings = URLs.Split('\n');
        //    foreach (var substring in substrings)
        //    {
        //        Response.Write(substring);
        //        urlStr = substring;

        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlStr);
        //        webRequest.AllowAutoRedirect = false;
        //        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        //        status = (int)(response.StatusCode);
        //        Response.Write(status);

        //        WebClient x = new WebClient();
        //        string source = x.DownloadString(urlStr);
        //        titleStr = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        //        Response.Write(titleStr);
        //        AddingToDb(urlStr, titleStr, status);
        //    }
        //    return View();
        //}

       public ActionResult  Statistics(string url) {
            Response.Write(url);
            //ViewBag.updates = DatesToDg(url);
            return View();
        }

        void AddingToDb(string substring, string title, int status)
        {
            var time = DateTime.Now;
            using (var ctx = new UrlUpdateDbContext())
            {
                var url = ctx.URLs.FirstOrDefault(c => c.URLPath.ToLower() == substring.ToLower());
                if (url == null)
                {
                    url = new URL()
                    {
                        CreationDate = time,
                        URLPath = substring
                    };
                }
                url.LastStatus = status;
                url.LastTitle = title;
                url.LastUpdateDate = time;

                if (url.ID == 0) //New row
                    ctx.URLs.Add(url);
                ctx.SaveChanges();

                ctx.URLUpdates.Add(new URLUpdate
                {
                    Status = status,
                    Title = title,
                    UpdateDate = time,
                    URLId = url.ID
                });
                ctx.SaveChanges();
            }
        }

        IList<URLUpdate> AddingToDb(string url)
        {
            using (var ctx = new UrlUpdateDbContext())
            {
                var urlEntity = ctx.URLs.FirstOrDefault(c => c.URLPath == url);
                if (urlEntity == null) return null;
                var updates = ctx.URLUpdates.Where(c => c.URLId == urlEntity.ID).ToList();
                return updates;
            }
        }
        [HttpGet]
        IList<URLUpdate> DatesToDg(string URL)
        {
            using (var ctx = new UrlUpdateDbContext())
            {
                var urlEntity = ctx.URLs.FirstOrDefault(c => c.URLPath == URL);
                if (urlEntity == null) return null;
                var updates = ctx.URLUpdates.Where(c => c.URLId == urlEntity.ID).ToList();
                ViewBag.updates = updates;
                return updates;
            }
        }

        HashSet<int> StatusTodg(DateTime date, IList<URLUpdate> updates)
        {
            HashSet<int> statuses = new HashSet<int>();
            foreach (URLUpdate update in updates)
            {
                if (update.UpdateDate == date) statuses.Add(update.Status);
            }
            return statuses;
        }

        Dictionary<int, int> CountStatuses(DateTime date, HashSet<int> statuses, IList<URLUpdate> updates) {
            Dictionary<int, int> statusCount = new Dictionary<int, int>();

            for (int i = 0; i < statuses.Count; i++) statusCount.Add(statuses.ElementAt(i), 0);
            

            foreach (URLUpdate update in updates) if (update.UpdateDate == date) statusCount[update.Status] = statusCount[update.Status]+1;
            return statusCount;
        }
    }
}