namespace SOLID_Fundamentals
{
    // Специализированные интерфейсы

    public interface IOrderManagement
    {
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
    }

    public interface IPaymentProcessing
    {
        void ProcessPayment(Order order);
    }

    public interface IShippingManagement
    {
        void ShipOrder(Order order);
    }

    public interface IInvoiceGeneration
    {
        void GenerateInvoice(Order order);
    }

    public interface INotificationService
    {
        void SendNotification(Order order);
    }

    public interface IReportingService
    {
        void GenerateReport(DateTime from, DateTime to);
        void ExportToExcel(string filePath);
    }

    public interface IDatabaseBackup
    {
        void BackupDatabase();
        void RestoreDatabase();
    }

    // Класс OrderManager — реализует все интерфейсы, так как имеет полный функционал

    public class OrderManager : IOrderManagement, IPaymentProcessing, IShippingManagement,
                        IInvoiceGeneration, INotificationService, IReportingService, IDatabaseBackup
    {
        public void IOrderManagement.CreateOrder(Order order)
        {
            Console.WriteLine("Order created");
        }

        public void IOrderManagement.UpdateOrder(Order order)
        {
            Console.WriteLine("Order updated");
        }

        public void IOrderManagement.DeleteOrder(int orderId)
        {
            Console.WriteLine("Order deleted");
        }

        public void IPaymentProcessing.ProcessPayment(Order order)
        {
            Console.WriteLine("Payment processed");
        }

        public void IShippingManagement.ShipOrder(Order order)
        {
            Console.WriteLine("Order shipped");
        }

        public void IInvoiceGeneration.GenerateInvoice(Order order)
        {
            Console.WriteLine("Invoice generated");
        }

        public void INotificationService.SendNotification(Order order)
        {
            Console.WriteLine("Notification sent");
        }

        public void IReportingService.GenerateReport(DateTime from, DateTime to)
        {
            Console.WriteLine("Report generated");
        }

        public void IReportingService.ExportToExcel(string filePath)
        {
            Console.WriteLine("Exported to Excel");
        }

        public void IDatabaseBackup.BackupDatabase()
        {
            Console.WriteLine("Database backed up");
        }

        public void IDatabaseBackup.RestoreDatabase()
        {
            Console.WriteLine("Database restored");
        }
    }

    // Класс CustomerPortal — реализует только необходимые интерфейсы

    public class CustomerPortal : IOrderManagement, IPaymentProcessing
    {
        public void IOrderManagement.CreateOrder(Order order)
        {
            Console.WriteLine("Order created by customer");
        }

        public void IOrderManagement.UpdateOrder(Order order)
        {
            Console.WriteLine("Order updated by customer");
        }

        public void IOrderManagement.DeleteOrder(int orderId)
        {
            Console.WriteLine("Order deleted by customer");
        }

        public void IPaymentProcessing.ProcessPayment(Order order)
        {
            Console.WriteLine("Payment processed by customer");
        }
    }

}
