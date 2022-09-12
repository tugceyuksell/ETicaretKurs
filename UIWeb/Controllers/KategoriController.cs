using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.Controllers
{
    public class KategoriController : Controller
    {
        private readonly ICategoriesService service;

        public KategoriController(ICategoriesService service)
        {
            this.service = service;
        }

        [Route("/Kategori/{Id}")]
        public IActionResult Index(int Id)
        {
            var dss = service.GetAllProductsFirstCategoriesAsync(Id).Result;

            return View(service.GetAllProductsFirstCategoriesAsync(Id).Result);
        }
    }
}
