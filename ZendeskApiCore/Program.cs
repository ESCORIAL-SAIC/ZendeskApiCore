using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ZendeskApiCore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClaveSecreta"]!)
            )
        };

        options.TokenValidationParameters = tokenValidationParameters;
        var masterToken = builder.Configuration["JWT:VivaPeron"];

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? authorization = context.Request.Headers.Authorization;

                if (authorization != null && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authorization.Substring("Bearer ".Length).Trim();
                    if (!string.IsNullOrEmpty(masterToken) && token == masterToken)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, "super_user_from_master_token"),
                            new Claim(ClaimTypes.Name, "Super Usuario"),
                            new Claim(ClaimTypes.Role, "1 - Administrador"),
                            new Claim(ClaimTypes.Role, "4 - Administrador Zendesk"),
                            new Claim(ClaimTypes.Role, "2 - Usuario"),
                            new Claim(ClaimTypes.Role, "3 - Usuario Zendesk")
                        };
                        
                        var identity = new ClaimsIdentity(claims, "MasterTokenAuth");
                        context.Principal = new ClaimsPrincipal(identity);
                        
                        context.Success();
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("1 - Administrador", "4 - Administrador Zendesk"));
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("1 - Administrador", "4 - Administrador Zendesk", "2 - Usuario", "3 - Usuario Zendesk"));
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ESCORIALContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EscorialPostgreSql")));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Zendesk API Core",
        Description = "An ASP.NET Core Web API for managing Zendesk Web Complaints",
        Contact = new OpenApiContact
        {
            Name = "Mauricio López",
            Email = "mlopez@escorial.com.ar"
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    OpenApiSecurityScheme securityDefinition = new()
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };
    options.AddSecurityDefinition("jwt_auth", securityDefinition);
    OpenApiSecurityScheme securityScheme = new()
    {
        Reference = new OpenApiReference()
        {
            Id = "jwt_auth",
            Type = ReferenceType.SecurityScheme
        }
    };
    OpenApiSecurityRequirement securityRequirements = new()
    {
        {
            securityScheme, Array.Empty<string>()
        },
    };
    options.AddSecurityRequirement(securityRequirements);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

await app.RunAsync();