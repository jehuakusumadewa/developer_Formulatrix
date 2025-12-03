
using NUnit.Framework;
using SistemManagementInventory;
namespace InventoryTests;

public class InventoryUnitTest
{
    [SetUp]
    public void Setup()
    {
        Program.ClearProductsForTest();
    }

    [Test]
    public void AddNewProduct_ShouldIncreaseProductCount() 
    {
        Program.AddNewProductForTest("Bola", 20000, 10, "Olahraga", 5);


        Assert.AreEqual(1, Program.GetProductCount());
        Assert.AreEqual("Bola", Program.GetProductById(1).Name);
    }
    [Test]
    public void DeleteProduct_ShouldDecreaseProductCount()
    {
        Program.AddNewProductForTest("Bola", 20000, 10, "Olahraga", 5);
        Program.AddNewProductForTest("Sepatu", 150000, 20, "Olahraga", 10);

        Program.DeleteProductForTest(1);

        Assert.AreEqual(1, Program.GetProductCount());
        Assert.IsNull(Program.GetProductById(1));
    }
    [Test]
    public void UpdateProductStock_ShouldChangeStockQuantity()
    {
        Program.AddNewProductForTest("Bola", 20000, 10, "Olahraga", 5);

        Program.UpdateProductStockForTest(1, 25);

        Assert.AreEqual(25, Program.GetProductById(1).StockQuantity);
    }
}