using System.Drawing;
using System.Security.Cryptography.X509Certificates;

var rect = new Rectangle(3, 4);
(float width, float height) = rect;
System.Console.WriteLine(width + " " + height);
/*oww jadi udh di inisiate dulu terus di jalan in construct, 
baru jadi object baru dan kalau ingin ambil nilai field bisa langsung assign objectnya
*/

var kelinci = new Bunny { Nama = "chiko", Berat = 10 }; // kalau gk ada constructnya bisa di pakai object initializer

Panda momo = new Panda("momo"); // ini call atau manggil constructor dengan keyword new

// PAKAI THIS

Agent kimi = new Agent { Name = "kimi" };
Agent ali = new Agent { Name = "ali" };
kimi.Mission(ali);


//PAKAI INDEXER

Sentence k = new Sentence();
System.Console.WriteLine(k[0]);
k[2] = "Lo ha";


//PAKAI NAMEOF OPERATOR
int emergencyCall = 911;
string name = nameof(emergencyCall);
System.Console.WriteLine(name);





class Rectangle // kalau gk ada acces modifier otomatis internal
{
    public readonly float Width, Height;
    public Rectangle(float width, float height)
    {
        Width = width;
        Height = height;
    }
    public void Deconstruct(out float weight, out float height)
    {
        weight = Width;
        height = Height;
    }


}

public class Bunny
{
    public string Nama;
    public int Berat;
    public string Warna = "Putih"; // ini namanya field initializer

    /*method bisa overload, yaitu method dengan nama yang sama tapi beda di signaturenya (return type dan parameter)
     signature sendiri itu terdiri dari return type untuk yang kita buat itu void dan sama berarti
     tetapi berbeda di parameternya yang satunya string s dan yang satunya int i */
    void Makanan(string s) { }// 
    void Makanan(int i) { }//


}

public class Panda
{
    string name; // ini field
    public Panda (string n) // ini constructor
    {
        name = n; // ini menginitialisasi
    }
}

public class MortalKombat
{
    public string Tipe;
    public string Nama;

    public MortalKombat(string tipe) => Tipe = tipe;

    public MortalKombat(string tipe, string nama) : this(tipe) => Nama = nama;
    // disini bisa overload atau bisa sama nama constructornya tapi beda parameter. Untuk memanggil constructor lain 
    // pakai ": this" (pakai titik dua terus this)

}

public class Agent
{
    public string Name;
    public Agent Partner;
    public void Mission(Agent candidate)
    {
        Partner = candidate;
        candidate.Partner = this;
        System.Console.WriteLine($"Agent yang sedang menjalankan misi adalah {Name} dan {candidate.Name}");
    }
}

public class Sentence
{
    string[] Words = "Halo Selamat Pagi Semuanya".Split();
    public string this[int urutkata]
    {
        get { return Words[urutkata]; }
        set { Words[urutkata] = value; }

    }
}