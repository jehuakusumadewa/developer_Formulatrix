// 1. Definisikan interfaces dulu
public interface IOrderValidator
{
    void Validate(Order order);
}

public interface ITaxCalculator
{
    void CalculateTax(Order order);
}

public interface IDiscountApplicator
{
    void ApplyDiscount(Order order);
}

public interface IOrderRepository
{
    void Save(Order order);
}

public interface IInvoiceGenerator
{
    void GenerateInvoice(Order order);
}

public interface INotificationService
{
    void SendOrderNotification(Order order);
}

public interface IInventoryManager
{
    void UpdateInventory(Order order);
}

public interface ILogger
{
    void Log(string message);
}

// 2. Implementasi kelas-kelas
public class OrderValidator : IOrderValidator
{
    public void Validate(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
            
        if (order.TotalAmount <= 0)
            throw new InvalidOperationException("Invalid order amount");
            
        if (order.Items == null || !order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item");
    }
}

public class TaxCalculator : ITaxCalculator
{
    private const decimal TaxRate = 0.1m;
    
    public void CalculateTax(Order order)
    {
        order.TaxAmount = order.TotalAmount * TaxRate;
        order.TotalAmount += order.TaxAmount; // Tambahkan tax ke total
    }
}

public class DiscountApplicator : IDiscountApplicator
{
    private const decimal PremiumDiscountRate = 0.1m; // 10%
    
    public void ApplyDiscount(Order order)
    {
        if (order.Customer?.IsPremium == true)
        {
            var discountAmount = order.SubTotal * PremiumDiscountRate;
            order.DiscountAmount = discountAmount;
            order.TotalAmount -= discountAmount;
        }
    }
}

public class OrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        using (var context = new DbContext())
        {
            // Generate ID jika belum ada
            if (order.Id == 0)
                order.Id = context.Orders.Max(o => o.Id) + 1;
                
            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}

public class InvoiceGenerator : IInvoiceGenerator
{
    private readonly string _invoiceDirectory;
    
    public InvoiceGenerator(string invoiceDirectory = "Invoices")
    {
        _invoiceDirectory = invoiceDirectory;
        
        // Buat direktori jika belum ada
        if (!Directory.Exists(_invoiceDirectory))
            Directory.CreateDirectory(_invoiceDirectory);
    }
    
    public void GenerateInvoice(Order order)
    {
        var invoiceContent = $"""
            =================================
            INVOICE #{order.Id}
            =================================
            Customer: {order.Customer?.Name}
            Date: {DateTime.Now:yyyy-MM-dd HH:mm}
            
            Items:
            {string.Join("\n", order.Items.Select(i => $"  - {i.ProductName}: {i.Quantity} x {i.UnitPrice:C}"))}
            
            ---------------------------------
            Subtotal: {order.SubTotal:C}
            Discount: {order.DiscountAmount:C}
            Tax: {order.TaxAmount:C}
            Total: {order.TotalAmount:C}
            =================================
            """;
            
        var filePath = Path.Combine(_invoiceDirectory, $"invoice_{order.Id}.txt");
        File.WriteAllText(filePath, invoiceContent);
    }
}

public class EmailNotificationService : INotificationService
{
    public void SendOrderNotification(Order order)
    {
        var emailBody = $"""
            Dear {order.Customer?.Name},
            
            Your order #{order.Id} has been successfully processed.
            
            Order Details:
            - Total Amount: {order.TotalAmount:C}
            - Items: {order.Items?.Count ?? 0}
            
            Thank you for your purchase!
            
            Best regards,
            The Store Team
            """;
            
        SendEmail(order.Customer?.Email, "Order Confirmation", emailBody);
    }
    
    private void SendEmail(string to, string subject, string body)
    {
        // Implementasi pengiriman email
        Console.WriteLine($"Email sent to: {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
    }
}

public class InventoryManager : IInventoryManager
{
    public void UpdateInventory(Order order)
    {
        foreach (var item in order.Items)
        {
            DeductFromInventory(item.ProductId, item.Quantity);
        }
    }
    
