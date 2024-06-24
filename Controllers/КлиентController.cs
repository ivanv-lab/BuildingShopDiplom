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
    public class КлиентController : Controller
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
            Клиент клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ФИО,Адрес,Номер_телефона")] Клиент клиент)
        {
            if (ModelState.IsValid)
            {
                db.Клиент.Add(клиент);
                db.SaveChanges();
                Session["Notification"] = "Клиент №"+клиент.id.ToString() +" успешно создан";
                return RedirectToAction("Index");
            }

            return View(клиент);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиент клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ФИО,Адрес,Номер_телефона")] Клиент клиент)
        {
            if (ModelState.IsValid)
            {
                db.Entry(клиент).State = EntityState.Modified;
                db.SaveChanges();
                Session["Notification"] = "Клиент №"+клиент.id.ToString()+" успешно изменен";
                return RedirectToAction("Index");
            }
            return View(клиент);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиент клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Клиент клиент = db.Клиент.Find(id);
            db.Клиент.Remove(клиент);
            db.SaveChanges();
            Session["Notification"] = "Клиент №"+id.ToString()+" успешно удален";
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

        public PartialViewResult ClientPartialView(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FIO_desc" : "";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Adr_desc" : "Address";
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

            var clients = from c in db.Клиент select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(e => e.ФИО.Contains(searchString)
                || e.Адрес.Contains(searchString)
                || e.Номер_телефона.Contains(searchString)
                || e.id.ToString().Contains(searchString));

            }

            switch (sortOrder)
            {
                case "FIO_desc":
                    clients = clients.OrderByDescending(c => c.ФИО);
                    break;
                case "Address":
                    clients = clients.OrderBy(c => c.Адрес);
                    break;
                case "Adr_desc":
                    clients = clients.OrderByDescending(c => c.Адрес);
                    break;
                case "Id":
                    clients = clients.OrderBy(c => c.id);
                    break;
                case "id_desc":
                    clients = clients.OrderByDescending(c => c.id);
                    break;
                default:
                    clients = clients.OrderBy(c => c.ФИО);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(("_ClientPartialLayout"), clients.ToPagedList(pageNumber, pageSize));
        }
    }
}
