using FinTechApp.Interfaces;
using FinTechApp.Types;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для выполнения существующей финансовой операции на выбранном счете.
/// </summary>
public class PerformAccountOperationCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public PerformAccountOperationCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет выбранную существующую операцию на счете, показывая доступные счета и операции.
    /// </summary>
    public void Execute()
    {
        try
        {
            var accounts = _manager.GetBankAccounts().ToList();
            if (!accounts.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных счетов.");
                return;
            }

            Console.WriteLine("Доступные счета:");
            foreach (var account in accounts)
                Console.WriteLine($"{account.Id} - {account.Name} (Баланс: {account.Balance:C})");

            var operations = _manager.GetOperations().ToList();
            if (!operations.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных операций.");
                return;
            }

            Console.WriteLine("\nСуществующие операции:");
            foreach (var operation in operations)
            {
                var account = accounts.FirstOrDefault(a => a.Id == operation.BankAccountId);
                var category = _manager.GetCategories().FirstOrDefault(c => c.Id == operation.CategoryId);
                Console.WriteLine(
                    $"{operation.Id} - {operation.Type} (Счет: {account?.Name ?? operation.BankAccountId.ToString()}, Сумма: {operation.Amount:C}, Категория: {category?.Name ?? operation.CategoryId.ToString()}, Дата: {operation.Date}, Описание: {operation.Description ?? "Нет"})");
            }

            Console.Write("\nВведите ID операции для выполнения: ");
            var operationIdInput = Console.ReadLine();
            if (!Guid.TryParse(operationIdInput, out Guid operationId))
            {
                Console.WriteLine("Ошибка: Неверный формат ID операции.");
                return;
            }

            var selectedOperation = operations.FirstOrDefault(o => o.Id == operationId);
            if (selectedOperation == null)
            {
                Console.WriteLine("Ошибка: Операция не найдена.");
                return;
            }

            var targetAccount = accounts.FirstOrDefault(a => a.Id == selectedOperation.BankAccountId);
            if (targetAccount == null)
            {
                Console.WriteLine("Ошибка: Счет, связанный с операцией, не найден.");
                return;
            }

            // Выполняем операцию
            if (selectedOperation.Type == OperationType.Income)
            {
                targetAccount.Deposit(selectedOperation.Amount);
                Console.WriteLine(
                    $"Доход {selectedOperation.Amount:C} добавлен на счет {targetAccount.Name}. Новый баланс: {targetAccount.Balance:C}");
            }
            else // Expense
            {
                if (targetAccount.Balance < selectedOperation.Amount)
                {
                    Console.WriteLine("Ошибка: Недостаточно средств на счете для выполнения расходной операции.");
                    return;
                }

                targetAccount.Withdraw(selectedOperation.Amount);
                Console.WriteLine(
                    $"Расход {selectedOperation.Amount:C} списан со счета {targetAccount.Name}. Новый баланс: {targetAccount.Balance:C}");
            }

            // Операция остаётся в списке операций, но её повторное выполнение влияет только на баланс
            Console.WriteLine("Операция выполнена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выполнении операции: {ex.Message}");
        }
    }
}