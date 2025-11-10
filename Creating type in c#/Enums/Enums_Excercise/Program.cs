public class Program
{
    public static void Main()
    {
        Hari day = Hari.Senin; 
        Console.WriteLine(day);
    }
}


public enum BorderSide
{
    Left, Right, Top, Bottom
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