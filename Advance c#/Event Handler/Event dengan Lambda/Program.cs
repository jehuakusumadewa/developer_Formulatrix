public class Timer
{
    private int _waktu;
    public int Waktu
    {
        get => _waktu;
        set => _waktu = value;
    }
    public event EventHandler Tick;

    public void Tambahwaktu()
    {
        Waktu++;

        OnTick();
    }

    protected virtual void OnTick()
    {
        Tick?.Invoke(this, EventArgs.Empty);
    }
}

public class Project
{
    public static void Main()
    {
        var timer = new Timer();
        timer.Tick += (sender, e) => System.Console.WriteLine($"Waktu Bertambah {timer.Waktu} detik");

        System.Console.WriteLine("Timer dimulai...");

        for (int i = 0; i< 5; i++)
        {
            timer.Tambahwaktu();
            System.Threading.Thread.Sleep(1000); //delay 1 detik
        }
    }
}