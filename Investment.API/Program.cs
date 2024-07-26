using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Investment.Repository.DbContext;
using Investment.Service.Interfaces;
using Investment.Service.Services;
using System.Text;
using Investment.Repository.Interfaces;
using Investment.Repository.Repositories;
using Portfolio.Repository.DbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InvestmentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostGresConnectionString")));
builder.Services.AddSingleton(RabbitHutch.CreateBus("host=rabbitmq"));
builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

var bus = RabbitHutch.CreateBus("host=rabbitmq");
builder.Services.AddSingleton(bus);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sGQ7+cHIYRyCJoq1l0F9utfBhCG4jxDVq9DKhrWyXys=")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();
DbMgmt.MigrationInit(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var lifetimeScope = app.Services.CreateScope().ServiceProvider;
var paymentService = lifetimeScope.GetRequiredService<IInvestmentService>();


app.Run();