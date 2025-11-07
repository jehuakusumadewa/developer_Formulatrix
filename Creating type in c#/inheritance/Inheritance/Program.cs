static void Boo(Asset a) => Console.WriteLine("Foo(Asset)");
static void Boo(House h) => Console.WriteLine("Foo(House)");

House h1 = new House();
Boo(h1); // Foo(House)
Asset a1 = new House();


static void Display(Asset aset) => System.Console.WriteLine(aset.Name);

Stock alibaba = new Stock { Name = "Alibaba", SharesOwned = 500000L };
//ini bisa terjadi karena stock ngeinherit asset jadi bisa akses property name dan sharesowned

System.Console.WriteLine(alibaba.Name);
System.Console.WriteLine(alibaba.SharesOwned);

//POLIMORFISM 
// disini didefinisikan kemampuan untuk variable base class bisa di gantikan oleh sub classnya
//misal :
Display(alibaba);
//walaupun parameternya Asset tapi tetap bisa dijalankan dangan sub class nya yaitu Stock

//NYOBA DOWNCASTING DAN UPCASTING
Stock barang = new Stock();
Asset a = barang; // ini dinamakan upcasting, kenapa ?
// karena dari sub class Stock ke base class Asset

Stock supply = (Stock)a; // ini namanya downcasting, kenapa ?
//karena dari base class Asset ke sub class Stock

System.Console.WriteLine(supply.SharesOwned);

//PENGGUNAAN "AS"

Stock s = new Stock();
Asset v = s as Asset; // atau bisa pakai as, jadi bisa downcast atau upcase 
//dan kalau gk bisa maka di assaign null


//PENGGUNAAN "IS"

Asset K = new House();

if (K is House) // is disini operator untuk check apakah asset ini bisa diubah atau sukses downcasenya
//atau upcasenya kalau iya jalankan code dibawah
{
    System.Console.WriteLine(((House)K).Mortgage);
}

if (K is House H) // disini sama kayak diatas, bedanya kalau bisa / sukses di ubah 
// maka asignt ke varible H dan H bisa di operasikan
{
    System.Console.WriteLine(H.Mortgage);
}



BaseClass @base = new Override();
@base.Foo(); // ini bisa ngeluarin override.foo karena polymorfis. Base class bisa di wakili oleh sub classnya

BaseClass ngilang = new Hider();
ngilang.Foo(); // ini ngeluarin hider.foo karena new disini menghide methodnya, atau tidak berfungsi polymorfisnya.


// Menggunakan Required

Toko toko = new Toko { Nama = "toserba" };// ini kalau misal gk pakai object inisialiasi, bakan error karena 
// pakai required, maka harus di isi propertynya









public class Asset
{
    public string Name;
    public virtual decimal Liability => 0; // ini method virtual, gunanya agar sub class bisa 
    // override methodnya dengan isi yang berbeda

    public virtual Asset Clone() => new Asset { Name = Name };


}


public class Stock : Asset
// ini berarti stock dapet inheritance dari asset
// dimana semua property dan method yang ada di asset
// di stock juga bisa mengakses itu juga
{
    public long SharesOwned;
}

public class House : Asset
// ini berarti house ngeinherit aseet
{
    public decimal Mortgage;
    public override decimal Liability => base.Liability + Mortgage; // ini subclass mengoverride method yang virtual di base class
    // base disini juga ngerefer ke base classnya
    public override Asset Clone()
    {
        return new House { Name = Name, Mortgage = Mortgage };
    } // ada yang di namakan covariant yaitu 
    //kemampuan untuk ngereturn sebuah method yang ada di base class
    // yang awalnya return type base class itu sendiri jadi bisa return ke type sub class
    // dengan syarat harus inherit base classnya (tentu saja ^_^)
}


public abstract class Weapon // class abstract sendiri tidak bisa di inisialisasi, 
// jadi cuman jadi kek blueprint, nunggu di implementasi sub classnya
{
    public string Name;
    public abstract int Power { get; } // property dengan read-only
}

public class WeaponMaster : Weapon
{
    public int Cultivation;
    public int SpiritStone;

    public override int Power => Cultivation * SpiritStone; //class yang menginherit class abstract 
    //harus menyediakan implementasi dari method yang ada di base class abstract
}

public class BaseClass
{
    public virtual void Foo() { System.Console.WriteLine("BaseClass.Foo"); }
}

public class Override : BaseClass
{
    public override void Foo()
    {
        System.Console.WriteLine("Override.Foo");
    }
}

public class Hider : BaseClass
{
    public new void Foo() {System.Console.WriteLine("Hider.Foo");}
}


// memang kalau sub class inherit base class dia memperoleh property dan methodnya tapi tidak untuk consturctornya
// kalau ingin harus di explisit / beri tau bahwa sub class menerapkan constructor dari base classnya
// seperti dibawah ini

public class Transportasi
{
    public string Name;

    public Transportasi() { }

    public Transportasi(string name) => Name = name;
}


public class Kapal : Transportasi
{
    public Kapal(string name) : base(name) { } // nah di beritau gini jadi bisa palai construct dari base class

}

public class Toko
{
    public required string Nama; // ini "required acces modifier dimana variable harus di inisialiasi sebelum objecet dibuat

    // kalau mau pakai constructor yang harus ada namenya kita bikin
    //attribute "setsrequired"

    public Toko() { } // ini construct biasa

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]

    public Toko(string n) => Nama = n; // nah ini construct yang harus required name

    // jadi setiap object gk harus required name kalo gk butuh 
    // bisa pakai construct biasa jadi lebih fleksible

}

