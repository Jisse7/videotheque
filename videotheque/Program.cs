using videotheque.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using videotheque.Models;
using videotheque.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // Ajoute cette ligne pour utiliser l'email comme nom d'utilisateur
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
/*14/12*/
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();


/*15/12*/
builder.Services.AddAntiforgery();


/*16/12*/
builder.Services.AddScoped<IDVDManagementService, DVDManagementService>();

builder.Services.AddScoped<IRetourService, RetourService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

    // Crée le rôle "client" s'il n'existe pas déjà
    if (!await roleManager.RoleExistsAsync("client"))
    {
        await roleManager.CreateAsync(new IdentityRole("client"));
    }
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

    // Crée le rôle "client" s'il n'existe pas déjà
    if (!await roleManager.RoleExistsAsync("client"))
    {
        await roleManager.CreateAsync(new IdentityRole("client"));
    }

    
    var adminEmail = "admin@admin";
    var adminPassword = "12345678";

    // Créer le rôle Admin
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Vérifier si l'admin existe
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new Users
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Administrateur",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/*app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
