/**********************************************************************/
/*                          Topik: Interface                          */
/*    Purpose: Untuk contract, yaitu menentukan methods, properties,  */
/*    events dan indexers yang harus di implementasikan oleh          */
/*    class ataupun struct                                            */
/**********************************************************************/




/*==================================Main=============================*/
public class Program
{
    public static void Main()
    {
        IEnumerator er = new Countdown(); // countdown di cast/diubah ke ienumerator

        while (er.MoveNext())
        {
            System.Console.WriteLine(er.Current);
        }

        /*----------- Explisit interface implementasion -----------*/

        Robot walky = new Robot();
        walky.Rest(); // ini untuk call method rest interface 1
        ((I2)walky).Rest(); // ini explisit interface method interface 2

        /*----------- Interface dan boxing -----------*/

        RobotSapi nino = new RobotSapi();
        ISapi sapi = nino; // disini terjadi boxing dimana value type struct ke reference type interface
        sapi.Moo();



    }
}






/*==================================Class=============================*/
public interface IEnumerator
{
    bool MoveNext(); // method
    object Current { get; } //read-only property
    void Rest();//method juga

}
/*----------- Implementing interface member virtually -----------*/
// jadi misal kita, menginplementasikan interface dalam class catur, 
//nah method di catur ini agar bisa di override sub classnya harus pakai virtual kalau publicaja gk bs.

public interface WinningCondition
{
    void Menang();
}


public class Catur : WinningCondition
{
    public virtual void Menang() => System.Console.WriteLine("Skak");
}

public class TurnamenCatur : Catur
{
    public override void Menang()
    {
        System.Console.WriteLine("Waktu Lawan Habis, dia tidak bisa bergerak, Kamu menang ^_^");
         
    }
}

/*----------- Reimplementing interface in sub class -----------*/
// ini mah sama kek diatas, intinya pakai override, dgn embel embel implenetasi interface di base buat manggil method yang dibikin tapi pake virtual
public interface IMemory { void Save(); }

public class HandPhone : IMemory
{
    void IMemory.Save() => Save();
    protected virtual void Save() => System.Console.WriteLine("Data tersimpan");

}

public class Pesan : HandPhone
{
    protected override void Save() => System.Console.WriteLine("Pesan Tersimpan");
}


/*----------- Interface dan boxing -----------*/
public interface ISapi { void Moo(); }
struct RobotSapi:ISapi {public void Moo()=> System.Console.WriteLine("Moo Lapar....");}




/*----------- Explisit interface implementasion -----------*/
//ketika 2 atau lebih interface memiliki method dengan signature yang sama atau nama yang sama bakal terjadi collision
// untuk menghindari itu di gunakan explisit interface implementation


interface I1 { void Rest(); }
interface I2 { int Rest(); }


struct  Robot :I1, I2
{
    public void Rest() => System.Console.WriteLine("Istirahat bro");

    int I2.Rest() => 1; // ini mengimplimentasi explisit interface

}


/*----------- Extend interface -----------*/
public interface ITenis { void Hit(); }
public interface IPadel:ITenis { void Indor(); } // IPadel dia extend dari Itenis, jadi yang implementasi Ipadel harus bikin method yg ada di kedua interface

public struct Game : IPadel // contohnya seperti ini
{
    public void Indor() => System.Console.WriteLine("Ruangan Bersih");
    public void Hit() => System.Console.Write("Pukul yang keras");
}


public class Countdown : IEnumerator
{
    int Count = 5;
    public bool MoveNext()
    {
        return Count-- > 0;
    }
    public object Current => Count;

    public void Rest() => Count = 0;
}