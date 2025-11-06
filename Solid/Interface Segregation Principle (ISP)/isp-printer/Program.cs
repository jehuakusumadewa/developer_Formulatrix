List<IPrint> allPrinters = new List<IPrint>
{
    new BasicPrinter(),
    new AdvancePrinter(),
    new OfficePrinter()
};

Console.WriteLine("Proses print document");

    foreach (var printer in allPrinters)
    {
    printer.Print("Document perusahaan");
    }

System.Console.WriteLine("Proses scan document");
foreach (var printer in allPrinters)
{
    if (printer is IScan scanner)
    {
        scanner.Scan("Document Perizinan");
    }
}

System.Console.WriteLine("Proses Fax document");
foreach (var printer in allPrinters){
    if(printer is IFax faxMahine)
    {
        faxMahine.Fax("Dokument Penting");
    }
}

public interface IPrint
{
    void Print(string document);
}

public interface IScan
{
    void Scan(string document);
}

public interface IFax
{
    void Fax(string document);
}

public class BasicPrinter : IPrint
{
    public void Print(string document)
    {
        System.Console.WriteLine($"Mencetak {document}");
    }

}

public class AdvancePrinter : IPrint, IScan
{
    public void Print(string document)
    {
        System.Console.WriteLine($"mencetak {document}");
    }
    public void Scan(string document)
    {
        System.Console.WriteLine($"Scan {document}");
    }
}

public class OfficePrinter: IPrint, IScan, IFax
{
    public void Print(string document)
    {
        System.Console.WriteLine($"mencetak {document}");
    }
    public void Scan(string document)
    {
        System.Console.WriteLine($"scan {document}");
    }
    public void Fax(string document)
    {
        System.Console.WriteLine($"Fax {document}");
    }
}