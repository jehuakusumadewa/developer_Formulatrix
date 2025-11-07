/*====================================================Main======================================================*/
Stack stack = new Stack();
int nomer = 10;

stack.Push(nomer);
System.Console.WriteLine(stack.data[0]);
System.Console.WriteLine(stack.Pop());
System.Console.WriteLine(stack.data[0]);

/* >>>>>>>>  Boxing <<<<<<<< */
int x = 9;
Object obj = x;
// ini dinamakan boxing yaitu mengubah dari value type instance ke referense type instance
/* >>>>>>>>  unBoxing <<<<<<<< */

int o = (int)x;
//ini reversenya yaitu mengubah obj (referense type kembali value type)

/* >>>>>>>>  C# programs undergo type checking at two stages <<<<<<<< */



//1. Compile-time check:
// int x = "5"; // Compile-time error: Cannot implicitly convert string to int


//2. Run-time check:
object y = "5";
// int z = (int)y; // Runtime error: InvalidCastException, because 'y' actually holds a string, not an int.


/* >>>>>>>>  The GetType Method and typeof Operator <<<<<<<< */

Point p = new Point();
//1. Get type adalah runtime type instance check type:
System.Console.WriteLine(p.GetType().Name); // output Point


//2. typeof adalah compile type instance check type
System.Console.WriteLine(typeof(Point).Name);


/* >>>>>>>>  ToString <<<<<<<< */
Cat c = new Cat();
System.Console.WriteLine(c); // output auto ketika object dari class di panggil tanpa property 
// ataupun method maka auto call function to string









/*====================================================Class======================================================*/
public class Stack
{
    public int position;
    // object merupakan base class utama dari segala type di c# maka dari itu
    // array object bisa masuk semua tipe
    public object[] data = new object[10];

    public void Push(object obj)
    {
        data[position++] = obj;
    }
    public object Pop()
    {
        return data[--position];
    }
}

/* >>>>>>>>  The GetType Method and typeof Operator <<<<<<<< */
public class Point { public int X, Y; }

/* >>>>>>>>  ToString <<<<<<<< */
public class Cat
{
    public string Name;
    public override string ToString() // ToString sendiri itu method inherit dari object
    // kita override sesuai kebutuhan
    {
        return Name;
    }
}