using MangaBotApi.Application;
using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Intefaces;
using MangaBotApi.Infra.Data.Context;
using MangaBotApi.Infra.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MySqlContext>(options =>
{

    var server = builder.Configuration["database:mysql:server"];
    var port = builder.Configuration["database:mysql:port"];
    var database = builder.Configuration["database:mysql:database"];
    var username = builder.Configuration["database:mysql:username"];
    var password = builder.Configuration["database:mysql:password"];

    options.UseMySql($"Server={server};Port={port};Database={database};Uid={username};Pwd={password}",
        ServerVersion.AutoDetect($"Server={server};Port={port};Database={database};Uid={username};Pwd={password}"), opt =>
    {
        opt.CommandTimeout(180);
        opt.EnableRetryOnFailure(5);
    });
});

IdentityBuilder Ibuilder = builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
});

Ibuilder = new IdentityBuilder(Ibuilder.UserType, typeof(Role), Ibuilder.Services);
Ibuilder.AddEntityFrameworkStores<MySqlContext>();
Ibuilder.AddRoleValidator<RoleValidator<Role>>();
Ibuilder.AddRoleManager<RoleManager<Role>>();
Ibuilder.AddSignInManager<SignInManager<User>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IBaseRepository<Usuario>, BaseRepository<Usuario>>();
builder.Services.AddScoped<IBaseRepository<Manga>, BaseRepository<Manga>>();
builder.Services.AddScoped<IBaseRepository<MangaUsuario>, BaseRepository<MangaUsuario>>();
builder.Services.AddScoped<IBaseRepository<LogMangaBotApi>, BaseRepository<LogMangaBotApi>>();
builder.Services.AddScoped<IBaseRepository<LogMangaBot>, BaseRepository<LogMangaBot>>();
builder.Services.AddScoped<IMangaUsuarioRepository, MangaUsuarioRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
