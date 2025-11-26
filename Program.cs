using Microsoft.EntityFrameworkCore;
using HeatExchangeApp.Data;
using HeatExchangeApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=HeatExchange.db"));

builder.Services.AddScoped<IHeatExchangeCalculator, HeatExchangeCalculator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculation}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.CalculationParameters.Any())
    {
        context.CalculationParameters.Add(new HeatExchangeApp.Models.CalculationParameters
        {
            Name = "Пример расчета",
            LayerHeight = "5",
            HeatTransferCoefficient = "2450",
            GasVelocity = "0.6",
            GasHeatCapacity = "1.34",
            MaterialConsumption = "1.7",
            MaterialHeatCapacity = "1.49",
            InitialMaterialTemp = "600",
            InitialGasTemp = "20",
            ApparatusDiameter = "2.1",
            CreatedDate = DateTime.Now
        });
        context.SaveChanges();
    }
}

app.Run();