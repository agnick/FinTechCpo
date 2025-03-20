using FinTechApp.DomainClasses;
using FinTechApp.Types;

namespace FinTechApp
{
    /// <summary>
    /// Интерфейс фасада для управления финансовыми данными: счетами, категориями, операциями, импортом и экспортом.
    /// </summary>
    public interface IFinTechManager
    {
        void AddBankAccount(BankAccount account);
        void RemoveBankAccount(Guid accountId);
        IEnumerable<BankAccount> GetBankAccounts();
        void EditBankAccount(Guid accountId, string newName);
        void AddCategory(Category category);
        void RemoveCategory(Guid categoryId);
        IEnumerable<Category> GetCategories();
        void EditCategory(Guid categoryId, string newName, OperationType newType);
        void AddOperation(Operation operation);
        void RemoveOperation(Guid operationId);
        IEnumerable<Operation> GetOperations();
        void EditOperation(Guid operationId, decimal newAmount, DateTime newDate, string newDescription, Guid newCategoryId);
        decimal GetIncomeExpenseDifference(DateTime start, DateTime end);
        Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end);
        void ExportData(string format, string directoryOrFilePath);
        void ImportData(string format, string directoryOrFilePath);
    }
}