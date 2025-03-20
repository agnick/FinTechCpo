using FinTechApp;
using FinTechApp.DomainClasses;
using FinTechApp.Managers;
using Moq;

namespace FinTechTests;

public class FinTechManagerProxyTests
{
    private readonly Mock<IFinTechManager> _mockManager;
    private readonly FinTechManagerProxy _proxy;

    public FinTechManagerProxyTests()
    {
        _mockManager = new Mock<IFinTechManager>();
        _proxy = new FinTechManagerProxy(_mockManager.Object);
    }

    [Fact]
    public void GetBankAccounts_CacheValid_ReturnsCachedData()
    {
        var accounts = new List<BankAccount> { new("Test Account", 100m) };
        _mockManager.Setup(m => m.GetBankAccounts()).Returns(accounts);

        var result1 = _proxy.GetBankAccounts();
        var result2 = _proxy.GetBankAccounts();

        Assert.Equal(accounts, result1);
        Assert.Equal(accounts, result2);
        _mockManager.Verify(m => m.GetBankAccounts(), Times.Once()); // Вызван только один раз
    }

    [Fact]
    public void AddBankAccount_InvalidatesCache_CallsRealManager()
    {
        var account = new BankAccount("Test Account", 100m);
        _proxy.AddBankAccount(account);

        _mockManager.Verify(m => m.AddBankAccount(account), Times.Once());
        var result = _proxy.GetBankAccounts();
        _mockManager.Verify(m => m.GetBankAccounts(), Times.Once()); // Кэш сброшен, вызов реального менеджера
    }

    [Fact]
    public void ImportData_InvalidatesAllCaches_CallsRealManager()
    {
        _proxy.ImportData("json", "test.json");

        _mockManager.Verify(m => m.ImportData("json", "test.json"), Times.Once());
        _proxy.GetBankAccounts();
        _proxy.GetCategories();
        _proxy.GetOperations();
        _mockManager.Verify(m => m.GetBankAccounts(), Times.Once());
        _mockManager.Verify(m => m.GetCategories(), Times.Once());
        _mockManager.Verify(m => m.GetOperations(), Times.Once()); // Все кэши сброшены
    }
}