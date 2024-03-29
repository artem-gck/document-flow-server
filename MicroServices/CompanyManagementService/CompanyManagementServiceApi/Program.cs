using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.Realisation;
using CompanyManagementService.Services.Interfaces;
using CompanyManagementService.Services.MapperProfiles;
using CompanyManagementService.Services.Realisation;
using CompanyManagementServiceApi;
using CompanyManagementServiceApi.MapperProfiles;
using CompanyManagementServiceApi.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var redisConnectionString = Environment.GetEnvironmentVariable("CacheConnection") ?? builder.Configuration.GetConnectionString("CacheConnection");
var departmentsConnectionString = Environment.GetEnvironmentVariable("DepartmentsConnection") ?? builder.Configuration.GetConnectionString("DepartmentsConnection");
var usersStructureConnectionString = Environment.GetEnvironmentVariable("UsersStructureConnection") ?? builder.Configuration.GetConnectionString("UsersStructureConnection");
var positionsConnectionString = Environment.GetEnvironmentVariable("PositionsConnection") ?? builder.Configuration.GetConnectionString("PositionsConnection");
var usersInfoConnectionString = Environment.GetEnvironmentVariable("UsersInfoConnection") ?? builder.Configuration.GetConnectionString("UsersInfoConnection");
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

// Add services to the container.

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHealthChecks().AddRedis(redisConnectionString);

builder.Services.AddAutoMapper(typeof(ServicesProfile), typeof(ControllerProfile));

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisConnectionString;
    options.InstanceName = "CompanyManagementService_";
});

builder.Services.AddScoped<IDepartmentAccess, DepartmentAccess>();
builder.Services.AddScoped<IPositionsAccess, PositionsAccess>();
builder.Services.AddScoped<IUserInfoAccess, UserInfoAccess>();
builder.Services.AddScoped<IUserStructureAccess, UserStructureAccess>();

builder.Services.AddScoped<IStructureService, StructureService>();

builder.Services.AddHttpClient<IDepartmentAccess, DepartmentAccess>(httpClient => { httpClient.BaseAddress = new Uri(departmentsConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IUserStructureAccess, UserStructureAccess>(httpClient => { httpClient.BaseAddress = new Uri(usersStructureConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IPositionsAccess, PositionsAccess>(httpClient => { httpClient.BaseAddress = new Uri(positionsConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IUserInfoAccess, UserInfoAccess>(httpClient => { httpClient.BaseAddress = new Uri(usersInfoConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = identityString;
    options.RequireHttpsMetadata = false;
    options.Audience = "management_api";
    options.BackchannelHttpHandler = clientHandler;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagementScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "management_api");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Company management API",
        Description = "An ASP.NET Core Web API for company management"
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityString}/connect/authorize"),
                TokenUrl = new Uri($"{identityString}/connect/token"),
                Scopes = new Dictionary<string, string> { { "management_api", "management api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("management_api");
        setup.OAuthAppName("Document api");
        //setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
    });
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.ConfigureCustomExceptionMiddleware();

app.UseCors("AllowCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
