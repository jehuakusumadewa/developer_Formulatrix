
//deklarasi dalegate
using System.ComponentModel;

public delegate void EventHandlerDelegate(string message);
// {
//     add { _OnProcessCompleted += value; } 
//     remove {_OnProcessCompleted -= value}   
// }
// private EventHandlerDelagete _OnProcessCompleted;

// intinya ada properti/accessor dari eventhandler/delegate itu buat add dan remove


//class publisher
public class Publisher
{
    //publikasi event/deklarasi event
    public event EventHandlerDelegate OnProcessCompleted;

    public void StartProcess()
    {
        System.Console.WriteLine("Prosess Started...");
        //logic process apa gitu


        //ingat event itu biasanya notifikasi jadi seperti
        //kehidupannyata tugas kerjakan dulu, baru notif selesai keluar ^_^

        //panggil event ketika prosess selesai

        OnProcessCompleted.Invoke("Prosess Completed successfully");
        //jadi nama event di ikuti invoke kemudian pesan apa gitu


    }

}

public class Button
{
    //Event dengan EventHandler jadi ini delegate built in ya
    public event EventHandler Click;

    public void SimulateClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
        /* biar aku jelaskan apa yang terjadi
        jadi dia Click nama eventnya
        tanda tanya '?' ngecheck null gk kalo gk maka lanjut 
        invoke ya manggil eventnya
        this disini manggil object yang pakai method simulateclick misal a.SimulateClick, ya this itu a
        EventArgs itu buat custom data event
        Empty berarti datanya kosong
        */
    }
}

public class Subscriber
{
    public void HandleEvent(string message)
    {
        System.Console.WriteLine($"Received: {message}");
    }
}
/*Biar aku jelaskan class subscriber itu kelas yang 
punya method sesuai signature nya sama dengan delegate / event handler
atau singkatnya yang mengaplikasikannya
 */






public class Program
{
    public static void Main()
    {
        var publisher = new Publisher();
        var subscriber = new Subscriber();
        /*Biar aku jelaskan jadi kita bikin class
        1. class untuk publisher atau yang bikin event tadi
        terus yang manggil event juga,
        2. class lagi untuk subscriber yang punya method sesuai signature dari delagate/event handler
        yaitu class yang mengimplementasi
        */

        publisher.OnProcessCompleted += subscriber.HandleEvent;
        /* Biar aku jelaskan disini class yang nerbitin event,
            dia manggil eventnya, agar kita bisa memasukkan class yang mengimplementasi/ subscriber
            atau yang punya signature yang sama ke class event yang telah dibuat. 
            'Intinya di sini cuma ngedartarin class subscriber dengan method nya yg sesuai gt sih'
        */
        publisher.StartProcess();
        /*kemudian dia manggil method yg ada di class publisher yang ada event.invokenya
          disitu dia akan menjalankan logic, kemudian invoke event yang isinya tadi method dari class subsriber.
          Satu demi satu di jelankan sesuai urutan penambahan class subscribernya
        */

        publisher.OnProcessCompleted -= subscriber.HandleEvent;
        // disini dia menghilangkan subcriber/ unsubcribe method class tadi yang didaftarkan
        //pengunduran diri intinya

        // publisher.OnProcessCompleted = null;
        // kalau ini parah lagi yaitu pengunduran diri masal/ unsubcribe semua

        publisher.OnProcessCompleted += (msg) => System.Console.WriteLine("$Lambda: {msg}");
        /*kalau ini dia assign / ngedaftarin tapi gk dari class subscriber melainkan dari method yang kita bikin sendiri
        melalui lambda expression (=>) bisa juga valid 
        jadi langung parenteses variable ap gk usah pakai type. 
        kemudian di ikuti return nya apa
        */

    }
}


