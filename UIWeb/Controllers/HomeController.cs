using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService products;
        public HomeController(IProductsService products)
        {
            this.products = products;
        }
        public IActionResult Index()
        {
            return View(products.GetAllProducts().Result);
        }
        [Route("/urun/detay/{Id}")]
        public IActionResult detay(int Id)
        {

            return View(products.GetRelationProduct(Id).Result);
        }
    }
}
