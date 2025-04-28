using LibraryManagement.Data;
using LibraryManagement.Helpers.Interface;
using LibraryManagement.Helpers;
using LibraryManagement.Repository;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Models;
using Sprache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DotNetEnv.Env.Load();

builder.Configuration["ConnectionStrings:PostgreSQLConnection"] = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__PostgreSQLConnection");
builder.Configuration["JWT:SecretKey"] = Environment.GetEnvironmentVariable("JWT__SecretKey");

builder.Configuration["EmailSettings:SmtpServer"] = Environment.GetEnvironmentVariable("EMAILSETTINGS__SMTPSERVER");
builder.Configuration["EmailSettings:SmtpPort"] = Environment.GetEnvironmentVariable("EMAILSETTINGS__SMTPPORT");
builder.Configuration["EmailSettings:SenderEmail"] = Environment.GetEnvironmentVariable("EMAILSETTINGS__SENDEREMAIL");
builder.Configuration["EmailSettings:SenderPassword"] = Environment.GetEnvironmentVariable("EMAILSETTINGS__SENDERPASSWORD");
builder.Configuration["EmailSettings:EnableSSL"] = Environment.GetEnvironmentVariable("EMAILSETTINGS__ENABLESSL");

builder.Configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__CLOUDNAME");
builder.Configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APIKEY");
builder.Configuration["CloudinarySettings:ApiSecret"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APISECRET");




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình cho phép tất cả các ứng dụng được gọi đến API
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
 policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));



builder.Services.AddSingleton(option =>
{
    var settings = option.GetRequiredService<IOptions<CloudinarySettings>>().Value;

    var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
    return new Cloudinary(account);

}); 

// Tạo chữ ký kết nối

// Đăng ký dbContex
builder.Services.AddDbContext<LibraryManagermentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new() { Title = "LibraryManagement", Version = "v1" });
});
// Đăng ký sử dụng Mapper
builder.Services.AddAutoMapper(typeof(Program));

// Tạo service sử dụng JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new
    Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});


// Life cycle DI
builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
builder.Services.AddScoped<IAuthenRepository, AuthenRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Swagger UI sẽ ở /docs
        c.DocumentTitle = "API Docs";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors();
app.Run();
