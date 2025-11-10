




public class Program
{
    interface ITransformer<in TInput, out TOutput>
    {
        TOutput Transform(TInput input);
    }
    interface IConsumer<in T>
    {
        void Consume(T item);
    }
    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal a)
        {
            System.Console.WriteLine($"Consumed: {a.GetType().Name}");
        }
    }
    interface IProducer<out T>
    {
        T Produce();
    }

    public class CatProducer : IProducer<Cat>
    {
        public Cat Produce() => new Cat();
        
    }

    public class Animal
    {
        public virtual string Name => "Some Animal";
    }
    public class Cat:Animal{}
    public static T FindMax<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? a: b;
    }
    public class Repository<T> where T : class
    {
        List<T> hasil = new List<T>();
        public void Add(T item)
        {
            hasil.Add(item);
        }
        public List<T> GetAll() => hasil;
    }
    public class Person
    {
        public string Name { get; set; }
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;

    }
    public class resPair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public resPair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
        public void Display()
        {
            System.Console.WriteLine($"{First} - {Second} ");
        }
    }
    public static void Main()
    {

        Console.WriteLine(FindMax(10, 5));          // Output: 10
        Console.WriteLine(FindMax(3.5, 7.2));

        var person = new resPair<string, int>("Alice", 30);
        person.Display();

        // int x = 5, y = 10;

        // Swap(ref x, ref y);
        // Console.WriteLine($"{x}, {y}"); // Output: 10, 5

        Repository<Person> repo = new Repository<Person>();
        repo.Add(new Person { Name = "yaya" });
        repo.Add(new Person { Name = "gopal" });
        foreach (var p in repo.GetAll())
        {
            System.Console.WriteLine(p.Name);
        }

        IProducer<Cat> catProduser = new CatProducer();
        IProducer<Animal> animalProduser = catProduser;
        // karena interface pakai out T kita bisa naik
        Animal animal = animalProduser.Produce();//menghasilkan cat tapi di tampung animal

        IConsumer<Animal> consume = new AnimalConsumer();
        IConsumer<Cat> catConsumer = consume; // covariant
        catConsumer.Consume(new Cat());





    }
}







// public class Box<T>
// {
//     public T Content;
//     public void SetContent(T value)
//     {
//         Content = value;
//     }
//     public T GetContent() => Content;
// }

// public class Stack<T>
// {
//     int position;
//     T[] data = new T[100];

//     public void Push(T obj) => data[position++] = obj;
//     public T Pop() => data[--position];

// }