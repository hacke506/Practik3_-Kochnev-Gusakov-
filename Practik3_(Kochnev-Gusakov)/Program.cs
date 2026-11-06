using Microsoft.EntityFrameworkCore;
using Practik3__Kochnev_Gusakov_.Models;
using Practik3__Kochnev_Gusakov_.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение базы данных SQL Server
builder.Services.AddDbContext<Pr3MagazContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Регистрация сервисов Авторизации (Модуль напарника)
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();

// 3. Регистрация сервисов Товаров и Заказов (Твой модуль)
builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<OrdersService>();

// 4. Настройка JWT-безопасности (Секретный ключ совпадает с JwtService твоего напарника)
var secretKey = "SuperSecretSecureKeyForMyShopApplication123!!!";
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. Настройка Swagger с кнопкой добавления JWT-токена (Замочек)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Practik 3 Shop API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен в формате: Bearer {ваш_токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6. Порядок включения защиты 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
