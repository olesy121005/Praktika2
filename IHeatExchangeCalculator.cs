using HeatExchangeApp.Models;

namespace HeatExchangeApp.Services
{
    public interface IHeatExchangeCalculator
    {
        List<CalculationResult> CalculateDetailed(CalculationParameters parameters);
    }
}