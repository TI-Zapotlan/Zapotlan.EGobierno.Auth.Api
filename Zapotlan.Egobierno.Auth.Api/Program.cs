using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.Services;
using Zapotlan.EGobierno.Auth.Infrastructure.Data;
using Zapotlan.EGobierno.Auth.Infrastructure.Filters;
using Zapotlan.EGobierno.Auth.Infrastructure.Interfaces;
using Zapotlan.EGobierno.Auth.Infrastructure.Repositories;
using Zapotlan.EGobierno.Auth.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);
var dataCenterConnection = builder.Configuration.GetConnectionString("DataCenterConnection");
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddCors(o => {
    o.AddPolicy(name: MyAllowSpecificOrigins,
        policy => {
            policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173"
                )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
}).AddNewtonsoftJson(options => {
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection("ZapPagination"));

builder.Services.AddDbContext<DataCenterContext>(options => 
    options.UseSqlServer(dataCenterConnection));

builder.Services.AddTransient<IAreaService, AreaService>();
builder.Services.AddTransient<IDerechoService, DerechoService>();
builder.Services.AddTransient<IGrupoService, GrupoService>();
builder.Services.AddTransient<IPersonaService, PersonaService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton<IUriService>(provider => { 
    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
    var request = accesor.HttpContext.Request;
    var absoulteUrl = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(absoulteUrl);
});

// xBlaze: Busca todos los profiles de Automapper en todos los proyectos => https://www.youtube.com/watch?v=DEOBWo2YodI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(o => { 
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters { 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
    };
});

// Casi todo va antes de cargar MVC

builder.Services.AddMvcCore();
// La forma anterior es obsoleta, basado en: https://stackoverflow.com/questions/73402059/asp-net-core-web-api-fluentvalidationmvcextensions-addfluentvalidationimvcbui
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
