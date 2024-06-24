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
    public class ЗаказController : Controller
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
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        public ActionResult Create()
        {
            ViewBag.id_клиента = new SelectList(db.Клиент, "id", "ФИО");
            ViewBag.id_сотрудника = new SelectList(db.Сотрудник, "id", "ФИО");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_клиента,id_сотрудника,Готовность,Дата,Сумма")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Заказ.Add(заказ);
                db.SaveChanges();
                Session["Notification"] = "Заказ №"+заказ.id.ToString() +" успешно создан";
                return RedirectToAction("Details", new { id = заказ.id });
            }

            ViewBag.id_клиента = new SelectList(db.Клиент, "id", "ФИО", заказ.id_клиента);
            ViewBag.id_сотрудника = new SelectList(db.Сотрудник, "id", "ФИО", заказ.id_сотрудника);
            return View(заказ);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }

            ViewBag.id_клиента = new SelectList(db.Клиент, "id", "ФИО", заказ.id_клиента);
            ViewBag.id_сотрудника = new SelectList(db.Сотрудник, "id", "ФИО", заказ.id_сотрудника);
            return View(заказ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_клиента,id_сотрудника,Готовность,Дата,Сумма")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказ).State = EntityState.Modified;
                db.SaveChanges();
                Session["Notification"] = "Заказ №"+заказ.id.ToString()+" успешно изменен";
                return RedirectToAction("Index");
            }
            ViewBag.id_клиента = new SelectList(db.Клиент, "id", "ФИО", заказ.id_клиента);
            ViewBag.id_сотрудника = new SelectList(db.Сотрудник, "id", "ФИО", заказ.id_сотрудника);
            return View(заказ);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказ заказ = db.Заказ.Find(id);
            заказ.Список_товаров.Clear();
            db.Заказ.Remove(заказ);
            db.SaveChanges();
            Session["Notification"] = "Заказ №"+id.ToString()+" успешно удален";
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

        public PartialViewResult OrderPartialView(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DoneSortParm = String.IsNullOrEmpty(sortOrder) ? "Done_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.SumSortParm = sortOrder == "Sum" ? "sum_desc" : "Sum";
            ViewBag.ClientSortParm = sortOrder == "Client" ? "client_desc" : "Client";
            ViewBag.EmployeeSortParm = sortOrder == "Empl" ? "empl_desc" : "Empl";
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

            var orders = from o in db.Заказ select o;
            var allOrders = orders.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                allOrders = allOrders.Where(e => e.Дата.ToString("dd.MM.yyyy").Contains(searchString)
                    || e.Клиент.ФИО.Contains(searchString)
                    || e.Сотрудник.ФИО.Contains(searchString)
                    || e.Сумма.ToString().Contains(searchString)
                    || e.id.ToString().Contains(searchString)).ToList();
            }
            var s = searchString;
            switch (sortOrder)
            {
                case "Done_desc":
                    allOrders = allOrders.OrderByDescending(e => e.Готовность).ToList();
                    break;
                case "Date":
                    allOrders = allOrders.OrderBy(e => e.Дата).ToList();
                    break;
                case "date_desc":
                    allOrders = allOrders.OrderByDescending(e => e.Дата).ToList();
                    break;
                case "Sum":
                    allOrders = allOrders.OrderBy(e => e.Сумма).ToList();
                    break;
                case "sum_desc":
                    allOrders = allOrders.OrderByDescending(e => e.Сумма).ToList();
                    break;
                case "Client":
                    allOrders = allOrders.OrderBy(e => e.Клиент.ФИО).ToList();
                    break;
                case "client_desc":
                    allOrders = allOrders.OrderByDescending(e => e.Клиент.ФИО).ToList();
                    break;
                case "Empl":
                    allOrders = allOrders.OrderBy(e => e.Сотрудник.ФИО).ToList();
                    break;
                case "empl_desc":
                    allOrders = allOrders.OrderByDescending(e => e.Сотрудник.ФИО).ToList();
                    break;
                case "Id":
                    allOrders = allOrders.OrderBy(e => e.id).ToList();
                    break;
                case "id_desc":
                    allOrders = allOrders.OrderByDescending(e => e.id).ToList();
                    break;
                default:
                    allOrders = allOrders.OrderBy(e => e.Готовность).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(("_OrderPartialLayout"), allOrders.ToPagedList(pageNumber, pageSize));
        }
    }
}
