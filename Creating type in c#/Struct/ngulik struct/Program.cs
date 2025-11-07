//struct itu tipe value type tapiii punya kemampuan seperti class

/*======================================Main====================================*/

Point p = new Point(); // kalau gk ada consturtor/ gk di set maka semua field 
// atau propertynya di assign ke nilai default
Point p1 = default; // atau bisa seperti ini



/*======================================Class====================================*/
struct Point
{
    int x = 1;
    int y;
    public Point() => y = 1; // ini constructor tanpa parameter

}

readonly struct Cat
{ // read only menjaga agar semua field tidak diubah
    public readonly int X, Y;
} // segala cara untuk memodifikasi x atau y setalah construction maka akan compile error


ref struct Pohon { public int X, Y; }
// penggunaan ref disini agar instace hanya bisa ke bentuk value type
// tidak bisa reference type
// contoh var Phinokio = new Pohon[100] ini compile run error