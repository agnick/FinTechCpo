using System.Globalization;
using FinTechApp.DomainClasses;
using FinTechApp.Interfaces;
using FinTechApp.Types;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для создания новой финансовой операции.
/// </summary>
public class CreateOperationCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public CreateOperationCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет создание операции с вводом данных пользователем.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.Write("Введите тип операции (income/expense): ");
            var typeInput = Console.ReadLine()?.ToLower();
            if (!Enum.TryParse<OperationType>(typeInput, true, out OperationType type) ||
                !Enum.IsDefined(typeof(OperationType), type))
            {
                Console.WriteLine("Ошибка: Тип операции должен быть 'income' или 'expense'.");
                return;
            }

            Console.WriteLine("Доступные счета:");
            var accounts = _manager.GetBankAccounts();
            if (!accounts.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных счетов.");
                return;
            }

            foreach (var bankAccount in accounts)
                Console.WriteLine($"{bankAccount.Id} - {bankAccount.Name} (Баланс: {bankAccount.Balance:C})");

            Console.Write("Введите ID счета: ");
            var accountIdInput = Console.ReadLine();
            if (!Guid.TryParse(accountIdInput, out Guid accountId))
            {
                Console.WriteLine("Ошибка: Неверный формат ID счета.");
                return;
            }

            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                Console.WriteLine("Ошибка: Счет не найден.");
                return;
            }

            Console.Write("Введите сумму операции: ");
            var amountInput = Console.ReadLine();
            if (!decimal.TryParse(amountInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) ||
                amount <= 0)
            {
                Console.WriteLine("Ошибка: Сумма должна быть положительным числом.");
                return;
            }

            Console.Write("Введите дату операции (формат: yyyy-MM-dd HH:mm:ss): ");
            var dateInput = Console.ReadLine();
            if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("Ошибка: Неверный формат даты. Используйте yyyy-MM-dd HH:mm:ss.");
                return;
            }

            Console.Write("Введите описание операции (опционально): ");
            var description = Console.ReadLine();

            Console.WriteLine("Доступные категории:");
            var categories = _manager.GetCategories().Where(c => c.Type == type);
            if (!categories.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных категорий для выбранного типа.");
                return;
            }

            foreach (var el in categories)
                Console.WriteLine($"{el.Id} - {el.Name}");

            Console.Write("Введите ID категории: ");
            var categoryIdInput = Console.ReadLine();
            if (!Guid.TryParse(categoryIdInput, out Guid categoryId))
            {
                Console.WriteLine("Ошибка: Неверный формат ID категории.");
                return;
            }

            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                Console.WriteLine("Ошибка: Категория не найдена или не соответствует типу операции.");
                return;
            }

            var operation = new Operation(type, accountId, amount, date, description, categoryId);
            _manager.AddOperation(operation);
            Console.WriteLine($"Операция создана успешно с ID: {operation.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании операции: {ex.Message}");
        }
    }
}