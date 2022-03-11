using MCPackServer.Areas.Identity;
using MCPackServer.Data;
using MCPackServer.Data.Entity;
using MCPackServer.Services;
using MCPackServer.Services.Interfaces;
using MCPackServer.Utility.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddDbContext<MCPACKDBContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection")));

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IArticlesToPurchaseService, ArticlesToPurchaseService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IContactsService, ContactsService>();
builder.Services.AddScoped<IFamiliesService, FamiliesService>();
builder.Services.AddScoped<IProjectProductsService, ProjectProductsService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<IProvidersService, ProvidersService>();
builder.Services.AddScoped<IPurchaseOrdersService, PurchaseOrdersService>();
builder.Services.AddScoped<IQuotesService, QuotesService>();
builder.Services.AddScoped<IRequisitionArticlesService, RequisitionArticlesService>();
builder.Services.AddScoped<IRequisitionsService, RequisitionsService>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();