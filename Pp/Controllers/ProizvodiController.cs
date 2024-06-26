using Pp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;

namespace Pp.Controllers
{
    [Authorize]
    public class ProizvodiController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Početna o proizvodima";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Popis(string vrsta, string materijal)
        {
            var proizvodi = bazaPodataka.PopisProizvoda.ToList();

            if (!String.IsNullOrWhiteSpace(vrsta))
                proizvodi = proizvodi.Where(x => x.Vrsta.ToUpper().Contains(vrsta.ToUpper())).ToList();

            if (!String.IsNullOrWhiteSpace(materijal))
                proizvodi = proizvodi.Where(x => x.Materijal == materijal).ToList();

            return View(proizvodi);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            Proizvodi proizvodi = bazaPodataka.PopisProizvoda.FirstOrDefault(x => x.Id == id);

            if (proizvodi == null)
            {
                return RedirectToAction("Popis");
            }

            return View(proizvodi);
        }

        public ActionResult Azuriraj(int? id)
        {
            Proizvodi proizvodi;

            if (!id.HasValue)
            {
                proizvodi = new Proizvodi();
                ViewBag.Title = "Kreiranje proizvoda";
                ViewBag.Novi = true;
            }
            else
            {
                proizvodi = bazaPodataka.PopisProizvoda.FirstOrDefault(x => x.Id == id);
            }

            if (proizvodi == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Ažuriranje podataka o proizvodu";
            ViewBag.Novi = false;

            return View(proizvodi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Proizvodi p)
        {
            if (ModelState.IsValid)
            {
                if (p.Id != 0)
                {
                    bazaPodataka.Entry(p).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.PopisProizvoda.Add(p);
                }
                bazaPodataka.SaveChanges();

                return RedirectToAction("Popis");
            }

            if (p.Id == 0)
            {
                ViewBag.Title = "Kreiranje proizvoda";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o proizvodu";
                ViewBag.Novi = false;
            }

            return View(p);
        }

        public ActionResult Brisi(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            Proizvodi p = bazaPodataka.PopisProizvoda.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja proizvoda";
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Proizvodi p = bazaPodataka.PopisProizvoda.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                return HttpNotFound();
            }

            bazaPodataka.PopisProizvoda.Remove(p);
            bazaPodataka.SaveChanges();

            return View("BrisiStatus");
        }
    }
}
