// See https://aka.ms/new-console-template for more information
System.Console.Write("Masukkan banyaknya n :");
int number = int.Parse(Console.ReadLine());

for (int i = 1; i<= number; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        System.Console.Write("foobar, ");
    }else if (i % 3 == 0)
    {
        System.Console.Write("foo, ");
    }else if (i % 5 == 0)
    {
        System.Console.Write("bar, ");
    }else
    {
        System.Console.Write($"{i}, ");
    }
}

