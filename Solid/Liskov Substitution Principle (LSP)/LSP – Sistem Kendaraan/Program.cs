
/*Lsp itu subclass dapat menggantikan baseclass 
tanpa membuat program itu rusak atau selah */

List<Vehicle> vehicles = new List<Vehicle>
{
    new CarGasoline(),
    new TruckDiesel(),
    new ElectricCar(),
    new MotorcycleGasoline()
};

System.Console.WriteLine("Menyalakan semua kendaraan");

foreach (var vehicle in vehicles)
{
    vehicle.StartEngine();
}

System.Console.WriteLine("Mengisi bahan bakar fosil");
foreach (var vehicle in vehicles)
{
    if (vehicle is IBahanBakarFosil fosil) {
        fosil.Refuel();
    }
}

System.Console.WriteLine("mengisi baterai kendaraan");
foreach (var vehicle in vehicles)
{
    if (vehicle is IBahanBakarAlami alami)
    {
        alami.ChargeBattery();
    }
}

public class Vehicle
{
    public void StartEngine()
    {
        System.Console.WriteLine("Menyalakan mesin");
    }
}

public interface IBahanBakarFosil 
{
    void Refuel();
}

public interface IBahanBakarAlami
{
    void ChargeBattery();
}

public class CarGasoline : Vehicle, IBahanBakarFosil 
{
    public void Refuel()
    {
        System.Console.WriteLine("Mengisi bensin mobil");
    }
}

public class TruckDiesel : Vehicle, IBahanBakarFosil
{
    public void Refuel()
    {
        System.Console.WriteLine("Mengisi diesel Truck");
    }
}

public class ElectricCar : Vehicle, IBahanBakarAlami
{
    public void ChargeBattery()
    {
        System.Console.WriteLine("Mengganti baterai mobil");
    }
}

public class MotorcycleGasoline : Vehicle, IBahanBakarFosil
{
    public void Refuel()
    {
        System.Console.WriteLine("Mengisi bensin motor");
    }
}