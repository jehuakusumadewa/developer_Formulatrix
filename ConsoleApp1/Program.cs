System.Console.Write("Masukkan banyaknya n :");
int number = int.Parse(Console.ReadLine());

for (int i = 1; i<= number; i++)
{
    if (i % 7 == 0 && i % 3 == 0 && i % 5 == 0 && i % 9 == 0)
    {
        System.Console.WriteLine("foobarjazzhuzz");
    } else if (i % 7 == 0 && i % 9 == 0 && i % 5 == 0){ 
        System.Console.Write("barjazzhuzz, ");
    } else if (i % 9 == 0 && i % 3 == 0 && i % 7 == 0)
    { 
        System.Console.Write("foojazzhuzz, ");
    } else if (i % 9 == 0 && i % 3 == 0 && i % 5 == 0)
    { 
        System.Console.Write("foobarhuzz, ");
    } else if (i % 7 == 0 && i % 3 == 0 && i % 5 == 0)
    { 
        System.Console.Write("foobarjazz, ");
    }else if (i % 7 == 0 && i % 3 == 0)
    {
        System.Console.WriteLine("foojazz, ");
    }else if (i % 7 == 0 && i % 5 == 0)
    {
        System.Console.WriteLine("barjazz, ");
    }
    else if (i % 3 == 0 && i % 5 == 0)
    {
        System.Console.Write("foobar, ");
    } else if (i % 3 == 0)
    {
        System.Console.Write("foo, ");
    } else if (i % 5 == 0)
    {
        System.Console.Write("bar, ");
    } else if (i % 7 == 0)
    {
        System.Console.WriteLine("jazz, ");
    }
    else
    {
        System.Console.Write($"{i}, ");
    }
}