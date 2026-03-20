using System;
using System.Collections.Generic;
using System.Linq;

namespace SOLID_Fundamentals
{
    // Вспомогательный класс Order
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public string CustomerEmail { get; set; }
    }

    // Интерфейсы для каждой ответственности
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        Order GetOrder(int orderId);
        IEnumerable<Order> GetAllOrders();
    }

    public interface IPaymentProcessor
    {
        void ProcessPayment(string paymentMethod, decimal amount);
    }

    public interface IInventoryService
    {
        void UpdateInventory(List<string> items);
    }

    public interface INotificationService
    {
        void SendEmail(string to, string message);
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public interface IReceiptGenerator
    {
        void GenerateReceipt(Order order);
    }

    public interface IReportGenerator
    {
        void GenerateMonthlyReport(IEnumerable<Order> orders);
    }

    public interface IExcelExporter
    {
        void ExportToExcel(IEnumerable<Order> orders, string filePath);
    }

    // Реализации интерфейсов
    public class OrderRepository : IOrderRepository
    {
        private List<Order> _orders = new List<Order>();

        public void IOrderRepository.AddOrder(Order order)
        {
            _orders.Add(order);
            Console.WriteLine($"Order {order.Id} added");
        }

        public Order IOrderRepository.GetOrder(int orderId)
        {
            return _orders.FirstOrDefault(o => o.Id == orderId);
        }

        public IEnumerable<Order> IOrderRepository.GetAllOrders()
        {
            return _orders;
        }
    }

    public class PaymentProcessor : IPaymentProcessor
    {
        public void IPaymentProcessor.ProcessPayment(string paymentMethod, decimal amount)
        {
            Console.WriteLine($"Processing payment: {amount:C} via {paymentMethod}");
        }
    }

    public class InventoryService : IInventoryService
    {
        public void IInventoryService.UpdateInventory(List<string> items)
        {
            Console.WriteLine($"Updating inventory for {items.Count} items");
        }
    }

    public class NotificationService : INotificationService
    {
        public void INotificationService.SendEmail(string to, string message)
        {
            Console.WriteLine($"Sending email to {to}: {message}");
        }
    }

    public class Logger : ILogger
    {
        public void ILogger.Log(string message)
        {
            Console.WriteLine($"LOG: {message}");
        }
    }

    public class ReceiptGenerator : IReceiptGenerator
    {
        public void IReceiptGenerator.GenerateReceipt(Order order)
        {
            Console.WriteLine($"Generating receipt for order {order.Id}");
        }
    }

    public class ReportGenerator : IReportGenerator
    {
        public void IReportGenerator.GenerateMonthlyReport(IEnumerable<Order> orders)
        {
            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count();
            Console.WriteLine($"Monthly Report: {totalOrders} orders, Revenue: {totalRevenue:C}");
        }
    }

    public class ExcelExporter : IExcelExporter
    {
        public void IExcelExporter.ExportToExcel(IEnumerable<Order> orders, string filePath)
        {
            Console.WriteLine($"Exporting {orders.Count()} orders to {filePath}");
        }
    }

    // OrderProcessor — теперь отвечает только за обработку заказов
    public class OrderProcessor
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IInventoryService _inventoryService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly IReceiptGenerator _receiptGenerator;

        public OrderProcessor(
            IOrderRepository orderRepository,
            IPaymentProcessor paymentProcessor,
            IInventoryService inventoryService,
            INotificationService notificationService,
            ILogger logger,
            IReceiptGenerator receiptGenerator)
        {
            _orderRepository = orderRepository;
            _paymentProcessor = paymentProcessor;
            _inventoryService = inventoryService;
            _notificationService = notificationService;
            _logger = logger;
            _receiptGenerator = receiptGenerator;
        }

        public void AddOrder(Order order)
        {
            ((IOrderRepository)_orderRepository).AddOrder(order);
        }

        public void ProcessOrder(int orderId)
        {
            var order = ((IOrderRepository)_orderRepository).GetOrder(orderId);
            if (order != null)
            {
                Console.WriteLine($"Processing order {orderId}");

                if (order.TotalAmount <= 0)
                    throw new Exception("Invalid order amount");

                ((IPaymentProcessor)_paymentProcessor).ProcessPayment(order.PaymentMethod, order.TotalAmount);
                ((IInventoryService)_inventoryService).UpdateInventory(order.Items);
                ((INotificationService)_notificationService).SendEmail(order.CustomerEmail, $"Order {orderId} processed");
                ((ILogger)_logger).Log($"Order {orderId} processed at {DateTime.Now}");
                ((IReceiptGenerator)_receiptGenerator).GenerateReceipt(order);
            }
        }
    }

    // ReportingService — отдельный класс для отчётов и экспорта
    public class ReportingService
    {
        private readonly IReportGenerator _reportGenerator;
        private readonly IExcelExporter _excelExporter;
        private readonly IOrderRepository _orderRepository;

        public ReportingService(
            IReportGenerator reportGenerator,
            IExcelExporter excelExporter,
            IOrderRepository orderRepository)
        {
            _reportGenerator = reportGenerator;
            _excelExporter = excelExporter;
            _orderRepository = orderRepository;
        }

        public void GenerateMonthlyReport()
        {
            ((IReportGenerator)_reportGenerator).GenerateMonthlyReport(((IOrderRepository)_orderRepository).GetAllOrders());
        }

        public void ExportToExcel(string filePath)
        {
            ((IExcelExporter)_excelExporter).ExportToExcel(((IOrderRepository)_orderRepository).GetAllOrders(), filePath);
        }
    }
}
