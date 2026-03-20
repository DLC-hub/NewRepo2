using System;
using System.Collections.Generic;

namespace SOLID_Fundamentals
{
    //  Интерфейс для стратегии скидки 
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(decimal orderAmount);
    }

    // Конкретные стратегии скидок 
    public class RegularDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.05m;
    }

    public class PremiumDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.10m;
    }

    public class VIPDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.15m;
    }

    public class StudentDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.08m;
    }

    public class SeniorDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.07m;
    }

    // Интерфейс для стратегии доставки
    public interface IShippingCostStrategy
    {
        decimal CalculateShippingCost(decimal weight, string destination);
    }

    // Конкретные стратегии доставки
    public class StandardShipping : IShippingCostStrategy
    {
        public decimal CalculateShippingCost(decimal weight, string destination) => 5.00m + (weight * 0.5m);
    }

    public class ExpressShipping : IShippingCostStrategy
    {
        public decimal CalculateShippingCost(decimal weight, string destination) => 15.00m + (weight * 1.0m);
    }

    public class OvernightShipping : IShippingCostStrategy
    {
        public decimal CalculateShippingCost(decimal weight, string destination) => 25.00m + (weight * 2.0m);
    }

    public class InternationalShipping : IShippingCostStrategy
    {
        public decimal CalculateShippingCost(decimal weight, string destination)
        {
            return destination switch
            {
                "USA" => 30.00m,
                "Europe" => 35.00m,
                "Asia" => 40.00m,
                _ => 50.00m
            };
        }
    }

    // Ультра новый, расширяемый калькулятор 
    public class DiscountCalculator
    {
        private readonly Dictionary<Type, IDiscountStrategy> _discountStrategies;
        private readonly Dictionary<Type, IShippingCostStrategy> _shippingStrategies;

        public DiscountCalculator()
        {
            // Регистрация стратегий (можно вынести в DI-контейнер)
            _discountStrategies = new()
            {
                { typeof(RegularDiscount), new RegularDiscount() },
                { typeof(PremiumDiscount), new PremiumDiscount() },
                { typeof(VIPDiscount), new VIPDiscount() },
                { typeof(StudentDiscount), new StudentDiscount() },
                { typeof(SeniorDiscount), new SeniorDiscount() }
            };

            _shippingStrategies = new()
            {
                { typeof(StandardShipping), new StandardShipping() },
                { typeof(ExpressShipping), new ExpressShipping() },
                { typeof(OvernightShipping), new OvernightShipping() },
                { typeof(InternationalShipping), new InternationalShipping() }
            };
        }

        

        public decimal CalculateDiscount<T>(decimal orderAmount) where T : IDiscountStrategy
        {
            if (_discountStrategies.TryGetValue(typeof(T), out var strategy))
            {
                return strategy.CalculateDiscount(orderAmount);
            }
            return 0;
        }

        public decimal CalculateShippingCost<T>(decimal weight, string destination) where T : IShippingCostStrategy
        {
            if (_shippingStrategies.TryGetValue(typeof(T), out var strategy))
            {
                return strategy.CalculateShippingCost(weight, destination);
            }
            return 0;
        }
    }
}
