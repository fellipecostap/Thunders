using Microsoft.EntityFrameworkCore;
using Rebus.Config;
using Rebus.Handlers;
using System.Reflection;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;
using Rebus.ServiceProvider;  

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Thunders Toll System API",
        Version = "v1",
        Description = "API para gerenciamento e relatórios de pedágios",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Suporte Thunders",
            Email = "suporte@thunders.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var features = Features.BindFromConfiguration(builder.Configuration);

builder.Services.AddProblemDetails();

if (features.UseMessageBroker)
{
    builder.Services.AddRebus(configure => configure
        .Transport(t => t.UseRabbitMq(
            builder.Configuration.GetConnectionString("RabbitMq"),
            builder.Configuration["Rebus:QueueName"]))
        .Options(o => {
            o.SetNumberOfWorkers(1);
            o.SetMaxParallelism(5);
        }));

    builder.Services.AutoRegisterHandlersFromAssembly(Assembly.GetExecutingAssembly());
}

if (features.UseEntityFramework)
{
    builder.Services.AddSqlServerDbContext<AppDbContext>(builder.Configuration);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thunders Toll API v1");
        c.RoutePrefix = "swagger"; // Acessível em /swagger
    });
}

app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();