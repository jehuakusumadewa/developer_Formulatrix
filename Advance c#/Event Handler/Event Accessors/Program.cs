public class Button
{
    private EventHandler _click;
    private int _handlerCount = 0;
    private const int MAX_HANDLERS = 3;

    public event EventHandler Click
    {
        add
        {
            if (_handlerCount < MAX_HANDLERS)
            {
                _click += value;
                _handlerCount++;
                System.Console.WriteLine($"Handler ditambahkan. Total: {_handlerCount}");
            }
            else
            {
                System.Console.WriteLine($"Gagal tambah handler. Maksimal {MAX_HANDLERS} handler!");
            }
        }
        remove
        {
            _click -= value;
            _handlerCount--;
            System.Console.WriteLine($"Handler dihapus. Total: {_handlerCount}");
        }
    }

    public void SimulateClick()
    {
        System.Console.WriteLine("Button Diklik !");
        _click?.Invoke(this, EventArgs.Empty);
    }
}

public class Program
{
    public static void Main()
    {
        Button button = new Button();
        EventHandler handler1 = (s, e) => System.Console.WriteLine("Handler 1 telah di excekusi");
        EventHandler handler2 = (s, e) => System.Console.WriteLine("Handler 2 telah di excekusi");
        EventHandler handler3 = (s, e) => System.Console.WriteLine("Handler 3 telah di excekusi");
        EventHandler handler4 = (s, e) => System.Console.WriteLine("Handler 4 telah di excekusi");

        button.Click += handler1;
        button.Click += handler2;
        button.Click += handler3;
        button.Click += handler4;
        button.SimulateClick();

        System.Console.WriteLine("Menghapus event handler");
        button.Click -= handler3;

        System.Console.WriteLine("menambahkan lagi");
        button.Click += handler4;
        button.SimulateClick();

    }
}