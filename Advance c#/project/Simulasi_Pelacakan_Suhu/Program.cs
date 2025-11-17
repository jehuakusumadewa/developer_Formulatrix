/*Proyek ini adalah simulasi sistem yang melacak pembacaan suhu dari sensor. 
Sistem ini memiliki kemampuan untuk menghasilkan data suhu, melaporkan ketika suhu mencapai 
ambang batas (menggunakan Events), dan memproses data secara efisien (menggunakan Iterators).*/

using System;
using System.Collections.Generic;
using System.Linq;

// 1. Delegates & Events: Digunakan untuk notifikasi ketika ambang batas suhu tercapai.
public delegate void TemperatureThresholdHandler(decimal currentTemp, decimal threshold);

public class TemperatureTracker
{
    // 2. Event: Dideklarasikan menggunakan Delegate di atas.
    public event TemperatureThresholdHandler HighTemperatureReached;

    // 3. Nullable Value Type (decimal?): Digunakan untuk menyimpan ambang batas yang opsional.
    public decimal? Threshold { get; set; } = null; 

    // 4. Operator Overloading: Overload operator > untuk perbandingan suhu
    public static bool operator >(TemperatureTracker tracker, decimal temp)
    {
        // Jika Threshold tidak null, bandingkan suhu saat ini dengan Threshold.
        return tracker.Threshold.HasValue && temp > tracker.Threshold.Value;
    }

    public static bool operator <(TemperatureTracker tracker, decimal temp)
    {
        // Harus overload pasangan operator.
        return tracker.Threshold.HasValue && temp < tracker.Threshold.Value;
    }

    // Iterator: Menghasilkan serangkaian pembacaan suhu secara malas (lazy).
    public IEnumerable<decimal> GetSimulatedReadings(int count)
    {
        Console.WriteLine("\n--- Memulai Pembacaan Data Sensor ---");
        var rand = new Random();
        for (int i = 0; i < count; i++)
        {
            // Simulasikan pembacaan suhu acak (misalnya antara 18.0 dan 35.0)
            decimal currentTemp = (decimal)Math.Round(rand.NextDouble() * 17.0 + 18.0, 1); 
            
            // Logika Operator Overloading dan Event
            if (this > currentTemp) // Menggunakan operator overloading yang baru didefinisikan!
            {
                // Memanggil Event jika ada subscriber dan Threshold diatur
                HighTemperatureReached?.Invoke(currentTemp, Threshold.Value);
            }
            
            // 5. yield return (Iterator): Menghasilkan satu pembacaan pada satu waktu
            yield return currentTemp;
        }
        Console.WriteLine("--- Pembacaan Data Sensor Selesai ---\n");
    }

    // 6. Exception Handling: Menggunakan TryParse pattern
    public void SetThreshold(string input)
    {
        if (decimal.TryParse(input, out decimal newThreshold))
        {
            Threshold = newThreshold;
            Console.WriteLine($"[INFO] Ambang batas suhu diatur ke: {Threshold}");
        }
        else
        {
             // Menggunakan TryXXX method pattern
             Console.WriteLine("[ERROR] Input ambang batas tidak valid. Gunakan angka.");
        }
    }

    // 7. Exception Handling: Menggunakan Try-Catch-Finally
    public void RunTracker(int readingCount)
    {
        try
        {
            foreach (var temp in GetSimulatedReadings(readingCount))
            {
                Console.WriteLine($"Suhu saat ini: {temp}°C");
                // Simulasikan penundaan
                System.Threading.Thread.Sleep(50); 
            }
        }
        catch (Exception ex)
        {
            // Tangkap exception tak terduga (misalnya I/O atau memory)
            Console.WriteLine($"[KRITIS] Terjadi kesalahan tak terduga: {ex.Message}");
        }
        finally
        {
            // Kode cleanup yang selalu dijalankan
            Console.WriteLine("\n[CLEANUP] Sesi pelacakan telah berakhir.");
        }
    }
}

// MAIN PROGRAM
public class Program
{
    public static void Main()
    {
        var tracker = new TemperatureTracker();

        // Subscriber untuk event (metode yang akan dipanggil saat event terjadi)
        tracker.HighTemperatureReached += (currentTemp, threshold) =>
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n*** PERINGATAN! Suhu {currentTemp}°C melampaui ambang batas {threshold}°C! ***");
            Console.ResetColor();
        };

        // Atur Ambang Batas
        tracker.SetThreshold("30.0"); 
        
        // Panggil tracker
        tracker.RunTracker(15); 
        
        // Contoh: Set Threshold ke null lagi menggunakan Nullable Type
        tracker.Threshold = null; 
        Console.WriteLine("\n[INFO] Ambang batas dinonaktifkan (null).");
    }
}