using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для удаления категории.
/// </summary>
public class RemoveCategoryCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public RemoveCategoryCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет удаление категории по введённому ID.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.WriteLine("Доступные категории:");
            var categories = _manager.GetCategories();
            if (!categories.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных категорий.");
                return;
            }

            foreach (var el in categories)
                Console.WriteLine($"{el.Id} - {el.Name}");
            
            Console.Write("Введите ID категории для удаления: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID категории.");
                return;
            }

            _manager.RemoveCategory(id);
            Console.WriteLine("Категория удалена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении категории: {ex.Message}");
        }
    }
}