    private void DeductFromInventory(int productId, int quantity)
    {
        // Implementasi update inventory
        Console.WriteLine($"Deducted {quantity} units from product #{productId}");
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
    }
}

// 3. OrderProcessor yang sudah refactored
public class OrderProcessor
{
    private readonly IOrderValidator _validator;
    private readonly IDiscountApplicator _discountApplicator;
    private readonly ITaxCalculator _taxCalculator;
    private readonly IOrderRepository _orderRepository;
    private readonly IInvoiceGenerator _invoiceGenerator;
    private readonly INotificationService _notificationService;
    private readonly IInventoryManager _inventoryManager;
    private readonly ILogger _logger;
    
    // Dependency Injection melalui constructor
    public OrderProcessor(
        IOrderValidator validator,
        IDiscountApplicator discountApplicator,
        ITaxCalculator taxCalculator,
        IOrderRepository orderRepository,
        IInvoiceGenerator invoiceGenerator,
        INotificationService notificationService,
        IInventoryManager inventoryManager,
        ILogger logger)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _discountApplicator = discountApplicator ?? throw new ArgumentNullException(nameof(discountApplicator));
        _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _invoiceGenerator = invoiceGenerator ?? throw new ArgumentNullException(nameof(invoiceGenerator));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _inventoryManager = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public OrderProcessingResult ProcessOrder(Order order)
    {
        try
        {
            _logger.Log($"Starting order processing for order #{order.Id}");
            
            // 1. Validasi
            _validator.Validate(order);
            _logger.Log("Order validation passed");
            
            // 2. Apply discount (sebelum tax)
            _discountApplicator.ApplyDiscount(order);
            _logger.Log($"Discount applied: {order.DiscountAmount:C}");
            
            // 3. Calculate tax (setelah discount)
            _taxCalculator.CalculateTax(order);
            _logger.Log($"Tax calculated: {order.TaxAmount:C}");
            
            // 4. Save to database
            _orderRepository.Save(order);
            _logger.Log($"Order saved to database with ID: {order.Id}");
            
            // 5. Generate invoice
            _invoiceGenerator.GenerateInvoice(order);
            _logger.Log("Invoice generated");
            
            // 6. Update inventory
            _inventoryManager.UpdateInventory(order);
            _logger.Log("Inventory updated");
            
            // 7. Send notification
            _notificationService.SendOrderNotification(order);
            _logger.Log("Notification sent");
            
            _logger.Log($"Order #{order.Id} processed successfully");
            
            return new OrderProcessingResult
            {
                Success = true,
                OrderId = order.Id,
                Message = "Order processed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.Log($"Error processing order: {ex.Message}");
            
            return new OrderProcessingResult
            {
                Success = false,
                OrderId = order.Id,
                Message = $"Failed to process order: {ex.Message}"
            };
        }
    }
}

// Supporting classes
public class OrderProcessingResult
{
    public bool Success { get; set; }
    public int OrderId { get; set; }
    public string Message { get; set; }
}

// 4. Contoh penggunaan
public class Program
{
    public static void Main()
    {
        // Setup dependencies (biasanya dilakukan di DI Container)
        var validator = new OrderValidator();
        var discountApplicator = new DiscountApplicator();
        var taxCalculator = new TaxCalculator();
        var orderRepository = new OrderRepository();
        var invoiceGenerator = new InvoiceGenerator();
        var notificationService = new EmailNotificationService();
        var inventoryManager = new InventoryManager();
        var logger = new ConsoleLogger();
        
        // Create OrderProcessor with dependencies
        var orderProcessor = new OrderProcessor(
            validator,
            discountApplicator,
            taxCalculator,
            orderRepository,
            invoiceGenerator,
            notificationService,
            inventoryManager,
            logger);
        
        // Process an order
        var order = new Order
        {
            Customer = new Customer { Name = "John Doe", Email = "john@example.com", IsPremium = true },
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductName = "Laptop", Quantity = 1, UnitPrice = 1000 },
                new OrderItem { ProductId = 2, ProductName = "Mouse", Quantity = 2, UnitPrice = 25 }
            },
            SubTotal = 1050 // Total sebelum discount dan tax
        };
        
        var result = orderProcessor.ProcessOrder(order);
        Console.WriteLine($"Result: {result.Message}");
    }
}