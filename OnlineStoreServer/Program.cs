using Dapper;
using Entities.Models.Categories;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using OnlineStoreServer.Extensions;
using Shared.RequestFeatures.ProductsParameters;
using Shared.RequestFeatures.ParametersAttributes;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureServices();
builder.Services.ConfigureFluentMigrator(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddResponseCaching();
SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());

builder.Services.ConfigureControllers();


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.ConfigureExceptionHandler(logger);
app.MigrateDatabase(logger);

app.Run();