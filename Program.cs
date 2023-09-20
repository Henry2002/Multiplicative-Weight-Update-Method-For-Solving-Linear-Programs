using TYP.Angular.Core.Contracts.Modules;
using TYP.Angular.Core.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

//Dependency injections
builder.Services.AddScoped<ISolverModule, SolverModule>();
builder.Services.AddScoped<IIterationsModule, IterationsModule>();
builder.Services.AddScoped<IEfficiencyModule, EfficiencyModule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
