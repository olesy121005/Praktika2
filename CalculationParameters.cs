using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;

namespace HeatExchangeApp.Models
{
    public class CalculationParameters
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название расчета обязательно")]
        [Display(Name = "Название расчета")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Высота слоя обязательна")]
        [Display(Name = "Высота слоя, м")]
        public string LayerHeight { get; set; } = string.Empty;

        [Required(ErrorMessage = "Коэффициент теплоотдачи обязателен")]
        [Display(Name = "Объемный коэффициент теплоотдачи, Вт/(м³·К)")]
        public string HeatTransferCoefficient { get; set; } = string.Empty;

        [Required(ErrorMessage = "Скорость газа обязательна")]
        [Display(Name = "Скорость газа, м/с")]
        public string GasVelocity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Теплоемкость газа обязательна")]
        [Display(Name = "Теплоемкость газа, кДж/(м³·К)")]
        public string GasHeatCapacity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Расход материалов обязателен")]
        [Display(Name = "Расход материалов, кг/с")]
        public string MaterialConsumption { get; set; } = string.Empty;

        [Required(ErrorMessage = "Теплоемкость материалов обязательна")]
        [Display(Name = "Теплоемкость материалов, кДж/(кг·К)")]
        public string MaterialHeatCapacity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Начальная температура материала обязательна")]
        [Display(Name = "Начальная температура материала, °C")]
        public string InitialMaterialTemp { get; set; } = string.Empty;

        [Required(ErrorMessage = "Начальная температура газа обязательна")]
        [Display(Name = "Начальная температура газа, °C")]
        public string InitialGasTemp { get; set; } = string.Empty;

        [Required(ErrorMessage = "Диаметр аппарата обязателен")]
        [Display(Name = "Диаметр аппарата, м")]
        public string ApparatusDiameter { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public double LayerHeightValue => ConvertToDouble(LayerHeight);
        public double HeatTransferCoefficientValue => ConvertToDouble(HeatTransferCoefficient);
        public double GasVelocityValue => ConvertToDouble(GasVelocity);
        public double GasHeatCapacityValue => ConvertToDouble(GasHeatCapacity);
        public double MaterialConsumptionValue => ConvertToDouble(MaterialConsumption);
        public double MaterialHeatCapacityValue => ConvertToDouble(MaterialHeatCapacity);
        public double InitialMaterialTempValue => ConvertToDouble(InitialMaterialTemp);
        public double InitialGasTempValue => ConvertToDouble(InitialGasTemp);
        public double ApparatusDiameterValue => ConvertToDouble(ApparatusDiameter);

        private double ConvertToDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            try
            {
                string normalizedValue = value.Replace('.', ',');

                if (double.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out double result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Исключение при конвертации '{value}': {ex.Message}");
                return 0;
            }
        }
    }
}