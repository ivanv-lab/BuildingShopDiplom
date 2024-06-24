using BuildingShop.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BuildingShop.Controllers
{
    public class HomeController : Controller
    {
        private DiplomBuildingShopEntities db = new DiplomBuildingShopEntities();
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

        public JsonResult GetChartData()
        {
            var orders = db.Заказ
                .GroupBy(o => new { Year = o.Дата.Year, Month = o.Дата.Month })
                .Select(g => new
                {
                    year = g.Key.Year,
                    month = g.Key.Month,
                    sum = g.Sum(o => o.Сумма)
                })
                .OrderBy(o => o.year)
                .ToList();

            foreach (var o in orders)
            {
                var r = o;
            }
            return Json(orders, JsonRequestBehavior.AllowGet);
        }
    }
}