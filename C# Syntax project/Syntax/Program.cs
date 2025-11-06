/*Pascal case (e.g., MyMethod) is generally used for all 
other identifiers, including classes, public methods, and properties.*/

System.Console.Write("Selamat datang di restoran, bahan apa yang ingin disetorkan :");
/* Camel case (e.g., myVariable) is typically applied 
to parameters, local variables, and private fields.*/

string produkSetor = Console.ReadLine();
System.Console.Write("Berapa banyak yang ingin disetorkan :");
int jumlahKg = int.Parse(Console.ReadLine());
NewRestaurant KFC = new NewRestaurant(produkSetor, jumlahKg);
KFC.TampilkanMakanan();
KFC.TotalBiaya();
// semua bahan 50000/kg
public class NewRestaurant
{
    public string BahanMakanan; // semicolon (;), which serves to terminate a statement. 
    public int JumlahKg;
    public NewRestaurant(string bahanMakanan, int jumlahKg)
    {
        BahanMakanan = bahanMakanan;
        JumlahKg = jumlahKg;
    }
    public void TampilkanMakanan()
    {
        Console.WriteLine($"Bahan makanan yang disetorkan adalah: {BahanMakanan}");
    }
    public void TotalBiaya()
    {
        //menggunakan operator '*' perkalian untuk menghitung jumlah
        Console.WriteLine($"Total biayanya adalah: {JumlahKg * 50_000}");
    }
}



