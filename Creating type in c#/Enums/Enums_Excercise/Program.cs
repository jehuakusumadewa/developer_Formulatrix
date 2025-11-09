public class Program
{
    public static void Main()
    {
        Hari day = Hari.Senin; 
        Console.WriteLine(day);
    }
}



public enum Hari
{
    Senin,
    Selasa,
    Rabu,
    Kamis,
    Jumat,
    Sabtu,
    Minggu
}