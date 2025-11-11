
public class Siswa
{
    public string Nama { get; set; }
    public List<double> Nilai { get; set; }

    public DateTime? TanggalLahir { get; set; }


}
public class PencatatanLog
{
    public void SubLangganan() // methode subsribe blm tau
    {
        System.Console.WriteLine("Awas langganan berkahir");
    }
}

public class PengelolaNilai
{
    public List<Siswa> DaftarSiswa;
    public event NilaiBermasalahHandler OnNilaiRendahDitemukan;

    // micu event kalau dibawah ambang batas blm tau

    //Operator overloading, bikin method kah ? ataua this class yang sama ini ?

    //penggunaan iterator method
    // public IEnumerable<>, typenya apa ? sesuai nilai yg bakal di bandingkan kah ? atau apa





}
public delegate void NilaiBermasalahHandler(string siswa, double nilaiBermasalah);
public class Program
{
    
    public static void Main()
    {
        //inisiasi
        var pengelola = new PengelolaNilai();
        var logPencatatan = new PencatatanLog();
        //mengkaitkan event dengan delegate(subcribe)
        // pengelola.OnNilaiRendahDitemukan += logPencatatan.catatPeringantan;
        var siswa1 = new Siswa("Budi", new List<double> { 85.0, 78.0, 92.0 });
        //siswa dengan nilai rendah untuk memicu event
        var siswa2 = new Siswa("Ani", new List<double> { 75.0, 55.0, 63.0})
        //siswa dengan nullable type
        var siswa3 = new Siswa("Candra", new List<double> { 90.0, 88.0, 95.0 }) {
            NilaiBonus = 5.0,
            TanggalLahir = new DateTime(2005, 5, 10)
        };
        //tambah data ke list
        pengelola.TambahSiswa(siswa1);
        //
        //
        pengelola.PeriksaNilai(); // memeriksa, Event akan terpicu
        //demonstrasi exception halding input salah 

        pengelola.CobaTambahNilaiBaru(siswa1, "bukan_angka");// akan memicu exception
        //menggunakan nilaiBonus yang merupakan double?

        System.Console.WriteLine($"Nilai Bonus Candra: {siswa3.GetnilaiBonusOrDefault()}");
        
        if (Siswa.TanggalLahir.HasValue)
        {
            System.Console.WriteLine("Tanggal Lahir Budi");
        }else
        {
            System.Console.WriteLine("Tanggal lahir budi blm tersedia");
        }
        
        //coba try catch blm tau
        try
        {
            
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}




