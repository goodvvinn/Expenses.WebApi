using Expenses.Core;
using Expenses.Core.Abstractions;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

// Add services to the container.
// builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION_STRING"));
});


builder.Services.AddControllers();

builder.Services.AddTransient<IExpensesServices, ExpensesServices>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IStatisticsServices, StatisticsServices>();

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSwaggerDocument(settings =>
{
    settings.Title = "Expenses";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("ExpensesPolicy", builder =>
    {
        builder.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))

        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("ExpensesPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseOpenApi();

app.UseSwaggerUi3();

//app.MapRazorPages();

app.UseEndpoints(endpoints =>   endpoints.MapControllers());

var service = (IServiceScopeFactory)app.Services.GetRequiredService(typeof(IServiceScopeFactory));

using (var db = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>())
{
    if(db == null) 
    {
        throw new Exception("Could not connect to the database");
    }

    db.Database.Migrate();
};

app.Run();
