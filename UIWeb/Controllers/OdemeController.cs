using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIWeb.Controllers
{
    [Authorize]
    public class OdemeController : Controller
    {
        private readonly ITemporaryBasketService temporary;
        private readonly IOrdersRelationService orders;
        public OdemeController(ITemporaryBasketService temporary, IOrdersRelationService orders)
        {
            this.temporary = temporary;
            this.orders = orders;
        }
        public IActionResult Index()
        {
            int SepetId = int.Parse(Request.Cookies["SepetId"]);
            var data = temporary.GetBasketsAsync(SepetId).Result;
            if (data.Count() != 0)
            {
                return View(temporary.GetBasketsAsync(SepetId).Result);
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost]
        public IActionResult Index(OrderAddress address, string OdemeYontemi)
        {
            int SepetId = int.Parse(Request.Cookies["SepetId"]);
            var SatilanUrun = temporary.GetBasketsAsync(SepetId).Result;

            decimal ToplamFiyat = 0;
            Orders data = new Orders();
            data.OrderDetails = new List<OrderDetails>();
            data.OrderAddress = new List<OrderAddress>();
            foreach (var item in SatilanUrun)
            {
                ToplamFiyat += item.Piece * item.Price; 

                OrderDetails orderDetails = new OrderDetails();
                orderDetails.OrdersId = item.BasketId;
                orderDetails.ProductsId = item.ProductsId;
                orderDetails.BasketId = item.BasketId;
                orderDetails.Piece = item.Piece;
                orderDetails.Price = item.Price;
                orderDetails.MainImages = item.MainImages;
                orderDetails.Name = item.Name;

          
                data.OrderDetails.Add(orderDetails);
            }

            data.PaymentType = OdemeYontemi;
            data.Id = SepetId;
            data.OrderDate = DateTime.Now;
            data.OrderStatus = "Onay Bekliyor.";
            data.TotalPrice = ToplamFiyat * 1.18M;
            data.CargoNumber = "";
            data.CustomersId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value.ToString());
            data.OrderAddress.Add(address);

            var sonuc = orders.AllAddAsync(data).Result;

            if (sonuc.StatusCode == Core.Results.ComplexTypes.StatusCode.Success)
            {

                var TarayicidaBulunanCookie = Request.Cookies["SepetId"];
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(-7);
                Response.Cookies.Append("SepetId", TarayicidaBulunanCookie, options);


                foreach (var item in SatilanUrun)
                {
                    var so = temporary.DeleteAsync(item.Id).Result;
                }

                TempData["Mesaj"] = SepetId + " Numaralı Siparişiniz Alınmıştır.";
                return Redirect("/Odeme/Basarili");
            }
            else
            {
                TempData["Mesaj"] = SepetId + " Numaralı Siparişiniz Alınamamıştır.";
                return Redirect("/Odeme/Basarisiz");
            }
        }
        public IActionResult Basarili()
        {
            return View();
        }
        public IActionResult Basarisiz()
        {
            return View();
        }
    }
}
