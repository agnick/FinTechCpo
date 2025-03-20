using System.Globalization;
using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для редактирования финансовой операции.
/// </summary>
public class EditOperationCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public EditOperationCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет редактирование операции по введённому ID.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.WriteLine("Доступные операции:");
            var operations = _manager.GetOperations();
            if (!operations.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных операций.");
                return;
            }
            
            foreach (var op in operations)
                Console.WriteLine(op.ToString());
            
            Console.Write("Введите ID операции для редактирования: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID операции.");
                return;
            }

            Console.Write("Введите новую сумму операции: ");
            var amountInput = Console.ReadLine();
            if (!decimal.TryParse(amountInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newAmount) || newAmount < 0)
            {
                Console.WriteLine("Ошибка: Сумма должна быть неотрицательным числом.");
                return;
            }

            Console.Write("Введите новую дату операции (формат: yyyy-MM-dd HH:mm:ss): ");
            var dateInput = Console.ReadLine();
            if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
            {
                Console.WriteLine("Ошибка: Неверный формат даты. Используйте yyyy-MM-dd HH:mm:ss.");
                return;
            }

            Console.Write("Введите новое описание операции (опционально): ");
            var newDescription = Console.ReadLine();

            Console.Write("Введите новый ID категории: ");
            var categoryIdInput = Console.ReadLine();
            if (!Guid.TryParse(categoryIdInput, out Guid newCategoryId))
            {
                Console.WriteLine("Ошибка: Неверный формат ID категории.");
                return;
            }

            _manager.EditOperation(id, newAmount, newDate, newDescription, newCategoryId);
            Console.WriteLine("Операция отредактирована успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании операции: {ex.Message}");
        }
    }
}