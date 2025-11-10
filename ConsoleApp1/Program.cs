// See https://aka.ms/new-console-template for more information
System.Console.Write("Masukkan banyaknya n :");
int number = int.Parse(Console.ReadLine());

for (int i = 1; i<= number; i++)
{
    if (i % 7 == 0 && i % 3 == 0 && i % 5 == 0)
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
    }
    else if (i % 3 == 0)
    {
        System.Console.Write("foo, ");
    }
    else if (i % 5 == 0)
    {
        System.Console.Write("bar, ");
    }
    else if (i % 7 == 0)
    {
        System.Console.WriteLine("jazz, ");
    }
    else
    {
        System.Console.Write($"{i}, ");
    }
}

