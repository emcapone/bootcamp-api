using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using bootcamp_api.Data;
using bootcamp_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*");
            policy.WithMethods("GET", "POST", "OPTIONS", "PUT", "DELETE");
            policy.WithHeaders("Content-Type");
        });
});

// Add services to the container.
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "Pawssier API",
            Description = "An ASP.NET Core Web API for Pawssier",
            Contact = new OpenApiContact
            {
                Name = "Emily Foglia",
                Email = "efoglia@ceiamerica.com",
                Url = new Uri("https://www.ceiamerica.com/")
            },
            Version = "v1"
        });
        c.EnableAnnotations();
        c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, 
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    });

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:PawssierConnectionString");

builder.Services
    .AddDbContext<PawssierContext>(p => p.UseSqlServer(connectionString));

builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<ICalendarEventService, CalendarEventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Migrate the database to the latest version automatically on application startup
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var serviceScope = serviceScopeFactory.CreateScope())
{
    serviceScope.ServiceProvider.GetService<PawssierContext>()?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawssier API V1");
});

app.Run();