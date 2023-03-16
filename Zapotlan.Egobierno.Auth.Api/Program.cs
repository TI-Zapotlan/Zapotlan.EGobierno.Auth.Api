using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.Services;
using Zapotlan.EGobierno.Auth.Infrastructure.Data;
using Zapotlan.EGobierno.Auth.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var dataCenterConnection = builder.Configuration.GetConnectionString("DataCenterConnection");

// Add services to the container.

// xBlaze: Busca todos los profiles de Automapper en todos los proyectos => https://www.youtube.com/watch?v=DEOBWo2YodI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataCenterContext>(options => 
    options.UseSqlServer(dataCenterConnection));

builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

//builder.Services.AddMvcCore(options => { // Sin uso porque se utiliza el de ApiController
//    options.Filters.Add<ValidationFilters>();
//});

builder.Services.AddMvcCore();
//builder.Services.AddFluentValidation(options => {
//    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
//});

// La forma anterior es obsoleta, basado en: https://stackoverflow.com/questions/73402059/asp-net-core-web-api-fluentvalidationmvcextensions-addfluentvalidationimvcbui
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
