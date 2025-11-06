
/*Open-Closed Principle (OCP) =

Kelas sebaiknya bisa diperluas (open for extension), tapi tidak perlu diubah 
(closed for modification) ketika kita ingin menambah fitur baru.*/

IPayment kartuKredit = new KartuKredit { Nominal = 150_000.75M };
IPayment payPal = new PayPal { Nominal = 793_000.73M };
IPayment e_wallet = new Ewallet { Nominal = 93_000.87M };

MetodePembayaran metode = new MetodePembayaran();
System.Console.WriteLine(metode.Pay(kartuKredit));
System.Console.WriteLine(metode.Pay(payPal));
System.Console.WriteLine(metode.Pay(e_wallet));






public interface IPayment
{
    string GetMetodePembayaran();
}

public class KartuKredit : IPayment
{
    public decimal Nominal { get; set; }
    public string GetMetodePembayaran() => $"Membayar {Nominal} dengan Kartu Kredit";
}

public class PayPal : IPayment
{
    public decimal Nominal { get; set; }
    public string GetMetodePembayaran() => $"Membayar {Nominal} dengan PayPal";
}

public class Ewallet : IPayment
{
    public decimal Nominal { get; set; }
    public string GetMetodePembayaran() => $"Membayar {Nominal} dengan E-Wallet"; 
}


public class MetodePembayaran
{
    public double Amount { get; set; }

    public string Pay(IPayment payment)
    {
        return payment.GetMetodePembayaran();
    }
}
