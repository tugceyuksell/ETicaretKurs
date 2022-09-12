using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.AutoMapping;
using Business.DependencyAutofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutoProfile));

builder.Services.AddControllersWithViews().AddFluentValidation();

// Autofac'ın Burada Yani UIWEB'de kullanılacağını bildiriyoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(x => x.RegisterModule(new AutoFacModule()));


#region User Login için Gerekli Olan Ayarlamalar.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Uyelik";
    x.LogoutPath = "/admin/Cikis";
    x.AccessDeniedPath = "/YetkisizGiris";
});
#endregion


var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(x =>
{
    // Proje içerisinde URL Yapıları yazılımcı tarafından değiştirilmediği sürece Varsayılan Mantıkla çalışacağını belirten ayarlamalardır.

    // Normal insan ilk siteye girdiğinde Açılacak sayfayı belirtmemizi sağlıyor (Home/Index)
    x.MapDefaultControllerRoute();

    // Admin Paneline Girecek arkadaşların siteadresi.com/admin Yazıcınca gelecek ekranı belirtmemizi sağlar.
    x.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
        );
});
app.Run();
