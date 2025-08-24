using WepApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WepApi.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WepApi.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDbContext") ?? throw new InvalidOperationException("Connection string 'IdentityDbContext' not found.");

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowManyOrigins",
        builder => builder.WithOrigins(
            "http://localhost:9000",
            "https://nanyanggroup.nanyangtextile.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddDbContext<MySQLDbContext>();
var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
builder.Services.AddDbContext<WepApiIdentityDbContext>(
    options => options.UseMySql(connectionString!, serverVersion,
        mySqlOptions => mySqlOptions.EnableRetryOnFailure())
);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<WepApiIdentityDbContext>();


// custom config asp.net core identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // User settings
    options.User.RequireUniqueEmail = true;

    // SignIn settings
    options.SignIn.RequireConfirmedAccount = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

//  add jwt service for validate token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/uniqlo/swagger/index.html"))
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Request: {context.Request.Path}");
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Production") || app.Environment.IsEnvironment("Test"))
{
    var swaggerPath = app.Environment.IsDevelopment() ? "swagger" : "uniqlo/swagger";

    app.UseSwagger(
        c =>
        {
            c.RouteTemplate = $"{swaggerPath}/{{documentName}}/swagger.json";
        }
    );
    app.UseSwaggerUI(
        c =>
        {
            c.RoutePrefix = swaggerPath;
            c.SwaggerEndpoint($"/{swaggerPath}/v1/swagger.json", "API V1");
        }
    );
}


app.MapGet("/", context =>
{
    var swaggerPath = app.Environment.IsDevelopment() ? "/swagger" : "/uniqlo/swagger";
    context.Response.Redirect(swaggerPath);
    // context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});



app.UseAuthentication();
app.UseCors("AllowManyOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
