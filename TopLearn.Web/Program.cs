using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing;
using TopLearn.Core.Convertors;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 60000000000;
});

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
#region Authentication
builder.Services.AddAuthentication(Option =>
{
    Option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    Option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    Option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  

})
.AddCookie(option =>
{
    option.LoginPath = "/Login";
    option.LogoutPath = "/Logout";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(42300);

});
#endregion

#region DataBase Context 
builder.Services.AddDbContext<TopLearnContext>(option =>
{
    option.UseSqlServer("Data Source =.; Initial Catalog=TopLearnCore_DB; Integrated security =true; TrustServerCertificate=true");
});
#endregion

#region IOC
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IForumService, ForumService>();
#endregion



var app = builder.Build();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Home/Error404");
    }
});
app.Use(async (context, next) =>
{
    string name = context.Request.Path.Value.ToString();
    if (context.Request.Path.Value.ToString().ToLower().StartsWith("/coursefilesonline"))
    {
        var callingUrl = context.Request.Headers["Referer"].ToString();
        if (callingUrl != "" && (callingUrl.StartsWith("https://localhost:44392") || callingUrl.StartsWith("http://localhost:44392")))
        {
            await next.Invoke();
        }
        else
        {

            context.Response.Redirect("/Login");
        }
    }
    else
    {
        await next.Invoke();
    }
});
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
