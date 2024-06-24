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
    public class СотрудникController : Controller
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
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ФИО,Адрес,Номер_телефона")] Сотрудник сотрудник)
        {
            if (ModelState.IsValid)
            {
                db.Сотрудник.Add(сотрудник);
                db.SaveChanges();
                Session["Notification"] = "Сотрудник №"+сотрудник.id.ToString()+" успешно создан";
                return RedirectToAction("Index");
            }

            return View(сотрудник);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ФИО,Адрес,Номер_телефона")] Сотрудник сотрудник)
        {
            if (ModelState.IsValid)
            {
                db.Entry(сотрудник).State = EntityState.Modified;
                db.SaveChanges();
                Session["Notification"] = "Сотрудник №"+сотрудник.id.ToString()+" успешно изменен";
                return RedirectToAction("Index");
            }
            return View(сотрудник);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            db.Сотрудник.Remove(сотрудник);
            db.SaveChanges();
            Session["Notification"] = "Сотрудник №"+id.ToString()+" успешно удален";
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

        public PartialViewResult EmployeePartialView(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "NAME_desc" : "";
            ViewBag.AddressSortParm = sortOrder == "Adr" ? "adr_desc" : "Adr";
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

            var employees = from e in db.Сотрудник select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.ФИО.Contains(searchString)
                || e.Адрес.Contains(searchString)
                || e.Номер_телефона.Contains(searchString)
                || e.id.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NAME_desc":
                    employees = employees.OrderByDescending(e => e.ФИО);
                    break;
                case "Adr":
                    employees = employees.OrderBy(e => e.Адрес);
                    break;
                case "adr_desc":
                    employees = employees.OrderByDescending(e => e.Адрес);
                    break;
                case "Id":
                    employees = employees.OrderBy(e => e.id);
                    break;
                case "id_desc":
                    employees = employees.OrderByDescending(e => e.id);
                    break;
                default:
                    employees = employees.OrderBy(e => e.ФИО);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(("_EmployeePartialLayout"), employees.ToPagedList(pageNumber, pageSize));
        }
    }
}
