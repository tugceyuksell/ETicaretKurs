using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.ViewComponents
{
    public class KategoriGetirViewComponent:ViewComponent
    {
        private readonly ICategoriesService service;
        public KategoriGetirViewComponent(ICategoriesService service)
        {
            this.service = service;
        }
        public IViewComponentResult Invoke()
        {
            return View(service.GetAllCategoriesAsync().Result);
        }
    }
}
