using DataBase;
using Microsoft.EntityFrameworkCore;
using SignalRApp;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DataBase");

ConfigurationManager manager = builder.Configuration;

builder.Services.AddSignalR(options => options.MaximumParallelInvocationsPerClient = 10);
builder.Services.AddSingleton(manager);
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<DbService>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<ModelService>();
builder.Services.AddSingleton<ModelHubService>();

var app = builder.Build();
app.MapHub<AuthorizationHub>("/authorization");
app.MapHub<ModelHub>("/model");
app.MapHub<JokesDataBaseHub>("/jokes_data_base");

app.Run();
