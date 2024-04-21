﻿using IdentityService;

using Serilog;

using Shared.Contracts.Options;
using Shared.Utils.Mail;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs\\IdentityServiceLog-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    const string logsOutputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: logsOutputTemplate)
        .WriteTo.File("Logs\\IdentityServiceLog-.txt", rollingInterval: RollingInterval.Day, outputTemplate: logsOutputTemplate)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(nameof(MailOptions)));
    builder.Services.AddScoped<IMailService, MailService>();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app, builder.Configuration);

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}