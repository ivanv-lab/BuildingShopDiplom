using BuildingShop.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BuildingShop.Controllers
{
    public class Список_товаровController : Controller
    {
        private DiplomBuildingShopEntities db = new DiplomBuildingShopEntities();
        public ActionResult Index()
        {
            var список_товаров = db.Список_товаров.Include(с => с.Заказ).Include(с => с.Товар);
            return View(список_товаров.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Список_товаров список_товаров = db.Список_товаров.Find(id);
            if (список_товаров == null)
            {
                return HttpNotFound();
            }
            return View(список_товаров);
        }

        public ActionResult Create(int id)
        {
            ViewBag.id_заказа = id;
            ViewBag.id_товара = new SelectList(db.Товар, "id", "Наименование");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_товара,id_заказа,Кол_во,Цена")] Список_товаров список_товаров, int id)
        {
            список_товаров.id_заказа = id;
            if (ModelState.IsValid)
            {
                db.Список_товаров.Add(список_товаров);
                db.SaveChanges();
                Заказ заказ = db.Заказ.Find(id);
                var Sum = db.Список_товаров.Where(s => s.id_заказа == заказ.id).Sum(s => s.Кол_во * s.Цена);
                заказ.Сумма = Sum;
                db.Entry(заказ).State = EntityState.Modified;
                db.SaveChanges();
                Товар товар = db.Товар.Find(список_товаров.id_товара);
                int productCount = товар.Количество;
                int newCount= productCount-список_товаров.Кол_во.Value;
                товар.Количество = newCount;
                db.Entry(товар).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Заказ/Details", new { id = id });
            }
            ViewBag.id_товара = new SelectList(db.Товар, "id", "Наименование", список_товаров.id_товара);
            return View(список_товаров);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Список_товаров список_товаров = db.Список_товаров.Find(id);
            if (список_товаров == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_заказа = new SelectList(db.Заказ, "id", "id", список_товаров.id_заказа);
            ViewBag.id_товара = new SelectList(db.Товар, "id", "Наименование", список_товаров.id_товара);
            return View(список_товаров);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_товара,id_заказа,Кол_во,Цена")] Список_товаров список_товаров)
        {
            if (ModelState.IsValid)
            {
                db.Entry(список_товаров).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_заказа = new SelectList(db.Заказ, "id", "id", список_товаров.id_заказа);
            ViewBag.id_товара = new SelectList(db.Товар, "id", "Наименование", список_товаров.id_товара);
            return View(список_товаров);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Список_товаров список_товаров = db.Список_товаров.Find(id);
            if (список_товаров == null)
            {
                return HttpNotFound();
            }
            return View(список_товаров);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Список_товаров список_товаров = db.Список_товаров.Find(id);
            var orderId = список_товаров.id_заказа;
            db.Список_товаров.Remove(список_товаров);
            db.SaveChanges();
            Заказ заказ = db.Заказ.Find(orderId);
            var Sum = db.Список_товаров.Where(s => s.id_заказа == заказ.id).Sum(s => s.Кол_во * s.Цена);
            заказ.Сумма = Sum;
            db.Entry(заказ).State = EntityState.Modified;
            db.SaveChanges();
            Session["Notification"] = "Товар успешно удален из заказа";
            return RedirectToAction("../Заказ/Details", new { id = orderId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult GetProductPrice(int ProductId)
        {
            var price = db.Товар.Where(x => x.id == ProductId).First().Цена;
            var image = Url.Content("~/ProductImages/" + ProductId + ".jpg");
            var count=db.Товар.Where(x=>x.id== ProductId).First().Количество;
            return Json(new { price = price, image = image, count=count });
        }   
    }
}
