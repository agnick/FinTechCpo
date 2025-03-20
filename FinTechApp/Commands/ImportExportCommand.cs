using System;
using FinTechApp.Interfaces;
using FinTechApp.Managers;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для импорта и экспорта данных в различных форматах.
/// </summary>
public class ImportExportCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public ImportExportCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет импорт или экспорт данных на основе выбора пользователя.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Экспорт данных в CSV");
            Console.WriteLine("2. Экспорт данных в JSON");
            Console.WriteLine("3. Экспорт данных в YAML");
            Console.WriteLine("4. Импорт данных из CSV");
            Console.WriteLine("5. Импорт данных из JSON");
            Console.WriteLine("6. Импорт данных из YAML");
            Console.Write("Ваш выбор: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите директорию для экспорта CSV: ");
                    var csvDir = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(csvDir))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ExportData("csv", csvDir);
                    Console.WriteLine("Экспорт в CSV выполнен.");
                    break;

                case "2":
                    Console.Write("Введите путь для экспорта JSON: ");
                    var jsonPath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(jsonPath))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ExportData("json", jsonPath);
                    Console.WriteLine("Экспорт в JSON выполнен.");
                    break;

                case "3":
                    Console.Write("Введите путь для экспорта YAML: ");
                    var yamlPath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(yamlPath))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ExportData("yaml", yamlPath);
                    Console.WriteLine("Экспорт в YAML выполнен.");
                    break;

                case "4":
                    Console.Write("Введите директорию для импорта CSV: ");
                    var importCsvDir = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(importCsvDir))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ImportData("csv", importCsvDir);
                    Console.WriteLine("Импорт из CSV выполнен.");
                    break;

                case "5":
                    Console.Write("Введите путь для импорта JSON: ");
                    var importJsonPath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(importJsonPath))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ImportData("json", importJsonPath);
                    Console.WriteLine("Импорт из JSON выполнен.");
                    break;

                case "6":
                    Console.Write("Введите путь для импорта YAML: ");
                    var importYamlPath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(importYamlPath))
                    {
                        Console.WriteLine("Ошибка: Путь не может быть пустым.");
                        return;
                    }

                    _manager.ImportData("yaml", importYamlPath);
                    Console.WriteLine("Импорт из YAML выполнен.");
                    break;

                default:
                    Console.WriteLine("Ошибка: Неверный выбор.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при импорте/экспорте: {ex.Message}");
        }
    }
}