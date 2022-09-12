using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.Controllers
{
    public class JsonIslemleriController : Controller
    {
        private readonly ITemporaryBasketService temporary;
        private readonly IProductsService products;
        public JsonIslemleriController(ITemporaryBasketService temporary, IProductsService products)
        {
            this.temporary = temporary;
            this.products = products;
        }
        public JsonResult Sepet(int ProductsId, int Piece)
        {
            int SepetId = 0;
            #region KULLANICI BAZLI SEPET NUMARASI ÜRETME VE KONTROL ETME
            if (Request.Cookies["SepetId"] != null)
            {
                SepetId = int.Parse(Request.Cookies["SepetId"]);
            }
            else
            {
                Random random = new Random();
                SepetId = random.Next(0, 99987987);
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("SepetId", SepetId.ToString(), cookie);
            }
            #endregion
            // Sepet Ekleme ve Güncelleme İşlemleri
            var EklenmekIstenenUrun = products.GetRelationProduct(ProductsId).Result;
            TemporaryBaskets Models = new TemporaryBaskets();
            Models.ProductsId = ProductsId;
            Models.Piece = 1;
            Models.Name = EklenmekIstenenUrun.Name;
            Models.MainImages = EklenmekIstenenUrun.MainImage;
            Models.BasketId = SepetId;
            Models.Price = EklenmekIstenenUrun.Price;
            var sonuc = temporary.AddAsync(Models).Result;
            return Json(sonuc.UserMessage);
        }


        // Sepet'teki Ürünün adeti Artırılması veya azaltılması istendiğinde çalışacak metot.
        public JsonResult SepetUrunAdet(int Id, bool islem)
        {
            var sonuc = temporary.PieceUpdateAsync(Id,islem).Result;
            return Json(sonuc.UserMessage);
        }

        // Sepetteki ürün silinmek istendiğinde çalışacak olan metot.
        public JsonResult SepetUrunDelete(int Id)
        {
            var sonuc = temporary.DeleteAsync(Id).Result;
            return Json(sonuc.UserMessage);
        }
    }
}
