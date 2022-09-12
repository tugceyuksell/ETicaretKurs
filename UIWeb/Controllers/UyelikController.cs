using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UIWeb.Controllers
{
    public class UyelikController : Controller
    {
        private readonly ICustomerService service;
        public UyelikController(ICustomerService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string Email, string Password)
        {
            var Bulunan = service.LoginAsync(Email, Password).Result;
            if (Bulunan != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("ID", Bulunan.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, Bulunan.NameSurname));
                if (Bulunan.Email == "admin@admin.com")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Yönetici"));
                }
                var UserIdentity = new ClaimsIdentity(claims, "UserInfo");
                ClaimsPrincipal principal = new ClaimsPrincipal(UserIdentity);
                var CookiesTime = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddDays(1),
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, CookiesTime);

                if (Bulunan.Email == "admin@admin.com")
                {
                    return Redirect("/admin/Kategoriler");
                }
                else
                {
                    return Redirect("/");
                }
            }
            else
            {
                ViewBag.Message = "Böyle bir Kullanıcı bulunamadı";
                return View();
            }

        }

        [HttpPost]
        public IActionResult KayitOl(Customers data)
        {
            var Sonuc = service.AddAsync(data).Result;
            if (Sonuc.StatusCode == Core.Results.ComplexTypes.StatusCode.Success)
            {
                var Bulunan = service.LoginAsync(data.Email, data.Password).Result;
                var claims = new List<Claim>();
                claims.Add(new Claim("ID", Bulunan.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, Bulunan.NameSurname));

                var UserIdentity = new ClaimsIdentity(claims, "UserInfo");
                ClaimsPrincipal principal = new ClaimsPrincipal(UserIdentity);
                var CookiesTime = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddDays(1),
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, CookiesTime);
            }
            else
            {
                ViewBag.Message = Sonuc.UserMessage;
            }
            return Redirect("/");
        }
    }
}
