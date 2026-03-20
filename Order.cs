using System.Collections.Generic;

namespace SOLID_Fundamentals
{
    // Теперь это отдельная сущность — Клиент (отвечает за контактные данные)
    public class Customer
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        
    }

    // Заказ — отвечает только за данные заказа
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public List<string> Items { get; set; }

        // Ссылка на клиента — а не копирование его данных
        public Customer Customer { get; set; }
    }
}
