using HeatExchangeApp.Models;
using System.Diagnostics;

namespace HeatExchangeApp.Services
{
    public class HeatExchangeCalculator : IHeatExchangeCalculator
    {
        public List<CalculationResult> CalculateDetailed(CalculationParameters parameters)
        {
            var results = new List<CalculationResult>();

            try
            {
                Debug.WriteLine("Начало расчета...");

                double crossSectionArea = Math.PI * Math.Pow(parameters.ApparatusDiameterValue / 2, 2);
                Debug.WriteLine($"Площадь сечения: {crossSectionArea}");

                double numeratorM = parameters.MaterialConsumptionValue * parameters.MaterialHeatCapacityValue;
                double denominatorM = parameters.GasVelocityValue * crossSectionArea * parameters.GasHeatCapacityValue;
                double m = numeratorM / denominatorM;
                Debug.WriteLine($"m = {m}");

                double numeratorY0 = parameters.HeatTransferCoefficientValue * parameters.LayerHeightValue;
                double denominatorY0 = parameters.GasVelocityValue * parameters.GasHeatCapacityValue * 1000;
                double Y0 = numeratorY0 / denominatorY0;
                Debug.WriteLine($"Y0 = {Y0}");

                double denominator = 1 - m * Math.Exp(((m - 1) * Y0) / m);
                Debug.WriteLine($"denominator = {denominator}");

                int resultId = 1;

                for (double y = 0; y <= parameters.LayerHeightValue; y += 0.5)
                {
                    double Y = (parameters.HeatTransferCoefficientValue * y) /
                              (parameters.GasVelocityValue * parameters.GasHeatCapacityValue * 1000);

                    double expValue = Math.Exp(((m - 1) * Y) / m);
                    double expCalculation1 = 1 - expValue;
                    double expCalculation2 = 1 - m * expValue;

                    double numerator1 = expCalculation1;
                    double numerator2 = expCalculation2;

                    double theta1 = numerator1 / denominator;
                    double theta2 = numerator2 / denominator;

                    double materialTemp = parameters.InitialMaterialTempValue +
                                        (parameters.InitialGasTempValue - parameters.InitialMaterialTempValue) * theta1;

                    double gasTemp = parameters.InitialMaterialTempValue +
                                   (parameters.InitialGasTempValue - parameters.InitialMaterialTempValue) * theta2;

                    double tempDifference = materialTemp - gasTemp;

                    results.Add(new CalculationResult
                    {
                        Id = resultId++,
                        ParametersId = parameters.Id,
                        CoordinateY = Math.Round(y, 1),
                        RelativeHeight = Math.Round(Y, 2),
                        ExpCalculation1 = Math.Round(expCalculation1, 4),
                        ExpCalculation2 = Math.Round(expCalculation2, 4),
                        Theta1 = Math.Round(theta1, 4),
                        Theta2 = Math.Round(theta2, 4),
                        MaterialTemp = Math.Round(materialTemp, 2),
                        GasTemp = Math.Round(gasTemp, 2),
                        TempDifference = Math.Round(Math.Abs(tempDifference), 2),
                        Parameters = parameters
                    });

                    Debug.WriteLine($"y={y}, exp1={expCalculation1}, exp2={expCalculation2}");
                }

                Debug.WriteLine($"Расчет завершен. Получено {results.Count} точек.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка в расчетах: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                for (double y = 0; y <= 5; y += 0.5)
                {
                    results.Add(new CalculationResult
                    {
                        Id = (int)(y * 2) + 1,
                        ParametersId = parameters.Id,
                        CoordinateY = Math.Round(y, 1),
                        RelativeHeight = 0,
                        ExpCalculation1 = 0,
                        ExpCalculation2 = 0,
                        Theta1 = 0,
                        Theta2 = 0,
                        MaterialTemp = 0,
                        GasTemp = 0,
                        TempDifference = 0,
                        Parameters = parameters
                    });
                }
            }

            return results;
        }
    }
}