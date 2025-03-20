using FinTechApp.Interfaces;
using FinTechApp.Types;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для редактирования категории.
/// </summary>
public class EditCategoryCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public EditCategoryCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет редактирование категории по введённому ID.
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
            
            Console.Write("Введите ID категории для редактирования: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID категории.");
                return;
            }

            Console.Write("Введите новое название категории: ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Ошибка: Название категории не может быть пустым.");
                return;
            }

            Console.Write("Введите новый тип категории (income/expense): ");
            var typeInput = Console.ReadLine()?.ToLower();
            if (!Enum.TryParse<OperationType>(typeInput, true, out OperationType newType) || !Enum.IsDefined(typeof(OperationType), newType))
            {
                Console.WriteLine("Ошибка: Тип категории должен быть 'income' или 'expense'.");
                return;
            }

            _manager.EditCategory(id, newName, newType);
            Console.WriteLine("Категория отредактирована успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании категории: {ex.Message}");
        }
    }
}