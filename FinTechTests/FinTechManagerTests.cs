using FinTechApp.DomainClasses;
using FinTechApp.Managers;
using FinTechApp.Types;

namespace FinTechTests;

public class FinTechManagerTests
{
    private readonly FinTechManager _manager;

    public FinTechManagerTests()
    {
        _manager = new FinTechManager();
    }

    // Тесты для BankAccount
    [Fact]
    public void AddBankAccount_ValidAccount_AddsSuccessfully()
    {
        var account = new BankAccount("Test Account", 100m);
        _manager.AddBankAccount(account);

        Assert.Contains(account, _manager.GetBankAccounts());
    }

    [Fact]
    public void AddBankAccount_NullAccount_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _manager.AddBankAccount(null));
        Assert.Equal("Банковский счет не может быть null.", exception.Message.Split('(')[0].Trim());
    }

    [Fact]
    public void RemoveBankAccount_ExistingId_RemovesSuccessfully()
    {
        var account = new BankAccount("Test Account", 100m);
        _manager.AddBankAccount(account);

        _manager.RemoveBankAccount(account.Id);

        Assert.DoesNotContain(account, _manager.GetBankAccounts());
    }

    [Fact]
    public void EditBankAccount_ExistingId_UpdatesName()
    {
        var account = new BankAccount("Old Name", 100m);
        _manager.AddBankAccount(account);

        _manager.EditBankAccount(account.Id, "New Name");

        Assert.Equal("New Name", account.Name);
    }

    // Тесты для Category
    [Fact]
    public void AddCategory_ValidCategory_AddsSuccessfully()
    {
        var category = new Category(OperationType.Income, "Salary");
        _manager.AddCategory(category);

        Assert.Contains(category, _manager.GetCategories());
    }

    [Fact]
    public void AddCategory_NullCategory_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _manager.AddCategory(null));
        Assert.Equal("Категория не может быть null.", exception.Message.Split('(')[0].Trim());
    }

    [Fact]
    public void RemoveCategory_ExistingId_RemovesSuccessfully()
    {
        var category = new Category(OperationType.Expense, "Food");
        _manager.AddCategory(category);

        _manager.RemoveCategory(category.Id);

        Assert.DoesNotContain(category, _manager.GetCategories());
    }

    [Fact]
    public void EditCategory_ExistingId_UpdatesFields()
    {
        var category = new Category(OperationType.Income, "Old Name");
        _manager.AddCategory(category);

        _manager.EditCategory(category.Id, "New Name", OperationType.Expense);

        Assert.Equal("New Name", category.Name);
        Assert.Equal(OperationType.Expense, category.Type);
    }

    // Тесты для Operation
    [Fact]
    public void AddOperation_ValidIncomeOperation_AddsAndUpdatesBalance()
    {
        var account = new BankAccount("Test Account", 100m);
        var category = new Category(OperationType.Income, "Salary");
        var operation = new Operation(OperationType.Income, account.Id, 50m, DateTime.Now, "Test", category.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(category);
        _manager.AddOperation(operation);

        Assert.Contains(operation, _manager.GetOperations());
        Assert.Equal(150m, account.Balance);
    }

    [Fact]
    public void AddOperation_ExpenseExceedsBalance_ThrowsInvalidOperationException()
    {
        var account = new BankAccount("Test Account", 100m);
        var category = new Category(OperationType.Expense, "Food");
        var operation = new Operation(OperationType.Expense, account.Id, 150m, DateTime.Now, "Test", category.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(category);

        var exception = Assert.Throws<InvalidOperationException>(() => _manager.AddOperation(operation));
        Assert.Equal("Недостаточно средств на счете для выполнения расходной операции.", exception.Message);
    }

    [Fact]
    public void RemoveOperation_ExistingId_RemovesSuccessfully()
    {
        var account = new BankAccount("Test Account", 100m);
        var category = new Category(OperationType.Income, "Salary");
        var operation = new Operation(OperationType.Income, account.Id, 50m, DateTime.Now, "Test", category.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(category);
        _manager.AddOperation(operation);

        _manager.RemoveOperation(operation.Id);

        Assert.DoesNotContain(operation, _manager.GetOperations());
    }

    [Fact]
    public void EditOperation_ValidUpdate_UpdatesFieldsAndBalance()
    {
        var account = new BankAccount("Test Account", 100m);
        var category = new Category(OperationType.Expense, "Food");
        var operation = new Operation(OperationType.Expense, account.Id, 50m, DateTime.Now, "Old", category.Id);
        var newDate = DateTime.Now.AddDays(1);
        var newCategory = new Category(OperationType.Expense, "NewCat");

        _manager.AddBankAccount(account);
        _manager.AddCategory(category);
        _manager.AddOperation(operation);
        _manager.AddCategory(newCategory);

        _manager.EditOperation(operation.Id, 20m, newDate, "New", newCategory.Id);

        Assert.Equal(20m, operation.Amount);
        Assert.Equal(newDate, operation.Date);
        Assert.Equal("New", operation.Description);
        Assert.Equal(newCategory.Id, operation.CategoryId);
        Assert.Equal(80m, account.Balance); // 100 - 50 (откат) + 20 (новая)
    }

    [Fact]
    public void EditOperation_NegativeAmount_ThrowsArgumentException()
    {
        var account = new BankAccount("Test Account", 100m);
        var category = new Category(OperationType.Income, "Salary");
        var operation = new Operation(OperationType.Income, account.Id, 50m, DateTime.Now, "Test", category.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(category);
        _manager.AddOperation(operation);

        var exception = Assert.Throws<ArgumentException>(() => _manager.EditOperation(operation.Id, -10m, DateTime.Now, "Test", category.Id));
        Assert.Equal("Сумма операции не может быть отрицательной.", exception.Message.Split('(')[0].Trim());
    }

    // Тесты для аналитики
    [Fact]
    public void GetIncomeExpenseDifference_WithOperations_ReturnsCorrectDifference()
    {
        var account = new BankAccount("Test Account", 100m);
        var incomeCat = new Category(OperationType.Income, "Salary");
        var expenseCat = new Category(OperationType.Expense, "Food");
        var incomeOp = new Operation(OperationType.Income, account.Id, 200m, DateTime.Now, "Income", incomeCat.Id);
        var expenseOp = new Operation(OperationType.Expense, account.Id, 50m, DateTime.Now, "Expense", expenseCat.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(incomeCat);
        _manager.AddCategory(expenseCat);
        _manager.AddOperation(incomeOp);
        _manager.AddOperation(expenseOp);

        var difference = _manager.GetIncomeExpenseDifference(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

        Assert.Equal(150m, difference); // 200 - 50
    }

    [Fact]
    public void GroupOperationsByCategory_WithOperations_ReturnsCorrectGrouping()
    {
        var account = new BankAccount("Test Account", 100m);
        var cat1 = new Category(OperationType.Income, "Salary");
        var cat2 = new Category(OperationType.Expense, "Food");
        var op1 = new Operation(OperationType.Income, account.Id, 100m, DateTime.Now, "Test1", cat1.Id);
        var op2 = new Operation(OperationType.Expense, account.Id, 50m, DateTime.Now, "Test2", cat2.Id);

        _manager.AddBankAccount(account);
        _manager.AddCategory(cat1);
        _manager.AddCategory(cat2);
        _manager.AddOperation(op1);
        _manager.AddOperation(op2);

        var result = _manager.GroupOperationsByCategory(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

        Assert.Equal(100m, result[cat1.Id]);
        Assert.Equal(50m, result[cat2.Id]);
    }
}