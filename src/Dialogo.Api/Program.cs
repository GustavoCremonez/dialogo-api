using Dialogo.Api.Extensions;
using Dialogo.Application;
using Dialogo.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

app.UseExceptionHandling();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
