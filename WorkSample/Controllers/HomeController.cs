using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Order()
        {
            string orderApiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "order", });
            string colorApiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "color", });
            string productApiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "product", });
            string sizeApiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "size", });
            ViewBag.OrderApi = new Uri(Request.Url, orderApiUri).AbsoluteUri.ToString();
            ViewBag.ProductApi = new Uri(Request.Url, productApiUri).AbsoluteUri.ToString();
            ViewBag.SizeApi = new Uri(Request.Url, sizeApiUri).AbsoluteUri.ToString();
            ViewBag.ColorApi = new Uri(Request.Url, colorApiUri).AbsoluteUri.ToString();


            return View();
        }
    }
}