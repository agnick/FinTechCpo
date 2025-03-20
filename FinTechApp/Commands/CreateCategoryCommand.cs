using System;
using FinTechApp.DomainClasses;
using FinTechApp.Interfaces;
using FinTechApp.Managers;
using FinTechApp.Types;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для создания новой категории.
/// </summary>
public class CreateCategoryCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public CreateCategoryCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет создание категории с вводом данных пользователем.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.Write("Введите название категории: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ошибка: Название категории не может быть пустым.");
                return;
            }

            Console.Write("Введите тип категории (income/expense): ");
            var typeInput = Console.ReadLine()?.ToLower();
            if (!Enum.TryParse<OperationType>(typeInput, true, out OperationType type) ||
                !Enum.IsDefined(typeof(OperationType), type))
            {
                Console.WriteLine("Ошибка: Тип категории должен быть 'income' или 'expense'.");
                return;
            }

            var category = new Category(type, name);
            _manager.AddCategory(category);
            Console.WriteLine($"Категория создана успешно с ID: {category.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании категории: {ex.Message}");
        }
    }
}