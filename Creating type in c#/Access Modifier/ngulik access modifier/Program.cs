namespace NgulikAccessModifier;


class Program
{
    public static void Main()
    {
        Console.WriteLine("Running tests...");
    }
}

class c1 { } // class yang tidak didefinisi access modifiernya maka akan otomatis internal
// acces modifier internal sendiri berarti class dapat di akses jika dalam 1 assembly atau 
// bahasa mudahnya masih ada dalam 1 project, maka class bisa di akses.



// class A
// {
//     int bisa; // disini field/ property auto private kalo gk di set. Yang dapat mengaksesnya hanya dalam 1 class

// }

// class B
// {
//     internal int berat; // disini yang bisa mengaksesnya dalam 1 assembly atau dalam 1 project
// }

class BaseClass
{
    void Foo() { } // Foo hanya bisa di akses dalam class baseclass secara default
                   //auto private
    protected void Bar() { } // protected ini memungkinkan hanya base class dan sub class yang dapat mengaksesnya

}

class SubClass : BaseClass
{
    void Test1()
    {
        // Foo(); // ini gk bisa karena foo private, hanya ada di baseclass yang bisa akses
    }

    void Test2()
    {
        Bar(); // ini bisa karena protected, bisa diakses dari subclass atau baseclass
    }
}