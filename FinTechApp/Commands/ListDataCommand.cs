using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для отображения всех данных в системе (счета, категории, операции).
/// </summary>
public class ListDataCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public ListDataCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет отображение всех данных в консоли.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.WriteLine("Счета:");
            foreach (var account in _manager.GetBankAccounts())
                Console.WriteLine(account.ToString());

            Console.WriteLine("\nКатегории:");
            foreach (var category in _manager.GetCategories())
                Console.WriteLine(category.ToString());

            Console.WriteLine("\nОперации:");
            foreach (var operation in _manager.GetOperations())
                Console.WriteLine(operation.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении данных: {ex.Message}");
        }
    }
}