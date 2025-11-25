using System.Text;


System.Console.Write("Masukkan banyaknya n :");

int number = int.Parse(Console.ReadLine());
var magicWord = new OlahKata();

magicWord.AddRule(3, "foo");
magicWord.AddRule(5, "bar"); 
magicWord.AddRule(7, "jazz");
magicWord.AddRule(9, "huzz");
magicWord.Generate(number);

public class OlahKata
{
    public Dictionary<int, string> data;
    public StringBuilder hasil;

    public OlahKata()
    {
        data = new Dictionary<int, string>();
        hasil = new StringBuilder();
    }

    public void AddRule(int angka, string result )
    {
         data.Add(angka, result);
    }

    public void Generate(int angka)
    {
        // var sortedByKeyDesc = data.OrderByDescending(kvp => kvp.Key);
        var kamus = data.OrderBy(kvp => kvp.Key);
        for (int i = 1; i <= angka; i++)
        {
            StringBuilder tempResult = new StringBuilder(); 
            foreach (var kvp in kamus)
            {
                // Console.WriteLine($"{kvp.Key} : {kvp.Value}");
                if (i % kvp.Key == 0)
                {
                    tempResult.Append(kvp.Value);
                }
            }
            if (tempResult.Length == 0)
            {
                tempResult.Append(i.ToString());
            }

            if (hasil.Length > 0)
            {
                hasil.Append(", ");
            }
            hasil.Append(tempResult);

        }
        
        System.Console.WriteLine(hasil.ToString());
    }
}