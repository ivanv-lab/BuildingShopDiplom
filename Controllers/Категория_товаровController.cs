using BuildingShop.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BuildingShop.Controllers
{
    public class Категория_товаровController : Controller
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
            Категория_товаров категория_товаров = db.Категория_товаров.Find(id);
            if (категория_товаров == null)
            {
                return HttpNotFound();
            }
            return View(категория_товаров);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Наименование")] Категория_товаров категория_товаров)
        {
            if (ModelState.IsValid)
            {
                db.Категория_товаров.Add(категория_товаров);
                db.SaveChanges();
                Session["Notification"] = "Категория товаров №"+категория_товаров.id.ToString()+" успешно создана";
                return RedirectToAction("Index");
            }

            return View(категория_товаров);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Категория_товаров категория_товаров = db.Категория_товаров.Find(id);
            if (категория_товаров == null)
            {
                return HttpNotFound();
            }
            return View(категория_товаров);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Наименование")] Категория_товаров категория_товаров)
        {
            if (ModelState.IsValid)
            {
                db.Entry(категория_товаров).State = EntityState.Modified;
                db.SaveChanges();
                Session["Notification"] = "Категория товаров №"+категория_товаров.id.ToString() +" успешно изменена";
                return RedirectToAction("Index");
            }
            return View(категория_товаров);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Категория_товаров категория_товаров = db.Категория_товаров.Find(id);
            if (категория_товаров == null)
            {
                return HttpNotFound();
            }
            return View(категория_товаров);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Категория_товаров категория_товаров = db.Категория_товаров.Find(id);
            db.Категория_товаров.Remove(категория_товаров);
            db.SaveChanges();
            Session["Notification"] = "Категория товаров №"+id.ToString()+" успешно удалена";
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

        public PartialViewResult CategoryPartialView(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "NAME_desc" : "";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var categories = from c in db.Категория_товаров select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Наименование.Contains(searchString) 
                || c.id.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NAME_desc":
                    categories = categories.OrderByDescending(c => c.Наименование);
                    break;
                case "Id":
                    categories = categories.OrderBy(c => c.id);
                    break;
                case "id_desc":
                    categories = categories.OrderByDescending(c => c.id);
                    break;
                default:
                    categories = categories.OrderBy(c => c.Наименование);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(("_CategoryPartialLayout"), categories.ToPagedList(pageNumber, pageSize));
        }
    }
}
