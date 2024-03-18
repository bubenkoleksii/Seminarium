var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddApplication();
builder.Services.SetupOptions(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
