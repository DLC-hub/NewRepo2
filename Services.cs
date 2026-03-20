using System;

namespace SOLID_Fundamentals
{
    // Вспомогательный класс Order
    public class Order
    {
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }

    // Интерфейсы — абстракции для сервисов уведомлений
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public interface ISmsService
    {
        void SendSms(string phoneNumber, string message);
    }

    // Конкретные реализации сервисов, реализующие интерфейсы
    public class EmailService : IEmailService
    {
        public void IEmailService.SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Sending email to {to}: {subject}");
        }
    }

    public class SmsService : ISmsService
    {
#pragma warning disable IDE0051 
        private void SomeInternalMethod() { }
#pragma warning restore IDE0051

        public void ISmsService.SendSms(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }
    }

    // OrderService — теперь зависит от абстракций, а не от реализаций
    public class OrderService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        
        public OrderService(IEmailService emailService, ISmsService smsService)
        {
            _emailService = emailService;
            _smsService = smsService;
        }

        public void PlaceOrder(Order order)
        {
            ((IEmailService)_emailService).SendEmail(
                order.CustomerEmail,
                "Order Confirmation",
                "Your order has been placed"
            );
            ((ISmsService)_smsService).SendSms(
                order.CustomerPhone,
                "Your order has been placed"
            );
        }
    }

    // NotificationService — также зависит от абстракции IEmailService
    public class NotificationService
    {
        private readonly IEmailService _emailService;

        
        public NotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

#pragma warning disable CA1822 
        public void SendPromotion(string email, string promotion)
#pragma warning restore CA1822
        {
            ((IEmailService)_emailService).SendEmail(
                email,
                "Special Promotion",
                promotion
            );
        }
    }
}
