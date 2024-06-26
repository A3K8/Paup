using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pp.Controllers
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
        public ActionResult Pregled()
        {
            ViewBag.Message = "Pregled proizvoda.";

            return View();
        }

        public ActionResult Ponuda()
        {
            ViewBag.Message = "Zatraži ponudu.";

            return View();
        }
        public ActionResult ONama()
        {
            ViewBag.Message = "O nama.";

            return View();
        }

        public ActionResult Stolarija()
        {
            ViewBag.Message = "Opis o PVC i ALU stolariji.";

            return View();
        }

    }
}
