using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthSqlConnection")));

builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.Password.RequiredLength = 8;
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedEmail = false;

}).AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

var app = builder.Build();


app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var user = new UserEntity { UserName = "admin@domain.com", FirstName = "admin", LastName = "admin", Email = "admin@domain.com" };

    var userExists = await userManager.Users.AnyAsync(x => x.Email == user.Email);
    if (!userExists)
    {

        var result = await userManager.CreateAsync(user, "BytMig123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

app.MapControllers();

app.Run();
