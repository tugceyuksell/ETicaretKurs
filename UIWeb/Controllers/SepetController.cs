using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.Controllers
{
    public class SepetController : Controller
    {
        private readonly ITemporaryBasketService temporary;
        public SepetController(ITemporaryBasketService temporary)
        {
            this.temporary = temporary;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SepetGetir()
        {
            if (Request.Cookies["SepetId"] != null)
            {
                int SepetId = int.Parse(Request.Cookies["SepetId"]);
                return PartialView("/Views/PartialViews/SepetGetir.cshtml", temporary.GetBasketsAsync(SepetId).Result);
            }
            return PartialView("/Views/PartialViews/SepetGetir.cshtml", null);
        }

        public IActionResult SepetToplamAdet()
        {
            if (Request.Cookies["SepetId"] != null)
            {
                int SepetId = int.Parse(Request.Cookies["SepetId"]);

                var BulunanUrun = temporary.GetBasketsAsync(SepetId).Result;

                return PartialView("/Views/PartialViews/SepetToplamAdet.cshtml", BulunanUrun.Count());
            }
            return PartialView("/Views/PartialViews/SepetToplamAdet.cshtml", 0);
        }
    }
}
