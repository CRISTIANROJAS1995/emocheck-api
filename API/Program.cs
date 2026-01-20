using Application.Facades;
using Application.Services;
using DinkToPdf.Contracts;
using DinkToPdf;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configuraci�n de autenticaci�n JWT
var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"]
    };
});

// Configuraci�n de autorizaci�n con pol�ticas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("basic", policy => policy.RequireRole("basic"));
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("investigator", policy => policy.RequireRole("investigator"));
    options.AddPolicy("reports", policy => policy.RequireRole("reports"));
});

// CORS
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Repositorios
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IBusinessGroupRepository, BusinessGroupRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRl_RolePermissionRepository, Rl_RolePermissionRepository>();
builder.Services.AddScoped<IRl_UserAreaRepository, Rl_UserAreaRepository>();
builder.Services.AddScoped<IRl_UserCityRepository, Rl_UserCityRepository>();
builder.Services.AddScoped<IRl_UserRoleRepository, Rl_UserRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IExamTypeRepository, ExamTypeRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamSubjectRepository, ExamSubjectRepository>();
builder.Services.AddScoped<IExamResultRepository, ExamResultRepository>();
builder.Services.AddScoped<ICompanyLicenseRepository, CompanyLicenseRepository>();
builder.Services.AddScoped<IBusinessGroupLicenseRepository, BusinessGroupLicenseRepository>();
builder.Services.AddScoped<IAreaLicenseRepository, AreaLicenseRepository>();
builder.Services.AddScoped<ICityLicenseRepository, CityLicenseRepository>();
builder.Services.AddScoped<ILicenseLogRepository, LicenseLogRepository>();
builder.Services.AddScoped<ILicenseConfigurationRepository, LicenseConfigurationRepository>();

builder.Services.AddHttpClient();

// Servicios
builder.Services.AddScoped<IBusinessGroupService, BusinessGroupService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IExamSubjectService, ExamSubjectService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IExamResultService, ExamResultService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<ILicenseManagementService, LicenseManagementService>();

builder.Services.AddScoped<UserHierarchyFacade>();

//External
builder.Services.AddScoped<IVerifEyeService, VerifEyeService>();
builder.Services.AddScoped<IExamReportService, ExamReportService>();


// Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("corsapp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
