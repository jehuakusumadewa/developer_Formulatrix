// public class TemperatureSensor
// {
//     public double Temperature;
//     public event EventHandler TemperatureChanged;

//     public void SetTemperature(double temperature)
//     {
//         Temperature = temperature;

//         TemperatureChanged?.Invoke(this, EventArgs.Empty);
//     }
// }

// yang atas yg aku buat

public class TemperatureSensor
{
    private double _temperature;
    public double Temperature
    {
        get => _temperature;
        set
        {
            if (_temperature != value)
            {
                _temperature = value;
                OnTemperatureChanged();
            }
        }
    }

    public event EventHandler TemperatureChanged;

    protected virtual void OnTemperatureChanged()
    {
        TemperatureChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetTemperature(double temperature)
    {
        Temperature = temperature;
    }
}
/*Jadi dia bikin 2 variable untuk temperature 1 private buat nilai awal public buat check 
sama buat asign kalau nilai awal berubah
sama invoke event dia bikin jadi method, jadi tinggal panggil methodnya

*/

public class Display
{
    public void ShowTemperature(object sender, EventArgs e)
    {
        if (sender is TemperatureSensor sensor)
        {
            System.Console.WriteLine($"suhu saat ini: {sensor.Temperature}");
        }
        /*kalau object yang mangggil method display itu class temperature sendor maka jalankan metodnya*/
    }
}

public class Program
{
    public static void Main()
    {
        //panggil class publisher
        TemperatureSensor sensor = new TemperatureSensor();
        //panggil subscriber
        Display display = new Display();

        //daftarin subscriber dgn cara panggil event dari class publisher
        sensor.TemperatureChanged += display.ShowTemperature;

        //panggil method publisher yang ngeinvoke event yg dibuatnya
        sensor.SetTemperature(25.5);
        sensor.SetTemperature(30.0);
        sensor.SetTemperature(28.5);
    }
}