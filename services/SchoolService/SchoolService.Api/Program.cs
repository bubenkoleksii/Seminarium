var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
AddServiceLog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ConvertToProblemDetailsFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile(typeof(ApiMappingProfile));
    configuration.AddProfile(typeof(CoreMappingProfile));
});

builder.Services.SetupOptions(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddIdentityAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseGlobalExceptionHandler();

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddServiceLog()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .WriteTo.File("Logs\\SchoolServiceLog-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();
}
