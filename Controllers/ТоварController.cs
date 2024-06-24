using BuildingShop.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BuildingShop.Controllers
{
    public class ТоварController : Controller
    {
        private DiplomBuildingShopEntities db = new DiplomBuildingShopEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Товар товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            return View(товар);
        }

        public ActionResult Create()
        {
            ViewBag.id_категории = new SelectList(db.Категория_товаров, "id", "Наименование");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Наименование,Описание,id_категории,Цена,Количество,Страна_производитель,Производитель")] Товар товар,
            HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.Товар.Add(товар);
                db.SaveChanges();
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = товар.id.ToString() + Path.GetExtension(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/ProductImages"), fileName);
                    ImageFile.SaveAs(path);
                }
                Session["Notification"] = "Товар №" + товар.id.ToString() + " успешно создан";
                return RedirectToAction("Index");
            }

            ViewBag.id_категории = new SelectList(db.Категория_товаров, "id", "Наименование", товар.id_категории);
            return View(товар);
        }

        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Товар товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_категории = new SelectList(db.Категория_товаров, "id", "Наименование", товар.id_категории);
            return View(товар);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Наименование,Описание,id_категории,Цена,Количество,Страна_производитель,Производитель")] Товар товар,
            HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(товар).State = EntityState.Modified;
                db.SaveChanges();
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = товар.id.ToString() + Path.GetExtension(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/ProductImages"), fileName);
                    ImageFile.SaveAs(path);
                }
                Session["Notification"] = "Товар №" + товар.id.ToString() + " успешно изменен";
                return RedirectToAction("Index");
            }
            ViewBag.id_категории = new SelectList(db.Категория_товаров, "id", "Наименование", товар.id_категории);
            return View(товар);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Товар товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            return View(товар);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Товар товар = db.Товар.Find(id);
            db.Товар.Remove(товар);
            db.SaveChanges();
            var imagePath = Path.Combine(Server.MapPath("~/ProductImages"), id.ToString() + ".jpg");
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            Session["Notification"] = "Товар №" + id.ToString() + " успешно удален";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult ProductPartialView(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "NAME_desc" : "";
            ViewBag.DescSortParm = sortOrder == "Desc" ? "desc_desc" : "Desc";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.CatSortParm = sortOrder == "Cat" ? "cat_desc" : "Cat";
            ViewBag.ManufSortParm = sortOrder == "Manuf" ? "manuf_desc" : "Manuf";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
            ViewBag.CountSortParm = sortOrder == "Count" ? "count_desc" : "Count";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var products = from p in db.Товар select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(e => e.Наименование.Contains(searchString)
                || e.Описание.Contains(searchString)
                || e.Страна_производитель.Contains(searchString)
                || e.Категория_товаров.Наименование.Contains(searchString)
                || e.Цена.ToString().Contains(searchString)
                || e.Производитель.Contains(searchString)
                || e.id.ToString().Contains(searchString)
                || e.Количество.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NAME_desc":
                    products = products.OrderByDescending(p => p.Наименование);
                    break;
                case "Desc":
                    products = products.OrderBy(p => p.Описание);
                    break;
                case "desc_desc":
                    products = products.OrderByDescending(p => p.Описание);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Цена);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Цена);
                    break;
                case "Country":
                    products = products.OrderBy(p => p.Страна_производитель);
                    break;
                case "country_desc":
                    products = products.OrderByDescending(p => p.Страна_производитель);
                    break;
                case "Cat":
                    products = products.OrderBy(p => p.Категория_товаров.Наименование);
                    break;
                case "cat_desc":
                    products = products.OrderByDescending(p => p.Категория_товаров.Наименование);
                    break;
                case "Manuf":
                    products = products.OrderBy(p => p.Производитель);
                    break;
                case "manuf_desc":
                    products = products.OrderByDescending(p => p.Производитель);
                    break;
                case "Id":
                    products = products.OrderBy(p => p.id);
                    break;
                case "id_desc":
                    products = products.OrderByDescending(p => p.id);
                    break;
                case "Count":
                    products = products.OrderBy(p => p.Количество);
                    break;
                case "count_desc":
                    products = products.OrderByDescending(p => p.Количество);
                    break;
                default:
                    products = products.OrderBy(p => p.Наименование);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(("_ProductPartialLayout"), products.ToPagedList(pageNumber, pageSize));
        }
    }
}
