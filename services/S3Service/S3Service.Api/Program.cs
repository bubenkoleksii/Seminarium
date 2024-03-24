var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
AddServiceLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddServiceLog()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .WriteTo.File("Logs\\S3ServiceLog-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();
}
