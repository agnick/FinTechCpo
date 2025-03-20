using System;
using System.Globalization;
using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для выполнения аналитики финансовых данных.
/// </summary>
public class AnalyticsCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public AnalyticsCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет аналитику за указанный период: разницу доходов и расходов, группировку по категориям.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.Write("Введите начальную дату (yyyy-MM-dd): ");
            var startInput = Console.ReadLine();
            if (!DateTime.TryParseExact(startInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime start))
            {
                Console.WriteLine("Ошибка: Неверный формат даты. Используйте yyyy-MM-dd.");
                return;
            }

            Console.Write("Введите конечную дату (yyyy-MM-dd): ");
            var endInput = Console.ReadLine();
            if (!DateTime.TryParseExact(endInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime end))
            {
                Console.WriteLine("Ошибка: Неверный формат даты. Используйте yyyy-MM-dd.");
                return;
            }

            if (start > end)
            {
                Console.WriteLine("Ошибка: Начальная дата не может быть позже конечной.");
                return;
            }

            var difference = _manager.GetIncomeExpenseDifference(start, end);
            Console.WriteLine($"Разница между доходами и расходами: {difference:C}");

            var grouped = _manager.GroupOperationsByCategory(start, end);
            Console.WriteLine("Суммы по категориям:");
            foreach (var kvp in grouped)
            {
                var category = _manager.GetCategories().FirstOrDefault(c => c.Id == kvp.Key);
                Console.WriteLine($"Категория: {category?.Name ?? kvp.Key.ToString()}, Сумма: {kvp.Value:C}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выполнении аналитики: {ex.Message}");
        }
    }
}