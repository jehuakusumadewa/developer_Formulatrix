using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantReservationMentahan.Models;

namespace RestaurantReservationMentahan
{
    class Program
    {
        // Data storage (in-memory)
        static List<Customer> customers = new List<Customer>();
        static List<Reservation> reservations = new List<Reservation>();
        static List<RestaurantTable> tables = new List<RestaurantTable>();
        
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEM RESERVASI RESTORAN ===");
            
            // Initialize some sample data
            InitializeSampleData();
            
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nMenu Utama:");
                Console.WriteLine("1. Buat Reservasi Baru");
                Console.WriteLine("2. Batalkan Reservasi");
                Console.WriteLine("3. Lihat Daftar Reservasi");
                Console.WriteLine("4. Cek Ketersediaan Meja");
                Console.WriteLine("5. Keluar");
                Console.Write("Pilih menu: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        CreateReservation();
                        break;
                    case "2":
                        CancelReservation();
                        break;
                    case "3":
                        ViewReservations();
                        break;
                    case "4":
                        CheckTableAvailability();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Terima kasih!");
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }
            }
        }
        
        static void InitializeSampleData()
        {
            // Add some tables
            tables.Add(new RestaurantTable { Id = 1, TableNumber = "T01", Capacity = 4, Location = "Area Utama" });
            tables.Add(new RestaurantTable { Id = 2, TableNumber = "T02", Capacity = 2, Location = "Area Utama" });
            tables.Add(new RestaurantTable { Id = 3, TableNumber = "T03", Capacity = 6, Location = "Area VIP" });
            tables.Add(new RestaurantTable { Id = 4, TableNumber = "T04", Capacity = 8, Location = "Area Keluarga" });
            tables.Add(new RestaurantTable { Id = 5, TableNumber = "T05", Capacity = 4, Location = "Area Teras" });
            
            // Add some customers
            customers.Add(new Customer { Id = 1, Name = "Budi Santoso", Email = "budi@email.com", Phone = "08123456789" });
            customers.Add(new Customer { Id = 2, Name = "Sari Dewi", Email = "sari@email.com", Phone = "08234567890" });
            customers.Add(new Customer { Id = 3, Name = "Ahmad Fauzi", Email = "ahmad@email.com", Phone = "08345678901" });

            // Add some sample reservations
            reservations.Add(new Reservation
            { 
                Id = 1, 
                CustomerId = 1, 
                TableId = 1, 
                ReservationTime = DateTime.Today.AddHours(19), 
                PartySize = 3, 
                Status = "Confirmed" 
            });
            
            reservations.Add(new Reservation 
            { 
                Id = 2, 
                CustomerId = 2, 
                TableId = 3, 
                ReservationTime = DateTime.Today.AddHours(20), 
                PartySize = 5, 
                Status = "Confirmed" 
            });
        }
        
        static void CreateReservation()
        {
            Console.WriteLine("\n=== BUAT RESERVASI BARU ===");
            
            try
            {
                // Get customer info
                Customer customer;
                Console.Write("Apakah Anda pelanggan baru? (y/n): ");
                string isNewCustomer = Console.ReadLine().ToLower();
                
                if (isNewCustomer == "y")
                {
                    customer = RegisterNewCustomer();
                }
                else
                {
                    Console.Write("Masukkan ID Customer: ");
                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("ID tidak valid!");
                        return;
                    }
                    
                    customer = customers.FirstOrDefault(c => c.Id == customerId);
                    if (customer == null)
                    {
                        Console.WriteLine("Customer tidak ditemukan!");
                        return;
                    }
                }
                
                // Get reservation details
                Console.Write("Jumlah orang: ");
                if (!int.TryParse(Console.ReadLine(), out int partySize) || partySize <= 0)
                {
                    Console.WriteLine("Jumlah orang tidak valid!");
                    return;
                }
                
                Console.Write("Tanggal reservasi (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    Console.WriteLine("Tanggal tidak valid!");
                    return;
                }
                
                Console.Write("Jam reservasi (HH:mm): ");
                if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
                {
                    Console.WriteLine("Jam tidak valid!");
                    return;
                }
                
                DateTime reservationTime = date.Date.Add(time);
                
                // Show available tables
                Console.WriteLine("\nMeja yang tersedia:");
                var availableTables = GetAvailableTables(reservationTime, partySize);
                
                if (availableTables.Count == 0)
                {
                    Console.WriteLine("Maaf, tidak ada meja yang tersedia untuk waktu dan jumlah orang tersebut.");
                    return;
                }
                
                foreach (var table in availableTables)
                {
                    Console.WriteLine($"ID: {table.Id}, No: {table.TableNumber}, Kapasitas: {table.Capacity}, Lokasi: {table.Location}");
                }
                
                Console.Write("Pilih ID meja: ");
                if (!int.TryParse(Console.ReadLine(), out int tableId))
                {
                    Console.WriteLine("ID meja tidak valid!");
                    return;
                }
                
                var selectedTable = tables.FirstOrDefault(t => t.Id == tableId);
                if (selectedTable == null)
                {
                    Console.WriteLine("Meja tidak ditemukan!");
                    return;
                }
                
                // Check if table is still available
                if (!IsTableAvailable(selectedTable.Id, reservationTime))
                {
                    Console.WriteLine("Maaf, meja sudah tidak tersedia untuk waktu tersebut.");
                    return;
                }
                
                // Check capacity
                if (partySize > selectedTable.Capacity)
                {
                    Console.WriteLine($"Maaf, meja hanya bisa menampung maksimal {selectedTable.Capacity} orang.");
                    return;
                }
                
                // Create reservation
                var newReservation = new Reservation
                {
                    Id = reservations.Count > 0 ? reservations.Max(r => r.Id) + 1 : 1,
                    CustomerId = customer.Id,
                    TableId = selectedTable.Id,
                    ReservationTime = reservationTime,
                    PartySize = partySize,
                    Status = "Confirmed"
                };
                
                reservations.Add(newReservation);
                
                Console.WriteLine("\nReservasi berhasil dibuat!");
                Console.WriteLine($"ID Reservasi: {newReservation.Id}");
                Console.WriteLine($"Nama: {customer.Name}");
                Console.WriteLine($"Tanggal: {reservationTime:dd-MM-yyyy HH:mm}");
                Console.WriteLine($"Meja: {selectedTable.TableNumber} ({selectedTable.Location})");
                Console.WriteLine($"Jumlah Orang: {partySize}");
                
                // Send confirmation
                SendConfirmation(customer, newReservation, selectedTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        static void CancelReservation()
        {
            Console.WriteLine("\n=== BATALKAN RESERVASI ===");
            
            Console.Write("Masukkan ID Reservasi: ");
            if (!int.TryParse(Console.ReadLine(), out int reservationId))
            {
                Console.WriteLine("ID tidak valid!");
                return;
            }
            
            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null)
            {
                Console.WriteLine("Reservasi tidak ditemukan!");
                return;
            }
            
            // Check if reservation can be cancelled (max 2 hours before)
            TimeSpan timeUntilReservation = reservation.ReservationTime - DateTime.Now;
            if (timeUntilReservation.TotalHours < 2)
            {
                Console.WriteLine("Reservasi hanya bisa dibatalkan minimal 2 jam sebelum waktu reservasi!");
                return;
            }
            
            reservation.Status = "Cancelled";
            Console.WriteLine("Reservasi berhasil dibatalkan!");
            
            var customer = customers.FirstOrDefault(c => c.Id == reservation.CustomerId);
            if (customer != null)
            {
                SendCancellationNotification(customer, reservation);
            }
        }
        
        static void ViewReservations()
        {
            Console.WriteLine("\n=== DAFTAR RESERVASI ===");
            
            Console.Write("Masukkan tanggal (yyyy-mm-dd) atau kosongkan untuk hari ini: ");
            string dateInput = Console.ReadLine();
            
            DateTime targetDate;
            if (string.IsNullOrWhiteSpace(dateInput))
            {
                targetDate = DateTime.Today;
            }
            else if (!DateTime.TryParse(dateInput, out targetDate))
            {
                Console.WriteLine("Tanggal tidak valid!");
                return;
            }
            
            var dateReservations = reservations
                .Where(r => r.ReservationTime.Date == targetDate.Date && r.Status == "Confirmed")
                .OrderBy(r => r.ReservationTime)
                .ToList();
            
            if (dateReservations.Count == 0)
            {
                Console.WriteLine($"Tidak ada reservasi untuk tanggal {targetDate:dd-MM-yyyy}");
                return;
            }
            
            Console.WriteLine($"\nReservasi untuk {targetDate:dd-MM-yyyy}:");
            Console.WriteLine("==================================================================");
            Console.WriteLine("ID  | Waktu      | Nama Customer      | Meja  | Orang | Status");
            Console.WriteLine("==================================================================");
            
            foreach (var res in dateReservations)
            {
                var customer = customers.FirstOrDefault(c => c.Id == res.CustomerId);
                var table = tables.FirstOrDefault(t => t.Id == res.TableId);
                
                string customerName = customer?.Name ?? "Unknown";
                string tableNumber = table?.TableNumber ?? "Unknown";
                
                Console.WriteLine($"{res.Id,-3} | {res.ReservationTime:HH:mm}    | {customerName,-18} | {tableNumber,-5} | {res.PartySize,-5} | {res.Status}");
            }
        }
        
        static void CheckTableAvailability()
        {
            Console.WriteLine("\n=== CEK KETERSEDIAAN MEJA ===");
            
            Console.Write("Tanggal (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Tanggal tidak valid!");
                return;
            }
            
            Console.Write("Jam (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
            {
                Console.WriteLine("Jam tidak valid!");
                return;
            }
            
            Console.Write("Jumlah orang: ");
            if (!int.TryParse(Console.ReadLine(), out int partySize) || partySize <= 0)
            {
                Console.WriteLine("Jumlah orang tidak valid!");
                return;
            }
            
            DateTime checkTime = date.Date.Add(time);
            var availableTables = GetAvailableTables(checkTime, partySize);
            
            Console.WriteLine($"\nMeja tersedia untuk {checkTime:dd-MM-yyyy HH:mm}, {partySize} orang:");
            
            if (availableTables.Count == 0)
            {
                Console.WriteLine("Tidak ada meja yang tersedia.");
            }
            else
            {
                foreach (var table in availableTables)
                {
                    Console.WriteLine($"- {table.TableNumber} (Kapasitas: {table.Capacity}, Lokasi: {table.Location})");
                }
            }
        }
        
        static List<RestaurantTable> GetAvailableTables(DateTime reservationTime, int partySize)
        {
            var availableTables = new List<RestaurantTable>();
            
            // Standard duration: 1.5 hours
            DateTime reservationEnd = reservationTime.AddHours(1.5);
            
            foreach (var table in tables)
            {
                // Check capacity
                if (table.Capacity < partySize)
                    continue;
                
                // Check if table is reserved at this time
                bool isReserved = reservations.Any(r =>
                    r.TableId == table.Id &&
                    r.Status == "Confirmed" &&
                    r.ReservationTime < reservationEnd &&
                    r.ReservationTime.AddHours(1.5) > reservationTime);
                
                if (!isReserved)
                {
                    availableTables.Add(table);
                }
            }
            
            return availableTables;
        }
        
        static bool IsTableAvailable(int tableId, DateTime reservationTime)
        {
            DateTime reservationEnd = reservationTime.AddHours(1.5);
            
            return !reservations.Any(r =>
                r.TableId == tableId &&
                r.Status == "Confirmed" &&
                r.ReservationTime < reservationEnd &&
                r.ReservationTime.AddHours(1.5) > reservationTime);
        }
        
        static Customer RegisterNewCustomer()
        {
            Console.WriteLine("\n=== REGISTRASI CUSTOMER BARU ===");
            
            var newCustomer = new Customer
            {
                Id = customers.Count > 0 ? customers.Max(c => c.Id) + 1 : 1
            };
            
            Console.Write("Nama: ");
            newCustomer.Name = Console.ReadLine();
            
            Console.Write("Email: ");
            newCustomer.Email = Console.ReadLine();
            
            Console.Write("Telepon: ");
            newCustomer.Phone = Console.ReadLine();
            
            customers.Add(newCustomer);
            Console.WriteLine("Customer berhasil didaftarkan!");
            
            return newCustomer;
        }
        
        static void SendConfirmation(Customer customer, Reservation reservation, RestaurantTable table)
        {
            Console.WriteLine("\n=== MENGIRIM KONFIRMASI ===");
            Console.WriteLine($"To: {customer.Email}");
            Console.WriteLine($"Subject: Konfirmasi Reservasi Restoran");
            Console.WriteLine($"Message: Reservasi Anda telah dikonfirmasi.");
            Console.WriteLine($"         ID: {reservation.Id}, Tanggal: {reservation.ReservationTime:dd-MM-yyyy HH:mm}");
            Console.WriteLine($"         Meja: {table.TableNumber}, Jumlah Orang: {reservation.PartySize}");
            Console.WriteLine("Email konfirmasi telah dikirim (simulasi).");
            
            // In real application, you would send actual email/SMS here
        }
        
        static void SendCancellationNotification(Customer customer, Reservation reservation)
        {
            Console.WriteLine($"\nNotifikasi pembatalan dikirim ke {customer.Email} (simulasi)");
        }
    }



    // Simple classes (without proper encapsulation)





    // TimeSlot is not implemented as a separate class in this simple version
    // It's handled within the methods
}