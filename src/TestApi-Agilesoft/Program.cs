using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infraestructure;
using Infraestructure.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.DTO.Api.Auth;
using System.Text;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development
});

/*configuración que define un nuevo proveedor de servicio*/
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

/*se agrega el modulo de autoFact*/
builder.Host.ConfigureContainer<ContainerBuilder>(builderAutofac => builderAutofac.RegisterModule(new InfraestructureModule(builder.Environment.EnvironmentName == "Development")));


// Obtiene los detalles de la cadena de conexión desde las variables de entorno
var dbServer = builder.Configuration["DB_SERVER1"] ?? Environment.GetEnvironmentVariable("DB_SERVER1");
var dbDatabase = builder.Configuration["DB_DATABASE1"] ?? Environment.GetEnvironmentVariable("DB_DATABASE1");
var dbUser = builder.Configuration["DB_USER"] ?? Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = builder.Configuration["DB_PASSWORD1"] ?? Environment.GetEnvironmentVariable("DB_PASSWORD1");

// Construye la cadena de conexión
var connectionString = $"Server={dbServer};Database={dbDatabase};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";

/*se define de donde se obtiene la cadena de conexión de la base de datos*/
builder.Services.AddDbContext<Context>(
    options =>
    {
        options.UseSqlServer(connectionString);
    });

// Obtiene valor de la variable para las politicas CORS desde las variables de entorno
var CorsPolicy = builder.Configuration["VAR_CORS"] ?? Environment.GetEnvironmentVariable("VAR_CORS");

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, ApplicationBuilder =>
    {
        ApplicationBuilder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
        //ApplicationBuilder.WithOrigins("localhost: 4200");
        //ApplicationBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //.AllowAnyHeader().AllowAnyMethod();

    });
});

//Obtiene la llave para la creación de los tokens desde las variables de entorno
var JWTKey = builder.Configuration["JWTKey"] ?? Environment.GetEnvironmentVariable("JWTKey");

builder.Services.Configure<JWTKeySettings>(opts =>
{
    opts.JWTKey = JWTKey;
});




builder.Services.AddSwaggerGen(
c =>
{
    //c.SwaggerDoc("Test", new OpenApiInfo { Title = "POSAPI",Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
            },
            new string[]{}
        }
    });
}
);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = "JwtBearer";
    config.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("61M4dXz96vXuvqoBbwHKiRRXXghZ94pn")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
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

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(CorsPolicy);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agilesoft-Api"));



app.MapControllers();


app.Run();
