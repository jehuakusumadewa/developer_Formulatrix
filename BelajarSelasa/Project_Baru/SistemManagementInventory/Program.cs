using System;
using System.Collections.Generic;

namespace SistemManagementInventory
{
    public class Program
    {
        // Produk list harus static agar bisa diakses dari semua method static
        static List<Product> products = new List<Product>();
        static public int nextId = 1;

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string Category { get; set; }
            public int MinimumStockLevel { get; set; }
           
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=== INVENTORY MANAGEMENT SYSTEM ===");
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Tambah Produk Baru");
                Console.WriteLine("2. Lihat Semua Produk");
                Console.WriteLine("3. Update Stok Produk");
                Console.WriteLine("4. Hapus Produk");
                Console.WriteLine("5. Cari Produk");
                Console.WriteLine("6. Laporan Stok Rendah");
                Console.WriteLine("7. Keluar");
                Console.Write("Pilih menu (1-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewProduct();
                        break;
                    case "2":
                        ViewAllProducts();
                        break;
                    case "3":
                        UpdateProductStock();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        SearchProduct();
                        break;
                    case "6":
                        LowStockReport();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        break;
                }
            }
        }

        // Method harus di luar Main()
        public static void AddNewProduct()
        {
            Console.WriteLine("Tambah produk baru...");
            System.Console.WriteLine("Masukkan nama produk: ");
            string nama = Console.ReadLine();
            System.Console.WriteLine("Masukkan harga produk: ");
            decimal harga = decimal.Parse(Console.ReadLine());
            System.Console.WriteLine("Masukkan jumlah stok produk: ");
            int stok = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Masukkan kategori produk: ");
            string kategori = Console.ReadLine();
            System.Console.WriteLine("Masukkan level stok minimum produk: ");
            int stokMinimum = int.Parse(Console.ReadLine());
            
            products.Add(new Product{
                Id = nextId++,
                Name = nama,
                Price = harga,
                StockQuantity = stok,
                Category = kategori,
                MinimumStockLevel = stokMinimum
            });
        }

        public static void ViewAllProducts()
        {
            Console.WriteLine("Daftar semua produk:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Nama: {product.Name}, Harga: {product.Price}, Stok: {product.StockQuantity}, Kategori: {product.Category}, Stok Minimum: {product.MinimumStockLevel}");
            }
        }
        public static void UpdateProductStock()
        {
            ViewAllProducts();
            Console.WriteLine("pilih produk yang akan diupdate stoknya...");
            Console.WriteLine("Masukkan ID produk: ");
            var productId = int.Parse(Console.ReadLine());
            var product = products.Find(p => p.Id == productId);
            var newStock = int.Parse(Console.ReadLine());
            product.StockQuantity = newStock;
        }
        public static void DeleteProduct()
        {
            ViewAllProducts();
            Console.WriteLine("pilih produk yang akan dihapus...");
            Console.WriteLine("Masukkan ID produk: ");
            var productId = int.Parse(Console.ReadLine());
            var product = products.Find(p => p.Id == productId);
            products.Remove(product);   
        }
        public static void SearchProduct()
        {
            Console.WriteLine("Cari produk...");
            System.Console.WriteLine("Masukkan nama produk yang dicari: "); 
            var searchTerm = Console.ReadLine();
            var result = products.Find(p => p.Name.Contains(searchTerm)); 
            Console.WriteLine($"ID: {result.Id}, Nama: {result.Name}, Harga: {result.Price}, Stok: {result.StockQuantity}, Kategori: {result.Category}, Stok Minimum: {result.MinimumStockLevel}");   
        }
        public static void LowStockReport()
        {
            Console.WriteLine("Laporan stok rendah:");
            foreach (var product in products)
            {
                if (product.StockQuantity < product.MinimumStockLevel)
                {
                    Console.WriteLine($"ID: {product.Id}, Nama: {product.Name}, Stok: {product.StockQuantity}, Stok Minimum: {product.MinimumStockLevel}");
                }
            }
        }

        public static Product AddNewProductForTest(string nama, decimal harga, int stok, string kategori, int stokMinimum)
        {
            var p = new Product
            {
                Id = nextId++,
                Name = nama,
                Price = harga,
                StockQuantity = stok,
                Category = kategori,
                MinimumStockLevel = stokMinimum
            };

            products.Add(p);
            return p;
        }

        public static void ClearProductsForTest()
        {
            products.Clear();
            nextId = 1;
        }

        public static int GetProductCount()
        {
            return products.Count;
        }

        public static Product GetProductById(int id)
        {
            return products.Find(p => p.Id == id);
        }
        public static void DeleteProductForTest(int id)
        {
            var product = products.Find(p => p.Id == id);
             products.Remove(product);
        }
        public static void UpdateProductStockForTest(int id, int stock)
        {
            var product = products.Find(p => p.Id == id);
            product.StockQuantity = stock;
        }


    }
}
