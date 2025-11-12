public class ProsessEventArgs : EventArgs
{
    public string Message { get; }
    public DateTime Timestamp { get; }

    public ProsessEventArgs(string message)
    {
        Message = message;
        Timestamp = DateTime.Now;
    }
}
/*Biar aku jelaskan ProsessEventArgs ini mendefinisikan
class tipe custom data buat event yang di gunakan nanti 
ketika event dipanggil atau di invoke
*/

public class Processor
{
    //disini class custom data di pakai
    //caranya EventHandler<class custom data>
    // terus pakai invoke seperti panggil event biasa
    
    public event EventHandler<ProsessEventArgs> ProcessCompleted;

    public void Run()
    {
        //logic kalau mau

        //selesai logic panggil event
        ProcessCompleted?.Invoke(this, new ProsessEventArgs("Done!"));
    }
}