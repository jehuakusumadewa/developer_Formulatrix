using Serilog;
using System;
class Program
{
    static void Main(string[] args)
    {
        // Konfigurasi dasar
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Aplikasi dimulai");
            
            // Kode aplikasi Anda
            Console.WriteLine("Hello World!");
            
            Log.Debug("Operasi berjalan");
            Log.Warning("Peringatan: sesuatu perlu diperhatikan");
            Log.Error(new Exception("Contoh error"), "Terjadi kesalahan");
            
            Log.Information("Aplikasi selesai");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Aplikasi terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}