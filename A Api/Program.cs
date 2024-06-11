using System.Reflection;
using Data.Context;
using Data.Persistencia;
using Data.Persistencia.Impl;
using Domain.Service;
using Domain.Service.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UniqueTrip.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:5103") // Reemplaza con la URL de tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FINANCASH API",
        Description = "An ASP.NET Core Web API for managing FINANCASH API items",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


//Dependencie Injection 
builder.Services.AddScoped<IClientDomain, ClientDomain>();
builder.Services.AddScoped<IClientData, ClientData>();
builder.Services.AddScoped<IContractDomain, ContractDomain>();
builder.Services.AddScoped<IContractData, ContractData>();
builder.Services.AddScoped<ILoanDomain, LoanDomain>();
builder.Services.AddScoped<ILoanData, LoanData>();
builder.Services.AddScoped<IMonthlyScheduleDomain, MonthlyScheduleDomain>();
builder.Services.AddScoped<IMonthlyScheduleData, MonthlyScheduleData>();
builder.Services.AddScoped<IScheduleDomain, ScheduleDomain>();
builder.Services.AddScoped<IScheduleData, ScheduleData>();
builder.Services.AddScoped<IUserDomain, UserDomain>();
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<FinancialUtils>(); 
builder.Services.AddScoped<IntermediateClass>(); 




//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<AppDbContext>(
    dbContextOptions =>
    {
        dbContextOptions.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: System.TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
        );
    });

builder.Services.AddAutoMapper(
    typeof(ModelToResource),
    typeof(ResourceToModel)
);





var app = builder.Build();


using (var scoope = app.Services.CreateScope())
using (var context = scoope.ServiceProvider.GetService<AppDbContext>())
{
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();