using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для удаления финансовой операции.
/// </summary>
public class RemoveOperationCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public RemoveOperationCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет удаление операции по введённому ID.
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
            
            Console.Write("Введите ID операции для удаления: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID операции.");
                return;
            }

            _manager.RemoveOperation(id);
            Console.WriteLine("Операция удалена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении операции: {ex.Message}");
        }
    }
}